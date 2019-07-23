using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SlowMember
{
    public abstract class BaseObjectDescription
    {
        public string Name { get; protected set; }

        public Type Type { get; protected set; }

        public bool IsGeneric { get; protected set; }

        public bool IsEnumerable { get; protected set; }

        public List<Type> GenericArguments { get; } = new List<Type>(10);

        protected void FillIsGenericEnumerable()
        {
            var typeInfo = (TypeInfo) Type;
            var any = typeInfo.GetInterface("IEnumerable");
            IsEnumerable = any != null;
            IsGeneric = Type.IsGenericType;
            GenericArguments.AddRange(Type.GetGenericArguments());
        }
    }
}
