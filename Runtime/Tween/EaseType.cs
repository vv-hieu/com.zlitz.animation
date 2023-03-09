using System;
using UnityEngine;

namespace Zlitz.Animation
{
    [Serializable]
    public class EaseFunction : IEasingFunction
    {
        [SerializeField]
        private Type m_type;

        [SerializeField]
        private CustomEasingFunction m_custom;

        public Type type
        {
            get => m_type;
            set => m_type = value;
        }
        public CustomEasingFunction custom
        {
            get => m_custom;
            set => m_custom = value;
        }

        public IEasingFunction Get()
        {
            switch (m_type)
            {
                case Type.Linear:
                    return Easing.linear;
                case Type.InSine:
                    return Easing.inSine;
                case Type.OutSine:
                    return Easing.outSine;
                case Type.InOutSine:
                    return Easing.inOutSine;
                case Type.InQuad:
                    return Easing.inQuad;
                case Type.OutQuad:
                    return Easing.outQuad;
                case Type.InOutQuad:
                    return Easing.inOutQuad;
                case Type.InCubic:
                    return Easing.inCubic;
                case Type.OutCubic:
                    return Easing.outCubic;
                case Type.InOutCubic:
                    return Easing.inOutCubic;
                case Type.InQuart:
                    return Easing.inQuart;
                case Type.OutQuart:
                    return Easing.outQuart;
                case Type.InOutQuart:
                    return Easing.inOutQuart;
                case Type.InQuint:
                    return Easing.inQuint;
                case Type.OutQuint:
                    return Easing.outQuint;
                case Type.InOutQuint:
                    return Easing.inOutQuint;
                case Type.InExpo:
                    return Easing.inExpo;
                case Type.OutExpo:
                    return Easing.outExpo;
                case Type.InOutExpo:
                    return Easing.inOutExpo;
                case Type.InCirc:
                    return Easing.inCirc;
                case Type.OutCirc:
                    return Easing.outCirc;
                case Type.InOutCirc:
                    return Easing.inOutCirc;
                case Type.InBack:
                    return Easing.inBack;
                case Type.OutBack:
                    return Easing.outBack;
                case Type.InOutBack:
                    return Easing.inOutBack;
                case Type.InElastic:
                    return Easing.inElastic;
                case Type.OutElastic:
                    return Easing.outElastic;
                case Type.InOutElastic:
                    return Easing.inOutElastic;
                case Type.InBounce:
                    return Easing.inBounce;
                case Type.OutBounce:
                    return Easing.outBounce;
                case Type.InOutBounce:
                    return Easing.inOutBounce;
            }
            return m_custom == null ? Easing.linear : m_custom;
        }

        public float Evaluate(float t)
        {
            return Get().Evaluate(t);
        }

        public enum Type
        {
            Linear,
            InSine,
            OutSine,
            InOutSine,
            InQuad,
            OutQuad,
            InOutQuad,
            InCubic,
            OutCubic,
            InOutCubic,
            InQuart,
            OutQuart,
            InOutQuart,
            InQuint,
            OutQuint,
            InOutQuint,
            InExpo,
            OutExpo,
            InOutExpo,
            InCirc,
            OutCirc,
            InOutCirc,
            InBack,
            OutBack,
            InOutBack,
            InElastic,
            OutElastic,
            InOutElastic,
            InBounce,
            OutBounce,
            InOutBounce,
            Custom
        }
    }

    public abstract class CustomEasingFunction : ScriptableObject, IEasingFunction
    {
        public abstract float Evaluate(float t);
    }
}
