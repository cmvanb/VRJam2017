using UnityEngine;

namespace AltSrc.UnityCommon.Patterns
{
    /// <summary>
    /// Singleton pattern ensures that there is only ever one single instance of a type which is
    /// globally accessible. Although this may be useful in a few specific cases, the singleton
    /// pattern can easily become an anti-pattern when overused in large scale projects. Beware!
    /// To use this pattern, have the class that should be singleton inherit this one. This
    /// implementation of the pattern requires the derived class to have the type MonoBehaviour.
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Gets the instance. If the instance does not exist it will create it.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed on application quit." +
                        " Won't create again - returning null.");
                    return null;
                }

                lock (__lock)
                {
                    if (instance == null)
                    {
                        instance = (T)Object.FindObjectOfType(typeof(T));

                        if (Object.FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return instance;
                        }

                        if (instance == null)
                        {
                            GameObject singleton = new GameObject();
                            instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();

                            Object.DontDestroyOnLoad(singleton);

                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                " is needed in the scene, so '" + singleton +
                                "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " +
                                instance.gameObject.name);
                        }
                    }

                    return instance;
                }
            }
        }

        private static T instance;

        private static object __lock = new object();

        private static bool applicationIsQuitting = false;

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
