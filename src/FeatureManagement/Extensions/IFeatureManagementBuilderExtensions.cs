using Microsoft.FeatureManagement;
using NOW.FeatureFlagExtensions.FeatureManagement.Filters;

namespace NOW.FeatureFlagExtensions.FeatureManagement.Extensions
{
    public static class IFeatureManagementBuilderExtensions
    {
        public static IFeatureManagementBuilder AddAggregateFeatureFilter(this IFeatureManagementBuilder featureManagementBuilder)
        {
            return featureManagementBuilder.AddFeatureFilter<AggregateFeatureFilter>();
        }

        public static IFeatureManagementBuilder AddClaimsFeatureFilter(this IFeatureManagementBuilder featureManagementBuilder)
        {
            return featureManagementBuilder.AddFeatureFilter<ClaimsFeatureFilter>();
        }

        public static IFeatureManagementBuilder AddEnvironementsFeatureFilter(this IFeatureManagementBuilder featureManagementBuilder)
        {
            return featureManagementBuilder.AddFeatureFilter<EnvironementsFeatureFilter>();
        }

        public static IFeatureManagementBuilder AddRequestHeadersFeatureFilter(this IFeatureManagementBuilder featureManagementBuilder)
        {
            return featureManagementBuilder.AddFeatureFilter<RequestHeadersFeatureFilter>();
        }

        public static IFeatureManagementBuilder AddFeatureFlagExtensionsFeatureFilters(this IFeatureManagementBuilder featureManagementBuilder)
        {
            return featureManagementBuilder
                .AddClaimsFeatureFilter()
                .AddEnvironementsFeatureFilter()
                .AddRequestHeadersFeatureFilter()
                .AddAggregateFeatureFilter();
        }
    }
}