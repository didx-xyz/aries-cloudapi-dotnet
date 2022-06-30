using System;

namespace AriesCloudAPI.Core.Models
{
    /// <summary>
    /// A model for a persisted invitation
    /// </summary>
    public class PersistedInvitation
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }
         
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }
    }
}