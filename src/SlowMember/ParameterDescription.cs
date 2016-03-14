using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlowMember
{
    public class ParameterDescription
    {
        private ParameterDescription()
        {
            AttributeDescriptions = new List<AttributeDescription>(10);
        }

        public ParameterDescription(IReflectionService reflectionService, ParameterInfo parameterInfo,
            MethodDescription parent) : this()
        {
            Parent = parent;
            ParameterInfo = parameterInfo;
            ParmeterType = parameterInfo.ParameterType;
            Name = parameterInfo.Name;
            FillAttributes(parameterInfo);
        }

        public string Name { get; private set; }

        public MethodDescription Parent { get; private set; }

        public List<AttributeDescription> AttributeDescriptions { get; }

        public ParameterInfo ParameterInfo { get; private set; }

        public Type ParmeterType { get; set; }

        private void FillAttributes(ParameterInfo parameterInfo)
        {
            var attributes = parameterInfo.GetCustomAttributes(true).ToList();
            if (!attributes.Any()) return;
            foreach (var description in attributes.Select(attribute => new AttributeDescription((Attribute) attribute)))
            {
                AttributeDescriptions.Add(description);
            }
        }
    }
}