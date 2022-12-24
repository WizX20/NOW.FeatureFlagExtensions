using System.ComponentModel;

namespace NOW.FeatureFlagExtensions.FeatureManagement.FeatureFlagGenerator.Extensions
{
    public static class DynamicExtensions
    {
        public static Dictionary<string, object> ToDictionary(dynamic dynamicObj)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynamicObj))
            {
                object obj = propertyDescriptor.GetValue(dynamicObj);
                dictionary.Add(propertyDescriptor.Name, obj);
            }

            return dictionary;
        }
    }
}