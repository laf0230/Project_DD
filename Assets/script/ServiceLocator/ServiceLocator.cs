using UnityEngine;
using System.Collections.Generic;
using System;

namespace ServiceLocator
{
    public static class Locator
    {
        private static Dictionary<Type, object> serviceLocator = new();

        public static void Subscribe(object subcriber)
        {
            if(serviceLocator.TryGetValue(subcriber.GetType(), out object value))
                    Debug.LogError($"Already Subcribed {subcriber.ToString()}");

            serviceLocator[subcriber.GetType()] = subcriber;
        }

        public static T Get<T>() where T : class
        {
            if(!serviceLocator.TryGetValue(typeof(T), out object value))
            {
                throw new NullReferenceException();
            }

            return (T)value;
        }
    }
}
