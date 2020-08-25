﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Altseed2.Test
{
    [TestFixture]
    class DrawnNode
    {
        [Test, Apartment(ApartmentState.STA)]
        public void SpriteNode()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var node = new SpriteNode();
            node.Texture = texture;
            //node.Src = new RectF(new Vector2F(100, 100), new Vector2F(200, 200));
            //node.Pivot = new Vector2F(0.5f, 0.5f);
            //node.AdjustSize();
            Engine.AddNode(node);

            tc.LoopBody(c =>
            {
                if (Engine.Keyboard.GetKeyState(Key.Right) == ButtonState.Hold) node.Position += new Vector2F(1.5f, 0);
                if (Engine.Keyboard.GetKeyState(Key.Left) == ButtonState.Hold) node.Position -= new Vector2F(1.5f, 0);
                if (Engine.Keyboard.GetKeyState(Key.Down) == ButtonState.Hold) node.Position += new Vector2F(0, 1.5f);
                if (Engine.Keyboard.GetKeyState(Key.Up) == ButtonState.Hold) node.Position -= new Vector2F(0, 1.5f);
                if (Engine.Keyboard.GetKeyState(Key.B) == ButtonState.Hold) node.Scale += new Vector2F(0.01f, 0.01f);
                if (Engine.Keyboard.GetKeyState(Key.S) == ButtonState.Hold) node.Scale -= new Vector2F(0.01f, 0.01f);
                if (Engine.Keyboard.GetKeyState(Key.R) == ButtonState.Hold) node.Angle += 3;
                if (Engine.Keyboard.GetKeyState(Key.L) == ButtonState.Hold) node.Angle -= 3;
                if (Engine.Keyboard.GetKeyState(Key.X) == ButtonState.Hold) node.Src = new RectF(node.Src.X, node.Src.Y, node.Src.Width - 2.0f, node.Src.Height - 2.0f);
                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Hold) node.Src = new RectF(node.Src.X, node.Src.Y, node.Src.Width + 2.0f, node.Src.Height + 2.0f);
                if (Engine.Keyboard.GetKeyState(Key.C) == ButtonState.Hold) node.Src = new RectF(node.Src.X - 2.0f, node.Src.Y - 2.0f, node.Src.Width, node.Src.Height);
                if (Engine.Keyboard.GetKeyState(Key.V) == ButtonState.Hold) node.Src = new RectF(node.Src.X + 2.0f, node.Src.Y + 2.0f, node.Src.Width, node.Src.Height);
            }
            , null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void SpriteNodeWithMaterial()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var node = new SpriteNode();
            node.Texture = texture;
            node.Src = new RectF(new Vector2F(100, 100), new Vector2F(200, 200));
            node.CenterPosition = new Vector2F(100, 100);
            Engine.AddNode(node);

            var node2 = new SpriteNode();
            node2.Texture = texture;
            node2.Src = new RectF(new Vector2F(100, 100), new Vector2F(200, 200));
            node2.CenterPosition = new Vector2F(100, 100);
            node2.Angle = 45;
            node2.ZOrder = 1;

            node2.Material = Material.Create();
            node2.Material.AlphaBlend = AlphaBlend.Multiply;

            var psCode = @"
Texture2D mainTex : register(t0);
SamplerState mainSamp : register(s0);
struct PS_INPUT
{
    float4  Position : SV_POSITION;
    float4  Color    : COLOR0;
    float2  UV1 : UV0;
    float2  UV2 : UV1;
};
float4 main(PS_INPUT input) : SV_TARGET 
{ 
    float4 c;
    c = mainTex.Sample(mainSamp, input.UV1) * input.Color;
    return c;
}";
            node2.Material.SetShader(Shader.Create("ps", psCode, ShaderStage.Pixel));
            node2.Material.SetTexture("mainTex", texture);
            Engine.AddNode(node2);

            tc.LoopBody(null, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TextNode()
        {
            var tc = new TestCore();
            tc.Init();

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 100);
            var font2 = Font.LoadDynamicFont("../Core/TestData/Font/GenYoMinJP-Bold.ttf", 100);
            Assert.NotNull(font);
            Assert.NotNull(font2);
            var imageFont = Font.CreateImageFont(font);
            imageFont.AddImageGlyph('〇', Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png"));

            Engine.AddNode(new TextNode() { Font = font, Text = "Hello, world! こんにちは" });
            Engine.AddNode(new TextNode() { Font = font, Text = "色を指定します。", Position = new Vector2F(0.0f, 100.0f), Color = new Color(0, 0, 255) });
            Engine.AddNode(new TextNode() { Font = font, Text = "太さを指定します。", Position = new Vector2F(0.0f, 200.0f), Weight = 1.0f });
            Engine.AddNode(new TextNode() { Font = font, Text = "太さを指定します。", Position = new Vector2F(0.0f, 200.0f), Weight = 1.0f });
            Engine.AddNode(new TextNode() { Font = font2, Text = "𠀋 𡈽 𡌛 𡑮 𡢽 𠮟 𡚴 𡸴 𣇄 𣗄 𣜿 𣝣 𣳾", Position = new Vector2F(0.0f, 300.0f) });
            Engine.AddNode(new TextNode() { Font = imageFont, Text = "Altseed〇Altseed", Position = new Vector2F(0.0f, 500.0f) });
            var rotated = new TextNode() { Font = font, Text = "太さを指定します。", Position = new Vector2F(400.0f, 400.0f) };
            Engine.AddNode(rotated);

            tc.LoopBody(c =>
            {
                rotated.Angle += 1.0f;
            }
            , null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TextNode2()
        {
            var tc = new TestCore();
            tc.Init();

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 40);
            var font2 = Font.LoadDynamicFont("../Core/TestData/Font/GenYoMinJP-Bold.ttf", 40);
            Assert.NotNull(font);
            Assert.NotNull(font2);
            var imageFont = Font.CreateImageFont(font);
            imageFont.AddImageGlyph('〇', Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png"));

            TextNode node = new TextNode() { Font = font2, Text = "Hello, world! こんにちは カーニングあるよ！！", IsEnableKerning = false, Color = new Color(255, 0, 0, 200) };
            Engine.AddNode(node);
            Engine.AddNode(new TextNode() { Font = font2, Text = "Hello, world! こんにちは カーニングないです", Color = new Color(0, 255, 0, 200) });
            Engine.AddNode(new TextNode() { Font = font, Text = node.ContentSize.ToString(), Position = new Vector2F(0.0f, 50.0f) });
            Engine.AddNode(new TextNode() { Font = font, Text = "字間5です。\n行間標準です。", CharacterSpace = 10, Position = new Vector2F(0.0f, 150.0f) });
            Engine.AddNode(new TextNode() { Font = font, Text = "字間10です。\n行間70です。", CharacterSpace = 50, LineGap = 200, Position = new Vector2F(0.0f, 250.0f) });
            tc.LoopBody(c =>
            {
            }
            , null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void StaticFont()
        {
            var tc = new TestCore();
            tc.Init();

            Assert.IsTrue(Font.GenerateFontFile("../Core/TestData/Font/mplus-1m-regular.ttf", "test.a2f", 100, "Hello, world! こんにちは"));

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 100);
            var font2 = Font.LoadStaticFont("test.a2f");
            Assert.NotNull(font);
            Assert.NotNull(font2);
            var imageFont = Font.CreateImageFont(font);
            imageFont.AddImageGlyph('〇', Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png"));

            Engine.AddNode(new TextNode() { Font = font, Text = "Hello, world! こんにちは" });
            Engine.AddNode(new TextNode() { Font = font2, Text = "Hello, world! こんにちは", Position = new Vector2F(0.0f, 100.0f), Color = new Color(0, 0, 255) });

            tc.LoopBody(c => { }, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Culling()
        {
            var tc = new TestCore();
            tc.Init();

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 30);
            Assert.NotNull(font);

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var text = new TextNode() { Font = font, Text = "", ZOrder = 10 };
            Engine.AddNode(text);

            var parent = new Altseed2.Node();
            Engine.AddNode(parent);

            tc.LoopBody(c =>
            {
                text.Text = "Drawing Object : " + Engine.CullingSystem.DrawingRenderedCount + " FPS: " + Engine.CurrentFPS.ToString();

                var node = new SpriteNode();
                node.Src = new RectF(new Vector2F(100, 100), new Vector2F(200, 200));
                node.Texture = texture;
                node.Position = new Vector2F(200, -500);
                parent.AddChildNode(node);

                foreach (var item in parent.Children.OfType<SpriteNode>())
                {
                    item.Position += new Vector2F(0, 10);
                }

            }, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void MassSpriteNode()
        {
            var tc = new TestCore(new Configuration() { WaitVSync = false });
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink256.png");
            Assert.NotNull(texture);

            var ws = Engine.WindowSize;
            var size = 10;
            for (var x = 0; x < ws.X / size; x++)
            {
                for (var y = 0; y < ws.Y / size; y++)
                {
                    var node = new SpriteNode();
                    node.Texture = texture;
                    node.Src = new RectF(new Vector2F(128 * (x % 2), 128 * (y % 2)), new Vector2F(128, 128));
                    node.Scale = new Vector2F(1, 1) * size / 128f;
                    node.Position = new Vector2F(x, y) * size;
                    Engine.AddNode(node);
                }
            }

            tc.LoopBody(null, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void PolygonNode_SetVertexesByVector2F()
        {
            var tc = new TestCore(new Configuration() { WaitVSync = false });
            tc.Init();

            var node = new PolygonNode();
            Engine.AddNode(node);

            tc.LoopBody(c =>
            {
                var sin = MathF.Sin(MathHelper.DegreeToRadian(c)) * 50;
                var cos = MathF.Cos(MathHelper.DegreeToRadian(c)) * 50;

                Span<Vector2F> span = stackalloc Vector2F[] {
                    new Vector2F(100 + cos, 100 - sin),
                    new Vector2F(100 - sin, 100 - cos),
                    new Vector2F(100 - cos, 100 + sin),
                    new Vector2F(100 + sin, 100 + cos),
                };

                node.SetVertexes(span, new Color(255, c % 255, 255, 255));
            }, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void PolygonNode()
        {
            var tc = new TestCore();
            tc.Init();

            PolygonNode node = new PolygonNode()
            {
                Position = new Vector2F(250, 250)
            };
            Engine.AddNode(node);

            Span<Vector2F> span = stackalloc Vector2F[]
            {
                new Vector2F(-100, -100),
                new Vector2F(100, -100),
                new Vector2F(100, 100),
                new Vector2F(-100, 100),
            };

            node.SetVertexes(span, new Color(255, 0, 0));

            tc.LoopBody(c => { }, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void CenterPosition()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var node = new SpriteNode();
            node.Texture = texture;
            node.CenterPosition = texture.Size / 2;
            node.Position = Engine.WindowSize / 2;
            Engine.AddNode(node);

            var child = new SpriteNode();
            child.Texture = texture;
            child.CenterPosition = texture.Size / 2;
            //child.Position = texture.Size / 2;
            node.AddChildNode(child);

            var child2 = new SpriteNode();
            child2.Texture = texture;
            child2.CenterPosition = texture.Size / 2;
            child2.Position = texture.Size / 2;
            node.AddChildNode(child2);

            tc.LoopBody(c =>
            {
                node.Angle += 1.0f;
                child.Angle += 1.0f;
                child2.Angle += 1.0f;
            }
            , null);

            tc.End();
        }


        [Test, Apartment(ApartmentState.STA)]
        public void Pivot()
        {
            var tc = new TestCore();
            tc.Init();

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 100);
            var font2 = Font.LoadDynamicFont("../Core/TestData/Font/GenYoMinJP-Bold.ttf", 100);
            Assert.NotNull(font);

            var rotated = new AnchorTransformNode<TextNode>()
            {
                Position = new Vector2F(300.0f, 300.0f),
                Pivot = new Vector2F(0.5f, 0.5f),
            };
            rotated.Source.Font = font;
            rotated.Source.Text = "中心で回転します";
            rotated.Size = rotated.ContentSize;
            Engine.AddNode(rotated);

            tc.LoopBody(c =>
            {
                rotated.Angle += 1.0f;
            }
            , null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Anchor()
        {
            var tc = new TestCore(new Configuration() { VisibleTransformInfo = true });
            tc.Init();

            var font = Font.LoadDynamicFont("../Core/TestData/Font/mplus-1m-regular.ttf", 30);
            Assert.NotNull(font);

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            Vector2F rectSize = texture.Size;
            var parent = new AnchorTransformNode<SpriteNode>();
            parent.Source.Texture = texture;
            parent.Position = Engine.WindowSize / 2;
            parent.ZOrder = 5;
            parent.Size = rectSize;
            parent.AnchorMode = AnchorMode.Fill;
            Engine.AddNode(parent);

            var child = new AnchorTransformNode<SpriteNode>();
            child.Source.Texture = texture;
            child.Source.Color = new Color(255, 0, 0, 200);
            child.Position = rectSize / 2;
            child.Pivot = new Vector2F(0.5f, 0.5f);
            child.AnchorMin = new Vector2F(0.0f, 0.0f);
            child.AnchorMax = new Vector2F(1f, 1f);
            child.HorizontalAlignment = HorizontalAlignment.Center;
            child.VerticalAlignment = VerticalAlignment.Center;
            child.ZOrder = 10;
            child.Size = child.ContentSize;
            child.AnchorMode = AnchorMode.KeepAspect;
            parent.AddChildNode(child);

            var childText = new AnchorTransformNode<TextNode>();
            childText.Source.Font = font;
            childText.Source.Color = new Color(0, 0, 0);
            childText.Source.Text = "あいうえお";
            childText.Pivot = new Vector2F(0.5f, 0.5f);
            childText.AnchorMin = new Vector2F(0.5f, 0.5f);
            childText.AnchorMax = new Vector2F(0.5f, 0.5f);
            childText.ZOrder = 15;
            //childText.HorizontalAlignment = HorizontalAlignment.Center;
            //childText.VerticalAlignment = VerticalAlignment.Center;
            childText.Size = childText.ContentSize;
            childText.AnchorMode = AnchorMode.ContentSize;
            child.AddChildNode(childText);

            var text = new TextNode()
            {
                Font = font,
                Text = "",
                ZOrder = 10,
                Scale = new Vector2F(0.8f, 0.8f),
                Color = new Color(255, 128, 0)
            };
            Engine.AddNode(text);

            tc.Duration = 10000;

            string infoText(IAnchorTransform n) =>
                    $"Scale:{n.Scale}\n" +
                    $"Position:{n.Position}\n" +
                    $"Pivot:{n.Pivot}\n" +
                    $"Size:{n.Size}\n" +
                    $"Margin: LT:{n.LeftTop} RB:{n.RightBottom}\n" +
                    $"Anchor: {n.AnchorMin} {n.AnchorMax}\n";

            tc.LoopBody(c =>
            {
                if (Engine.Keyboard.GetKeyState(Key.Right) == ButtonState.Hold) rectSize.X += 1.5f;
                if (Engine.Keyboard.GetKeyState(Key.Left) == ButtonState.Hold) rectSize.X -= 1.5f;
                if (Engine.Keyboard.GetKeyState(Key.Down) == ButtonState.Hold) rectSize.Y += 1.5f;
                if (Engine.Keyboard.GetKeyState(Key.Up) == ButtonState.Hold) rectSize.Y -= 1.5f;

                if (Engine.Keyboard.GetKeyState(Key.D) == ButtonState.Hold) parent.Position += new Vector2F(1.5f, 0);
                if (Engine.Keyboard.GetKeyState(Key.A) == ButtonState.Hold) parent.Position += new Vector2F(-1.5f, 0);
                if (Engine.Keyboard.GetKeyState(Key.S) == ButtonState.Hold) parent.Position += new Vector2F(0, 1.5f);
                if (Engine.Keyboard.GetKeyState(Key.W) == ButtonState.Hold) parent.Position += new Vector2F(0, -1.5f);

                if (Engine.Keyboard.GetKeyState(Key.Q) == ButtonState.Hold) child.Angle += 1.5f;
                if (Engine.Keyboard.GetKeyState(Key.E) == ButtonState.Hold) child.Angle -= 1.5f;

                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Hold) parent.Scale += new Vector2F(0.1f, 0);
                if (Engine.Keyboard.GetKeyState(Key.C) == ButtonState.Hold) parent.Scale -= new Vector2F(0.1f, 0);

                parent.Size = rectSize;

                text.Text = infoText(parent) + '\n' + infoText(child) + '\n' + infoText(childText);
            }, null);

            tc.End();
        }


        [Test, Apartment(ApartmentState.STA)]
        public void IsDrawn()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var node = new SpriteNode();
            node.Texture = texture;
            node.CenterPosition = texture.Size / 2;
            node.Position = new Vector2F(100, 100);
            Engine.AddNode(node);

            var node2 = new SpriteNode();
            node2.Texture = texture;
            node2.CenterPosition = texture.Size / 2;
            node2.Position = new Vector2F(200, 200);

            var node3 = new SpriteNode();
            node3.Texture = texture;
            node3.CenterPosition = texture.Size / 2;
            node3.Position = new Vector2F(300, 300);

            tc.LoopBody(c =>
            {
                if (c == 2)
                {
                    node.AddChildNode(node2);
                }

                if (c == 4)
                {
                    node.IsDrawn = false;
                    Assert.IsFalse(node.IsDrawnActually);
                    Assert.IsFalse(node2.IsDrawnActually);
                }

                if (c == 6)
                {
                    node2.AddChildNode(node3);
                }

                if (c == 8)
                {
                    node.IsDrawn = true;

                    Assert.IsTrue(node.IsDrawnActually);
                    Assert.IsTrue(node2.IsDrawnActually);
                    Assert.IsTrue(node3.IsDrawnActually);
                }

                if (c == 10)
                {
                    node2.IsDrawn = false;

                    Assert.IsTrue(node.IsDrawnActually);
                    Assert.IsFalse(node2.IsDrawnActually);
                    Assert.IsFalse(node3.IsDrawnActually);
                }
            }, null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void ZOrder()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var node = new SpriteNode();
            node.Texture = texture;
            node.CenterPosition = texture.Size / 2;
            node.Position = new Vector2F(100, 100);
            node.Color = new Color(255, 0, 0);
            node.ZOrder = 300;
            Engine.AddNode(node);

            var node2 = new SpriteNode();
            node2.Texture = texture;
            node2.CenterPosition = texture.Size / 2;
            node2.Position = new Vector2F(200, 200);
            node2.ZOrder = 200;
            node2.Color = new Color(0, 0, 255);
            Engine.AddNode(node2);

            var node3 = new SpriteNode();
            node3.Texture = texture;
            node3.CenterPosition = texture.Size / 2;
            node3.Position = new Vector2F(300, 300);
            node3.ZOrder = 150;
            node3.Color = new Color(0, 255, 0);
            Engine.AddNode(node3);

            tc.LoopBody(c =>
            {
                node3.ZOrder++;
            }, null);

            tc.End();
        }
    }
}
