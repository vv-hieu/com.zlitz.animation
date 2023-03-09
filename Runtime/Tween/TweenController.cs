using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Zlitz.Utilities;

namespace Zlitz.Animation
{
    public class TweenController : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private int m_tweenInstanceCount;

        private List<TweenInstance> m_tweenInstances;

        public void Enqueue(TweenInstance tweenInstance)
        {
            if (m_tweenInstances == null)
            {
                m_tweenInstances = new List<TweenInstance>();
            }
            m_tweenInstances.Add(tweenInstance);
        }

        public void Cancel(TweenId id)
        {
            if (m_tweenInstances != null)
            {
                m_tweenInstances = m_tweenInstances.Where(i => i != null && i.id.id != id.id).ToList();
            }
        }

        private void Update()
        {
            if (m_tweenInstances != null)
            {
                List<TweenInstance> additional = new List<TweenInstance>();
                foreach (TweenInstance tweenInstance in m_tweenInstances)
                {
                    tweenInstance.OnUpdate(Time.deltaTime);
                    if (tweenInstance.needRemoved && !tweenInstance.canceled && tweenInstance.next != null)
                    {
                        additional.AddRange(tweenInstance.next);
                    }
                }
                m_tweenInstances = m_tweenInstances.Where(i => !i.needRemoved).ToList();
                m_tweenInstances.AddRange(additional);

                m_tweenInstanceCount = m_tweenInstances.Count;
            }
        }
    }
}