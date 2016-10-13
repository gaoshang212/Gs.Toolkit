using System;
using System.Linq;

namespace Gs.Toolkit.Extension
{
    public static class TypeExtension
    {
        public static Type[] GetTypes(this object[] p_objects)
        {
            return p_objects?.Select(i => i.GetType()).ToArray();
        }

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}