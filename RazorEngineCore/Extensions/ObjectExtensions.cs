using System.Runtime.CompilerServices;

namespace RazorEngineCore
{
    /// <summary>
    /// Extension methods for <see cref="object" />-instances.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks whether <paramref name="obj" /> is an anonymous type.
        /// </summary>
        /// <param name="obj">The object to check for.</param>
        /// <returns>Returns <c>true</c> if <paramref="obj" /> is anonymous. Otherwise, <c>false</c>.</returns>
        public static bool IsAnonymous(this object obj)
        {
            Type type = obj.GetType();

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType");
        }
    }
}