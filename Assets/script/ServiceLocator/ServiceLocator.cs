using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace ServiceLocator
{
    public static class Locator
    {
        private static Dictionary<Type, object> services = new();
        private static Dictionary<object, List<Type>> pendingRequests = new();

        public static void Subscribe(object subcriber)
        {
            Type type = subcriber.GetType();

            if(services.TryGetValue(type, out object value))
                    Debug.LogError($"Already Subcribed {subcriber.ToString()}");

            services[type] = value;

            foreach(var entry in pendingRequests)
            {
                if(entry.Value.Contains(type))
                    entry.Value.Remove(type);
            }

            services[subcriber.GetType()] = subcriber;
        }

        public static T Get<T>(object requester) where T : class
        {
            Type t = typeof(T);

            if(services.TryGetValue(t, out object value))
            {
                return value as T;
            }

            if(!pendingRequests.TryGetValue(requester, out var list))
            {
                list = new List<Type>();
                pendingRequests.Add(requester, list);
            }

            if(!list.Contains(t))
            {
                list.Add(t);
            }

            Debug.LogWarning($"{requester} request missing locator {t}");
            return null;
        }
    }
}
