using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SlowMember
{
    public class ReflectionService: IReflectionService, IDisposable
    {
        public ReflectionService()
        {
            _cache = new Hashtable();
            _cacheLock = new object();
        }

        public bool CacheDisabled { get; set; }

        #region Cache
        private Hashtable _cache;
        private readonly object _cacheLock;

        private ObjectDescription GetFromCache(Type type)
        {
            var cacheItem = (ObjectDescription)_cache[type.FullName];
            return cacheItem;
        }

        private void SetCacheItem(ObjectDescription cacheItem)
        {
            if (CacheDisabled) return;
            lock (_cacheLock)
            {
                _cache[cacheItem.Type.FullName] = cacheItem;
            }
        }
        #endregion

        public MemberInfo[] GetFieldsAndProperties(Type type, bool includeNonPublicMembers = false)
        {
            if (type == null) return null;
            var members = !includeNonPublicMembers
                ? type.GetFields(Constants.PublicBindingFlags).Cast<MemberInfo>().Concat(type.GetProperties(Constants.PublicBindingFlags)).ToArray()
                : type.GetFields(Constants.NonPublicBindingFlags).Cast<MemberInfo>().Concat(type.GetProperties(Constants.NonPublicBindingFlags)).ToArray();
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

        private ObjectDescription GetObjectDescriptionInternal(Type type, bool includeNonPublicMembers = false)
        {
            var objectDescription = new ObjectDescription(this, type, includeNonPublicMembers);
            return objectDescription;
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
            where T: class
        {
            var type = instance.GetType();
            var objectDescription = GetObjectDescription(type, includeNonPublicMembers);
            return objectDescription;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose(bool disposing)
        {
            
            if (_disposed)
                return;
            if (disposing)
            {
                _handle.Dispose();
                _cache = null;
            }
            _disposed = true;
        }
    }
}
