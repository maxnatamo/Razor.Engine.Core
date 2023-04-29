namespace RazorEngineCore
{
    /// <summary>
    /// Extensions for <see cref="Type" />.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Render the name of a <see cref="Type" /> instance into a C# friendly format.
        /// </summary>
        /// <example>
        /// <code>
        /// // Type without generic arguments
        /// public class Model
        /// {
        ///     
        /// }
        /// 
        /// // Type with generic arguments
        /// public class Model<T>
        /// {
        ///     
        /// }
        /// 
        /// Type[] types =
        /// {
        ///     typeof(Model),
        ///     typeof(Model<int>),
        ///     typeof(Model<Model<int>>),
        /// };
        /// 
        /// foreach(Type type in types)
        /// {
        ///     Console.WriteLine("{0} => {1}", type.FullName, type.RenderTypeName());
        /// }
        /// 
        /// // The example displays output similar to the following:
        /// //
        /// //    Program.Model => Program.Model
        /// //    Program.Model`1[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]] => Program.Model<System.Int32>
        /// //    Program.Model`1[[Program.Model`1[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], test-console, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]] => Program.Model<Program.Model<System.Int32>>
        /// </code>
        /// </example>
        /// <param name="type">The <see cref="Type" /> from which to render the name.</param>
        /// <param name="includeNamespace">Whether to include namespace in the rendered name.</param>
        /// <returns>The C#-friendly name of <paramref name="type" />.</returns>
        public static string RenderTypeName(this Type type, bool includeNamespace = true)
        {
            string name = "";

            if(type.Namespace != null && includeNamespace)
            {
                name += $"{type.Namespace}.";
            }

            // Add parent types, if the type is nested.
            if(type.DeclaringType != null)
            {
                string? parent = type.DeclaringType.DeclaringType?.RenderTypeName(false);

                if(string.IsNullOrEmpty(parent))
                {
                    name += $"{type.DeclaringType.Name}.";
                }
                else
                {
                    name += $"{parent}.{type.DeclaringType.Name}.";
                }
            }

            if(!type.IsGenericType)
            {
                return $"{name}{type.Name}";
            }

            // Add the actual name of the type
            string typeName = type.Name.Substring(0, type.Name.IndexOf("`"));
            name += typeName;

            // Add generic arguments
            string genericArguments = string.Join(", ", type.GetGenericArguments().Select(v => v.RenderTypeName()));
            name += $"<{genericArguments}>";

            return name;
        }
    }
}