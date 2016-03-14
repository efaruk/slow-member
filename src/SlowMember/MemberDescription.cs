using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlowMember
{
    public class MemberDescription
    {
        private readonly IReflectionService _reflectionService;

        private MemberDescription()
        {
            AttributeDescriptions = new List<AttributeDescription>(10);
        }

        public MemberDescription(IReflectionService reflectionService, PropertyInfo propertyInfo,
            ObjectDescription parent) : this()
        {
            _reflectionService = reflectionService;
            Parent = parent;
            PropertyInfo = propertyInfo;
            MemberType = propertyInfo.PropertyType;
            Name = propertyInfo.Name;
            FillAttributes(propertyInfo);
            FillIsGenericEnumerable();
        }

        public MemberDescription(IReflectionService reflectionService, FieldInfo fieldInfo, ObjectDescription parent)
            : this()
        {
            _reflectionService = reflectionService;
            Parent = parent;
            FieldInfo = fieldInfo;
            MemberType = fieldInfo.FieldType;
            Name = fieldInfo.Name;
            FillAttributes(fieldInfo);
            FillIsGenericEnumerable();
        }

        public ObjectDescription Parent { get; private set; }

        public FieldInfo FieldInfo { get; }

        public PropertyInfo PropertyInfo { get; }

        public Type MemberType { get; }


        public bool IsGeneric { get; private set; }


        public bool IsEnumerable { get; private set; }

        public List<AttributeDescription> AttributeDescriptions { get; }

        public string Name { get; private set; }

        private void FillAttributes(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes(true).ToList();
            if (!attributes.Any()) return;
            foreach (var description in attributes.Select(attribute => new AttributeDescription((Attribute) attribute)))
            {
                AttributeDescriptions.Add(description);
            }
        }

        private void FillIsGenericEnumerable()
        {
            var typeInfo = (TypeInfo) MemberType;
            var any = typeInfo.GetInterface("IEnumerable");
            IsEnumerable = any != null;
            IsGeneric = MemberType.IsGenericType;
        }

        public object GetValue(object instance)
        {
            var value = PropertyInfo != null
                ? PropertyInfo.GetValue(instance)
                : FieldInfo.GetValue(instance);
            return value;
        }

        public void SetValue(object instance, object value)
        {
            if (PropertyInfo != null)
            {
                PropertyInfo.SetValue(instance, value);
            }
            else
            {
                FieldInfo.SetValue(instance, value);
            }
        }
    }
}