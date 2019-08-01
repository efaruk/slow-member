using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SlowMember
{
    public class ReflectionService : IReflectionService, IDisposable
    {
        private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool CacheDisabled { get; set; }

        public MemberInfo[] GetFieldsAndProperties(Type type, bool includeNonPublicMembers = false)
        {
            if (type == null) return null;
            var members = !includeNonPublicMembers
                ? type.GetFields(Constants.PublicBindingFlags)
                    .Cast<MemberInfo>()
                    .Concat(type.GetProperties(Constants.PublicBindingFlags))
                    .ToArray()
                : type.GetFields(Constants.NonPublicBindingFlags)
                    .Cast<MemberInfo>()
                    .Concat(type.GetProperties(Constants.NonPublicBindingFlags))
                    .ToArray();
            return members;
        }

        public FieldInfo[] GetFields(Type type, bool includeNonPublicMembers = false)
        {
            if (type == null) return null;
            var members = !includeNonPublicMembers
                ? type.GetFields(Constants.PublicBindingFlags).ToArray()
                : type.GetFields(Constants.NonPublicBindingFlags).ToArray();
            return members;
        }

        public PropertyInfo[] GetProperties(Type type, bool includeNonPublicMembers = false)
        {
            if (type == null) return null;
            var members = !includeNonPublicMembers
                ? type.GetProperties(Constants.PublicBindingFlags).ToArray()
                : type.GetProperties(Constants.NonPublicBindingFlags).ToArray();
            return members;
        }

        public MethodInfo[] GetMethods(Type type, bool includeNonPublicMembers = false)
        {
            if (type == null) return null;
            var methods = !includeNonPublicMembers
                ? type.GetMethods(Constants.PublicBindingFlags).ToArray()
                : type.GetMethods(Constants.NonPublicBindingFlags).ToArray();
            return methods;
        }

        public ObjectDescription GetObjectDescription(Type type, bool includeNonPublicMembers = false)
        {
            var objectDescription = GetFromCache(type);
            if (objectDescription != null) return objectDescription;
            objectDescription = GetObjectDescriptionInternal(type, includeNonPublicMembers);
            SetCacheItem(objectDescription);
            return objectDescription;
        }

        public ObjectDescription GetObjectDescription<T>(T instance, bool includeNonPublicMembers = false)
            where T : class
        {
            var type = instance.GetType();
            var objectDescription = GetObjectDescription(type, includeNonPublicMembers);
            return objectDescription;
        }

        private ObjectDescription GetObjectDescriptionInternal(Type type, bool includeNonPublicMembers = false)
        {
            var objectDescription = new ObjectDescription(this, type, includeNonPublicMembers);
            return objectDescription;
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _handle.Dispose();
                Cache.Clear();
            }
            _disposed = true;
        }

        #region Cache

        private static readonly Hashtable Cache = new Hashtable();
        private static readonly object CacheLock = new object();

        private ObjectDescription GetFromCache(Type type)
        {
            if (type == null) return null;
            var cacheItem = (ObjectDescription) Cache[type.FullName];
            return cacheItem;
        }

        private void SetCacheItem(ObjectDescription cacheItem)
        {
            if (cacheItem == null) return;
            if (CacheDisabled) return;
            lock (CacheLock)
            {
                Cache[cacheItem.Type.FullName] = cacheItem;
            }
        }

        #endregion
    }
}