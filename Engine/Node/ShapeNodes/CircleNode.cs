using System;

namespace Altseed
{
    /// <summary>
    /// 円を描画するノードのクラス
    /// </summary>
    [Serializable]
    public class CircleNode : DrawnNode
    {
        private bool changed = false;
        private readonly RenderedPolygon renderedPolygon;

        internal override int CullingId => renderedPolygon.Id;

        public override Matrix44F AbsoluteTransform => renderedPolygon.Transform;

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

        /// <summary>
        /// 使用するマテリアルを取得または設定します。
        /// </summary>
        public Material Material { get => renderedPolygon.Material; set { renderedPolygon.Material = value; } }

        /// <summary>
        /// 半径を取得または設定します。
        /// </summary>
        public float Radius
        {
            get => _radius;
            set
            {
                if (_radius == value) return;
                _radius = value;
                changed = true;
            }
        }
        private float _radius;

        /// <summary>
        /// 描画するテクスチャを取得または設定します。
        /// </summary>
        public TextureBase Texture
        {
            get => renderedPolygon.Texture;
            set
            {
                renderedPolygon.Texture = value;
            }
        }

        /// <summary>
        /// 頂点の個数を取得または設定します。
        /// </summary>
        public int VertNum
        {
            get => _vertnum;
            set
            {
                if (value < 3) throw new ArgumentOutOfRangeException(nameof(value), "設定しようとした値が3未満です");
                if (_vertnum == value) return;
                _vertnum = value;
                changed = true;
            }
        }
        private int _vertnum = 3;

        /// <summary>
        /// <see cref="CircleNode"/>の新しいインスタンスを生成する
        /// </summary>
        public CircleNode()
        {
            renderedPolygon = RenderedPolygon.Create();
            renderedPolygon.Vertexes = VertexArray.Create(_vertnum);
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

        internal override void UpdateInheritedTransform()
        {
            renderedPolygon.Transform = CalcInheritedTransform();
        }

        private void UpdateVertexes()
        {
            var deg = 360f / _vertnum;          
            var positions = new Vector2F[_vertnum];
            var vec = new Vector2F(0.0f, -_radius);
            
            float minx = 0.0f, miny = 0.0f, maxx = 0.0f, maxy = 0.0f;
            for (int i = 0; i < _vertnum; i++)
            {
                positions[i] = vec;
                vec.Degree += deg;

                if (vec.X < minx) minx = vec.X;
                if (maxx < vec.X) maxx = vec.X;
                if (vec.Y < miny) miny = vec.Y;
                if (maxy < vec.Y) maxy = vec.Y;
            }

            Size = new Vector2F(maxx - minx, maxy - miny);
            // NOTE: 半径から雑に計算してもいいかもしれない

            var array = Vector2FArray.Create(positions.Length);
            array.FromArray(positions);
            renderedPolygon.CreateVertexesByVector2F(array);
            renderedPolygon.OverwriteVertexesColor(_color);
        }
    }
}
