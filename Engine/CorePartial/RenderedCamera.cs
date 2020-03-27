﻿using System;
using System.Runtime.Serialization;

namespace Altseed
{
    internal partial class RenderedCamera
    {
        #region SerializeName
        private const string S_Transform = "S_Transform";
        #endregion

        partial void Deserialize_GetPtr(ref IntPtr ptr, SerializationInfo info) => ptr = cbg_RenderedCamera_Create();

        partial void OnDeserialize_Constructor(SerializationInfo info, StreamingContext context)
        {
            Transform = info.GetValue<Matrix44F>(S_Transform);            
        }

        partial void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(S_Transform, Transform);            
        }
    }
}
