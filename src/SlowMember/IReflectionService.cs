using System;
using System.Reflection;

namespace SlowMember
{
    public interface IReflectionService
    {
        /// <summary>
        ///     Disable Enable Cache Temporarily (Not cleans the cache)
        /// </summary>
        bool CacheDisabled { get; set; }

        /// <summary>
        ///     Get Fields and Properties from type.
        /// </summary>
        /// <param name="type">Type to get <see cref="MemberInfo">MemberInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields/properties</param>
        /// <returns>Array of <see cref="MemberInfo">MemberInfo</see></returns>
        MemberInfo[] GetFieldsAndProperties(Type type, bool includeNonPublicMembers = false);

        /// <summary>
        ///     Get Fields from type.
        /// </summary>
        /// <param name="type">Type to get <see cref="FieldInfo">FieldInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields</param>
        /// <returns>Array of <see cref="FieldInfo">FieldInfo</see></returns>
        FieldInfo[] GetFields(Type type, bool includeNonPublicMembers = false);

        /// <summary>
        ///     Get Fields from type.
        /// </summary>
        /// <param name="type">Type to get <see cref="PropertyInfo">PropertyInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public properties</param>
        /// <returns>Array of <see cref="PropertyInfo">PropertyInfo</see></returns>
        PropertyInfo[] GetProperties(Type type, bool includeNonPublicMembers = false);

        /// <summary>
        ///     Get methods from type
        /// </summary>
        /// <param name="type">Type to get <see cref="MethodInfo">MethodInfo</see>'s</param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public properties</param>
        /// <returns>Array of <see cref="MethodInfo">MethodInfo</see></returns>
        MethodInfo[] GetMethods(Type type, bool includeNonPublicMembers = false);

        /// <summary>
        ///     Get Fields and Properties from type and fill ObjectDescription. This method use caching mechanism.
        /// </summary>
        /// <param name="type">Type to get <see cref="ObjectDescription" /></param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields/properties</param>
        ObjectDescription GetObjectDescription(Type type, bool includeNonPublicMembers = false);

        /// <summary>
        ///     Get Fields and Properties from type and fill ObjectDescription. This method use caching mechanism.
        /// </summary>
        /// <param name="instance">Instance to get <see cref="ObjectDescription" /></param>
        /// <param name="includeNonPublicMembers">Wheter or not include non public fields/properties</param>
        /// <returns>
        ///     <see cref="ObjectDescription" />
        /// </returns>
        ObjectDescription GetObjectDescription<T>(T instance, bool includeNonPublicMembers = false) where T : class;
    }
}