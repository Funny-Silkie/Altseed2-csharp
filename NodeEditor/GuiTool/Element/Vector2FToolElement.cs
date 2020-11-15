﻿using System;

namespace Altseed2
{
    /// <summary>
    /// 
    /// </summary>
    public class Vector2FToolElement : ToolElement
    {
        /// <summary>
        /// 
        /// </summary>
        public float Speed { get; }

        /// <summary>
        /// 
        /// </summary>
        public float Min { get; }

        /// <summary>
        /// 
        /// </summary>
        public float Max { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="speed"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Vector2FToolElement(string name, object source, string propertyName, float speed = 1, float min = -1000, float max = 1000) : base(name, source, propertyName)
        {
            Speed = speed;
            Min = min;
            Max = max;

            if (!typeof(Vector2F).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がVector2Fではありません");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            Vector2F vector = (Vector2F)PropertyInfo.GetValue(Source);
            float x = vector.X;
            float y = vector.Y;

            var width = Engine.Tool.GetColumnWidth(0) * 0.65f;
            Engine.Tool.Columns(2, false);
            Engine.Tool.SetColumnWidth(0, width / 2);
            Engine.Tool.SetColumnWidth(1, width / 2);
            Engine.Tool.PushItemWidth(-1);

            if (Engine.Tool.DragFloat($"##{Name}_X", ref x, Speed, Min, Max))
            {
                vector.X = x;
                PropertyInfo.SetValue(Source, vector);
            }

            Engine.Tool.PopItemWidth();
            Engine.Tool.NextColumn();
            Engine.Tool.PushItemWidth(-1);

            if (Engine.Tool.DragFloat($"##{Name}_Y", ref y, Speed, Min, Max))
            {
                vector.Y = y;
                PropertyInfo.SetValue(Source, vector);
            }
            Engine.Tool.PopItemWidth();
            Engine.Tool.Columns(1, false);
            Engine.Tool.SameLine();
            Engine.Tool.Text(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="guiInfo"></param>
        /// <returns></returns>
        public static Vector2FToolElement Create(object source, MemberGuiInfo guiInfo)
        {
            var speed = guiInfo.Options.ContainsKey("speed") ? (float)guiInfo.Options["speed"] : 1;
            var min = guiInfo.Options.ContainsKey("min") ? (float)guiInfo.Options["min"] : -1000;
            var max = guiInfo.Options.ContainsKey("max") ? (float)guiInfo.Options["max"] : 1000;
            return new Vector2FToolElement(guiInfo.Name, source, guiInfo.PropertyName, speed, min, max);
        }
    }
}
