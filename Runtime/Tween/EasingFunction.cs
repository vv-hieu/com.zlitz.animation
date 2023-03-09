using UnityEngine;

namespace Zlitz.Animation
{
    public interface IEasingFunction
    {
        float Evaluate(float t);
    }

    public sealed class LinearEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            return t;
        }
    }

    public sealed class InSineEasingFunction : IEasingFunction
    {
        private static readonly float s_PiOver2 = Mathf.PI * 0.5f;

        public float Evaluate(float t)
        {
            return 1.0f - Mathf.Cos(t * s_PiOver2);
        }
    }

    public sealed class OutSineEasingFunction : IEasingFunction
    {
        private static readonly float s_PiOver2 = Mathf.PI * 0.5f;

        public float Evaluate(float t)
        {
            return Mathf.Sin(t * s_PiOver2);
        }
    }

    public sealed class InOutSineEasingFunction : IEasingFunction
    {
        private static readonly float s_Pi = Mathf.PI;

        public float Evaluate(float t)
        {
            return 0.5f - 0.5f * Mathf.Cos(t * s_Pi);
        }
    }

    public sealed class InPowerEasingFunction : IEasingFunction
    {
        private float m_power;

        public float Evaluate(float t)
        {
            return Mathf.Pow(t, m_power);
        }

        public InPowerEasingFunction(float power)
        {
            m_power = power;
        }
    }

    public sealed class OutPowerEasingFunction : IEasingFunction
    {
        private float m_power;

        public float Evaluate(float t)
        {
            return 1.0f - Mathf.Pow(1.0f - t, m_power);
        }

        public OutPowerEasingFunction(float power)
        {
            m_power = power;
        }
    }

    public sealed class InOutPowerEasingFunction : IEasingFunction
    {
        private float m_power;

        public float Evaluate(float t)
        {
            return (t < 0.5f) ? (0.5f * Mathf.Pow(2.0f * t, m_power)) : (1.0f - 0.5f * Mathf.Pow(2.0f - 2.0f * t, m_power));
        }

        public InOutPowerEasingFunction(float power)
        {
            m_power = power;
        }
    }

    public sealed class InExpoEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            return (t <= 0.0f) ? (0.0f) : Mathf.Pow(2.0f, 10.0f * (t - 1.0f));
        }
    }

    public sealed class OutExpoEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            return (t >= 1.0f) ? (1.0f) : (1.0f - Mathf.Pow(2.0f, t * -10.0f));
        }
    }

    public sealed class InOutExpoEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            return (t < 0.5f)
                ? (t <= 0.0f) ? (0.0f) : (Mathf.Pow(2.0f, t * 20.0f - 11.0f))
                : (t >= 1.0f) ? (1.0f) : (1.0f - Mathf.Pow(2.0f, t * -20.0f + 9.0f));
        }
    }

    public sealed class InCircEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            return 1.0f - Mathf.Sqrt(1.0f - t * t);
        }
    }

    public sealed class OutCircEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            float a = 1.0f - t;
            return Mathf.Sqrt(1.0f - a * a);
        }
    }

    public sealed class InOutCircEasingFunction : IEasingFunction
    {
        public float Evaluate(float t)
        {
            float a = 2.0f - t * 2.0f;
            return (t < 0.5f) ? (0.5f * (1.0f - Mathf.Sqrt(1.0f - 4.0f * t * t))) : (0.5f * (1.0f + Mathf.Sqrt(1.0f - a * a)));
        }
    }

    public sealed class InBackEasingFunction : IEasingFunction
    {
        private static readonly float s_a = 1.70158f;
        private static readonly float s_b = s_a + 1.0f;

        public float Evaluate(float t)
        {
            return t * t * (s_b * t - s_a);
        }
    }

    public sealed class OutBackEasingFunction : IEasingFunction
    {
        private static readonly float s_a = 1.70158f;
        private static readonly float s_b = s_a + 1.0f;

        public float Evaluate(float t)
        {
            float a = t - 1.0f;
            return 1.0f + a * a * (s_b * a + s_a);
        }
    }

    public sealed class InOutBackEasingFunction : IEasingFunction
    {
        private static readonly float s_a = 1.70158f;
        private static readonly float s_b = s_a * 1.525f;

        public float Evaluate(float t)
        {
            float a = t - 1.0f;
            return (t < 0.5f) ? (2.0f * t * t * (2.0f * t * (s_b + 1.0f) - s_b)) : (2.0f * a * a * (2.0f * a * (s_b + 1.0f) + s_b) + 1.0f);
        }
    }

    public sealed class InElasticEasingFunction : IEasingFunction
    {
        private static readonly float s_2piOver3 = 2.0f * Mathf.PI / 3.0f;

        public float Evaluate(float t)
        {
            return (t <= 0.0f) ? (0.0f) : (t >= 1.0f) ? (1.0f) : (-Mathf.Pow(2.0f, 10.0f * t - 10.0f) * Mathf.Sin(s_2piOver3 * (10.0f * t - 10.75f)));
        }
    }

    public sealed class OutElasticEasingFunction : IEasingFunction
    {
        private static readonly float s_2piOver3 = 2.0f * Mathf.PI / 3.0f;

        public float Evaluate(float t)
        {
            return (t <= 0.0f) ? (0.0f) : (t >= 1.0f) ? (1.0f) : (Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin(s_2piOver3 * (10.0f * t - 0.75f)) + 1.0f);
        }
    }

    public sealed class InOutElasticEasingFunction : IEasingFunction
    {
        private static readonly float s_2piOver45 = 2.0f * Mathf.PI / 4.5f;

        public float Evaluate(float t)
        {
            return (t < 0.5f)
                ? (t <= 0.0f) ? (0.0f) : (-Mathf.Pow(2.0f, 20.0f * t - 11.0f) * Mathf.Sin(s_2piOver45 * (20.0f * t - 11.125f)))
                : (t >= 1.0f) ? (1.0f) : (Mathf.Pow(2.0f, -20.0f * t + 9.0f) * Mathf.Sin(s_2piOver45 * (20.0f * t - 11.125f)) + 1.0f);
        }
    }

    public sealed class InBounceEasingFunction : IEasingFunction
    {
        private static readonly float s_n = 7.5625f;
        private static readonly float s_d = 2.75f;
        private static readonly float s_i = 1.0f / s_d;

        public float Evaluate(float t)
        {
            t = 1.0f - t;
            return 1.0f - ((t < s_i)
                ? (s_n * t * t)
                : (t < 2.0f * s_i)
                ? (s_n * (t -= 1.5f * s_i) * t + 0.75f)
                : (t < 2.5f * s_i)
                ? (s_n * (t -= 2.25f * s_i) * t + 0.9375f)
                : (s_n * (t -= 2.625f * s_i) * t + 0.984375f));
        }
    }

    public sealed class OutBounceEasingFunction : IEasingFunction
    {
        private static readonly float s_n = 7.5625f;
        private static readonly float s_d = 2.75f;
        private static readonly float s_i = 1.0f / s_d;

        public float Evaluate(float t)
        {
            return (t < s_i)
                ? (s_n * t * t)
                : (t < 2.0f * s_i)
                ? (s_n * (t -= 1.5f * s_i) * t + 0.75f)
                : (t < 2.5f * s_i)
                ? (s_n * (t -= 2.25f * s_i) * t + 0.9375f)
                : (s_n * (t -= 2.625f * s_i) * t + 0.984375f);
        }
    }

    public sealed class InOutBounceEasingFunction : IEasingFunction
    {
        private static readonly float s_n = 7.5625f;
        private static readonly float s_d = 2.75f;
        private static readonly float s_i = 1.0f / s_d;

        public float Evaluate(float t)
        {
            float s = 0.5f;
            if (t < 0.5f)
            {
                s = -0.5f;
                t = 1.0f - 2.0f * t;
            }
            else
            {
                t = 2.0f * t - 1.0f;
            }

            return 0.5f + s * ((t < s_i)
                ? (s_n * t * t)
                : (t < 2.0f * s_i)
                ? (s_n * (t -= 1.5f * s_i) * t + 0.75f)
                : (t < 2.5f * s_i)
                ? (s_n * (t -= 2.25f * s_i) * t + 0.9375f)
                : (s_n * (t -= 2.625f * s_i) * t + 0.984375f));
        }
    }
}
