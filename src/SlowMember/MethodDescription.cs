using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlowMember
{
    public class MethodDescription
    {
        public MethodDescription()
        {
            AttributeDescriptions = new List<AttributeDescription>(10);
            ParameterDescriptions = new List<ParameterDescription>(10);
        }

        public MethodDescription(IReflectionService reflectionService, MethodInfo methodInfo, ObjectDescription parent) : this()
        {
            Parent = parent;
            MethodInfo = methodInfo;
            Name = methodInfo.Name;
            ReturnParameter = new ParameterDescription(reflectionService, methodInfo.ReturnParameter, this);
            ReturnType = methodInfo.ReturnType;
            var parameterInfos = methodInfo.GetParameters();
            foreach (var parameterDescription in parameterInfos.Select(parameterInfo => new ParameterDescription(reflectionService, parameterInfo, this)))
            {
                ParameterDescriptions.Add(parameterDescription);
            }
            FillAttributes(MethodInfo);
        }

        public ObjectDescription Parent { get; private set; }

        public Type ReturnType { get; set; }

        public ParameterDescription ReturnParameter { get; private set; }

        public string Name { get; private set; }

        public MethodInfo MethodInfo { get; private set; }

        public List<ParameterDescription> ParameterDescriptions { get; private set; }

        public List<AttributeDescription> AttributeDescriptions { get; private set; }

        private void FillAttributes(MethodInfo methodInfo)
        {
            var attributes = methodInfo.GetCustomAttributes(true).ToList();
            if (!attributes.Any()) return;
            foreach (var description in attributes.Select(attribute => new AttributeDescription((Attribute)attribute)))
            {
                AttributeDescriptions.Add(description);
            }
        }
    }
}