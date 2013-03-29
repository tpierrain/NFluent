namespace Spike.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Check
    {
        // TODO :create a dedicated collection for this registry with properties such as FluentInterfaceType & FluentImplementationType 
        private static readonly Dictionary<Type, Type> fluentInterfacesToFluentImplementationRegistry = new Dictionary<Type, Type>();

        /// <summary>
        /// Initializes the <see cref="Check" /> class.
        /// </summary>
        static Check()
        {
            // TODO search plugins within the appdomain instead of within the ExecutingAssembly
            var types = Assembly.GetExecutingAssembly().GetTypes()
                                                        .Where(x => x.GetInterfaces()
                                                        .Any(i => i.Name.StartsWith("IFluentAssertion")));

            foreach (var type in types)
            {
                var name = type.GetInterfaces()[0].GetGenericArguments()[0];
                if (!fluentInterfacesToFluentImplementationRegistry.ContainsKey(name))
                {
                    fluentInterfacesToFluentImplementationRegistry.Add(name, type);
                }
            }
        }

        public static IFluentAssertion<T> That<T>(T sut)
        {
            return GetSutWrapper<T>(sut);
        }

        private static IFluentAssertion<T> GetSutWrapper<T>(object sut)
        {
            if (fluentInterfacesToFluentImplementationRegistry.ContainsKey(sut.GetType()))
            {
                return Activator.CreateInstance(fluentInterfacesToFluentImplementationRegistry[sut.GetType()], sut) as IFluentAssertion<T>;
            }
            else
            {
                // No direct entry for the concrete type of this sut; thus,
                // try to find one of its base type as an alternative
                // or, would be nice to create the corresponding "enveloppe type" on the fly ;-)
                foreach (var type in fluentInterfacesToFluentImplementationRegistry)
                {
                    if (type.Key.IsAssignableFrom(sut.GetType()) && !type.Key.Equals(typeof(object)))
                    {
                        var temp = Activator.CreateInstance(type.Value, sut);
                        // incorrect, cause IFluentAssertion<IComparable> is not assignable to IFluentAssertion<Version> ;-(
                        return temp as IFluentAssertion<T>;
                    }
                }
            }

            // worst case scenario; we wrao the sut with a IFluentAssertion<object> instance
            var factory = new FluentAssertFactory();
            var interfacesut = factory.GetInterface(sut);
            var result = interfacesut as IFluentAssertion<T>;
            return result;
        }
    }
}