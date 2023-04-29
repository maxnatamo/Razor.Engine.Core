using System.Globalization;
using System.Text;
using System.Web;

namespace RazorEngineCore
{
    public abstract class RazorEngineTemplateBase<T> : RazorEngineTemplateBase
    {
        public new T Model { get; set; } = default(T)!;
    }

    /// <summary>
    /// Template for all compiled Razor views.
    /// </summary>
    public abstract class RazorEngineTemplateBase
    {
        /// <summary>
        /// Content of the template. This is only populated when running <see cref="ExecuteAsync" />.
        /// </summary>
        private readonly StringBuilder TemplateContents = new StringBuilder();

        /// <summary>
        /// Suffix for the current attribute context.
        /// </summary>
        private string AttributeSuffix = string.Empty;

        /// <summary>
        /// Optional model for the template.
        /// </summary>
        public dynamic? Model { get; set; } = null;

        /// <summary>
        /// Write the specified <paramref name="value" /> to the template with HTML encoding.
        /// </summary>
        /// <param name="value">The <see cref="string" /> value to write.</param>
        public void Write(string? value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                value = this.HtmlEncode(value);
                this.TemplateContents.Append(value);
            }
        }

        /// <summary>
        /// Write the specified <paramref name="value" /> to the template with HTML encoding.
        /// </summary>
        /// <param name="value">The <see cref="object" /> value to write.</param>
        public void Write(object? value = null)
        {
            if(value is null)
            {
                return;
            }

            this.Write(value.ToString());
        }

        /// <summary>
        /// Write the specified <paramref name="value" /> to the template, without any conversion and/or encoding.
        /// </summary>
        /// <param name="value">The <see cref="string" /> value to write.</param>
        public void WriteLiteral(string? value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                this.TemplateContents.Append(value);
            }
        }

        /// <summary>
        /// Write the specified <paramref name="value" /> to the template, without any conversion and/or encoding.
        /// </summary>
        /// <param name="value">The <see cref="object" /> value to write.</param>
        public void WriteLiteral(object? value)
        {
            if(value is null)
            {
                return;
            }

            this.WriteLiteral(value.ToString());
        }

        /// <summary>
        /// Begins writing out an attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="prefixOffset">The prefix offset.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="suffixOffset">The suffix offset.</param>
        /// <param name="attributeValuesCount">The attribute values count.</param>
        public void BeginWriteAttribute(
            string name,
            string prefix,
            int prefixOffset,
            string suffix,
            int suffixOffset,
            int attributeValuesCount)
        {
            ArgumentNullException.ThrowIfNull(prefix);
            ArgumentNullException.ThrowIfNull(suffix);

            this.AttributeSuffix = suffix;
            this.TemplateContents.Append(prefix);
        }

        /// <summary>
        /// Writes out an attribute value.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="prefixOffset">The prefix offset.</param>
        /// <param name="value">The value.</param>
        /// <param name="valueOffset">The value offset.</param>
        /// <param name="valueLength">The value length.</param>
        /// <param name="isLiteral">Whether the attribute is a literal.</param>
        public void WriteAttributeValue(
            string prefix,
            int prefixOffset,
            object value,
            int valueOffset,
            int valueLength,
            bool isLiteral)
        {
            this.TemplateContents.Append(prefix);
            this.TemplateContents.Append(value);
        }

        /// <summary>
        /// Ends writing an attribute.
        /// </summary>
        public void EndWriteAttribute()
        {
            this.TemplateContents.Append(this.AttributeSuffix);
            this.AttributeSuffix = string.Empty;
        }

        /// <summary>
        /// Renders the page and writes the output to the <see cref="RazorEngineTemplateBase.TemplateContents" />.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the result of executing the page.</returns>
        public virtual async Task ExecuteAsync()
            => await Task.CompletedTask;

        /// <summary>
        /// Get the generated contents of the template.
        /// </summary>
        public virtual string Result()
            => this.TemplateContents.ToString();

        /// <summary>
        /// Encode the specified object using HTML entities.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <returns>The HTML encoded value as a <see cref="string" />.</returns>
        private string? HtmlEncode(object? value)
        {
            if(value == null)
            {
                return string.Empty;
            }

            return HttpUtility.HtmlEncode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }
}