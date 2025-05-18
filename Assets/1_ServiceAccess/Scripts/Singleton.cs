using System;
using UnityEngine;

namespace Universal.Singletons
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T instance { get; protected set; }
        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}