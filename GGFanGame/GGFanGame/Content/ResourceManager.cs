using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GameProvider;

namespace GGFanGame.Content
{
    /// <summary>
    /// An interface for all resource managers to implement.
    /// </summary>
    /// <typeparam name="T">The type of resource.</typeparam>
    abstract class ResourceManager<T>
    {
        protected Dictionary<string, T> resources { get; } = new Dictionary<string, T>();

        protected string defaultFolder { get; set; } = "";

        protected ResourceManager()
        { }

        private string createIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(defaultFolder))
                return identifier.ToLowerInvariant();
            else
                return $"{defaultFolder}\\{identifier}".ToLowerInvariant();
        }

        /// <summary>
        /// Returns if a specific resource exists.
        /// </summary>
        /// <param name="identifier">The identifier of the resource.</param>
        public bool resourceExists(string identifier)
        {
            string internalIdentifier = createIdentifier(identifier);
            if (resources.Keys.Contains(internalIdentifier))
                return true;
            else
            {
                T loadedResource = load(internalIdentifier);
                if (loadedResource.Equals(default(T)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Requests a resource from the resource manager.
        /// </summary>
        /// <param name="identifier">The identifier of the resource.</param>
        public T load(string identifier)
        {
            string internalIdentifier = createIdentifier(identifier);

            if (!resources.Keys.Contains(internalIdentifier))
            {
                try
                {
                    resources.Add(internalIdentifier, gameInstance.Content.Load<T>(internalIdentifier));
                }
                catch (Exception)
                {
                    return default(T);
                }
            }

            return resources[internalIdentifier];
        }
    }
}