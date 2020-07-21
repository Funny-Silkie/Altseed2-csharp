using System.Threading;
using NUnit.Framework;

namespace Altseed2.Test
{
    [TestFixture]
    class Collider
    {
        [Test, Apartment(ApartmentState.STA)]
        public void SpriteNode()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.Load(@"../Core/TestData/IO/AltseedPink.png");
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
        public void AutoCollisionSystem_Circle()
        {
            var tc = new TestCore();
            tc.Init();
            //tc.Duration = int.MaxValue;

            var texture = Texture2D.LoadStrict(@"TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var scene = new Altseed2.Node();
            var manager = new CollisionManagerNode();
            scene.AddChildNode(manager);

            Engine.AddNode(scene);

            var player = new Player_Circle(texture);

            scene.AddChildNode(player);

            var comparison = new SpriteNode()
            {
                Texture = texture,
                Pivot = new Vector2F(0.5f, 0.5f),
                Position = new Vector2F(500f, 300f)
            };
            comparison.AdjustSize();
            var colliderNode = new CircleColliderNode()
            {
                Radius = texture.Size.X / 2
            };
            colliderNode.AddChildNode(ColliderVisualizeNode.CreateVisualizeNode(colliderNode));
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
        private sealed class Player_Circle : SpriteNode, ICollisionEventReceiver
        {
            private readonly CircleColliderNode node;
            private readonly TextNode text = new TextNode()
            {
                Font = Font.LoadDynamicFontStrict("TestData/Font/mplus-1m-regular.ttf", 40)
            };
            public Player_Circle(Texture2D texture)
            {
                Engine.AddNode(text);
                Texture = texture;
                Pivot = new Vector2F(0.5f, 0.5f);
                Position = new Vector2F(0f, 300f);
                AdjustSize();
                node = new CircleColliderNode()
                {
                    Radius = texture.Size.X / 2
                };
                node.AddChildNode(ColliderVisualizeNode.CreateVisualizeNode(node));
                AddChildNode(node);
            }
            protected override void OnUpdate()
            {
                Position += new Vector2F(5f, 0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Up) == ButtonState.Hold) Position += new Vector2F(0.0f, -2.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Down) == ButtonState.Hold) Position += new Vector2F(0.0f, 2.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Left) == ButtonState.Hold) Position += new Vector2F(-2.0f, 0.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Right) == ButtonState.Hold) Position += new Vector2F(2.0f, 0.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Num1) == ButtonState.Hold) Angle++;
                //if (Engine.Keyboard.GetKeyState(Keys.Num2) == ButtonState.Hold) Angle--;
            }
            void ICollisionEventReceiver.OnCollisionEnter(CollisionInfo info)
            {
                text.Text = "Colliding";
            }
            void ICollisionEventReceiver.OnCollisionExit(CollisionInfo info)
            {
                text.Text = string.Empty;
            }
            void ICollisionEventReceiver.OnCollisionStay(CollisionInfo info)
            {

            }
        }

        private readonly static Vector2F[] array = new[] {
            default, 
            new Vector2F(-225f, 0f), new Vector2F(-75f, -75f), 
            new Vector2F(0f, -225f), new Vector2F(75f, -75f), 
            new Vector2F(225f, 0f), new Vector2F(75f, 75f), 
            new Vector2F(0f, 225f), new Vector2F(-75f, 75f), 
            new Vector2F(-225f, 0f), default
        };

        [Test, Apartment(ApartmentState.STA)]
        public void AutoCollisionSystem_Polygon()
        {
            var tc = new TestCore();
            tc.Init();
            //tc.Duration = int.MaxValue;

            var texture = Texture2D.LoadStrict(@"TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);
            
            var scene = new Altseed2.Node();
            var manager = new CollisionManagerNode();
            scene.AddChildNode(manager);

            Engine.AddNode(scene);

            var player = new Player_Polygon(texture);
            player.Angle += 45f;

            scene.AddChildNode(player);

            var comparison = new SpriteNode()
            {
                Texture = texture,
                Pivot = new Vector2F(0.5f, 0.5f),
                Position = new Vector2F(580f, 300f)
            };
            comparison.AdjustSize();
            var colliderNode = new PolygonColliderNode
            {
                Vertexes = array
            };
            colliderNode.AddChildNode(ColliderVisualizeNode.CreateVisualizeNode(colliderNode));
            comparison.AddChildNode(colliderNode);

            scene.AddChildNode(comparison);

            tc.LoopBody(null, x =>
            {
                player.Angle++;
                comparison.Angle++;
                if (Engine.Keyboard.GetKeyState(Keys.Escape) == ButtonState.Push) tc.Duration = 0;
                if (x == 10)
                {
                    Assert.True(manager.ContainsCollider(colliderNode));
                    Assert.AreEqual(manager.ColliderCount, 2);
                }
            });
            tc.End();
        }
        private sealed class Player_Polygon: SpriteNode, ICollisionEventReceiver
        {
            private readonly PolygonColliderNode node;
            private readonly TextNode text = new TextNode()
            {
                Font = Font.LoadDynamicFontStrict("TestData/Font/mplus-1m-regular.ttf", 40)
            };
            public Player_Polygon(Texture2D texture)
            {
                Engine.AddNode(text);
                Texture = texture;
                Pivot = new Vector2F(0.5f, 0.5f);
                Position = new Vector2F(220f, 300f);
                AdjustSize();
                node = new PolygonColliderNode()
                {
                    Vertexes = array
                };
                node.AddChildNode(ColliderVisualizeNode.CreateVisualizeNode(node));
                AddChildNode(node);
            }
            //protected override void OnUpdate()
            //{
            //    if (Engine.Keyboard.GetKeyState(Keys.Up) == ButtonState.Hold) Position += new Vector2F(0.0f, -2.0f);
            //    if (Engine.Keyboard.GetKeyState(Keys.Down) == ButtonState.Hold) Position += new Vector2F(0.0f, 2.0f);
            //    if (Engine.Keyboard.GetKeyState(Keys.Left) == ButtonState.Hold) Position += new Vector2F(-2.0f, 0.0f);
            //    if (Engine.Keyboard.GetKeyState(Keys.Right) == ButtonState.Hold) Position += new Vector2F(2.0f, 0.0f);
            //    if (Engine.Keyboard.GetKeyState(Keys.Num1) == ButtonState.Hold) Angle++;
            //    if (Engine.Keyboard.GetKeyState(Keys.Num2) == ButtonState.Hold) Angle--;
            //}
            void ICollisionEventReceiver.OnCollisionEnter(CollisionInfo info)
            {
                text.Text = "Colliding";
            }
            void ICollisionEventReceiver.OnCollisionExit(CollisionInfo info)
            {
                text.Text = string.Empty;
            }
            void ICollisionEventReceiver.OnCollisionStay(CollisionInfo info)
            {

            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void AutoCollisionSystem_Rectangle()
        {
            var tc = new TestCore();
            tc.Init();

            var texture = Texture2D.LoadStrict(@"TestData/IO/AltseedPink.png");
            Assert.NotNull(texture);

            var scene = new Altseed2.Node();
            var manager = new CollisionManagerNode();
            scene.AddChildNode(manager);

            Engine.AddNode(scene);

            var player = new Player_Rectangle(texture);

            scene.AddChildNode(player);

            var comparison = new SpriteNode()
            {
                Texture = texture,
                Pivot = new Vector2F(0.5f, 0.5f),
                Position = new Vector2F(500f, 300f)
            };
            comparison.AdjustSize();
            var colliderNode = new RectangleColliderNode()
            {
                RectangleSize = texture.Size
            };
            colliderNode.AddChildNode(ColliderVisualizeNode.CreateVisualizeNode(colliderNode));
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
        private sealed class Player_Rectangle: SpriteNode, ICollisionEventReceiver
        {
            private readonly RectangleColliderNode node;
            private readonly TextNode text = new TextNode()
            {
                Font = Font.LoadDynamicFontStrict("TestData/Font/mplus-1m-regular.ttf", 40)
            };
            public Player_Rectangle(Texture2D texture)
            {
                Engine.AddNode(text);
                Texture = texture;
                Pivot = new Vector2F(0.5f, 0.5f);
                Position = new Vector2F(0f, 300f);
                AdjustSize();
                node = new RectangleColliderNode()
                {
                    RectangleSize = texture.Size
                };
                node.AddChildNode(ColliderVisualizeNode.CreateVisualizeNode(node));
                AddChildNode(node);
            }
            protected override void OnUpdate()
            {
                Position += new Vector2F(5f, 0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Up) == ButtonState.Hold) Position += new Vector2F(0.0f, -2.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Down) == ButtonState.Hold) Position += new Vector2F(0.0f, 2.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Left) == ButtonState.Hold) Position += new Vector2F(-2.0f, 0.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Right) == ButtonState.Hold) Position += new Vector2F(2.0f, 0.0f);
                //if (Engine.Keyboard.GetKeyState(Keys.Num1) == ButtonState.Hold) Angle++;
                //if (Engine.Keyboard.GetKeyState(Keys.Num2) == ButtonState.Hold) Angle--;
            }
            void ICollisionEventReceiver.OnCollisionEnter(CollisionInfo info)
            {
                text.Text = "Colliding";
            }
            void ICollisionEventReceiver.OnCollisionExit(CollisionInfo info)
            {
                text.Text = string.Empty;
            }
            void ICollisionEventReceiver.OnCollisionStay(CollisionInfo info)
            {

            }
        }
    }
}