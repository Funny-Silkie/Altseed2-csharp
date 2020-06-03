﻿using System;

namespace Altseed
{
    /// <summary>
    /// 三角形を描画するノードのクラス
    /// </summary>
    [Serializable]
    public class TriangleNode : DrawnNode
    {
        private bool changed = false;
        private readonly RenderedPolygon renderedPolygon;

        /// <summary>
        /// 色を取得または設定します。
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                renderedPolygon.OverwriteVertexesColor(value);
            }
        }
        private Color _color = new Color(255, 255, 255);

        internal override int CullingId => renderedPolygon.Id;

        /// <summary>
        /// 頂点1を取得または設定します。
        /// </summary>
        public Vector2F Point1
        {
            get => _point1;
            set
            {
                if (_point1 == value) return;
                _point1 = value;
                changed = true;
            }
        }
        private Vector2F _point1;

        /// <summary>
        /// 頂点2を取得または設定します。
        /// </summary>
        public Vector2F Point2
        {
            get => _point2;
            set
            {
                if (_point2 == value) return;
                _point2 = value;
                changed = true;
            }
        }
        private Vector2F _point2;

        /// <summary>
        /// 頂点3を取得または設定します。
        /// </summary>
        public Vector2F Point3
        {
            get => _point3;
            set
            {
                if (_point3 == value) return;
                _point3 = value;
                changed = true;
            }
        }
        private Vector2F _point3;

        /// <summary>
        /// 使用するマテリアルを取得または設定します。
        /// </summary>
        public Material Material { get => renderedPolygon.Material; set { renderedPolygon.Material = value; } }

        /// <summary>
        /// <see cref="TriangleNode"/>の新しいインスタンスを生成する
        /// </summary>
        public TriangleNode()
        {
            renderedPolygon = RenderedPolygon.Create();
            renderedPolygon.Vertexes = VertexArray.Create(3);
        }

        protected internal override void Draw()
        {
            if (changed)
            {
                UpdateVertexes();
                changed = false;
            }
            Engine.Renderer.DrawPolygon(renderedPolygon);
        }

        internal override void UpdateInheritedTransform() => renderedPolygon.Transform = CalcInheritedTransform();

        private void UpdateVertexes()
        {
            var positions = new Vector2F[3];
            positions[0] = _point1;
            positions[1] = _point2;
            positions[2] = _point3;

            float minx = 0.0f, miny = 0.0f, maxx = 0.0f, maxy = 0.0f;
            for (int i = 0; i < 3; i++)
            {
                if (positions[i].X < minx) minx = positions[i].X;
                if (maxx < positions[i].X) maxx = positions[i].X;
                if (positions[i].Y < miny) miny = positions[i].Y;
                if (maxy < positions[i].Y) maxy = positions[i].Y;
            }

            Size = new Vector2F(maxx - minx, maxy - miny);

            var array = Vector2FArray.Create(positions.Length);
            array.FromArray(positions);
            renderedPolygon.CreateVertexesByVector2F(array);
            renderedPolygon.OverwriteVertexesColor(_color);
        }
    }
}
