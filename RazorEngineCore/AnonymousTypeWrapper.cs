using System.Collections;
using System.Dynamic;
using System.Reflection;

namespace RazorEngineCore
{
    /// <summary>
    /// Wrapper for anonymous objects, allowing values to be accessed as properties.
    /// </summary>
    public class AnonymousTypeWrapper : DynamicObject
    {
        /// <summary>
        /// The underlying anonymous model.
        /// </summary>
        private readonly object Model;

        /// <summary>
        /// Initialize a new <see cref="AnonymousTypeWrapper" />-instance.
        /// </summary>
        /// <param name="model">The underlying anonymous model.</param>
        public AnonymousTypeWrapper(object model)
        {
            this.Model = model;
        }

        /// <summary>
        /// Try to get a member from the underlying model.
        /// </summary>
        /// <remarks>
        /// <paramref name="result" /> may be <c>null</c>, even if the member was found, if the actual value was <c>null</c>.
        /// </remarks>
        /// <param name="binder"><see cref="GetMemberBinder" />-instance, describing the member to get.</param>
        /// <param name="result">The content of the member, if found.</param>
        /// <returns>Returns <c>true</c>, if the member was found. Otherwise, <c>false</c>.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            PropertyInfo? propertyInfo = this.Model.GetType().GetProperty(binder.Name);

            if(propertyInfo == null)
            {
                result = null;
                return false;
            }

            result = propertyInfo.GetValue(this.Model, null);

            if(result == null)
            {
                return true;
            }

            var type = result.GetType();

            if(result.IsAnonymous())
            {
                result = new AnonymousTypeWrapper(result);
            }

            if(result is IDictionary dictionary)
            {
                List<object> keys = new List<object>();

                foreach(object key in dictionary.Keys)
                {
                    keys.Add(key);
                }

                foreach(object key in keys)
                {
                    object? value = dictionary[key];

                    if(value == null)
                    {
                        continue;
                    }

                    if(value.IsAnonymous())
                    {
                        dictionary[key] = new AnonymousTypeWrapper(value);
                    }
                }

                return true;
            }

            if(result is IEnumerable enumer && result is not string)
            {
                result = enumer.Cast<object>()
                        .Select(e =>
                        {
                            if(e.IsAnonymous())
                            {
                                return new AnonymousTypeWrapper(e);
                            }

                            return e;
                        })
                        .ToList();

                return true;
            }

            return true;
        }
    }
}