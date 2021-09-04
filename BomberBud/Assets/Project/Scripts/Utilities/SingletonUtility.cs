using UnityEngine;

namespace xOrfe.Utilities
{
    public class SingletonUtility<T> : MonoBehaviour where T: SingletonUtility<T>
    {
        private static volatile T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;
                }
                return instance;
            }
            protected set
            {
                instance = value;
            }
        }
    }
}
