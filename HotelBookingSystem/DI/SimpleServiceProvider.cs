using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.DI
{
    public class SimpleServiceProvider
    {
        private readonly Dictionary<Type, Func<object>> _registrations = new();

        public void Register<TService>(Func<object> factory)
        {
            _registrations[typeof(TService)] = factory;
        }

        public TService Resolve<TService>()
        {
            if (_registrations.TryGetValue(typeof(TService), out var factory))
            {
                return (TService)factory();
            }
            throw new InvalidOperationException($"Service of type {typeof(TService)} is not registered.");
        }
    }
}
