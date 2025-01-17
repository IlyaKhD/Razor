// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using Microsoft.AspNet.Testing;
using Microsoft.Extensions.WebEncoders.Testing;
using Xunit;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class DefaultTagHelperContentTest
    {
        [Fact]
        public void CanSetContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[Hello World!]]";

            // Act
            tagHelperContent.SetContent("Hello World!");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void SetContentClearsExistingContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[Hello World!]]";
            tagHelperContent.SetContent("Contoso");

            // Act
            tagHelperContent.SetContent("Hello World!");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Theory]
        [InlineData("HelloWorld!", "HtmlEncode[[HelloWorld!]]")]
        [InlineData("  ", "HtmlEncode[[  ]]")]
        public void SetContent_WithTagHelperContent_WorksAsExpected(string content, string expected)
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var copiedTagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetContent(content);

            // Act
            copiedTagHelperContent.SetContent(tagHelperContent);

            // Assert
            Assert.Equal(expected, copiedTagHelperContent.GetContent(new CommonTestEncoder()));
        }

        // GetContent - this one relies on the default encoder.
        [Fact]
        public void CanGetContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "Hello World!";
            tagHelperContent.SetContent(expected);

            // Act
            var actual = tagHelperContent.GetContent();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAppendContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[Hello World!]]";

            // Act
            tagHelperContent.Append("Hello World!");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void CanAppendFormatContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.AppendFormat("{0} {1} {2} {3}!", "First", "Second", "Third", "Fourth");

            // Assert
            Assert.Equal(
                "HtmlEncode[[First]] HtmlEncode[[Second]] HtmlEncode[[Third]] HtmlEncode[[Fourth]]!",
                tagHelperContent.GetContent(new CommonTestEncoder()));
        }
        
        [Fact]
        public void CanAppendFormat_WithCulture()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.AppendFormat(
                CultureInfo.InvariantCulture,
                "Numbers in InvariantCulture - {0:N} {1} {2} {3}!",
                1.1,
                2.98,
                145.82,
                32.86);

            // Assert
            Assert.Equal(
                "Numbers in InvariantCulture - HtmlEncode[[1.10]] HtmlEncode[[2.98]] " +
                    "HtmlEncode[[145.82]] HtmlEncode[[32.86]]!",
                tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void CanAppendDefaultTagHelperContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var helloWorldContent = new DefaultTagHelperContent();
            helloWorldContent.SetContent("HelloWorld");
            var expected = "Content was HtmlEncode[[HelloWorld]]";

            // Act
            tagHelperContent.AppendFormat("Content was {0}", helloWorldContent);

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void Append_WithTagHelperContent_MultipleAppends()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var copiedTagHelperContent = new DefaultTagHelperContent();
            var text1 = "Hello";
            var text2 = " World!";
            var expected = "HtmlEncode[[Hello]]HtmlEncode[[ World!]]";
            tagHelperContent.Append(text1);
            tagHelperContent.Append(text2);

            // Act
            copiedTagHelperContent.Append(tagHelperContent);

            // Assert
            Assert.Equal(expected, copiedTagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void IsModified_TrueAfterSetContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.SetContent(string.Empty);

            // Assert
            Assert.True(tagHelperContent.IsModified);
        }

        
        [Fact]
        public void IsModified_TrueAfterAppend()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.Append(string.Empty);

            // Assert
            Assert.True(tagHelperContent.IsModified);
        }

        [Fact]
        public void IsModified_TrueAfterClear()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.Clear();

            // Assert
            Assert.True(tagHelperContent.IsModified);
        }

        [Fact]
        public void IsModified_TrueIfAppendedNull()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            TagHelperContent NullContent = null;

            // Act
            tagHelperContent.Append(NullContent);

            // Assert
            Assert.True(tagHelperContent.IsModified);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        [InlineData("\r")]
        public void CanIdentifyWhiteSpace(string data)
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.SetContent("  ");
            tagHelperContent.Append(data);

            // Assert
            Assert.True(tagHelperContent.IsWhiteSpace);
        }

        [Fact]
        public void CanIdentifyWhiteSpace_WithoutIgnoringStrings()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.SetContent("  ");
            tagHelperContent.Append("Hello");

            // Assert
            Assert.False(tagHelperContent.IsWhiteSpace);
        }

        [Fact]
        public void IsEmpty_InitiallyTrue()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act & Assert
            Assert.True(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_TrueAfterSetEmptyContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.SetContent(string.Empty);

            // Assert
            Assert.True(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_TrueAfterAppendEmptyContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.Append(string.Empty);
            tagHelperContent.Append(string.Empty);

            // Assert
            Assert.True(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_TrueAfterAppendEmptyTagHelperContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var copiedTagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.Append(copiedTagHelperContent);
            tagHelperContent.Append(string.Empty);

            // Assert
            Assert.True(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_TrueAfterClear()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.Clear();

            // Assert
            Assert.True(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseAfterSetContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.SetContent("Hello");

            // Assert
            Assert.False(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseAfterAppend()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();

            // Act
            tagHelperContent.Append("Hello");

            // Assert
            Assert.False(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void IsEmpty_FalseAfterAppendTagHelper()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var copiedTagHelperContent = new DefaultTagHelperContent();
            copiedTagHelperContent.SetContent("Hello");

            // Act
            tagHelperContent.Append(copiedTagHelperContent);

            // Assert
            Assert.False(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void CanClearContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetContent("Hello World");

            // Act
            tagHelperContent.Clear();

            // Assert
            Assert.True(tagHelperContent.IsEmpty);
        }

        [Fact]
        public void Fluent_SetContent_Append_WritesExpectedContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[Hello ]]HtmlEncode[[World!]]";

            // Act
            tagHelperContent.SetContent("Hello ").Append("World!");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void Fluent_SetContent_AppendFormat_WritesExpectedContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[First ]]HtmlEncode[[Second]] Third";

            // Act
            tagHelperContent.SetContent("First ").AppendFormat("{0} Third", "Second");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void Fluent_SetContent_AppendFormat_Append_WritesExpectedContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[First ]]HtmlEncode[[Second]] Third HtmlEncode[[Fourth]]";

            // Act
            tagHelperContent
                .SetContent("First ")
                .AppendFormat("{0} Third ", "Second")
                .Append("Fourth");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void Fluent_Clear_SetContent_WritesExpectedContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[Hello World]]";
            tagHelperContent.SetContent("Some Random Content");

            // Act
            tagHelperContent.Clear().SetContent("Hello World");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void Fluent_Clear_Set_Append_WritesExpectedContent()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var expected = "HtmlEncode[[Hello ]]HtmlEncode[[World!]]";
            tagHelperContent.SetContent("Some Random Content");

            // Act
            tagHelperContent.Clear().SetContent("Hello ").Append("World!");

            // Assert
            Assert.Equal(expected, tagHelperContent.GetContent(new CommonTestEncoder()));
        }

        [Fact]
        public void WriteTo_WritesToATextWriter()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            var writer = new StringWriter();
            tagHelperContent.SetContent("Hello ");
            tagHelperContent.Append("World");

            // Act
            tagHelperContent.WriteTo(writer, new CommonTestEncoder());

            // Assert
            Assert.Equal("HtmlEncode[[Hello ]]HtmlEncode[[World]]", writer.ToString());
        }

        [Fact]
        public void Append_WrittenAsEncoded()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.Append("Hi");

            var writer = new StringWriter();

            // Act
            tagHelperContent.WriteTo(writer, new CommonTestEncoder());

            // Assert
            Assert.Equal("HtmlEncode[[Hi]]", writer.ToString());
        }

        [Fact]
        public void AppendEncoded_DoesNotGetEncoded()
        {
            // Arrange
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.AppendEncoded("Hi");

            var writer = new StringWriter();

            // Act
            tagHelperContent.WriteTo(writer, new CommonTestEncoder());

            // Assert
            Assert.Equal("Hi", writer.ToString());
        }
    }
}
