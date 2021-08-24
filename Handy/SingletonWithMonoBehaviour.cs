using UnityEngine;

namespace Handy
{
    public class SingletonWithMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = GameObject.Find(typeof(T).Name);
                    if (obj == null)
                    {
                        obj = new GameObject(typeof(T).Name);
                        instance = obj.AddComponent<T>();
                    }
                    else
                    {
                        instance = obj.GetComponent<T>();
                    }
                }

                return instance;
            }
        }
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

