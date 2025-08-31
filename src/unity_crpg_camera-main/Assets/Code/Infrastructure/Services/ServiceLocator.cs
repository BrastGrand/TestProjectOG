using System;
using System.Collections.Generic;

namespace Code.Infrastructure.Services
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance => _instance ??= new ServiceLocator();

        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public void RegisterService<T>(T service) where T : IService
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                UnityEngine.Debug.LogWarning($"Service of type {type.Name} is already registered. Overwriting...");
            }
            
            _services[type] = service;
            UnityEngine.Debug.Log($"Service {type.Name} registered successfully.");
        }

        public T GetService<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
        }

        public bool HasService<T>() where T : IService
        {
            return _services.ContainsKey(typeof(T));
        }

        public void UnregisterService<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.Remove(type))
            {
                UnityEngine.Debug.Log($"Service {type.Name} unregistered successfully.");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Service of type {type.Name} was not registered.");
            }
        }

        public void Clear()
        {
            _services.Clear();
            UnityEngine.Debug.Log("All services cleared.");
        }
    }
}

