﻿using System;

namespace Altseed
{
    /// <summary>
    /// 水平方向の配置
    /// </summary>
    [Serializable]
    public enum HorizontalAlignment
    {
        /// <summary>
        /// 左揃え
        /// </summary>
        Left,
        /// <summary>
        /// 中央揃え
        /// </summary>
        Center,
        /// <summary>
        /// 右揃え
        /// </summary>
        Right
    }

    /// <summary>
    /// 垂直方向の配置
    /// </summary>
    [Serializable]
    public enum VerticalAlignment
    {
        /// <summary>
        /// 上揃え
        /// </summary>
        Top,
        /// <summary>
        /// 中央揃え
        /// </summary>
        Center,
        /// <summary>
        /// 下揃え
        /// </summary>
        Bottom
    }

    /// <summary>
    /// テキストを描画するノードのクラス
    /// </summary>
    [Serializable]
    public class TextNode : DrawnNode
    {
        private readonly RenderedText renderedText;

        public override Matrix44F AbsoluteTransform => renderedText.Transform;

        /// <summary>
        /// 描画する文字列を取得または設定します。
        /// </summary>
        public string Text
        {
            get => renderedText.Text;
            set
            {
                if (renderedText.Text == value) return;
                renderedText.Text = value;
            }
        }

        /// <summary>
        /// 文字列の描画に使用するフォントを取得または設定します。
        /// </summary>
        public Font Font
        {
            get => renderedText.Font;
            set
            {
                if (renderedText.Font == value) return;
                renderedText.Font = value;
            }
        }

        /// <summary>
        /// 使用するマテリアルを取得または設定します。
        /// </summary>
        public Material Material
        {
            get => renderedText.Material;
            set
            {
                if (renderedText.Material == value) return;
                renderedText.Material = value;
            }
        }

        /// <summary>
        /// 文字の太さを取得または設定します。
        /// </summary>
        public float Weight
        {
            get => renderedText.Weight;
            set
            {
                if (renderedText.Weight == value) return;
                renderedText.Weight = value;
            }
        }

        /// <summary>
        /// 字間をピクセル単位で取得または設定します。
        /// </summary>
        public float CharacterSpace
        {
            get => renderedText.CharacterSpace;
            set
            {
                if (renderedText.CharacterSpace == value) return;
                renderedText.CharacterSpace = value;
            }
        }

        /// <summary>
        /// 行間をピクセル単位で取得または設定します。
        /// Nullの時、フォント標準の行間を指定します。
        /// </summary>
        public float? LineGap
        {
            get => _LineGap;
            set
            {
                if (_LineGap == value) return;
                _LineGap = value;
                renderedText.LineGap = value ?? Font?.LineGap ?? 0;
            }
        }
        private float? _LineGap = null;

        /// <summary>
        /// 色を取得または設定します。
        /// </summary>
        public Color Color
        {
            get => renderedText.Color;
            set
            {
                if (renderedText.Color == value) return;
                renderedText.Color = value;
            }
        }

        /// <summary>
        /// カーニングの有無を取得または設定します。
        /// </summary>
        public bool IsEnableKerning
        {
            get => renderedText.IsEnableKerning;
            set
            {
                if (renderedText.IsEnableKerning == value) return;
                renderedText.IsEnableKerning = value;
            }
        }

        /// <summary>
        /// 行の方向を取得または設定します。
        /// </summary>
        public WritingDirection WritingDirection
        {
            get => renderedText.WritingDirection;
            set
            {
                if (renderedText.WritingDirection == value) return;
                renderedText.WritingDirection = value;
            }
        }

        /// <summary>
        /// 水平方向の配置
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get => _HorizontalAlignment;
            set
            {
                if (value == _HorizontalAlignment)
                    return;

                _HorizontalAlignment = value;
            }
        }
        private HorizontalAlignment _HorizontalAlignment = HorizontalAlignment.Left;

        /// <summary>
        /// 垂直方向の配置
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get => _VerticalAlignment;
            set
            {
                if (value == _VerticalAlignment)
                    return;

                _VerticalAlignment = value;
            }
        }
        private VerticalAlignment _VerticalAlignment = VerticalAlignment.Top;

        /// <summary>
        /// カリング用ID
        /// </summary>
        internal override int CullingId => renderedText.Id;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TextNode()
        {
            renderedText = RenderedText.Create();
        }

        /// <summary>
        /// 描画を実行します。
        /// </summary>
        internal override void Draw()
        {
            Engine.Renderer.DrawText(renderedText);
        }

        internal override void UpdateInheritedTransform()
        {
            var mat = Matrix44F.Identity;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    break;
                case HorizontalAlignment.Center:
                    mat *= Matrix44F.GetTranslation2D(new Vector2F((Size.X - renderedText.TextureSize.X) / 2, 0));
                    break;
                case HorizontalAlignment.Right:
                    mat *= Matrix44F.GetTranslation2D(new Vector2F(Size.X - renderedText.TextureSize.X, 0));
                    break;
                default:
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    break;
                case VerticalAlignment.Center:
                    mat *= Matrix44F.GetTranslation2D(new Vector2F(0, (Size.Y - renderedText.TextureSize.Y) / 2));
                    break;
                case VerticalAlignment.Bottom:
                    mat *= Matrix44F.GetTranslation2D(new Vector2F(0, Size.Y - renderedText.TextureSize.Y));
                    break;
                default:
                    break;
            }

            renderedText.Transform = CalcInheritedTransform() * mat;
        }

        public override void AdjustSize()
        {
            Size = renderedText.TextureSize;
        }
    }
}
