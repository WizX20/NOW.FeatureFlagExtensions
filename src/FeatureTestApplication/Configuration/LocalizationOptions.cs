namespace FeatureTestApplication.Configuration
{
    public class LocalizationOptions
    {
        /// <summary>
        /// Gets or sets the default culture.
        /// </summary>
        public string? DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the supported cultures. Multiple items supported as comma-separated list.
        /// </summary>
        public string? SupportedCultures { get; set; }
    }
}