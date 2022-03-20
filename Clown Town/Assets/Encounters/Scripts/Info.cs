using System.Collections.Generic;
using UnityEngine;
using System;

namespace Encounters
{
    public class Info<T1> : MonoBehaviour
    {
        [SerializeField]
        private bool initOnAwake;

        protected Dictionary<Type, IInitializable<T1>> components;

        protected virtual void Awake()
        {
            if (initOnAwake) Init();
        }

        public virtual void Init()
        {
            var self = gameObject.GetComponent<T1>(); // Easiest way to cast this to its subclass.
            components = new Dictionary<Type, IInitializable<T1>>();
            foreach (var component in GetComponents<IInitializable<T1>>())
            {
                components.Add(component.GetType(), component);
                
            }

            foreach (var item in components)
            {
                item.Value.Init(self);
            }
        }

        public new T2 GetComponent<T2>()
        {
            if (false == components.TryGetValue(typeof(T2), out var component))
            {
                return default(T2);
            } 
            else
            {
                return (T2)component;
            }
        }
    }
}