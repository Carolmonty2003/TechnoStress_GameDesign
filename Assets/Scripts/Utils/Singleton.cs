using UnityEngine;

namespace Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T instance;
        public static T Instance { get { return instance; } }

        protected void InitSingleton()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this as T;
        }
    }
}