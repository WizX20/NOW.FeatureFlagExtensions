using ImpromptuInterface;
using System.Dynamic;
using System.Reflection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Helpers
{
    public static class CustomActivator
    {
        public static void AddListThing(this Type type)
        {
        }

        public static object? CastValueToPropertyType(
            PropertyInfo propertyToBeSetted,
            object value)
        {
            if (propertyToBeSetted is null)
            {
                throw new ArgumentNullException(nameof(propertyToBeSetted));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Type propertyType = Nullable.GetUnderlyingType(propertyToBeSetted.PropertyType) ?? propertyToBeSetted.PropertyType;
            object? safeValue = (value == null) ? null : Convert.ChangeType(value, propertyType);

            return safeValue;
        }

        public static TObject CreateInstance<TObject>(object[] implementations)
            where TObject : class
        {
            if (implementations is null)
            {
                throw new ArgumentNullException(nameof(implementations));
            }

            if (implementations.Length < 1)
            {
                throw new ArgumentException("Must contain one or more items.", nameof(implementations));
            }

            var propertyValues = new Dictionary<string, object>();

            return CreateInstanceAndPopulateProperties<TObject>(propertyValues, implementations);
        }

        public static TObject CreateInstanceAndPopulateProperties<TObject>(
            Dictionary<string, object> propertiesValues,
            object[] implementations)
            where TObject : class
        {
            if (propertiesValues is null)
            {
                throw new ArgumentNullException(nameof(propertiesValues));
            }

            if (implementations is null)
            {
                throw new ArgumentNullException(nameof(implementations));
            }

            if (implementations.Length < 1)
            {
                throw new ArgumentException("Must contain one or more items.", nameof(implementations));
            }

            var desiredType = typeof(TObject);

            if (desiredType.IsInterface)
            {
                // The "Activator.CreateInstance<>" from the .NET Framework can't instantiate interfaces.
                // To do this we need to create an object (in runtime) that implements this interface.
                return CreateInterfaceInstance<TObject>(propertiesValues, implementations);
            }
            else
            {
                var objectCreated = Activator.CreateInstance<TObject>();
                PopulateObjectProperties(objectCreated, propertiesValues);
                return objectCreated;
            }
        }

        public static MethodInfo[] GetPublicMethods(this Type type)
        {
            if (type.IsInterface)
            {
                var methodInfos = new List<MethodInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();

                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface))
                        {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeMethods = subType.GetMethods(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeMethods
                        .Where(x => !methodInfos.Contains(x));

                    methodInfos.InsertRange(0, newPropertyInfos);
                }

                return methodInfos.ToArray();
            }

            return type.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
        }

        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();

                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface))
                        {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
        }

        public static void PopulateObjectProperties(
            object obj,
            Dictionary<string, object>? propertiesValues)
        {
            PopulatePropertiesOfObjectWithValuesFromDictionary(obj, propertiesValues);
        }

        private static TInterface CreateInterfaceInstance<TInterface>(
            Dictionary<string, object> propertiesValues,
            object[] implementations)
            where TInterface : class
        {
            dynamic expandoObject = new ExpandoObject();

            // custom start
            expandoObject.Instances = implementations;
            // custom end

            var expandoType = typeof(TInterface);
            var expandoProperties = expandoObject as IDictionary<string, object>;

            if (expandoProperties == null)
            {
                throw new InvalidOperationException($"Unable to convert {nameof(expandoObject)} to {nameof(expandoProperties)}.");
            }

            //foreach (MethodInfo method in GetPublicMethods(expandoObject))
            //{
            //    method
            //        .MakeGenericMethod(expandoType, expandoObject)
            //        .Invoke(null, null)

            //}

            foreach (var property in GetPublicProperties(expandoType))
            {
                //// custom start
                //var setter = property.GetSetMethod();
                //if (setter != null)
                //{
                //    expandoProperties[property.Name].
                //}

                ////Action<string> setter = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), null, typeof(MyClass).GetProperty(item.Name).GetSetMethod());

                //// custom end

                // Get value from dictionary if it exists.
                if (propertiesValues != null && propertiesValues.ContainsKey(property.Name))
                {
                    var parameterValue = propertiesValues[property.Name];
                    var convertedValue = CastValueToPropertyType(property, parameterValue);
                    expandoProperties[property.Name] = convertedValue;
                }
                // If not, get the default value.
                else
                {
                    var propertyType = property.PropertyType;
                    var defaultValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
                    expandoProperties[property.Name] = defaultValue;
                }
            }

            var instance = Impromptu.ActLike<TInterface>(expandoObject);
            var interceptor = new MyInterceptor<TInterface>();
            var interceptedInstance = interceptor.Decorate(instance);

            return instance;
        }

        private static void PopulatePropertiesOfObjectWithValuesFromDictionary(
            object obj,
            Dictionary<string, object>? propertiesValues)
        {
            if (propertiesValues == null)
            {
                return;
            }

            var objectType = obj.GetType();

            foreach (var property in GetPublicProperties(objectType))
            {
                if (propertiesValues.ContainsKey(property.Name))
                {
                    var parameterValue = propertiesValues[property.Name];
                    var convertedValue = CastValueToPropertyType(property, parameterValue);
                    property.SetValue(obj, convertedValue);
                }
            }
        }
    }
}