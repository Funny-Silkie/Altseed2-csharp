using System;

namespace Altseed2
{
    public sealed class PostEffectGrayScaleNode : PostEffectNode
    {
        readonly Material material;

        public PostEffectGrayScaleNode()
        {
            material = Material.Create();
            var shader = Shader.Create("GrayScale", Engine.Graphics.BuiltinShader.GrayScaleShader, ShaderStage.Pixel);
            material.SetShader(shader);
        }

        protected override void Draw(RenderTexture src, Color clearColor)
        {
            material.SetTexture("mainTex", src);
            Engine.Graphics.CommandList.RenderToRenderTarget(material);
        }
    }
}