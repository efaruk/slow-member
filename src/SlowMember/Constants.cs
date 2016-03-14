using System.Reflection;

namespace SlowMember
{
    public class Constants
    {
        public const BindingFlags PublicBindingFlags = BindingFlags.Public | BindingFlags.Instance;
        public const BindingFlags NonPublicBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    }
}