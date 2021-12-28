namespace FeatureTestApplication.Configuration
{
    public class ResponseCachingOptions
    {
        /// <summary>
        /// The largest cacheable size for the response body in bytes. The default is set to 64 MB.
        /// </summary>
        public long MaximumBodySize { get; set; }

        /// <summary>
        /// true if request paths are case-sensitive; otherwise false. The default is to
        /// treat paths as case-insensitive.
        /// </summary>
        public bool UseCaseSensitivePaths { get; set; }
    }
}