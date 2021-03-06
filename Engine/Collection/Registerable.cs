﻿using System;

namespace Altseed2
{
    /// <summary>
    /// 登録状況を表します。
    /// </summary>
    [Serializable]
    public enum RegisteredStatus : int
    {
        /// <summary>
        /// 所属なし
        /// </summary>
        Free,

        /// <summary>
        /// 追加待ち
        /// </summary>
        WaitingAdded,

        /// <summary>
        /// 所属有り
        /// </summary>
        Registered,

        /// <summary>
        /// 削除待ち
        /// </summary>
        WaitingRemoved,
    }

    /// <summary>
    /// <see cref="RegisterableCollection{T}"/>に登録や削除が可能な要素であることを表します。
    /// </summary>
    [Serializable]
    public abstract class Registerable<T>
        where T : Registerable<T>
    {
        /// <summary>
        /// <see cref="Registerable{TOwner}"/>の新しいインスタンスを生成します。
        /// </summary>
        protected Registerable() { }

        /// <summary>
        /// 要素が<paramref name="owner"/>に登録されたとき実行します。
        /// </summary>
        /// <param name="owner">新たなオーナー</param>
        internal abstract void Added(T owner);

        /// <summary>
        /// 要素が親要素から削除されたときに実行します。
        /// </summary>
        internal abstract void Removed();

        /// <summary>
        /// 登録状況を取得します。
        /// </summary>
        public virtual RegisteredStatus Status { get; internal set; } = RegisteredStatus.Free;

        /// <summary>
        /// 親要素
        /// </summary>
        [NonSerialized]
        internal T _Parent;

        [NonSerialized]
        internal T _ParentReserved;
    }
}
