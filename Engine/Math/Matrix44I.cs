﻿using System;
using System.Runtime.InteropServices;

namespace Altseed
{
    /// <summary>
    /// <see cref="int"/>型の4x4行列を表す構造体
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix44I : ICloneable, IEquatable<Matrix44I>
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 4 * 4)]
        private int[,] Values;
        /// <summary>
        /// 単位行列を取得する
        /// </summary>
        public static Matrix44I Identity
        {
            get
            {
                var result = new Matrix44I();
                result.SetIdentity();
                return result;
            }
        }

        /// <summary>
        /// 転置行列を取得する
        /// </summary>
        public readonly Matrix44I TransPosition
        {
            get
            {
                var result = new Matrix44I();
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        result[i, j] = this[j, i];
                return result;
            }
        }

        /// <summary>
        /// 指定した位置の値を取得または設定する
        /// </summary>
        /// <param name="x">取得する要素の位置</param>
        /// <param name="y">取得する要素の位置</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/>または<paramref name="y"/>が0未満または4以上</exception>
        /// <returns><paramref name="x"/>と<paramref name="y"/>に対応する値</returns>
        public int this[int x, int y]
        {
            readonly get
            {
                if (x < 0 || x >= 4) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(x));
                if (y < 0 || y >= 4) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(y));
                return Values?[x, y] ?? 0;
            }
            set
            {
                Values ??= new int[4, 4];
                if (x < 0 || x >= 4) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(x));
                if (y < 0 || y >= 4) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(y));
                Values[x, y] = value;
            }
        }

        /// <summary>
        /// クオータニオンを元に回転行列(右手)を取得する
        /// </summary>
        /// <param name="quaternion">使用するクオータニオン</param>
        public static Matrix44I GetQuaternion(Vector4I quaternion)
        {
            var result = Identity;

            var xx = quaternion.X * quaternion.X;
            var yy = quaternion.Y * quaternion.Y;
            var zz = quaternion.Z * quaternion.Z;
            var xy = quaternion.X * quaternion.Y;
            var xz = quaternion.X * quaternion.Z;
            var yz = quaternion.Y * quaternion.Z;
            var wx = quaternion.W * quaternion.X;
            var wy = quaternion.W * quaternion.Y;
            var wz = quaternion.W * quaternion.Z;

            result[0, 0] = 1 - 2 * (yy + zz);
            result[0, 1] = 2 * (xy - wz);
            result[0, 2] = 2 * (xz + wy);
            result[1, 0] = 2 * (xy + wz);
            result[1, 1] = 1 - 2 * (xx + zz);
            result[1, 2] = 2 * (yz - wx);
            result[2, 0] = 2 * (xz - wy);
            result[2, 1] = 2 * (yz + wx);
            result[2, 2] = 1 - 2 * (xx + yy);

            return result;
        }

        /// <summary>
        /// 2D座標の拡大率を表す行列を取得する
        /// </summary>
        /// <param name="scale2D">設定する拡大率</param>
        /// <returns><paramref name="scale2D"/>分の拡大/縮小を表す行列</returns>
        public static Matrix44I GetScale2D(Vector2I scale2D) => GetScale3D(new Vector3I(scale2D.X, scale2D.Y, 1));

        /// <summary>
        /// 3D座標の拡大率を表す行列を取得する
        /// </summary>
        /// <param name="scale3D">設定する拡大率</param>
        /// <returns><paramref name="scale3D"/>分の拡大/縮小を表す行列</returns>
        public static Matrix44I GetScale3D(Vector3I scale3D)
        {
            var result = Identity;
            result[0, 0] = scale3D.X;
            result[1, 1] = scale3D.Y;
            result[2, 2] = scale3D.Z;

            return result;
        }

        /// <summary>
        /// 2D座標の平行移動分を表す行列を取得する
        /// </summary>
        /// <param name="position2D">平行移動する座標</param>
        /// <returns><paramref name="position2D"/>分の平行移動を表す行列</returns>
        public static Matrix44I GetTranslation2D(Vector2I position2D) => GetTranslation3D(new Vector3I(position2D.X, position2D.Y, 0));

        /// <summary>
        /// 3D座標の平行移動分を表す行列を取得する
        /// </summary>
        /// <param name="position3D">平行移動する座標</param>
        /// <returns><paramref name="position3D"/>分の平行移動を表す行列</returns>
        public static Matrix44I GetTranslation3D(Vector3I position3D)
        {
            var result = Identity;

            result[0, 3] = position3D.X;
            result[1, 3] = position3D.Y;
            result[2, 3] = position3D.Z;
            return result;
        }

        /// <summary>
        /// 単位行列に設定する
        /// </summary>
        public void SetIdentity()
        {
            Values ??= new int[4, 4];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Values[i, j] = 0;

            Values[0, 0] = 1;
            Values[1, 1] = 1;
            Values[2, 2] = 1;
            Values[3, 3] = 1;
        }

        /// <summary>
        /// 行列でベクトルを変形させる。
        /// </summary>
        /// <param name="in_">変形前ベクトル</param>
        /// <returns>変形後ベクトル</returns>
        public readonly Vector3I Transform3D(Vector3I in_)
        {
            int[] values = new int[4];

            for (int i = 0; i < 4; i++)
            {
                values[i] = 0;
                values[i] += in_.X * this[i, 0];
                values[i] += in_.Y * this[i, 1];
                values[i] += in_.Z * this[i, 2];
                values[i] += this[i, 3];
            }

            Vector3I o;
            o.X = values[0] / values[3];
            o.Y = values[1] / values[3];
            o.Z = values[2] / values[3];
            return o;
        }

        /// <summary>
        /// 行列でベクトルを変形させる。
        /// </summary>
        /// <param name="in_">変形前ベクトル</param>
        /// <returns>変形後ベクトル</returns>
        public readonly Vector4I Transform4D(Vector4I in_)
        {
            int[] values = new int[4];

            for (int i = 0; i < 4; i++)
            {
                values[i] = 0;
                values[i] += in_.X * this[i, 0];
                values[i] += in_.Y * this[i, 1];
                values[i] += in_.Z * this[i, 2];
                values[i] += in_.W * this[i, 3];
            }

            Vector4I o;
            o.X = values[0];
            o.Y = values[1];
            o.Z = values[2];
            o.W = values[3];

            return o;
        }

        public static Matrix44I operator +(Matrix44I left, Matrix44I right)
        {
            var result = new Matrix44I();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i, j] = left[i, j] + right[i, j];
            return result;
        }

        public static Matrix44I operator -(Matrix44I matrix) => -1 * matrix;

        public static Matrix44I operator -(Matrix44I left, Matrix44I right)
        {
            var result = new Matrix44I();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i, j] = left[i, j] - right[i, j];
            return result;
        }

        public static Matrix44I operator *(Matrix44I matrix, int scalar)
        {
            var result = new Matrix44I();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i, j] = matrix[i, j] * scalar;
            return result;
        }

        public static Matrix44I operator *(int scalar, Matrix44I matrix) => matrix * scalar;

        public static Matrix44I operator /(Matrix44I matrix, int scalar)
        {
            var result = new Matrix44I();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i, j] = matrix[i, j] / scalar;
            return result;
        }

        public static Matrix44I operator *(Matrix44I left, Matrix44I right)
        {
            var o_ = new Matrix44I();
            Mul(ref o_, ref left, ref right);
            return o_;
        }

        public static Vector3I operator *(Matrix44I left, Vector3I right)
        {
            return left.Transform3D(right);
        }

        /// <summary>
        /// 乗算を行う。
        /// </summary>
        /// <param name="o">出力先</param>
        /// <param name="in1">行列1</param>
        /// <param name="in2">行列2</param>
        public static void Mul(ref Matrix44I o, ref Matrix44I in1, ref Matrix44I in2)
        {
            Matrix44I _in1 = in1;
            Matrix44I _in2 = in2;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int v = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        v += _in1[i, k] * _in2[k, j];
                    }
                    o[i, j] = v;
                }
            }
        }

        #region IEquatable
        /// <summary>
        /// 2つの<see cref="Matrix44I"/>間の等価性を判定する
        /// </summary>
        /// <param name="other">等価性を判定する<see cref="Matrix44I"/>のインスタンス</param>
        /// <returns><paramref name="other"/>との間に等価性が認められたらtrue，それ以外でfalse</returns>
        public readonly bool Equals(Matrix44I other)
        {
            if (Values == null && other.Values == null) return true;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (this[i, j] != other[i, j])
                        return false;
            return true;
        }

        /// <summary>
        /// 指定したオブジェクトとの等価性を判定する
        /// </summary>
        /// <param name="obj">等価性を判定するオブジェクト</param>
        /// <returns><paramref name="obj"/>との間の等価性が認められたらtrue，それ以外でfalse</returns>
        public readonly override bool Equals(object obj) => obj is Matrix44I m ? Equals(m) : false;

        /// <summary>
        /// このオブジェクトのハッシュコードを返す
        /// </summary>
        /// <returns>このオブジェクトのハッシュコード</returns>
        public readonly override int GetHashCode()
        {
            var hash = new HashCode();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    hash.Add(this[i, j]);
            return hash.ToHashCode();
        }

        public static bool operator ==(Matrix44I m1, Matrix44I m2) => m1.Equals(m2);
        public static bool operator !=(Matrix44I m1, Matrix44I m2) => !m1.Equals(m2);
        #endregion

        /// <summary>
        /// このインスタンスの複製を作成する
        /// </summary>
        /// <returns>このインスタンスの複製</returns>
        public readonly Matrix44I Clone()
        {
            if (Values == null) return default;
            var clone = new Matrix44I();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    clone[i, j] = this[i, j];
            return clone;
        }
        readonly object ICloneable.Clone() => Clone();
    }
}
