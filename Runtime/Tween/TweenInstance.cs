using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Zlitz.Utilities;

namespace Zlitz.Animation
{
    public class TweenInstance
    {
        private TweenController     m_controller;
        private List<TweenInstance> m_children;
        private TweenInstance       m_next;
        private TweenInstance       m_parent;
        private Callback            m_callback;
        private float               m_delay;
        private TweenId             m_tweenId;

        public TweenId id => m_tweenId;
        public bool done { get; protected set; } = false;
        public bool canceled { get; private set; } = false;
        public bool needRemoved { get; private set; } = false;
        public TweenInstance root
        {
            get
            {
                if (m_parent == null)
                {
                    return this;
                }
                return m_parent.root;
            }
        }

        public TweenInstance[] next
        {
            get
            {
                List<TweenInstance> res = new List<TweenInstance>(m_children.Select(c => c.m_next));
                res.Add(m_next);
                return res.Where(t => t != null).ToArray();
            }
        }

        protected TweenController controller => m_controller;

        public TransformTweenInstance Use(Transform target, float delay = 0.0f)
        {
            TransformTweenInstance res = new TransformTweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public MaterialTweenInstance Use(Material target, float delay = 0.0f)
        {
            MaterialTweenInstance res = new MaterialTweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public FloatTweenInstance Use(ValueReference<float> target, float delay = 0.0f)
        {
            FloatTweenInstance res = new FloatTweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public Vector2TweenInstance Use(ValueReference<Vector2> target, float delay = 0.0f)
        {
            Vector2TweenInstance res = new Vector2TweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public Vector3TweenInstance Use(ValueReference<Vector3> target, float delay = 0.0f)
        {
            Vector3TweenInstance res = new Vector3TweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public Vector4TweenInstance Use(ValueReference<Vector4> target, float delay = 0.0f)
        {
            Vector4TweenInstance res = new Vector4TweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public QuaternionTweenInstance Use(ValueReference<Quaternion> target, float delay = 0.0f)
        {
            QuaternionTweenInstance res = new QuaternionTweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public ColorTweenInstance Use(ValueReference<Color> target, float delay = 0.0f)
        {
            ColorTweenInstance res = new ColorTweenInstance(m_controller, m_tweenId, delay, target);
            m_children.Add(res);
            res.m_parent = this;
            return res;
        }

        public TweenInstance Do(Callback callback)
        {
            m_callback += callback;
            return this;
        }

        public TweenInstance Then(TweenInstance next)
        {
            m_next = next.root;
            if (m_next != null)
            {
                m_next.m_tweenId = m_tweenId;
                m_next.p_ApplyNewId();
            }
            return this;
        }

        public void Cancel(bool triggerCallback = false)
        {
            needRemoved = true;
            if (!done && triggerCallback)
            {
                m_callback?.Invoke();
            }
            done = true;
            canceled = true;
        }

        public void OnUpdate(float dt)
        {
            if (IsValid())
            {
                if (m_delay > 0.0f)
                {
                    m_delay -= dt;
                }
                else
                {
                    if (!done)
                    {
                        Update(dt);
                    }
                    bool childrenComplete = true;
                    foreach (TweenInstance child in m_children)
                    {
                        child.OnUpdate(dt);
                        if (!child.needRemoved)
                        {
                            childrenComplete = false;
                        }
                    }
                    if (childrenComplete && done)
                    {
                        needRemoved = true;
                        m_callback?.Invoke();
                    }
                }
            }
            else
            {
                needRemoved = true;
            }
        }

        protected virtual void Update(float dt)
        {
        }

        protected virtual bool IsValid()
        {
            return true;
        }

        protected void AddChild(TweenInstance child)
        {
            m_children.Add(child);
        }

        public TweenInstance(TweenController controller, TweenId id, float delay)
        {
            m_controller = controller;
            m_children   = new List<TweenInstance>();
            m_next       = null;
            m_callback   = null;
            m_delay      = delay;
            m_tweenId    = id;

            done        = false;
            needRemoved = false;
        }

        private void p_ApplyNewId()
        {
            foreach (TweenInstance child in m_children)
            {
                child.m_tweenId = m_tweenId;
                child.p_ApplyNewId();
            }
        }

        public delegate void Callback();
    }

    public class TransformTweenInstance : TweenInstance
    {
        private Transform m_target;

        private ValueReference<Vector3>    m_position;
        private ValueReference<Quaternion> m_rotation;
        private ValueReference<Vector3>    m_scale;

        public TransformTweenInstance Translate(Vector3 to, float time, float delay = 0.0f)
        {
            return Translate(to, time, Easing.linear, delay);
        }

        public TransformTweenInstance Translate(Vector3 from, Vector3 to, float time, float delay = 0.0f)
        {
            return Translate(from, to, time, Easing.linear, delay);
        }

        public TransformTweenInstance Translate(Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Translate(m_target.localPosition, to, time, easing, delay);
        }

        public TransformTweenInstance Translate(Vector3 from, Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector3TweenInstance(controller, id, delay, m_position).Value(from, to, time, easing));
            return this;
        }

        public TransformTweenInstance Rotate(Quaternion to, float time, float delay = 0.0f)
        {
            return Rotate(m_target.localRotation, to, time, Easing.linear, delay);
        }

        public TransformTweenInstance Rotate(Quaternion from, Quaternion to, float time, float delay = 0.0f)
        {
            return Rotate(from, time, Easing.linear, delay);
        }

        public TransformTweenInstance Rotate(Quaternion to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Rotate(m_target.localRotation, to, time, easing, delay);
        }

        public TransformTweenInstance Rotate(Quaternion from, Quaternion to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new QuaternionTweenInstance(controller, id, delay, m_rotation).Value(from, to, time, easing));
            return this;
        }

        public TransformTweenInstance Scale(Vector3 to, float time, float delay = 0.0f)
        {
            return Scale(to, time, Easing.linear, delay);
        }

        public TransformTweenInstance Scale(Vector3 from, Vector3 to, float time, float delay = 0.0f)
        {
            return Scale(from, to, time, Easing.linear, delay);
        }

        public TransformTweenInstance Scale(Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Scale(m_target.localScale, to, time, easing, delay);
        }

        public TransformTweenInstance Scale(Vector3 from, Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector3TweenInstance(controller, id, delay, m_scale).Value(from, to, time, easing));
            return this;
        }

        protected override void Update(float dt)
        {
            done = true;
        }

        protected override bool IsValid()
        {
            return m_target != null;
        }

        public TransformTweenInstance(TweenController controller, TweenId id, float delay, Transform target) : base(controller, id, delay)
        {
            m_target = target;

            m_position = new ValueReference<Vector3>(() => m_target.localPosition, (value) => m_target.localPosition = value);
            m_rotation = new ValueReference<Quaternion>(() => m_target.localRotation, (value) => m_target.localRotation = value);
            m_scale    = new ValueReference<Vector3>(() => m_target.localScale, (value) => m_target.localScale = value);
        }
    }

    public class MaterialTweenInstance : TweenInstance
    {
        private Material m_target;

        public MaterialTweenInstance Property(string name, float to, float time, float delay = 0.0f)
        {
            return Property(name, m_target.GetFloat(name), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, float from, float to, float time, float delay = 0.0f)
        {
            return Property(name, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, float to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(name, m_target.GetFloat(name), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(string name, float from, float to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new FloatTweenInstance(controller, id, delay, new ValueReference<float>(() => m_target.GetFloat(name), (value) => m_target.SetFloat(name, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(string name, Vector2 to, float time, float delay = 0.0f)
        {
            return Property(name, m_target.GetVector2(name), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Vector2 from, Vector2 to, float time, float delay = 0.0f)
        {
            return Property(name, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Vector2 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(name, m_target.GetVector2(name), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(string name, Vector2 from, Vector2 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector2TweenInstance(controller, id, delay, new ValueReference<Vector2>(() => m_target.GetVector(name), (value) => m_target.SetVector(name, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(string name, Vector3 to, float time, float delay = 0.0f)
        {
            return Property(name, m_target.GetVector3(name), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Vector3 from, Vector3 to, float time, float delay = 0.0f)
        {
            return Property(name, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(name, m_target.GetVector3(name), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(string name, Vector3 from, Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector3TweenInstance(controller, id, delay, new ValueReference<Vector3>(() => m_target.GetVector(name), (value) => m_target.SetVector(name, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(string name, Vector4 to, float time, float delay = 0.0f)
        {
            return Property(name, m_target.GetVector4(name), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Vector4 from, Vector4 to, float time, float delay = 0.0f)
        {
            return Property(name, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Vector4 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(name, m_target.GetVector4(name), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(string name, Vector4 from, Vector4 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector4TweenInstance(controller, id, delay, new ValueReference<Vector4>(() => m_target.GetVector(name), (value) => m_target.SetVector(name, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(string name, Color to, float time, float delay = 0.0f)
        {
            return Property(name, m_target.GetColor(name), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Color from, Color to, float time, float delay = 0.0f)
        {
            return Property(name, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(string name, Color to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(name, m_target.GetColor(name), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(string name, Color from, Color to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new ColorTweenInstance(controller, id, delay, new ValueReference<Color>(() => m_target.GetVector(name), (value) => m_target.SetVector(name, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(int nameId, float to, float time, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetFloat(nameId), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, float from, float to, float time, float delay = 0.0f)
        {
            return Property(nameId, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, float to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetFloat(nameId), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(int nameId, float from, float to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new FloatTweenInstance(controller, id, delay, new ValueReference<float>(() => m_target.GetFloat(nameId), (value) => m_target.SetFloat(nameId, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(int nameId, Vector2 to, float time, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetVector2(nameId), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector2 from, Vector2 to, float time, float delay = 0.0f)
        {
            return Property(nameId, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector2 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetVector2(nameId), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector2 from, Vector2 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector2TweenInstance(controller, id, delay, new ValueReference<Vector2>(() => m_target.GetVector(nameId), (value) => m_target.SetVector(nameId, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(int nameId, Vector3 to, float time, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetVector3(nameId), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector3 from, Vector3 to, float time, float delay = 0.0f)
        {
            return Property(nameId, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetVector3(nameId), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector3 from, Vector3 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector3TweenInstance(controller, id, delay, new ValueReference<Vector3>(() => m_target.GetVector(nameId), (value) => m_target.SetVector(nameId, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(int nameId, Vector4 to, float time, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetVector4(nameId), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector4 from, Vector4 to, float time, float delay = 0.0f)
        {
            return Property(nameId, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector4 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetVector4(nameId), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(int nameId, Vector4 from, Vector4 to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new Vector4TweenInstance(controller, id, delay, new ValueReference<Vector4>(() => m_target.GetVector(nameId), (value) => m_target.SetVector(nameId, value))).Value(from, to, time, easing));
            return this;
        }

        public MaterialTweenInstance Property(int nameId, Color to, float time, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetColor(nameId), to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Color from, Color to, float time, float delay = 0.0f)
        {
            return Property(nameId, from, to, time, Easing.linear, delay);
        }

        public MaterialTweenInstance Property(int nameId, Color to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            return Property(nameId, m_target.GetColor(nameId), to, time, easing, delay);
        }

        public MaterialTweenInstance Property(int nameId, Color from, Color to, float time, IEasingFunction easing, float delay = 0.0f)
        {
            AddChild(new ColorTweenInstance(controller, id, delay, new ValueReference<Color>(() => m_target.GetVector(nameId), (value) => m_target.SetVector(nameId, value))).Value(from, to, time, easing));
            return this;
        }

        protected override void Update(float dt)
        {
            done = true;
        }

        protected override bool IsValid()
        {
            return m_target != null;
        }

        public MaterialTweenInstance(TweenController controller, TweenId id, float delay, Material target) : base(controller, id, delay)
        {
            m_target = target;
        }
    }

    public class FloatTweenInstance : TweenInstance
    {
        private ValueReference<float> m_target;
        private float                 m_time = 0.0f;

        private float           m_from;
        private float           m_to;
        private float           m_inverseDuration;
        private IEasingFunction m_easing;

        public FloatTweenInstance Value(float to, float time)
        {
            return Value(m_target.value, to, time, Easing.linear);
        }

        public FloatTweenInstance Value(float from, float to, float time)
        {
            return Value(from, to, time, Easing.linear);
        }

        public FloatTweenInstance Value(float to, float time, IEasingFunction easing)
        {
            return Value(m_target.value, to, time, easing);
        }

        public FloatTweenInstance Value(float from, float to, float time, IEasingFunction easing)
        {
            m_from   = from;
            m_to     = to;
            m_easing = easing;

            m_inverseDuration = 1.0f / Mathf.Max(0.0001f, time);

            return this;
        }

        protected override void Update(float dt)
        {
            m_time += dt;
            float progress = m_time * m_inverseDuration;

            m_target.value = s_Lerp(m_from, m_to, progress, m_easing);

            done = progress >= 1.0f;
        }

        public FloatTweenInstance(TweenController controller, TweenId id, float delay, ValueReference<float> target) : base(controller, id, delay)
        {
            m_target = target;
        }

        private static float s_Lerp(float from, float to, float progress, IEasingFunction easing)
        {
            return Mathf.LerpUnclamped(from, to, Easing.Evaluate(progress, easing));
        }
    }

    public class Vector2TweenInstance : TweenInstance
    {
        private ValueReference<Vector2> m_target;
        private float                   m_time = 0.0f;

        private Vector2         m_from;
        private Vector2         m_to;
        private float           m_inverseDuration;
        private IEasingFunction m_easing;

        public Vector2TweenInstance Value(Vector2 to, float time)
        {
            return Value(m_target.value, to, time, Easing.linear);
        }

        public Vector2TweenInstance Value(Vector2 from, Vector2 to, float time)
        {
            return Value(from, to, time, Easing.linear);
        }

        public Vector2TweenInstance Value(Vector2 to, float time, IEasingFunction easing)
        {
            return Value(m_target.value, to, time, easing);
        }

        public Vector2TweenInstance Value(Vector2 from, Vector2 to, float time, IEasingFunction easing)
        {
            m_from   = from;
            m_to     = to;
            m_easing = easing;

            m_inverseDuration = 1.0f / Mathf.Max(0.0001f, time);

            return this;
        }

        protected override void Update(float dt)
        {
            m_time += dt;
            float progress = m_time * m_inverseDuration;

            m_target.value = s_Lerp(m_from, m_to, progress, m_easing);

            done = progress >= 1.0f;
        }

        public Vector2TweenInstance(TweenController controller, TweenId id, float delay, ValueReference<Vector2> target) : base(controller, id, delay)
        {
            m_target = target;
        }

        private static Vector2 s_Lerp(Vector2 from, Vector2 to, float progress, IEasingFunction easing)
        {
            return Vector2.LerpUnclamped(from, to, Easing.Evaluate(progress, easing));
        }
    }

    public class Vector3TweenInstance : TweenInstance
    {
        private ValueReference<Vector3> m_target;
        private float                   m_time = 0.0f;

        private Vector3         m_from;
        private Vector3         m_to;
        private float           m_inverseDuration;
        private IEasingFunction m_easing;

        public Vector3TweenInstance Value(Vector3 to, float time)
        {
            return Value(m_target.value, to, time, Easing.linear);
        }

        public Vector3TweenInstance Value(Vector3 from, Vector3 to, float time)
        {
            return Value(from, to, time, Easing.linear);
        }

        public Vector3TweenInstance Value(Vector3 to, float time, IEasingFunction easing)
        {
            return Value(m_target.value, to, time, easing);
        }

        public Vector3TweenInstance Value(Vector3 from, Vector3 to, float time, IEasingFunction easing)
        {
            m_from   = from;
            m_to     = to;
            m_easing = easing;

            m_inverseDuration = 1.0f / Mathf.Max(0.0001f, time);

            return this;
        }

        protected override void Update(float dt)
        {
            m_time += dt;
            float progress = m_time * m_inverseDuration;

            m_target.value = s_Lerp(m_from, m_to, progress, m_easing);

            done = progress >= 1.0f;
        }

        public Vector3TweenInstance(TweenController controller, TweenId id, float delay, ValueReference<Vector3> target) : base(controller, id, delay)
        {
            m_target = target;
        }

        private static Vector3 s_Lerp(Vector3 from, Vector3 to, float progress, IEasingFunction easing)
        {
            return Vector3.LerpUnclamped(from, to, Easing.Evaluate(progress, easing));
        }
    }

    public class Vector4TweenInstance : TweenInstance
    {
        private ValueReference<Vector4> m_target;
        private float                   m_time = 0.0f;

        private Vector4         m_from;
        private Vector4         m_to;
        private float           m_inverseDuration;
        private IEasingFunction m_easing;

        public Vector4TweenInstance Value(Vector4 to, float time)
        {
            return Value(m_target.value, to, time, Easing.linear);
        }

        public Vector4TweenInstance Value(Vector4 from, Vector4 to, float time)
        {
            return Value(from, to, time, Easing.linear);
        }

        public Vector4TweenInstance Value(Vector4 to, float time, IEasingFunction easing)
        {
            return Value(m_target.value, to, time, easing);
        }

        public Vector4TweenInstance Value(Vector4 from, Vector4 to, float time, IEasingFunction easing)
        {
            m_from   = from;
            m_to     = to;
            m_easing = easing;

            m_inverseDuration = 1.0f / Mathf.Max(0.0001f, time);

            return this;
        }

        protected override void Update(float dt)
        {
            m_time += dt;
            float progress = m_time * m_inverseDuration;

            m_target.value = s_Lerp(m_from, m_to, progress, m_easing);

            done = progress >= 1.0f;
        }

        public Vector4TweenInstance(TweenController controller, TweenId id, float delay, ValueReference<Vector4> target) : base(controller, id, delay)
        {
            m_target = target;
        }

        private static Vector4 s_Lerp(Vector4 from, Vector4 to, float progress, IEasingFunction easing)
        {
            return Vector4.LerpUnclamped(from, to, Easing.Evaluate(progress, easing));
        }
    }

    public class QuaternionTweenInstance : TweenInstance
    {
        private ValueReference<Quaternion> m_target;
        private float                      m_time = 0.0f;

        private Quaternion      m_from;
        private Quaternion      m_to;
        private float           m_inverseDuration;
        private IEasingFunction m_easing;

        public QuaternionTweenInstance Value(Quaternion to, float time)
        {
            return Value(m_target.value, to, time, Easing.linear);
        }

        public QuaternionTweenInstance Value(Quaternion from, Quaternion to, float time)
        {
            return Value(from, to, time, Easing.linear);
        }

        public QuaternionTweenInstance Value(Quaternion to, float time, IEasingFunction easing)
        {
            return Value(m_target.value, to, time, easing);
        }

        public QuaternionTweenInstance Value(Quaternion from, Quaternion to, float time, IEasingFunction easing)
        {
            m_from   = from;
            m_to     = to;
            m_easing = easing;

            m_inverseDuration = 1.0f / Mathf.Max(0.0001f, time);

            return this;
        }

        protected override void Update(float dt)
        {
            m_time += dt;
            float progress = m_time * m_inverseDuration;

            m_target.value = s_Lerp(m_from, m_to, progress, m_easing);

            done = progress >= 1.0f;
        }

        public QuaternionTweenInstance(TweenController controller, TweenId id, float delay, ValueReference<Quaternion> target) : base(controller, id, delay)
        {
            m_target = target;
        }

        private static Quaternion s_Lerp(Quaternion from, Quaternion to, float progress, IEasingFunction easing)
        {
            return Quaternion.SlerpUnclamped(from, to, Easing.Evaluate(progress, easing));
        }
    }

    public class ColorTweenInstance : TweenInstance
    {
        private ValueReference<Color> m_target;
        private float                 m_time = 0.0f;

        private Color           m_from;
        private Color           m_to;
        private float           m_inverseDuration;
        private IEasingFunction m_easing;

        public ColorTweenInstance Value(Color to, float time)
        {
            return Value(m_target.value, to, time, Easing.linear);
        }

        public ColorTweenInstance Value(Color from, Color to, float time)
        {
            return Value(from, to, time, Easing.linear);
        }

        public ColorTweenInstance Value(Color to, float time, IEasingFunction easing)
        {
            return Value(m_target.value, to, time, easing);
        }

        public ColorTweenInstance Value(Color from, Color to, float time, IEasingFunction easing)
        {
            m_from   = from;
            m_to     = to;
            m_easing = easing;

            m_inverseDuration = 1.0f / Mathf.Max(0.0001f, time);

            return this;
        }

        protected override void Update(float dt)
        {
            m_time += dt;
            float progress = m_time * m_inverseDuration;

            m_target.value = s_Lerp(m_from, m_to, progress, m_easing);

            done = progress >= 1.0f;
        }

        public ColorTweenInstance(TweenController controller, TweenId id, float delay, ValueReference<Color> target) : base(controller, id, delay)
        {
            m_target = target;
        }

        private static Color s_Lerp(Color from, Color to, float progress, IEasingFunction easing)
        {
            return Color.LerpUnclamped(from, to, Easing.Evaluate(progress, easing));
        }
    }

    public struct TweenId
    {
        private int m_id;

        public int id => m_id;

        private static int s_currentId = 0;

        private TweenId(int id)
        {
            m_id = id;
        }

        public static TweenId Next()
        {
            if (s_currentId >= 65536)
            {
                s_currentId = 0;
            }
            return new TweenId(s_currentId++);
        }
    }
}
