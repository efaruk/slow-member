using System;
using System.Reflection;

namespace SlowMember
{
    internal static class StaticReflectionHelper
    {
        private static readonly ReflectionService ReflectionService;

        static StaticReflectionHelper()
        {
            ReflectionService = new ReflectionService();
        }

        public static bool CacheDisabled
        {
            get { return ReflectionService.CacheDisabled; }
            set { ReflectionService.CacheDisabled = value; }
        }

        /// <summary>
        ///     Get Fields and Properties from type.
        /// </summary>
        /// <param name="type">Type to get <see cref="MemberInfo">MemberInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields/properties</param>
        /// <returns>Array of <see cref="MemberInfo">MemberInfo</see></returns>
        public static MemberInfo[] GetFieldsAndProperties(this Type type, bool includeNonPublicMembers = false)
        {
            return ReflectionService.GetFieldsAndProperties(type, includeNonPublicMembers);
        }

        /// <summary>
        ///     Get Fields from type.
        /// </summary>
        /// <param name="type">Type to get <see cref="FieldInfo">FieldInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields</param>
        /// <returns>Array of <see cref="FieldInfo">FieldInfo</see></returns>
        public static FieldInfo[] GetFields(this Type type, bool includeNonPublicMembers = false)
        {
            return ReflectionService.GetFields(type, includeNonPublicMembers);
        }

        /// <summary>
        ///     Get Fields from type.
        /// </summary>
        /// <param name="type">Type to get <see cref="PropertyInfo">PropertyInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public properties</param>
        /// <returns>Array of <see cref="PropertyInfo">PropertyInfo</see></returns>
        public static PropertyInfo[] GetProperties(this Type type, bool includeNonPublicMembers = false)
        {
            return ReflectionService.GetProperties(type, includeNonPublicMembers);
        }

        /// <summary>
        ///     Get methods from type
        /// </summary>
        /// <param name="type">Type to get <see cref="MethodInfo">MethodInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public properties</param>
        /// <returns>Array of <see cref="MethodInfo">MethodInfo</see></returns>
        public static MethodInfo[] GetMethods(this Type type, bool includeNonPublicMembers = false)
        {
            return ReflectionService.GetMethods(type, includeNonPublicMembers);
        }

        /// <summary>
        ///     Get Fields and Properties from type and fill ObjectDescription. This method use caching mechanism.
        /// </summary>
        /// <param name="type">Type to get <see cref="ObjectDescription" /></param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields/properties</param>
        /// <returns>
        ///     <see cref="ObjectDescription" />
        /// </returns>
        public static ObjectDescription GetObjectDescription(this Type type, bool includeNonPublicMembers = false)
        {
            return ReflectionService.GetObjectDescription(type, includeNonPublicMembers);
        }

        /// <summary>
        ///     Get Fields and Properties from type and fill ObjectDescription. This method use caching mechanism.
        /// </summary>
        /// <param name="instance">Instance to get <see cref="ObjectDescription" /></param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields/properties</param>
        /// <returns>
        ///     <see cref="ObjectDescription" />
        /// </returns>
        public static ObjectDescription GetObjectDescription<T>(this T instance, bool includeNonPublicMembers = false)
            where T : class
        {
            return ReflectionService.GetObjectDescription(instance, includeNonPublicMembers);
        }

        #region Cache

        #endregion
    }
}