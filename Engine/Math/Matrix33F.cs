﻿using System;
using System.Runtime.InteropServices;

namespace Altseed
{
    /// <summary>
    /// <see cref="float"/>型の3x3行列を表す構造体
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Matrix33F : ICloneable, IEquatable<Matrix33F>
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3 * 3)]
        private fixed float Values[9];

        /// <summary>
        /// 単位行列を取得する
        /// </summary>
        public static Matrix33F Identity
        {
            get
            {
                var result = new Matrix33F();
                for (int i = 0; i < 9; i++) result.Values[i] = 0.0f;

                result.Values[0 * 3 + 0] = 1.0f;
                result.Values[1 * 3 + 1] = 1.0f;
                result.Values[2 * 3 + 2] = 1.0f;

                return result;
            }
        }

        /// <summary>
        /// 逆行列を取得する
        /// </summary>
        public readonly Matrix33F Inversion
        {
            get
            {
                var result = this;

                {
                    float e = 0.00001f;

                    float a11 = result.Values[0 * 3 + 0];
                    float a12 = result.Values[0 * 3 + 1];
                    float a13 = result.Values[0 * 3 + 2];
                    float a21 = result.Values[1 * 3 + 0];
                    float a22 = result.Values[1 * 3 + 1];
                    float a23 = result.Values[1 * 3 + 2];
                    float a31 = result.Values[2 * 3 + 0];
                    float a32 = result.Values[2 * 3 + 1];
                    float a33 = result.Values[2 * 3 + 2];

                    /* 行列式の計算 */
                    float b11 = +a22 * a33 - a23 * a32;
                    float b12 = +a13 * a32 - a12 * a33;
                    float b13 = +a12 * a23 - a13 * a22;

                    float b21 = +a23 * a31 - a21 * a33;
                    float b22 = +a11 * a33 - a13 * a31;
                    float b23 = +a13 * a21 - a11 * a23;

                    float b31 = +a21 * a32 - a22 * a31;
                    float b32 = +a12 * a31 - a11 * a32;
                    float b33 = +a11 * a22 - a12 * a21;

                    // 行列式の逆数をかける
                    float Det = a11 * a22 * a33 + a21 * a32 * a13 + a31 * a12 * a23 - a11 * a32 * a23 - a31 * a22 * a13 - a21 * a12 * a33;
                    if ((-e <= Det) && (Det <= +e)) throw new InvalidOperationException("逆行列が存在しません。");

                    float InvDet = 1.0f / Det;

                    result.Values[0 * 3 + 0] = b11 * InvDet;
                    result.Values[0 * 3 + 1] = b12 * InvDet;
                    result.Values[0 * 3 + 2] = b13 * InvDet;
                    result.Values[1 * 3 + 0] = b21 * InvDet;
                    result.Values[1 * 3 + 1] = b22 * InvDet;
                    result.Values[1 * 3 + 2] = b23 * InvDet;
                    result.Values[2 * 3 + 0] = b31 * InvDet;
                    result.Values[2 * 3 + 1] = b32 * InvDet;
                    result.Values[2 * 3 + 2] = b33 * InvDet;
                }
                return result;
            }
        }

        /// <summary>
        /// 転置行列を取得する
        /// </summary>
        public readonly Matrix33F TransPosition
        {
            get
            {
                var result = new Matrix33F();
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        result[i, j] = this[j, i];
                return result;
            }
        }

        /// <summary>
        /// 指定した位置の値を取得または設定する
        /// </summary>
        /// <param name="x">取得する要素の位置</param>
        /// <param name="y">取得する要素の位置</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/>または<paramref name="y"/>が0未満または3以上</exception>
        /// <returns><paramref name="x"/>と<paramref name="y"/>に対応する値</returns>
        public float this[int x, int y]
        {
            readonly get
            {
                if (x < 0 || x > 3) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(x));
                if (y < 0 || y > 3) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(y));
                return Values[x * 3 + y];
            }
            set
            {
                if (x < 0 || x > 3) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(x));
                if (y < 0 || y > 3) throw new ArgumentOutOfRangeException("引数の値は0-3に収めてください", nameof(y));
                Values[x * 3 + y] = value;
            }
        }

        /// <summary>
        /// 指定した角度分の回転を表す行列を取得する
        /// </summary>
        /// <param name="radian">回転させる角度(弧度法)</param>
        /// <returns><paramref name="radian"/>の回転分を表す行列</returns>
        public static Matrix33F GetRotation(float radian)
        {
            var sin = (float)Math.Sin(radian);
            var cos = (float)Math.Cos(radian);

            var result = Identity;
            result[0, 0] = cos;
            result[0, 1] = -sin;
            result[1, 0] = sin;
            result[1, 1] = cos;

            return result;
        }

        /// <summary>
        /// 2D座標の拡大率を表す行列を取得する
        /// </summary>
        /// <param name="scale">設定する拡大率</param>
        /// <returns><paramref name="scale"/>分の拡大/縮小を表す行列</returns>
        public static Matrix33F GetScale(Vector2F scale)
        {
            var result = Identity;
            result[0, 0] = scale.X;
            result[1, 1] = scale.Y;

            return result;
        }

        /// <summary>
        /// 2D座標の平行移動分を表す行列を取得する
        /// </summary>
        /// <param name="position">平行移動する座標</param>
        /// <returns><paramref name="position"/>分の平行移動を表す行列</returns>
        public static Matrix33F GetTranslation(Vector2F position)
        {
            var result = Identity;

            result[0, 2] = position.X;
            result[1, 2] = position.Y;
            return result;
        }

        /// <summary>
        /// 行列でベクトルを変形させる。
        /// </summary>
        /// <param name="in_">変形前ベクトル</param>
        /// <returns>変形後ベクトル</returns>
        public readonly Vector2F Transform2D(Vector2F in_)
        {
            float[] values = new float[3];

            for (int i = 0; i < 2; i++)
            {
                values[i] = 0;
                values[i] += in_.X * this[i, 0];
                values[i] += in_.Y * this[i, 1];
                values[i] += 1.0f * this[i, 2];
            }

            Vector2F o;
            o.X = values[0];
            o.Y = values[1];
            return o;
        }

        /// <summary>
        /// 行列でベクトルを変形させる。
        /// </summary>
        /// <param name="in_">変形前ベクトル</param>
        /// <returns>変形後ベクトル</returns>
        public readonly Vector3F Transform3D(Vector3F in_)
        {
            float[] values = new float[3];

            for (int i = 0; i < 3; i++)
            {
                values[i] = 0;
                values[i] += in_.X * this[i, 0];
                values[i] += in_.Y * this[i, 1];
                values[i] += in_.Z * this[i, 2];
            }

            Vector3F o;
            o.X = values[0];
            o.Y = values[1];
            o.Z = values[2];
            return o;
        }

        public static Matrix33F operator +(Matrix33F left, Matrix33F right)
        {
            var result = new Matrix33F();
            for (int i = 0; i < 9; i++) result.Values[i] = left.Values[i] + right.Values[i];
            return result;
        }

        public static Matrix33F operator -(Matrix33F matrix) => -1 * matrix;

        public static Matrix33F operator -(Matrix33F left, Matrix33F right)
        {
            var result = new Matrix33F();
            for (int i = 0; i < 9; i++) result.Values[i] = left.Values[i] - right.Values[i];
            return result;
        }

        public static Matrix33F operator *(Matrix33F matrix, float scalar)
        {
            var result = new Matrix33F();
            for (int i = 0; i < 9; i++) result.Values[i] = matrix.Values[i] * scalar;
            return result;
        }

        public static Matrix33F operator *(float scalar, Matrix33F matrix) => matrix * scalar;

        public static Matrix33F operator /(Matrix33F matrix, float scalar)
        {
            var result = new Matrix33F();
            for (int i = 0; i < 9; i++) result.Values[i] = matrix.Values[i] / scalar;
            return result;
        }

        public static Matrix33F operator *(Matrix33F left, Matrix33F right)
        {
            var result = new Matrix33F();

            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < 3; ++k) result[i, j] += left[i, k] * right[k, j];
                }

            return result;
        }

        public static Vector3F operator *(Matrix33F left, Vector3F right)
        {
            float[] elements = { 0, 0, 0 };
            float[] rop = { right.X, right.Y, right.Z };

            for (int i = 0; i < 3; ++i)
                for (int k = 0; k < 3; ++k)
                    elements[i] += left[i, k] * rop[k];
                
            return new Vector3F(elements[0], elements[1], elements[2]);
        }

        #region IEquatable
        /// <summary>
        /// 2つの<see cref="Matrix33F"/>間の等価性を判定する
        /// </summary>
        /// <param name="other">等価性を判定する<see cref="Matrix33F"/>のインスタンス</param>
        /// <returns><paramref name="other"/>との間に等価性が認められたらtrue，それ以外でfalse</returns>
        public readonly bool Equals(Matrix33F other)
        {
            for (int i = 0; i < 9; i++)
                if (Values[i] != other.Values[i])
                    return false;
            return true;
        }

        /// <summary>
        /// 指定したオブジェクトとの等価性を判定する
        /// </summary>
        /// <param name="obj">等価性を判定するオブジェクト</param>
        /// <returns><paramref name="obj"/>との間の等価性が認められたらtrue，それ以外でfalse</returns>
        public readonly override bool Equals(object obj) => obj is Matrix33F m ? Equals(m) : false;

        /// <summary>
        /// このオブジェクトのハッシュコードを返す
        /// </summary>
        /// <returns>このオブジェクトのハッシュコード</returns>
        public readonly override int GetHashCode()
        {
            var hash = new HashCode();
            for (int i = 0; i < 9; i++) hash.Add(Values[i]);
            return hash.ToHashCode();
        }

        public static bool operator ==(Matrix33F m1, Matrix33F m2) => m1.Equals(m2);
        public static bool operator !=(Matrix33F m1, Matrix33F m2) => !m1.Equals(m2);
        #endregion

        /// <summary>
        /// このインスタンスの複製を作成する
        /// </summary>
        /// <returns>このインスタンスの複製</returns>
        public readonly Matrix33F Clone()
        {
            var clone = new Matrix33F();
            for (int i = 0; i < 9; i++) clone.Values[i] = Values[i];
            return clone;
        }
        readonly object ICloneable.Clone() => Clone();
    }
}
