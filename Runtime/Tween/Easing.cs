using UnityEngine;

namespace Zlitz.Animation
{
    public static class Easing
    {
        public static readonly IEasingFunction linear       = new LinearEasingFunction();
        public static readonly IEasingFunction inSine       = new InSineEasingFunction();
        public static readonly IEasingFunction outSine      = new OutSineEasingFunction();
        public static readonly IEasingFunction inOutSine    = new InOutSineEasingFunction();
        public static readonly IEasingFunction inQuad       = new InPowerEasingFunction(2.0f);
        public static readonly IEasingFunction outQuad      = new OutPowerEasingFunction(2.0f);
        public static readonly IEasingFunction inOutQuad    = new InOutPowerEasingFunction(2.0f);
        public static readonly IEasingFunction inCubic      = new InPowerEasingFunction(3.0f);
        public static readonly IEasingFunction outCubic     = new OutPowerEasingFunction(3.0f);
        public static readonly IEasingFunction inOutCubic   = new InOutPowerEasingFunction(3.0f);
        public static readonly IEasingFunction inQuart      = new InPowerEasingFunction(4.0f);
        public static readonly IEasingFunction outQuart     = new OutPowerEasingFunction(4.0f);
        public static readonly IEasingFunction inOutQuart   = new InOutPowerEasingFunction(4.0f);
        public static readonly IEasingFunction inQuint      = new InPowerEasingFunction(5.0f);
        public static readonly IEasingFunction outQuint     = new OutPowerEasingFunction(5.0f);
        public static readonly IEasingFunction inOutQuint   = new InOutPowerEasingFunction(5.0f);
        public static readonly IEasingFunction inExpo       = new InExpoEasingFunction();
        public static readonly IEasingFunction outExpo      = new OutExpoEasingFunction();
        public static readonly IEasingFunction inOutExpo    = new InOutExpoEasingFunction();
        public static readonly IEasingFunction inCirc       = new InCircEasingFunction();
        public static readonly IEasingFunction outCirc      = new OutCircEasingFunction();
        public static readonly IEasingFunction inOutCirc    = new InOutCircEasingFunction();
        public static readonly IEasingFunction inBack       = new InBackEasingFunction();
        public static readonly IEasingFunction outBack      = new OutBackEasingFunction();
        public static readonly IEasingFunction inOutBack    = new InOutBackEasingFunction();
        public static readonly IEasingFunction inElastic    = new InElasticEasingFunction();
        public static readonly IEasingFunction outElastic   = new OutElasticEasingFunction();
        public static readonly IEasingFunction inOutElastic = new InOutElasticEasingFunction();
        public static readonly IEasingFunction inBounce     = new InBounceEasingFunction();
        public static readonly IEasingFunction outBounce    = new OutBounceEasingFunction();
        public static readonly IEasingFunction inOutBounce  = new InOutBounceEasingFunction();

        public static float Evaluate(float t)
        {
            return linear.Evaluate(Mathf.Clamp01(t));
        }

        public static float Evaluate(float t, IEasingFunction easingFunction)
        {
            return easingFunction.Evaluate(Mathf.Clamp01(t));
        }

        public static float EvaluateUnclamped(float t)
        {
            return linear.Evaluate(t);
        }

        public static float EvaluateUnclamped(float t, IEasingFunction easingFunction)
        {
            return easingFunction.Evaluate(t);
        }
    }
}
