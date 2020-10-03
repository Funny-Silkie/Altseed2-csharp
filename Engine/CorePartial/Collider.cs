using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Altseed2
{
    public partial class Collider
    {
        /// <summary>
        /// 座標を取得または設定します。
        /// </summary>
        public Vector2F Position
        {
            get => _position ??= cbg_Collider_GetPosition(selfPtr);
            set
            {
                if (_position == value) return;
                _position = value;
                cbg_Collider_SetPosition(selfPtr, value);
                _transform = null;
            }
        }
        private Vector2F? _position;

        /// <summary>
        /// 回転(弧度法)を取得または設定します。
        /// </summary>
        public float Rotation
        {
            get => _rotation ??= cbg_Collider_GetRotation(selfPtr);
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                cbg_Collider_SetRotation(selfPtr, value);
                _transform = null;
            }
        }
        private float? _rotation;

        /// <summary>
        /// 変形行列を取得または設定します。
        /// </summary>
        public Matrix44F Transform
        {
            get => _transform ??= cbg_Collider_GetTransform(selfPtr);
            set
            {
                if (_transform == value) return;
                _transform = value;
                cbg_Collider_SetTransform(selfPtr, value);
                _position = null;
                _rotation = null;
            }
        }
        private Matrix44F? _transform;
    }

    public partial class CircleCollider
    {
        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info)
        {
            ptr = cbg_CircleCollider_Create();
        }
    }

    public partial class RectangleCollider
    {
        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info)
        {
            ptr = cbg_RectangleCollider_Create();
        }
    }

    public partial class PolygonCollider
    {
        #region SerializeName
        private const string S_Buffers = "S_Buffers";
        private const string S_Vertexes = "S_Vertexes";
        #endregion

        /// <summary>
        /// インデックスバッファーを取得または設定します。
        /// </summary>
        public IReadOnlyList<int> Buffers
        {
            get => BuffersInternal?.ToArray();
            set => SetBuffers(value);
        }
        internal Int32Array BuffersInternal
        {
            get => _buffersInternalCache ??= Int32Array.TryGetFromCache(cbg_PolygonCollider_GetBuffers(selfPtr));
            set
            {
                _buffersInternalCache = value ?? throw new ArgumentNullException(nameof(value), "引数がnullです");
                cbg_PolygonCollider_SetBuffers(selfPtr, value.selfPtr);
            }
        }
        private Int32Array _buffersInternalCache;

        /// <summary>
        /// 頂点情報のコレクションを取得または設定します。
        /// </summary>
        public IReadOnlyList<Vector2F> Vertexes
        {
            get => VertexesInternal?.ToArray();
            set
            {
                SetVertexes(value);
            }
        }

        internal Vector2FArray VertexesInternal
        {
            get => Vector2FArray.TryGetFromCache(cbg_PolygonCollider_GetVertexes(selfPtr));
            private set
            {
                cbg_PolygonCollider_SetVertexes(selfPtr, value?.selfPtr ?? IntPtr.Zero);
            }
        }
        private Vector2FArray _vertexesInternalCache;

        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info)
        {
            ptr = cbg_PolygonCollider_Create();
        }

        partial void OnDeserialize_Constructor(SerializationInfo info, StreamingContext context)
        {
            BuffersInternal = info.GetValue<Int32Array>(S_Buffers);
            VertexesInternal = info.GetValue<Vector2FArray>(S_Vertexes);
        }

        partial void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(S_Buffers, BuffersInternal);
            info.AddValue(S_Vertexes, VertexesInternal);
        }

        /// <summary>
        /// インデックスバッファーを設定します。
        /// </summary>
        /// <param name="buffers">設定する座標</param>
        /// <remarks>サイズは3の倍数である必要があります<br></br>余った値は無視されます</remarks>
        public void SetBuffers(IEnumerable<int> buffers)
        {
            if (buffers is null)
            {
                BuffersInternal = Int32Array.Create(0);
                return;
            }

            SetBuffers(buffers is int[] a ? (ReadOnlySpan<int>)a : buffers.ToArray());
        }

        /// <summary>
        /// インデックスバッファーを設定します。
        /// </summary>
        /// <param name="buffers">設定するインデックスバッファー</param>
        /// <remarks>サイズは3の倍数である必要があります<br></br>余った値は無視されます</remarks>
        public void SetBuffers(ReadOnlySpan<int> buffers)
        {
            if (buffers.Length == 0)
            {
                BuffersInternal = Int32Array.Create(0);
                return;
            }

            var length = buffers.Length / 3 * 3;
            if (buffers.Length != length) buffers = buffers.Slice(0, length);

            if (_buffersInternalCache is null) _buffersInternalCache = Int32Array.Create(buffers);
            else _buffersInternalCache.FromSpan(buffers);
            
            BuffersInternal = _buffersInternalCache;
        }

        /// <summary>
        /// インデックスバッファーを既定のものに設定します。
        /// </summary>
        public void SetDefaultIndexBuffer()
        {
            cbg_PolygonCollider_SetDefaultIndexBuffer(selfPtr);
            _buffersInternalCache = null;
        }

        /// <summary>
        /// 指定した座標に頂点を設定します。
        /// </summary>
        /// <param name="positions">設定する座標</param>
        /// <param name="resetIB"><see cref="Buffers"/>を自動計算したものに設定するかどうか</param>
        public void SetVertexes(IEnumerable<Vector2F> positions, bool resetIB = true)
        {
            if (positions is null)
            {
                VertexesInternal = null;
                if (resetIB) BuffersInternal = Int32Array.Create(0);
                return;
            }

            if (_vertexesInternalCache is null)
            {
                _vertexesInternalCache = Vector2FArray.Create(positions);
            }
            else
            {
                _vertexesInternalCache.FromEnumerable(positions);
            }
            VertexesInternal = _vertexesInternalCache;

            if (resetIB) SetDefaultIndexBuffer();
        }

        /// <summary>
        /// 指定した座標に頂点を設定する
        /// </summary>
        /// <param name="positions">設定する座標</param>
        /// <param name="resetIB"><see cref="Buffers"/>を自動計算したものに設定するかどうか</param>
        public void SetVertexes(ReadOnlySpan<Vector2F> positions, bool resetIB = true)
        {
            if (positions.Length == 0)
            {
                VertexesInternal = null;
                if (resetIB) BuffersInternal = Int32Array.Create(0);
                return;
            }

            if (_vertexesInternalCache is null)
            {
                _vertexesInternalCache = Vector2FArray.Create(positions);
            }
            else
            {
                _vertexesInternalCache.FromSpan(positions);
            }

            VertexesInternal = _vertexesInternalCache;

            SetDefaultIndexBuffer();
        }
    }
}
