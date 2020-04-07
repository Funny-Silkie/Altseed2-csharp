﻿using System;
using System.Runtime.Serialization;

namespace Altseed
{
    public partial class CircleCollider
    {
        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info) => ptr = cbg_CircleCollider_Constructor_0();
    }
    public partial class RectangleCollider
    {
        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info) => ptr = cbg_RectangleCollider_Constructor_0();
    }
    public partial class PolygonCollider
    {
        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info) => ptr = cbg_PolygonCollider_Constructor_0();
    }
}
