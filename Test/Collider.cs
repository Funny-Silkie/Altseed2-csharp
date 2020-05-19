﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Altseed.Test
{
    [TestFixture]
    class Collider
    {
        [Test, Apartment(ApartmentState.STA)]
        public void SpriteNode()
        {
            var tc = new TestCore();
            tc.Duration = 2000;
            tc.Init();

            var texture = Texture2D.Load(@"../../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var node = new SpriteNode();
            //node.Src = new RectF(new Vector2F(100, 100), new Vector2F(200, 200));
            node.Texture = texture;
            node.AdjustSize();
            node.Position = new Vector2F(200, 200);
            node.CenterPosition = texture.Size / 2;
            node.Scale = new Vector2F(0.2f, 0.2f);
            Engine.AddNode(node);

            var col = new CircleCollider();
            col.Radius = 200 * 0.2f;
            col.Position = node.Position;

            var node2 = new SpriteNode();
            //node.Src = new RectF(new Vector2F(100, 100), new Vector2F(200, 200));
            node2.Texture = texture;
            node2.AdjustSize();
            node2.Position = new Vector2F(200, 200);
            node2.CenterPosition = texture.Size / 2;
            node2.Scale = new Vector2F(0.2f, 0.2f);
            Engine.AddNode(node2);
            var col2 = new CircleCollider();
            col2.Radius = 200 * 0.2f;
            col2.Position = node.Position;

            tc.LoopBody(c =>
            {
                node2.Position = col2.Position = Engine.Mouse.Position;
                if (col.GetIsCollidedWith(col2))
                {
                    node2.Angle++;
                }
            }
            , null);

            tc.End();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void AutoCollisionSystem()
        {
            var tc = new TestCore()
            {
                Duration = int.MaxValue
            };
            tc.Init();

            var texture = Texture2D.Load(@"../../Core/TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var scene = new Altseed.Node();
            var manager = new CollisionManagerNode();
            scene.AddChildNode(manager);

            Engine.AddNode(scene);

            var player = new Player(texture);

            scene.AddChildNode(player);

            var comparison = new SpriteNode()
            {
                Texture = texture,
                Position = new Vector2F(300f, 300f)
            };
            var colliderNode = new CircleColliderNode()
            {
                Radius = texture.Size.X / 2,
                Position = comparison.Position
            };
            comparison.AddChildNode(colliderNode);

            scene.AddChildNode(comparison);

            tc.LoopBody(null, x =>
            {
                if (Engine.Keyboard.GetKeyState(Keys.Escape) == ButtonState.Push) tc.Duration = 0;
                if (x == 10)
                {
                    Assert.True(manager.ContainsCollider(colliderNode));
                    Assert.AreEqual(manager.ColliderCount, 2);
                }
            });
            tc.End();
        }
        private sealed class Player : SpriteNode, ICollisionEventReceiver
        {
            private readonly ColliderNode node;
            public Player(Texture2D texture)
            {
                Texture = texture;
                node = new CircleColliderNode()
                {
                    Radius = texture.Size.X / 2
                };
                AddChildNode(node);
            }
            protected override void OnUpdate()
            {
                node.Collider.Position = Position;
                if (Engine.Keyboard.GetKeyState(Keys.Up) == ButtonState.Hold) Position += new Vector2F(0.0f, -2.0f);
                if (Engine.Keyboard.GetKeyState(Keys.Down) == ButtonState.Hold) Position += new Vector2F(0.0f, 2.0f);
                if (Engine.Keyboard.GetKeyState(Keys.Left) == ButtonState.Hold) Position += new Vector2F(-2.0f, 0.0f);
                if (Engine.Keyboard.GetKeyState(Keys.Right) == ButtonState.Hold) Position += new Vector2F(2.0f, 0.0f);
            }
            void ICollisionEventReceiver.OnCollisionEnter(CollisionInfo info)
            {
                Debug.WriteLine("Enter");
            }
            void ICollisionEventReceiver.OnCollisionExit(CollisionInfo info)
            {
                Debug.WriteLine("Exit");
            }
            void ICollisionEventReceiver.OnCollisionStay(CollisionInfo info)
            {
                Debug.WriteLine("Stay");
            }
        }
    }
}