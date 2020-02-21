using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Altseed
{
    public partial class Easing
    {
        /// <param name="easing">�C�[�W���O�̎��</param>
        /// <param name="t">�C�[�W���O�ߒ��̕ϐ�</param>
        /// <returns></returns>
        public static float GetEasing(EasingType easing, float t)
        {
            if (t <= 0.0f) return 0.0f;
            if (t >= 1.0f) return 1.0f;

            switch (easing)
            {
                case EasingType.Linear:
                    {
                        return t;
                    }

                case EasingType.InSine:
                    {
                        return 1.0f - (float)Math.Cos(t * Math.PI * 0.5f);
                    }

                case EasingType.OutSine:
                    {
                        return (float)Math.Sin(t * Math.PI * 0.5f);
                    }

                case EasingType.InOutSine:
                    {
                        return -1.0f / 2.0f * (float)(Math.Cos(t * Math.PI) - 1.0f);
                    }

                case EasingType.InQuad:
                    {
                        return t * t;
                    }

                case EasingType.OutQuad:
                    {
                        return t * (2.0f - t);
                    }

                case EasingType.InOutQuad:
                    {
                        if (t < 0.5f) return 2.0f * t * t;

                        return t * (4.0f - 2.0f * t) - 1.0f;
                    }

                case EasingType.InCubic:
                    {
                        return t * t * t;
                    }

                case EasingType.OutCubic:
                    {
                        t = t - 1.0f;
                        return t * t * t + 1.0f;
                    }

                case EasingType.InOutCubic:
                    {
                        if (t < 0.5f) return 4.0f * t * t * t;

                        t = t - 1.0f;
                        return 1.0f + t * (2.0f * t) * (2.0f * t);
                    }

                case EasingType.InQuart:
                    {
                        return t * t * t * t;
                    }

                case EasingType.OutQuart:
                    {
                        t = t - 1.0f;
                        return -(t * t * t * t - 1.0f);
                    }

                case EasingType.InOutQuart:
                    {
                        if (t < 0.5f)
                        {
                            return 8.0f * t * t * t * t;
                        }

                        t = (t - 1.0f) * (t - 1.0f);
                        return 1.0f - 8.0f * t * t;
                    }

                case EasingType.InQuint:
                    {
                        return t * t * t * t * t;
                    }

                case EasingType.OutQuint:
                    {
                        float t_ = (t - 1.0f) * (t - 1.0f);
                        return (t - 1.0f) * t_ * t_ + 1.0f;
                    }

                case EasingType.InOutQuint:
                    {
                        if (t < 0.5f) return 16.0f * t * t * t * t * t;

                        float t_ = (t - 1.0f) * (t - 1.0f);
                        return 1.0f + 16.0f * (t - 1.0f) * t_ * t_;
                    }

                case EasingType.InExpo:
                    {
                        return (float)Math.Pow(2.0f, 10.0f * (t - 1.0f));
                    }

                case EasingType.OutExpo:
                    {
                        return -(float)Math.Pow(2.0f, -10.0f * t) + 1.0f;
                    }

                case EasingType.InOutExpo:
                    {
                        if (t < 0.5f) return (float)(Math.Pow(2.0f, 16.0f * t) - 1.0f) / 510.0f;

                        return 1.0f - 0.5f * (float)Math.Pow(2.0f, -16.0f * (t - 0.5f));
                    }

                case EasingType.InCirc:
                    {
                        return 1.0f - (float)Math.Sqrt(1.0f - t);
                    }

                case EasingType.OutCirc:
                    {
                        return (float)Math.Sqrt(t);
                    }

                case EasingType.InOutCirc:
                    {
                        if (t < 0.5f) return (1.0f - (float)Math.Sqrt(1.0f - 2.0f * t)) * 0.5f;
                        return (1.0f + (float)Math.Sqrt(2.0f * t - 1.0f)) * 0.5f;
                    }

                case EasingType.InBack:
                    {
                        return t * t * (2.70158f * t - 1.70158f);
                    }

                case EasingType.OutBack:
                    {
                        t = t - 1.0f;
                        return 1.0f + t * t * (2.70158f * t + 1.70158f);
                    }

                case EasingType.InOutBack:
                    {
                        if (t < 0.5f) return (t * t * (7.0f * t - 2.5f) * 2.0f);

                        t = t - 1.0f;
                        return 1.0f + t * t * 2.0f * (7.0f * t + 2.5f);
                    }

                case EasingType.InElastic:
                    {
                        return t * t * t * t * (float)Math.Sin(t * Math.PI * 4.5f);
                    }

                case EasingType.OutElastic:
                    {
                        float t_ = (t - 1.0f) * (t - 1.0f);
                        return 1.0f - t_ * t_ * (float)Math.Cos(t * Math.PI * 4.5f);
                    }

                case EasingType.InOutElastic:
                    {
                        if (t < 0.45f) return 8.0f * t * t * t * t * (float)Math.Sin(t * Math.PI * 9.0f);
                        if (t < 0.55f) return 0.5f + 0.75f * (float)Math.Sin(t * Math.PI * 4.0f);

                        float t_ = (t - 1.0f) * (t - 1.0f);
                        return 1.0f - 8.0f * t_ * t_ * (float)Math.Sin(t * Math.PI * 9.0f);
                    }

                case EasingType.InBounce:
                    {
                        return (float)Math.Pow(2.0f, 6.0f * (t - 1.0f)) * (float)Math.Abs(Math.Sin(t * Math.PI * 3.5f));
                    }

                case EasingType.OutBounce:
                    {
                        return 1.0f - (float)Math.Pow(2.0f, -6.0f * t) * (float)Math.Abs(Math.Cos(t * Math.PI * 3.5f));
                    }

                case EasingType.InOutBounce:
                    {
                        if (t < 0.5f) return 8.0f * (float)Math.Pow(2.0f, 8.0f * (t - 1.0f)) * (float)Math.Abs(Math.Sin(t * Math.PI * 7.0f));

                        return 1.0f - 8.0f * (float)Math.Pow(2.0f, -8.0f * t) * (float)Math.Abs(Math.Sin(t * Math.PI * 7.0f));
                    }
                default: return t;
            }
        }
    }
}