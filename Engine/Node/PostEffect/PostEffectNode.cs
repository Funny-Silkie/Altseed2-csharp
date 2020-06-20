using System;
using System.Collections.Generic;

namespace Altseed
{
    public abstract class PostEffectNode : Node
    {
        private static Dictionary<int, RenderTextureCache> Cache;

        protected static RenderTexture GetBuffer(int identifier, Vector2I size)
        {
            if (!Cache.ContainsKey(identifier))
            {
                Cache[identifier] = new RenderTextureCache();
            }

            return Cache[identifier].GetRenderTexture(size);
        }

        internal static void InitializeCache()
        {
            Cache = new Dictionary<int, RenderTextureCache>();
        }

        internal static void UpdateCache()
        {
            foreach (var cache in Cache)
            {
                cache.Value.Update();
            }
        }

        /// <summary>
        /// このノードを描画するカメラを取得または設定します。
        /// </summary>
        public ulong CameraGroup
        {
            get => _CameraGroup;
            set
            {
                if (_CameraGroup == value) return;
                var old = _CameraGroup;
                _CameraGroup = value;

                if (Status == RegisterStatus.Registered)
                {
                    Engine.UpdatePostEffectNodeCameraGroup(this, old);
                }
            }
        }
        private ulong _CameraGroup = 0;

        /// <summary>
        /// PostEffectNodeが描画される順番。DrawnNodeを描画した後にまとめて描画されます。
        /// </summary>
        public int DrawingOrder
        {
            get => _DrawingOrder;
            set
            {
                if (_DrawingOrder == value) return;
                var old = _DrawingOrder;
                _DrawingOrder = value;
                if (Status == RegisterStatus.Registered)
                {
                    Engine.UpdatePostEffectNodeOrder(this, old);
                }
            }
        }
        private int _DrawingOrder = 0;

        public PostEffectNode() { }

        internal void CallDraw(RenderTexture src)
        {
            Draw(src);
        }

        protected abstract void Draw(RenderTexture src);

        protected void RenderToRenderTarget(Material material)
        {
            Engine.Graphics.CommandList.RenderToRenderTarget(material);
        }

        protected void RenderToRenderTexture(Material material, RenderTexture texture)
        {
            Engine.Graphics.CommandList.RenderToRenderTexture(material, texture, new RenderPassParameter(new Color(50, 50, 50), true, true));// TODO: 適切な値を設定できるようにする
        }

        #region Node

        internal override void Registered()
        {
            base.Registered();
            Engine.RegisterPostEffectNode(this);
        }

        internal override void Unregistered()
        {
            base.Unregistered();
            Engine.UnregisterPostEffectNode(this);
        }

        #endregion
    }
}