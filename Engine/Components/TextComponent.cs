﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Altseed
{
    /// <summary>
    /// テキストの描画を行うコンポーネント
    /// </summary>
    [Serializable]
    public class TextComponent : DrawnComponent, IDeserializationCallback
    {
        private readonly List<RenderedSprite> sprites = new List<RenderedSprite>();

        /// <summary>
        /// 角度を取得または設定する
        /// </summary>
        public override float Angle
        {
            get => _angle;
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    M_angle.SetRotationZ((float)(value * Math.PI / 180d));
                    ChanegeTransform();
                }
            }
        }
        private float _angle;
        private Matrix44F M_angle = Matrix44F.GetIdentity();

        /// <summary>
        /// 描画するフォントを取得または設定する
        /// </summary>
        public Font Font
        {
            get => _font;
            set
            {
                if (_font != value)
                {
                    _font = value;
                    UpdateImages();
                }
            }
        }
        private Font _font;

        public Material Material
        {
            get => _material;
            set
            {
                if (_material != value)
                {
                    _material = value;
                    UpdateImages();
                }
            }
        }
        private Material _material = new Material();

        /// <summary>
        /// 座標を取得または設定する
        /// </summary>
        public override Vector2F Position
        {
            get => _position;
            set
            {
                if (value != _position)
                {
                    _position = value;
                    M_position.SetTranslation(value.X, value.Y, 0.0f);
                    ChanegeTransform();
                }
            }
        }
        [NonSerialized]
        private Vector2F _position;
        [NonSerialized]
        private Matrix44F M_position = Matrix44F.GetIdentity();

        /// <summary>
        /// 拡大率を取得または設定する
        /// </summary>
        public override Vector2F Scale
        {
            get => _scale;
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                    M_scale.SetScale(value.X, value.Y, 1.0f);
                    ChanegeTransform();
                }
            }
        }
        private Vector2F _scale = new Vector2F(1.0f, 1.0f);
        [NonSerialized]
        private Matrix44F M_scale = Matrix44F.GetIdentity();

        /// <summary>
        /// 描画するテキストを取得または設定する
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value ?? string.Empty;
                    UpdateImages(true);
                }
            }
        }
        private string _text = string.Empty;

        /// <summary>
        /// 新しいインスタンスを生成する
        /// </summary>
        public TextComponent()
        {
            _material.Shader = Engine.Graphics.BuildinShader.Create(BuildinShaderType.FontUnlitPS);
            UpdateImages();
        }

        internal override void Draw()
        {
            foreach (var s in sprites) Engine.Renderer.DrawSprite(s);
        }

        /// <summary>
        /// デシリアライズ時に実行
        /// </summary>
        /// <param name="sender">現段階では実装されていない 常にnullを返す</param>
        protected virtual void OnDeserialization(object sender)
        {
            M_position.SetTranslation(_position.X, _position.Y, 0.0f);
            M_scale.SetScale(_scale.X, _scale.Y, 1.0f);
        }
        void IDeserializationCallback.OnDeserialization(object sender) => OnDeserialization(sender);

        /// <summary>
        /// 描画する情報を更新する
        /// </summary>
        internal void UpdateImages(bool clear = false)
        {
            if (clear) sprites.Clear();
            var position = _position;
            for (int i = 0; i < _text.Length; i++)
            {
                var glyph = _font?.GetGlyph(_text[i]);
                if (glyph == null) continue;

                var tempPosition = position + glyph.Offset + new Vector2F(0, _font.Ascent);
                var sprite = clear ? RenderedSprite.Create() : sprites[i];
                sprite.Material = _material;
                sprite.Texture = _font.GetFontTexture(glyph.TextureIndex);

                sprite.Transform = GetTransform(tempPosition);

                sprite.Src = new RectF(glyph.Position.X, glyph.Position.Y, glyph.Size.X, glyph.Size.Y);

                if (clear) sprites.Add(sprite);
                else sprites[i] = sprite;

                position += new Vector2F(glyph.GlyphWidth, 0);

                if (i < _text.Length - 1) position += new Vector2F(_font.GetKerning(_text[i], _text[i + 1]), 0);
            }
        }

        internal void ChanegeTransform()
        {
            var position = _position;
            for (int i = 0; i < _text.Length; i++)
            {
                var glyph = _font?.GetGlyph(_text[i]);
                if (glyph == null) continue;

                var tempPosition = position + glyph.Offset + new Vector2F(0, _font.Ascent);

                sprites[i].Transform = GetTransform(tempPosition);

                position += new Vector2F(glyph.GlyphWidth, 0);

                if (i < _text.Length - 1) position += new Vector2F(_font.GetKerning(_text[i], _text[i + 1]), 0);
            }
        }
        private Matrix44F GetTransform(Vector2F position) => Matrix44F.GetIdentity() * M_scale * M_angle * M_scale * Matrix44F.FromPosition(_position + position);
    }
}
