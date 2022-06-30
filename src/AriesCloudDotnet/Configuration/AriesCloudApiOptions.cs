namespace AriesCloudDotnet.Configuration
{
    /// <summary>
    /// The AriesCloudAPIOptions class is the top level container for all configuration settings of AriesCloudAPI.
    /// </summary>
    public class AriesCloudAPIOptions
    {
        /// <summary>
        /// Gets or sets the url to the Aries Cloud API
        /// </summary> 
        private string _baseUri;

        public string BaseUri
        {
            get
            {
                return _baseUri;
            }
            set
            {
                _baseUri = value?.TrimEnd('/');
            }
        }

        /// <summary>
        /// Gets or sets the API Key
        /// </summary> 
        public string APIKey { get; set; }
    }
}