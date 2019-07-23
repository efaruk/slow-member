using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlowMember
{
    public class MemberDescription: BaseObjectDescription
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
            Type = propertyInfo.PropertyType;
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
            Type = fieldInfo.FieldType;
            Name = fieldInfo.Name;
            FillAttributes(fieldInfo);
            FillIsGenericEnumerable();
        }

        public ObjectDescription Parent { get; private set; }

        public FieldInfo FieldInfo { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public List<AttributeDescription> AttributeDescriptions { get; private set; }

        private void FillAttributes(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes(true).ToList();
            if (!attributes.Any()) return;
            foreach (var description in attributes.Select(attribute => new AttributeDescription((Attribute) attribute)))
            {
                AttributeDescriptions.Add(description);
            }
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