﻿using System;
using System.IO;
using System.Runtime.Serialization;

namespace Altseed2
{
    public sealed partial class Sound
    {
        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info)
        {
            Sound_Unsetter_Deserialize(info, out var path, out var isDecompressed);
            ptr = cbg_Sound_Load(path, isDecompressed);
        }

        /// <summary>
        /// 指定パスから音源を読み込みます。失敗した場合、例外が発生します。
        /// </summary>
        /// <param name="path">読み込む音源のパス</param>
        /// <param name="isDecompressed">音をロード時に全て解凍しておくかどうか</param>
        /// <exception cref="ArgumentException"><paramref name="path"/>が空白文字のみからなる、または使用出来ない文字を含んでいる</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/>がnull</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/>で指定された音源が見つからない</exception>
        /// <exception cref="PathTooLongException"><paramref name="path"/>が指定するパスが見つからない</exception>
        /// <exception cref="SystemException">音源が破損または読み込みに失敗</exception>
        /// <returns><paramref name="path"/>をパスに持つ音源のデータを格納した<see cref="Sound"/>の新しいインスタンス</returns>
        public static Sound LoadStrict(string path, bool isDecompressed)
        {
            var ex = IOHelper.CheckLoadPath(path);
            if (ex != null) throw ex;
            var result = Load(path, isDecompressed) ?? throw new SystemException("ファイルが破損しているか読み込みに失敗しました");
            return result;
        }
    }

    public partial class SoundMixer
    {
        private FloatArray _InternalArray;

        /// <summary>
        /// 再生中の音のスペクトル情報を取得します。
        /// </summary>
        /// <param name="id">音のID</param>
        /// <param name="dataNum">音のスペクトル情報を格納するための配列の容量</param>
        /// <param name="window">フーリエ変換に用いる窓関数</param>
        public float[] GetSpectrum(int id, int dataNum, FFTWindow window)
        {
            if ((dataNum & (dataNum - 1)) != 0) return null;
            var array = new float[dataNum];

            GetSpectrum(id, dataNum, window, array);
            return array;
        }

        /// <summary>
        /// 再生中の音のスペクトル情報を取得します。
        /// </summary>
        /// <param name="id">音のID</param>
        /// <param name="dataNum">音のスペクトル情報を格納するための配列の容量</param>
        /// <param name="window">フーリエ変換に用いる窓関数</param>
        /// <param name="span">結果を書き込むSpan</param>
        /// <exception cref="ArgumentException"><paramref name="span"/>の長さが<paramref name="dataNum"/>未満。</exception>
        public void GetSpectrum(int id, int dataNum, FFTWindow window, Span<float> span)
        {
            if (dataNum > span.Length) throw new ArgumentException(nameof(span), $"Spanの長さが{dataNum}未満です。");
            if ((dataNum & (dataNum - 1)) != 0) return;
            if (_InternalArray is null)
            {
                _InternalArray = FloatArray.Create(dataNum);
            }
            else
            {
                _InternalArray.Resize(dataNum);
            }
            GetSpectrum(id, _InternalArray, window);
            _InternalArray.CopyTo(span);
        }
    }
}
