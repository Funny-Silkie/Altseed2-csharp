﻿using System;

namespace Altseed2
{
    /// <summary>
    /// ガウスぼかしを適用するポストエフェクトのクラス
    /// </summary>
    public sealed class PostEffectGaussianBlurNode : PostEffectNode
    {
        private readonly Material materialX;
        private readonly Material materialY;

        private float intensity;

        /// <summary>
        /// ぼけの強さを取得または設定します。
        /// </summary>
        /// <remarks>値が大きいほど画面がぼけます。</remarks>
        public float Intensity
        {
            get => intensity;
            set
            {
                if (intensity == value) return;

                intensity = value;

                float total = 0.0f;
                float dispersion = intensity * intensity;

                Span<float> ws = stackalloc float[3];

                for (int i = 0; i < 3; i++)
                {
                    float pos = 1.0f + 2.0f * i;
                    ws[i] = MathF.Exp(-0.5f * pos * pos / dispersion);
                    total += ws[i] * 2.0f;
                }

                Vector4F weights = new Vector4F(ws[0], ws[1], ws[2], 0.0f) / total;

                materialX.SetVector4F("weight", weights);
                materialY.SetVector4F("weight", weights);
            }
        }

        /// <summary>
        /// <see cref="PostEffectGaussianBlurNode"/>の新しいインスタンスを生成します。
        /// </summary>
        public PostEffectGaussianBlurNode()
        {
            materialX = Material.Create();
            materialY = Material.Create();

            var baseCode = Engine.Graphics.BuiltinShader.GaussianBlurShader;

            materialX.SetShader(Shader.Create("GaussianBlurX", "#define BLUR_X\n" + baseCode, ShaderStage.Pixel));
            materialY.SetShader(Shader.Create("GaussianBlurY", "#define BLUR_Y\n" + baseCode, ShaderStage.Pixel));

            Intensity = 5.0f;
        }

        /// <inheritdoc/>
        protected override void Draw(RenderTexture src, Color clearColor)
        {
            src.WrapMode = TextureWrapMode.Clamp;

            var buffer = GetBuffer(0, src.Size, src.Format);
            buffer.WrapMode = TextureWrapMode.Clamp;
            buffer.FilterType = TextureFilter.Linear;

            materialX.SetTexture("mainTex", src);
            Engine.Graphics.CommandList.RenderToRenderTexture(materialX, buffer, new RenderPassParameter(clearColor, true, true));

            materialY.SetTexture("mainTex", buffer);
            Engine.Graphics.CommandList.RenderToRenderTarget(materialY);
        }
    }
}
