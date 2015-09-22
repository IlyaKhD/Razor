// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class FormattedHtmlContent : IHtmlContent
    {
        private readonly IFormatProvider _formatProvider;
        private readonly string _format;
        private readonly object[] _args;

        public FormattedHtmlContent(string format, object[] args)
            : this(null, format, args)
        {
        }

        public FormattedHtmlContent(IFormatProvider formatProvider, string format, object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            _formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
            _format = format;
            _args = args;
        }

        public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            var formatProvider = new EncodingFormatProvider(_formatProvider, encoder);
            var builder = new StringBuilder();
            builder.AppendFormat(formatProvider, _format, _args);

            for (var i = 0; i < builder.Length; i++)
            {
                writer.Write(builder[i]);
            }
        }

        private class EncodingFormatProvider : IFormatProvider, ICustomFormatter
        {
            private IHtmlEncoder _encoder;
            private IFormatProvider _formatProvider;

            public EncodingFormatProvider(IFormatProvider formatProvider, IHtmlEncoder encoder)
            {
                _formatProvider = formatProvider;
                _encoder = encoder;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                var htmlContent = arg as IHtmlContent;
                if (htmlContent != null)
                {
                    using (var writer = new StringWriter())
                    {
                        htmlContent.WriteTo(writer, _encoder);
                        return writer.ToString();
                    }
                }

                var customFormatter = (ICustomFormatter)_formatProvider.GetFormat(typeof(ICustomFormatter));
                if (customFormatter != null)
                {
                    var result = customFormatter.Format(format, arg, CultureInfo.CurrentCulture);
                    if (result != null)
                    {
                        return _encoder.HtmlEncode(result);
                    }
                }

                var formattable = arg as IFormattable;
                if (formattable != null)
                {
                    var result = formattable.ToString(format, _formatProvider);
                    if (result != null)
                    {
                        return _encoder.HtmlEncode(result);
                    }
                }

                if (arg != null)
                {
                    var result = arg.ToString();
                    if (result != null)
                    {
                        return _encoder.HtmlEncode(result);
                    }
                }

                return string.Empty;
            }

            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                {
                    return this;
                }

                return null;
            }
        }
    }
}
