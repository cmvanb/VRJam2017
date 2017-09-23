// 2014-10-23

using System;

namespace AltSrc.UnityCommon.Patterns
{
    /// <summary>
    /// Singleton pattern ensures that there is only ever one single instance of a type which is
    /// globally accessible. Although this may be useful in a few specific cases, the singleton
    /// pattern can easily become an anti-pattern when overused in large scale projects. Beware!
    /// To use this pattern, have the class that should be singleton inherit this one. This
    /// implementation of the pattern requires the derived class to have a public parameterless
    /// constructor.
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// Gets the instance. If the instance does not exist it will create it.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }

        private static T instance;

        public Singleton()
        {
            if (instance != null)
            {
                throw new InvalidOperationException("can't have more than one instance of a class implementing singleton pattern");
            }
        }
    }
}