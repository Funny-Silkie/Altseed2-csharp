﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Altseed
{
    /// <summary>
    /// Altseed2の中枢を担うクラス
    /// </summary>
    public static class Engine
    {
        /// <summary>
        /// 現在処理している<see cref="Scene"/>を取得する
        /// </summary>
        public static Scene CurrentScene { get; private set; }
        /// <summary>
        /// エンジンを初期化する
        /// </summary>
        /// <param name="title">ウィンドウ左上に表示される文字列</param>
        /// <param name="width">ウィンドウの横幅</param>
        /// <param name="height">ウィンドウの縦幅</param>
        /// <param name="option">オプションのインスタンス</param>
        /// <returns>初期化に成功したらtrue，それ以外でfalse</returns>
        public static bool Initialize(string title, int width, int height, CoreOption option)
        {
            if (Core.Initialize(title, width, height, ref option))
            {
                Keyboard = Keyboard.GetInstance();
                File = File.GetInstance();
                CurrentScene = new Scene();
                return true;
            }
            return false;
        }
        /// <summary>
        /// イベントを実行する
        /// </summary>
        /// <returns>イベントの実行が出来たらtrue，それ以外でfalse</returns>
        public static bool DoEvents()
        {
            return Core.GetInstance().DoEvent();
        }
        /// <summary>
        /// エンジンの終了処理を行う
        /// </summary>
        public static void Terminate()
        {
            Core.Terminate();
        }
        /// <summary>
        /// 更新処理を実行する
        /// </summary>
        public static void Update()
        {
            CurrentScene.Update();
        }
        /// <summary>
        /// シーンを即変更する
        /// </summary>
        /// <param name="scene">変更先のシーン</param>
        /// <exception cref="ArgumentNullException"><paramref name="scene"/>がnull</exception>
        internal static void ChangeScene(Scene scene)
        {
            CurrentScene = scene ?? throw new ArgumentNullException("次のシーンがnullです", nameof(scene));
        }
        /// <summary>
        /// ファイルを管理するクラスを取得する
        /// </summary>
        public static File File { get; private set; }
        /// <summary>
        /// キーボードを管理するクラスを取得する
        /// </summary>
        public static Keyboard Keyboard { get; private set; }
    }
}
