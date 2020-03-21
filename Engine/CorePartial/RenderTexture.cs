﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Altseed
{
    [Serializable]
    public partial class RenderTexture : ICacheKeeper<RenderTexture>
    {
        #region SerializeName
        private const string S_Size = "S_Size";
        #endregion

        private SerializationInfo seInfo;

        /// <summary>
        /// シリアライズされたデータをもとに<see cref="Texture2D"/>のインスタンスを生成する
        /// </summary>
        /// <param name="info">シリアライズされたデータを格納するオブジェクト</param>
        /// <param name="context">送信元の情報</param>
        protected RenderTexture(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            seInfo = info;
        }

        #region ICacheKeeper
        IDictionary<IntPtr, WeakReference<RenderTexture>> ICacheKeeper<RenderTexture>.CacheRepo => cacheRepo;

        IntPtr ICacheKeeper<RenderTexture>.Self { get => selfPtr; set => selfPtr = value; }

        void ICacheKeeper<RenderTexture>.Release(IntPtr native) => cbg_RenderTexture_Release(native);
        #endregion

        /// <summary>
        /// シリアライズするデータを設定します。
        /// </summary>
        /// <param name="info">シリアライズされるデータを格納するオブジェクト</param>
        /// <param name="context">送信先のデータ</param>
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("引数がnullです", nameof(info));

            info.AddValue(S_Size, Size);
        }

        /// <summary>
        /// デシリアライズ時に実行
        /// </summary>
        /// <param name="sender">現在はサポートされていない 常にnullを返す</param>
        protected override void OnDeserialization(object sender)
        {
            if (seInfo == null) return;

            var size = seInfo.GetValue<Vector2I>(S_Size);
            var ptr = cbg_RenderTexture_Create(size);

            if (ptr == IntPtr.Zero) throw new SerializationException("インスタンス生成に失敗しました");

            CacheHelper.CacheHandling(this, ptr);

            seInfo = null;
        }
    }
}
