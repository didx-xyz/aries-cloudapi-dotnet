
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// AriesCloudAPI builder Interface
    /// </summary>
    public interface IAriesCloudAPIBuilder
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        IServiceCollection Services { get; }
    }
}