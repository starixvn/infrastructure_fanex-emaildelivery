using System;
using System.Configuration;
using Couchbase;
using Couchbase.Core;

namespace Fanex.MailStation.Admin.Service.Cache
{
    public static class Caching
    {
        private static IBucket bucket { get; set; }

        public static void Register()
        {
            if (bucket != null)
                return;
            
            var cluster = new Cluster("couchbaseClients/couchbase");
            var couchbaseClientBucket = ConfigurationManager.AppSettings["CouchbaseClient.Bucket"] ?? "MailStation";
            bucket = cluster.OpenBucket(couchbaseClientBucket);
        }

        public static void Set<T>(string key, T value, TimeSpan timeout)
        {
            if (bucket == null)
            {
                Register();
            }

            bucket.Upsert<T>(key, value, timeout);
        }

        public static void Set<T>(string key, T value)
        {
            if (bucket == null)
            {
                Register();
            }

            var timeout = TimeSpan.FromMinutes(10);
            bucket.Upsert<T>(key, value, timeout);
        }

        public static T Get<T>(string key)
        {
            if (bucket == null)
            {
                return default(T);
            }

            return bucket.Get<T>(key).Value;
        }
    }
}