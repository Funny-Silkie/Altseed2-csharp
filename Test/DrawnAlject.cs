﻿using System.Threading;
using NUnit.Framework;

namespace Altseed.Test
{
    [TestFixture]
    public class DrawnAlject
    {
        [Test, Apartment(ApartmentState.STA)]
        public void TextureAlject()
        {
            Assert.True(Engine.Initialize("TextureAlject", 960, 720));

            var texture = Texture2D.Load(@"../../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var obj = new TextureAlject()
            {
                Texture = texture
            };

            Engine.CurrentScene.AddObject(obj);

            while (Engine.DoEvents())
            {
                Assert.True(Engine.Graphics.BeginFrame());

                Engine.Update();

                var cmdList = Engine.Graphics.CommandList;
                cmdList.SetRenderTargetWithScreen();
                Engine.Renderer.Render(cmdList);

                if (Engine.Keyboard.GetKeyState(Keys.Right) == ButtonState.Hold) obj.Position += new Vector2F(1.5f, 0);
                if (Engine.Keyboard.GetKeyState(Keys.Left) == ButtonState.Hold) obj.Position -= new Vector2F(1, 0);
                if (Engine.Keyboard.GetKeyState(Keys.Down) == ButtonState.Hold) obj.Position += new Vector2F(0, 1);
                if (Engine.Keyboard.GetKeyState(Keys.Up) == ButtonState.Hold) obj.Position -= new Vector2F(0, 1);
                if (Engine.Keyboard.GetKeyState(Keys.B) == ButtonState.Hold) obj.Scale += new Vector2F(0.01f, 0.01f);
                if (Engine.Keyboard.GetKeyState(Keys.S) == ButtonState.Hold) obj.Scale -= new Vector2F(0.01f, 0.01f);
                if (Engine.Keyboard.GetKeyState(Keys.R) == ButtonState.Hold) obj.Angle += 3;
                if (Engine.Keyboard.GetKeyState(Keys.L) == ButtonState.Hold) obj.Angle -= 3;
                if (Engine.Keyboard.GetKeyState(Keys.X) == ButtonState.Hold) obj.Src = new RectF(obj.Src.X, obj.Src.Y, obj.Src.Width - 2.0f, obj.Src.Height - 2.0f);
                if (Engine.Keyboard.GetKeyState(Keys.Z) == ButtonState.Hold) obj.Src = new RectF(obj.Src.X, obj.Src.Y, obj.Src.Width + 2.0f, obj.Src.Height + 2.0f);

                Assert.True(Engine.Graphics.EndFrame());

                if (Engine.Keyboard.GetKeyState(Keys.Escape) == ButtonState.Push) break;
            }

            Engine.Terminate();
        }
    }
}