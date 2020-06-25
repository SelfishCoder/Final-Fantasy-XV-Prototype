using UnityEngine;

namespace SelfishCoder.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private static T instance;

        /// <summary>
        /// 
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<T>();
                    if (!instance)
                    {
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }

                }

                return instance;
            }
            private set
            {
                if (instance)
                {
                    Destroy(value.gameObject);
                    return;
                }
                instance = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Awake()
        {
            Instance = this as T;
            DontDestroyOnLoad(instance);
        }
    }
}