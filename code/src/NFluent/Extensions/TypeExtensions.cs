namespace NFluent.Extensions
{
    using System;
    using System.Collections;

#if !DOTNET_20 && !DOTNET_30
    using System.Linq;
#endif
    using System.Reflection;

    internal static class TypeExtensions
    {
        private const BindingFlags BindingFlagsAll = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;

        /// <summary>
        /// Checks if a type has at least one attribute of a give type.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="attribute">Attribute type to check for.</param>
        /// <returns>True if <paramref name="type"/> cref="type"/> has a least one attribute of type <paramref name="attribute"/>, false otherwise.</returns>
        public static bool TypeHasAttribute(this Type type, Type attribute)
        {
            return type.GetTypeInfo().GetCustomAttributes(false)
                .Any(customAttribute => customAttribute.GetType() == attribute);
        }

        /// <summary>
        /// Checks if a type possesses at least a field or a property.
        /// </summary>
        /// <param name="type">Type to be checked</param>
        /// <returns>true if the type as at least one field or property</returns>
        public static bool TypeHasMember(this Type type)
        {

            return !type.GetTypeInfo().IsEnum && (type.GetFields(BindingFlagsAll).Any() || type.GetProperties(BindingFlagsAll).Any());
        }

        /// <summary>
        /// Returns true if the provided type implements IEnumerable, disregarding well known enumeration (string).
        /// </summary>
        /// <param name="type">type to assess</param>
        /// <param name="evenWellKnown">treat well known enumerations (string) as enumeration as well</param>
        /// <returns>true is <see paramref="type"/> should treated as an enumeration.</returns>
        public static bool IsAnEnumeration(this Type type, bool evenWellKnown)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable)) && (evenWellKnown || type != typeof(string));
        }

        /// <summary>
        /// Returns true if the type is a generic type
        /// </summary>
        /// <param name="type">type to asses</param>
        /// <returns>true if <see paramref="type"/> is a generic type.</returns>
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
    }
}
