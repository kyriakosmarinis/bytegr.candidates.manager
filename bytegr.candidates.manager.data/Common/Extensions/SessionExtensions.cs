using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace bytegr.candidates.manager.web.Extensions
{
    public static class SessionExtensions
    {
        public static void SerializeObject<T>(this ISession session, string key, T value) {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? DeserializeObject<T>(this ISession session, string key) {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void AddToSessionList<T>(this ISession session, string key, T item) where T : class {
            List<T> itemList = session.DeserializeObject<List<T>>(key) ?? new List<T>();

            if (!itemList.Contains(item)) {
                itemList.Add(item);
                session.SerializeObject(key, itemList);
            }
        }

        public static void RemoveFromSessionList<T>(this ISession session, string key, T item) where T : class {
            List<T> itemList = session.DeserializeObject<List<T>>(key) ?? new List<T>();

            if (itemList.Contains(item)) {
                itemList.Remove(item);
                session.SerializeObject(key, itemList);
            }
        }

        public static void ClearSessionList<T>(this ISession session, string key) where T : class {
            session.Remove(key);
        }
    }
}

