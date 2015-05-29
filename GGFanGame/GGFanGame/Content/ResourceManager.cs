using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGFanGame.Content
{
    /// <summary>
    /// An interface for all resource managers to implement.
    /// </summary>
    /// <typeparam name="T">The type of resource.</typeparam>
    abstract class ResourceManager<T>
    {
        private GGGame _gameInstance = null;

        private Dictionary<string, T> _resources = new Dictionary<string, T>();
        /// <summary>
        /// The loaded resources.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, T> resources
        {
            get { return _resources; }
        }

        protected ResourceManager(GGGame game)
        {
            _gameInstance = game;
        }

        /// <summary>
        /// Returns if a specific resource exists.
        /// </summary>
        /// <param name="identifier">The identifier of the resource.</param>
        /// <returns></returns>
        public bool resourceExists(string identifier)
        {
            string internalIdentifier = identifier.ToLower();
            if (_resources.Keys.Contains(internalIdentifier))
                return true;
            else
            {
                T loadedResource = getResource(internalIdentifier);
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
        /// <returns></returns>
        public T getResource(string identifier)
        {
            string internalIdentifier = identifier.ToLower();

            if (!_resources.Keys.Contains(internalIdentifier))
            {
                try
                {
                    _resources.Add(internalIdentifier, _gameInstance.Content.Load<T>(internalIdentifier));
                }
                catch (Exception)
                {
                    return default(T);
                }
            }

            return _resources[internalIdentifier];
        }

        /// <summary>
        /// Clears all loaded resources.
        /// </summary>
        public void clear()
        {
            _resources.Clear();
        }
    }
}