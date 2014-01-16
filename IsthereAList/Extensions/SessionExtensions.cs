using IsThereAList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace IsThereAList.Extensions
{
    //TODO: This needs expiration
    public static class SessionExtensions
    {
        private static object sync = new object();

        public static T GetOrStore<T>(this HttpSessionStateBase session, string key, Func<T> generator)
        {
            return session.GetOrStore(key, (session[key] == null && generator != null) ? generator() : default(T));
        }

        public static T GetOrStore<T>(this HttpSessionStateBase session, string key, T obj)
        {
            var result = session[key];
            if(result == null)
            {
                lock(sync)
                {
                    if(result == null)
                    {
                        result = obj != null ? obj : default(T);
                        session[key] = result;
                    }
                }
            }

            return (T)result;
        }

        public static T Get<T>(this HttpSessionStateBase session, string key)
        {
            var result = session[key] ?? default(T);
            return (T)result;
        }

        public static ApplicationUser GetCurrentUser(this HttpSessionStateBase session)
        {
            return Get<ApplicationUser>(session, "currentUser");
        }

        public static void Store<T>(this HttpSessionStateBase session, string key, T obj)
        {
            lock (sync)
            {
                var storageObj = obj != null ? obj : default(T);
                session[key] = storageObj;
            }
        }
    }
}