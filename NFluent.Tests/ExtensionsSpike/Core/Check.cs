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
            // TOOD search plugins within the appdomain instead of within the ExecutingAssembly 
            var types = Assembly.GetExecutingAssembly().GetTypes()
                                                        .Where(x => x.GetInterfaces()
                                                        .Any(i => i.Name.StartsWith("IFluentAssertion")));

            foreach (var type in types)
            {
                var name = type.GetInterfaces()[0].GetGenericArguments()[0];
                if (!registry.ContainsKey(name))
                {
                    registry.Add(name, type);
                }
            }
        }

        public static IFluentAssertion<T> That<T>(T sut)
        {
            return GetSutWrapper(sut);
        }

        private static IFluentAssertion<T> GetSutWrapper<T>(T typedSut)
        {
            if (registry.ContainsKey(typedSut.GetType()))
            {
                return Activator.CreateInstance(registry[typedSut.GetType()], typedSut) as IFluentAssertion<T>;
            }

            var factory = new FluentAssertFactory();
            var interfacesut = factory.GetInterface(typedSut);
            var result = interfacesut as IFluentAssertion<T>;
            return result;
        }

    }
}