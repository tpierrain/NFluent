namespace Spike.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Check
    {
        private static readonly Dictionary<Type, Type> registry = new Dictionary<Type, Type>();

        /// <summary>
        /// Initializes the <see cref="Check" /> class.
        /// </summary>
        static Check()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(
                x => x.GetInterfaces().Any(i => i.Name.StartsWith("IGenericFluent")));

            foreach (var type in types)
            {
                var name = type.GetInterfaces()[0].GetGenericArguments()[0];
                if (!registry.ContainsKey(name))
                {
                    registry.Add(name, type);
                }
            }
        }

        public static IGenericFluent<T> That<T>(T sut)
        {
            return wrapper(sut);
        }

        private static IGenericFluent<T> wrapper<T>(T typedSut)
        {
            if (registry.ContainsKey(typedSut.GetType()))
            {
                return Activator.CreateInstance(registry[typedSut.GetType()], typedSut) as IGenericFluent<T>;
            }

            var factory = new FluentAssertFactory();
            var interfacesut = factory.GetInterface(typedSut);
            var result = interfacesut as IGenericFluent<T>;
            return result;
        }

    }
}