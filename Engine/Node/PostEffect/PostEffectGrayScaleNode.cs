namespace Altseed2
{
    /// <summary>
    /// �O���[�X�P�[������K�p����|�X�g�G�t�F�N�g�̃N���X
    /// </summary>
    public sealed class PostEffectGrayScaleNode : PostEffectNode
    {
        readonly Material material;

        /// <summary>
        /// <see cref="PostEffectGrayScaleNode"/>�̐V�����C���X�^���X�𐶐����܂��B
        /// </summary>
        public PostEffectGrayScaleNode()
        {
            material = Material.Create();
            var shader = Shader.Create("GrayScale", Engine.Graphics.BuiltinShader.GrayScaleShader, ShaderStage.Pixel);
            material.SetShader(shader);
        }

        /// <inheritdoc/>
        protected override void Draw(RenderTexture src, Color clearColor)
        {
            material.SetTexture("mainTex", src);
            Engine.Graphics.CommandList.RenderToRenderTarget(material);
        }
    }
}