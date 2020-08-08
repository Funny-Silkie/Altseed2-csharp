﻿using System.Runtime.Serialization;

namespace Altseed2
{
    public sealed partial class Configuration
    {
        #region SerializeName
        private const string S_VisibleTransformInfo = "S_VisibleTransformInfo";
        #endregion

        /// <summary>
        /// 変形に関する情報の表示を取得または設定します。
        /// </summary>
        public bool VisibleTransformInfo
        {
            get
            {
                return _VisibleTransformInfo;
            }
            set
            {
                if (_VisibleTransformInfo == value) return;
                _VisibleTransformInfo = value;
            }
        }
        private bool _VisibleTransformInfo　= false;

        public Configuration()
        {
            selfPtr = cbg_Configuration_Create();
        }

        partial void OnDeserialize_Constructor(SerializationInfo info, StreamingContext context)
        {
            VisibleTransformInfo = info.GetBoolean(S_VisibleTransformInfo);
        }

        partial void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(S_VisibleTransformInfo, _VisibleTransformInfo);
        }
    }
}
