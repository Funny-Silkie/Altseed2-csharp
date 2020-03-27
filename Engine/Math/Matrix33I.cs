﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Altseed
{
    /// <summary>
    /// <see cref="int"/>型の3x3行列を表す構造体
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Matrix33I : ICloneable, IEquatable<Matrix33I>, ISerializable
    {
        private const string S_Array = "S_Array";

        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 3 * 3)]
        private fixed int Values[9];

        /// <summary>
        /// 単位行列を取得する
        /// </summary>
        public static Matrix33I Identity
        {
            get
            {
                var result = new Matrix33I();
                for (int i = 0; i < 9; i++) result.Values[i] = 0;

                result.Values[0 * 3 + 0] = 1;
                result.Values[1 * 3 + 1] = 1;
                result.Values[2 * 3 + 2] = 1;

                return result;
            }
        }

        /// <summary>
        /// 転置行列を取得する
        /// </summary>
        public readonly Matrix33I TransPosition
        {
            get
            {
                var result = new Matrix33I();
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        result[i, j] = this[j, i];
                return result;
            }
        }

        private Matrix33I(SerializationInfo info, StreamingContext context)
        {
            var array = info.GetValue<int[]>(S_Array) ?? throw new SerializationException("デシリアライズに失敗しました");
            for (int i = 0; i < 9; i++) Values[i] = array[i];
        }

        /// <summary>
        /// 指定した位置の値を取得または設定する
        /// </summary>
        /// <param name="x">取得する要素の位置</param>
        /// <param name="y">取得する要素の位置</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/>または<paramref name="y"/>が0未満または3以上</exception>
        /// <returns><paramref name="x"/>と<paramref name="y"/>に対応する値</returns>
        public int this[int x, int y]
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
        /// 2D座標の拡大率を表す行列を取得する
        /// </summary>
        /// <param name="scale">設定する拡大率</param>
        /// <returns><paramref name="scale"/>分の拡大/縮小を表す行列</returns>
        public static Matrix33I GetScale(Vector2I scale)
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
        public static Matrix33I GetTranslation(Vector2I position)
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
        public readonly Vector2I Transform2D(Vector2I in_)
        {
            var values = new int[3];

            for (int i = 0; i < 2; i++)
            {
                values[i] = 0;
                values[i] += in_.X * this[i, 0];
                values[i] += in_.Y * this[i, 1];
                values[i] += 1 * this[i, 2];
            }

            return new Vector2I(values[0], values[1]);
        }

        /// <summary>
        /// 行列でベクトルを変形させる。
        /// </summary>
        /// <param name="in_">変形前ベクトル</param>
        /// <returns>変形後ベクトル</returns>
        public readonly Vector3I Transform3D(Vector3I in_)
        {
            var values = new int[3];

            for (int i = 0; i < 3; i++)
            {
                values[i] = 0;
                values[i] += in_.X * this[i, 0];
                values[i] += in_.Y * this[i, 1];
                values[i] += in_.Z * this[i, 2];
            }

            return new Vector3I(values[0], values[1], values[2]);
        }

        public static Matrix33I operator +(Matrix33I left, Matrix33I right)
        {
            var result = new Matrix33I();
            for (int i = 0; i < 9; i++) result.Values[i] = left.Values[i] + right.Values[i];
            return result;
        }

        public static Matrix33I operator -(Matrix33I matrix) => -1 * matrix;

        public static Matrix33I operator -(Matrix33I left, Matrix33I right)
        {
            var result = new Matrix33I();
            for (int i = 0; i < 9; i++) result.Values[i] = left.Values[i] - right.Values[i];
            return result;
        }

        public static Matrix33I operator *(Matrix33I matrix, int scalar)
        {
            var result = new Matrix33I();
            for (int i = 0; i < 9; i++) result.Values[i] = matrix.Values[i] * scalar;
            return result;
        }

        public static Matrix33I operator *(int scalar, Matrix33I matrix) => matrix * scalar;

        public static Matrix33I operator /(Matrix33I matrix, int scalar)
        {
            var result = new Matrix33I();
            for (int i = 0; i < 9; i++) result.Values[i] = matrix.Values[i] / scalar;
            return result;
        }

        public static Matrix33I operator *(Matrix33I left, Matrix33I right)
        {
            var result = new Matrix33I();

            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    for (int k = 0; k < 3; ++k)
                        result[i, j] += left[i, k] * right[k, j];

            return result;
        }

        public static Vector3I operator *(Matrix33I left, Vector3I right) => left.Transform3D(right);

        #region IEquatable
        /// <summary>
        /// 2つの<see cref="Matrix33I"/>間の等価性を判定する
        /// </summary>
        /// <param name="other">等価性を判定する<see cref="Matrix33I"/>のインスタンス</param>
        /// <returns><paramref name="other"/>との間に等価性が認められたらtrue，それ以外でfalse</returns>
        public readonly bool Equals(Matrix33I other)
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
        public readonly override bool Equals(object obj) => obj is Matrix33I m ? Equals(m) : false;

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

        public static bool operator ==(Matrix33I m1, Matrix33I m2) => m1.Equals(m2);
        public static bool operator !=(Matrix33I m1, Matrix33I m2) => !m1.Equals(m2);
        #endregion

        /// <summary>
        /// このインスタンスの複製を作成する
        /// </summary>
        /// <returns>このインスタンスの複製</returns>
        public readonly Matrix33I Clone()
        {
            var clone = new Matrix33I();
            for (int i = 0; i < 9; i++) clone.Values[i] = Values[i];
            return clone;
        }
        readonly object ICloneable.Clone() => Clone();

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("引数がnullです", nameof(info));
            var array = new int[9];
            for (int i = 0; i < 9; i++) array[i] = Values[i];
            info.AddValue(S_Array, array);
        }
    }
}
