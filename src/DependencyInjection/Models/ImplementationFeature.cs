using System.Diagnostics.CodeAnalysis;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Models
{
    public class ImplementationFeature<TService>
        where TService : class
    {
        public ImplementationFeature(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
            string feature)
        {
            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            if (feature == null)
            {
                throw new ArgumentNullException(nameof(feature));
            }

            if (string.IsNullOrWhiteSpace(feature))
            {
                throw new ArgumentException($"Empty or whitespace values are not allowed for the '{nameof(feature)}' argument.", nameof(feature));
            }

            ImplementationType = implementationType;
            Feature = feature;
        }

        public Type ImplementationType { get; set; }

        public string Feature { get; set; }
    }
}