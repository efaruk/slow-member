using System;

namespace SlowMember
{
    public class AttributeDescription
    {
        public AttributeDescription(Attribute attribute)
        {
            Attribute = attribute;
            Name = attribute.GetType().Name;
        }

        public Attribute Attribute { get; private set; }

        public string Name { get; private set; }
    }
}