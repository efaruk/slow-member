using System;
using System.Collections.Generic;
using System.Linq;

namespace SlowMember
{
    public class ObjectDescription
    {
        private ObjectDescription()
        {
            MemberDescriptions = new List<MemberDescription>(100);
            AttributeDescriptions = new List<AttributeDescription>(10);
            MethodDescriptions = new List<MethodDescription>(100);
        }

        public ObjectDescription(IReflectionService reflectionService, Type type, bool includeNonPublicMembers = false,
            string name = "") : this()
        {
            if (type == null) throw new ArgumentNullException("type");
            Name = string.IsNullOrWhiteSpace(name) ? type.Name : name;
            Type = type;
            var attributes = type.GetCustomAttributes(true).ToList();
            if (attributes.Any())
            {
                foreach (
                    var description in attributes.Select(attribute => new AttributeDescription((Attribute) attribute)))
                {
                    AttributeDescriptions.Add(description);
                }
            }
            var fieldInfos = reflectionService.GetFields(type, includeNonPublicMembers);
            var popertyInfos = reflectionService.GetProperties(type, includeNonPublicMembers);
            var methodInfos = reflectionService.GetMethods(type, includeNonPublicMembers);
            foreach (
                var memberDescription in
                    fieldInfos.Select(fieldInfo => new MemberDescription(reflectionService, fieldInfo, this)))
            {
                MemberDescriptions.Add(memberDescription);
            }
            foreach (
                var memberDescription in
                    popertyInfos.Select(propertyInfo => new MemberDescription(reflectionService, propertyInfo, this)))
            {
                MemberDescriptions.Add(memberDescription);
            }
            foreach (
                var methodDescription in
                    methodInfos.Select(methodInfo => new MethodDescription(reflectionService, methodInfo, this)))
            {
                MethodDescriptions.Add(methodDescription);
            }
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public List<MemberDescription> MemberDescriptions { get; set; }

        public List<AttributeDescription> AttributeDescriptions { get; set; }

        public List<MethodDescription> MethodDescriptions { get; set; }
    }
}