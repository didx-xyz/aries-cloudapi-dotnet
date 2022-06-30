using System.Threading.Tasks;

namespace AriesCloudAPI.Core.Interfaces
{
    /// <summary>
    /// Interface for the resolution of entity keys
    /// </summary>
    public interface IEntityKeyResolver
    {
        /// <summary>
        /// Resolves the specified key
        /// </summary>
        /// <param name="key">The entity key.</param>
        /// <returns></returns>
        Task<string> ResolveAsync(string key);

        string GenerateKey();
    }
}