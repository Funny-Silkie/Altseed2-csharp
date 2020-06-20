﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Altseed
{
    public partial class Font
    {
        #region SerializeName
        private const string S_Textures = "S_Textures";
        #endregion

        private Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info)
        {
            Font_Unsetter_Deserialize(info, out var size, out var isStatic, out var path);
            ptr = isStatic ? cbg_Font_LoadStaticFont(path) : cbg_Font_LoadDynamicFont(path, size);
        }

        partial void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(S_Textures, textures);  
        }

        partial void OnDeserialize_Method(object sender)
        {
            textures = seInfo.GetValue<Dictionary<int, Texture2D>>(S_Textures);
            foreach (var t in textures) AddImageGlyph(t.Key, t.Value);            
        }

        /// <summary>
        /// 動的にフォントを読み込む
        /// </summary>
        /// <param name="path">フォントファイルのパス</param>
        /// <param name="size">フォントサイズ</param>
        /// <returns>動的に生成されるフォント</returns>
        /// <exception cref="ArgumentException"><paramref name="path"/>が空白文字のみからなる又は使用できない文字を含む</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/>がnull</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/>が0以下</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/>で指定したファイルが見つからない</exception>
        /// <exception cref="PathTooLongException"><paramref name="path"/>が長すぎる</exception>
        /// <exception cref="SystemException">ファイルが破損していたまたは読み込みに失敗した</exception>
        public static Font LoadDynamicFontStrict(string path, int size)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), $"サイズは正の値にしてください\n実際の値：{size}");
            var ex = IOHelper.CheckLoadPath(path);
            if (ex != null) throw ex;
            return LoadDynamicFont(path, size) ?? throw new SystemException("ファイルが破損しているか読み込みに失敗しました");
        }

        /// <summary>
        /// 静的にフォントを読み込む
        /// </summary>
        /// <param name="path">フォントファイルのパス</param>
        /// <returns>静的に生成されるフォント</returns>
        /// <exception cref="ArgumentException"><paramref name="path"/>が空白文字のみからなる又は使用できない文字を含む</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/>がnull</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/>で指定したファイルが見つからない</exception>
        /// <exception cref="PathTooLongException"><paramref name="path"/>が長すぎる</exception>
        /// <exception cref="SystemException">ファイルが破損していたまたは読み込みに失敗した</exception>
        public static Font LoadStaticFontStrict(string path)
        {
            var ex = IOHelper.CheckLoadPath(path);
            if (ex != null) throw ex;
            return LoadStaticFont(path) ?? throw new SystemException("ファイルが破損しているか読み込みに失敗しました");
        }

        /// <summary>
        /// テクスチャ文字を追加する
        /// </summary>
        /// <param name="character">文字</param>
        /// <param name="texture">テクスチャ</param>
        public void AddImageGlyph(char character, Texture2D texture)
        {
            textures[character] = texture;
            AddImageGlyph((int)character, texture);
        }
    }
}
