using System.Collections.Generic;

namespace Gs.Toolkit.Extension
{
    public static class ObjectExtension
    {
        public static IDictionary<string, object> GetProperties(this object p_obj)
        {
            var dic = new Dictionary<string, object>();

            var obj = p_obj;
            do
            {
                if (obj == null)
                {
                    break;
                }

                var type = p_obj.GetType();

                var properties = type.GetProperties();

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj, null);
                    dic.Add(property.Name, value);
                }

            } while (false);

            return dic;
        }
    }
}
