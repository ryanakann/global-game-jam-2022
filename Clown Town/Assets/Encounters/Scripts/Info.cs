using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Encounters
{
    public class Info<T1> : MonoBehaviour
    {
        protected Dictionary<System.Type, IInitializable<T1>> components;

        protected virtual void Start()
        {
            var self = gameObject.GetComponent<T1>(); // Easiest way to cast this to its subclass.
            components = new Dictionary<System.Type, IInitializable<T1>>();
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