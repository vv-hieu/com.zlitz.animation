using UnityEngine;

using Zlitz.Utilities;

namespace Zlitz.Animation
{
    public static class Tween
    {
        private static GameObject s_controllerObject;

        private static GameObject controllerObject
        {
            get
            {
                if (s_controllerObject == null)
                {
                    s_controllerObject = PersistentGameObject.NewObject("Tween Controller");
                    s_controllerObject.AddComponent<TweenController>();
                }
                return s_controllerObject;
            }
        }

        private static TweenController controller => controllerObject.GetComponent<TweenController>();

        public static TweenId Start(TweenInstance tweenInstance)
        {
            controller.Enqueue(tweenInstance.root);
            return tweenInstance.root.id;
        }

        public static void Cancel(TweenId tweenId)
        {
            controller.Cancel(tweenId);
        }

        public static TransformTweenInstance Use(Transform target, float delay = 0.0f)
        {
            TransformTweenInstance res = new TransformTweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static MaterialTweenInstance Use(Material target, float delay = 0.0f)
        {
            MaterialTweenInstance res = new MaterialTweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static FloatTweenInstance Use(ValueReference<float> target, float delay = 0.0f)
        {
            FloatTweenInstance res = new FloatTweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static Vector2TweenInstance Use(ValueReference<Vector2> target, float delay = 0.0f)
        {
            Vector2TweenInstance res = new Vector2TweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static Vector3TweenInstance Use(ValueReference<Vector3> target, float delay = 0.0f)
        {
            Vector3TweenInstance res = new Vector3TweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static Vector4TweenInstance Use(ValueReference<Vector4> target, float delay = 0.0f)
        {
            Vector4TweenInstance res = new Vector4TweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static QuaternionTweenInstance Use(ValueReference<Quaternion> target, float delay = 0.0f)
        {
            QuaternionTweenInstance res = new QuaternionTweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }

        public static ColorTweenInstance Use(ValueReference<Color> target, float delay = 0.0f)
        {
            ColorTweenInstance res = new ColorTweenInstance(controller, TweenId.Next(), delay, target);
            return res;
        }
    }
}