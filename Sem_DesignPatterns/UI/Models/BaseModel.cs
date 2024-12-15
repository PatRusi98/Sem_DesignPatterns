using System.ComponentModel;
using System.Reflection;

namespace Sem_DesignPatterns.UI.Models
{
    public abstract class BaseModel
    {
        public static PropertyInfo[] GetPropertiesWithDescription<T>() where T : BaseModel
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static string GetPropertyDescription(PropertyInfo property)
        {
            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute != null ? descriptionAttribute.Description : property.Name;

        }
    }
}