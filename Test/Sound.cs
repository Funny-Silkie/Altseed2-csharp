﻿using System;
using System.Threading;
using System.IO;
using NUnit.Framework;

namespace Altseed2.Test
{
    [TestFixture]
    public class Sound
    {
        [Test, Apartment(ApartmentState.STA)]
        public void Play()
        {
            var tc = new TestCore();
            tc.Init();

            var bgm = Altseed2.Sound.Load(@"../Core/TestData/Sound/bgm1.ogg", false);
            var se = Altseed2.Sound.Load(@"../Core/TestData/Sound/se1.wav", true);

            Assert.NotNull(bgm);
            Assert.NotNull(se);

            var bgm_id = Engine.Sound.Play(bgm);
            var se_id = Engine.Sound.Play(se);

            tc.LoopBody(null, null);

            Engine.Sound.StopAll();

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Loop()
        {
            var tc = new TestCore();
            tc.Init();

            var bgm = Altseed2.Sound.Load(@"../Core/TestData/Sound/bgm1.ogg", false);
            Assert.NotNull(bgm);

            bgm.IsLoopingMode = true;
            bgm.LoopStartingPoint = 1f;
            bgm.LoopEndPoint = 2.5f;

            var bgm_id = Engine.Sound.Play(bgm);

            tc.LoopBody(null, null);

            Engine.Sound.StopAll();

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void GetPosition()
        {
            var tc = new TestCore();
            tc.Init();

            var bgm = Altseed2.Sound.Load(@"../Core/TestData/Sound/bgm1.ogg", false);
            Assert.NotNull(bgm);

            int bgm_id = Engine.Sound.Play(bgm);

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 100);
            Assert.NotNull(font);

            var textNode = new TextNode();
            textNode.Font = font;

            Engine.AddNode(textNode);

            tc.LoopBody(c =>
            {
                textNode.Text = $"{Engine.Sound.GetPlaybackPosition(bgm_id)}";
            }, null);

            Engine.Sound.StopAll();

            tc.End();
        }
    }
}
