using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KosherUtils.Framework
{
    public class ServiceProvider
    {
        private Dictionary<string, object> serviceToMap = new Dictionary<string, object>();

        private Dictionary<string, Type> registeredSingletonToMap = new Dictionary<string, Type>();

        private Dictionary<string, Type> registeredTransientToMap = new Dictionary<string, Type>();

        public void AddTransient<TService, TImplementation>() where TService : class
                                                                where TImplementation : TService
        {
            var name = typeof(TService).Name;
            registeredTransientToMap.Add(name, typeof(TImplementation));
        }
        public void AddSingleton<TService, TImplementation>() where TService : class
                                                                where TImplementation: TService
        {
            var name = typeof(TService).Name;
            registeredSingletonToMap.Add(name, typeof(TImplementation));
        }
        public void AddSingleton<TService>(TService implementation) where TService : class
        {
            var name = typeof(TService).Name;
            registeredSingletonToMap.Add(name, typeof(TService));
            serviceToMap.Add(name, implementation);
        }

        public T GetService<T>() where T : class
        {
            return GetService(typeof(T).Name) as T;
        }
        private object GetService(string typeName)
        {
            if(registeredSingletonToMap.ContainsKey(typeName) == true)
            {
                if (serviceToMap.ContainsKey(typeName) == true)
                {
                    return serviceToMap[typeName];
                }

                return GetSingletonService(typeName);
            }
            else if (registeredTransientToMap.ContainsKey(typeName) == true)
            {
                return GetTransientService(registeredTransientToMap[typeName]);
            }

            throw new Exception($"not found registered service type! {typeName}");
        }

        private object GetTransientService(Type type)
        {
            var constructors = type.GetConstructors();

            foreach (var item in constructors)
            {
                var parameters = item.GetParameters();
                var objects = new object[parameters.Length];
                for(int i=0; i< parameters.Length; ++i)
                {
                    objects[i] = GetService(parameters[i].ParameterType.Name);
                }
                var constructor = item.Invoke(objects);
                return constructor;
            }

            return default;
        }
        private object GetSingletonService(string key)
        {
            var service = GetTransientService(registeredSingletonToMap[key]);
            serviceToMap.Add(key, service);
            return service;
        }
    }
}
