using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class Info<T1> : MonoBehaviour
    {
        protected Dictionary<System.Type, IInitializable<T1>> components;

        protected virtual void Start()
        {
            var self = GetComponent<T1>(); // Easiest way to cast this to its subclass.
            components = new Dictionary<System.Type, IInitializable<T1>>();
            foreach (var component in GetComponents<IInitializable<T1>>())
            {
                components.Add(component.GetType(), component);
                component.Init(self);
            }
        }

        public new T2 GetComponent<T2>()
        {
            return (T2)components.GetValueOrDefault(typeof(T2), null);
        }
    }
}