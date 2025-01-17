// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.AspNet.Razor.Test.Internal;
using Xunit;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public abstract class TagHelperDescriptorFactoryTest
    {
        protected static readonly AssemblyName TagHelperDescriptorFactoryTestAssembly =
            typeof(TagHelperDescriptorFactoryTest).GetTypeInfo().Assembly.GetName();

        protected static readonly string AssemblyName = TagHelperDescriptorFactoryTestAssembly.Name;

        public static TheoryData RequiredParentData
        {
            get
            {
                // tagHelperType, expectedDescriptors
                return new TheoryData<Type, TagHelperDescriptor[]>
                {
                    {
                        typeof(RequiredParentTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "input",
                                TypeName = typeof(RequiredParentTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                RequiredParent = "div"
                            }
                        }
                    },
                    {
                        typeof(MultiSpecifiedRequiredParentTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "input",
                                TypeName = typeof(MultiSpecifiedRequiredParentTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                RequiredParent = "section"
                            },
                            new TagHelperDescriptor
                            {
                                TagName = "p",
                                TypeName = typeof(MultiSpecifiedRequiredParentTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                RequiredParent = "div"
                            }
                        }
                    },
                    {
                        typeof(MultiWithUnspecifiedRequiredParentTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "input",
                                TypeName = typeof(MultiWithUnspecifiedRequiredParentTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                RequiredParent = "div"
                            },
                            new TagHelperDescriptor
                            {
                                TagName = "p",
                                TypeName = typeof(MultiWithUnspecifiedRequiredParentTagHelper).FullName,
                                AssemblyName = AssemblyName
                            }
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(RequiredParentData))]
        public void CreateDescriptors_CreatesDesignTimeDescriptorsWithRequiredParent(
            Type tagHelperType,
            TagHelperDescriptor[] expectedDescriptors)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(tagHelperType),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);

            // We don't care about order. Mono returns reflected attributes differently so we need to ensure order
            // doesn't matter by sorting.
            descriptors = descriptors.OrderBy(descriptor => descriptor.TagName);

            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        public static TheoryData RestrictChildrenData
        {
            get
            {
                // tagHelperType, expectedDescriptors
                return new TheoryData<Type, TagHelperDescriptor[]>
                {
                    {
                        typeof(RestrictChildrenTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "restrict-children",
                                TypeName = typeof(RestrictChildrenTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                AllowedChildren = new[] { "p" },
                            }
                        }
                    },
                    {
                        typeof(DoubleRestrictChildrenTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "double-restrict-children",
                                TypeName = typeof(DoubleRestrictChildrenTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                AllowedChildren = new[] { "p", "strong" },
                            }
                        }
                    },
                    {
                        typeof(MultiTargetRestrictChildrenTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "div",
                                TypeName = typeof(MultiTargetRestrictChildrenTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                AllowedChildren = new[] { "p", "strong" },
                            },
                            new TagHelperDescriptor
                            {
                                TagName = "p",
                                TypeName = typeof(MultiTargetRestrictChildrenTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                AllowedChildren = new[] { "p", "strong" },
                            }
                        }
                    },
                };
            }
        }

        public abstract ITypeInfo GetTypeInfo(Type tagHelperType);

        [Theory]
        [MemberData(nameof(RestrictChildrenData))]
        public void CreateDescriptors_CreatesDescriptorsWithAllowedChildren(
            Type tagHelperType,
            TagHelperDescriptor[] expectedDescriptors)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(tagHelperType),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);

            // We don't care about order. Mono returns reflected attributes differently so we need to ensure order
            // doesn't matter by sorting.
            descriptors = descriptors.OrderBy(descriptor => descriptor.TagName);

            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        public static TheoryData TagStructureData
        {
            get
            {
                // tagHelperType, expectedDescriptors
                return new TheoryData<Type, TagHelperDescriptor[]>
                {
                    {
                        typeof(TagStructureTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "input",
                                TypeName = typeof(TagStructureTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                TagStructure = TagStructure.WithoutEndTag
                            }
                        }
                    },
                    {
                        typeof(MultiSpecifiedTagStructureTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "input",
                                TypeName = typeof(MultiSpecifiedTagStructureTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                TagStructure = TagStructure.WithoutEndTag
                            },
                            new TagHelperDescriptor
                            {
                                TagName = "p",
                                TypeName = typeof(MultiSpecifiedTagStructureTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                TagStructure = TagStructure.NormalOrSelfClosing
                            }
                        }
                    },
                    {
                        typeof(MultiWithUnspecifiedTagStructureTagHelper),
                        new[]
                        {
                            new TagHelperDescriptor
                            {
                                TagName = "input",
                                TypeName = typeof(MultiWithUnspecifiedTagStructureTagHelper).FullName,
                                AssemblyName = AssemblyName,
                                TagStructure = TagStructure.WithoutEndTag
                            },
                            new TagHelperDescriptor
                            {
                                TagName = "p",
                                TypeName = typeof(MultiWithUnspecifiedTagStructureTagHelper).FullName,
                                AssemblyName = AssemblyName
                            }
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(TagStructureData))]
        public void CreateDescriptors_CreatesDesignTimeDescriptorsWithTagStructure(
            Type tagHelperType,
            TagHelperDescriptor[] expectedDescriptors)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(tagHelperType),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);

            // We don't care about order. Mono returns reflected attributes differently so we need to ensure order
            // doesn't matter by sorting.
            descriptors = descriptors.OrderBy(descriptor => descriptor.TagName);

            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        public static TheoryData EditorBrowsableData
        {
            get
            {
                // tagHelperType, designTime, expectedDescriptors
                return new TheoryData<Type, bool, TagHelperDescriptor[]>
                {
                    {
                        typeof(InheritedEditorBrowsableTagHelper),
                        true,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "inherited-editor-browsable",
                                typeName: typeof(InheritedEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(InheritedEditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    { typeof(EditorBrowsableTagHelper), true, new TagHelperDescriptor[0] },
                    {
                        typeof(EditorBrowsableTagHelper),
                        false,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "editor-browsable",
                                typeName: typeof(EditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(EditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    {
                        typeof(HiddenPropertyEditorBrowsableTagHelper),
                        true,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "hidden-property-editor-browsable",
                                typeName: typeof(HiddenPropertyEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new TagHelperAttributeDescriptor[0])
                        }
                    },
                    {
                        typeof(HiddenPropertyEditorBrowsableTagHelper),
                        false,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "hidden-property-editor-browsable",
                                typeName: typeof(HiddenPropertyEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(HiddenPropertyEditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    {
                        typeof(OverriddenEditorBrowsableTagHelper),
                        true,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "overridden-editor-browsable",
                                typeName: typeof(OverriddenEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(OverriddenEditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    {
                        typeof(MultiPropertyEditorBrowsableTagHelper),
                        true,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "multi-property-editor-browsable",
                                typeName: typeof(MultiPropertyEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property2",
                                        PropertyName = nameof(MultiPropertyEditorBrowsableTagHelper.Property2),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    {
                        typeof(MultiPropertyEditorBrowsableTagHelper),
                        false,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "multi-property-editor-browsable",
                                typeName: typeof(MultiPropertyEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(MultiPropertyEditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    },
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property2",
                                        PropertyName = nameof(MultiPropertyEditorBrowsableTagHelper.Property2),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    {
                        typeof(OverriddenPropertyEditorBrowsableTagHelper),
                        true,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "overridden-property-editor-browsable",
                                typeName: typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new TagHelperAttributeDescriptor[0])
                        }
                    },
                    {
                        typeof(OverriddenPropertyEditorBrowsableTagHelper),
                        false,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "overridden-property-editor-browsable",
                                typeName: typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property2",
                                        PropertyName = nameof(OverriddenPropertyEditorBrowsableTagHelper.Property2),
                                        TypeName = typeof(int).FullName
                                    },
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(OverriddenPropertyEditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    {
                        typeof(DefaultEditorBrowsableTagHelper),
                        true,
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                tagName: "default-editor-browsable",
                                typeName: typeof(DefaultEditorBrowsableTagHelper).FullName,
                                assemblyName: AssemblyName,
                                attributes: new[]
                                {
                                    new TagHelperAttributeDescriptor
                                    {
                                        Name = "property",
                                        PropertyName = nameof(DefaultEditorBrowsableTagHelper.Property),
                                        TypeName = typeof(int).FullName
                                    }
                                })
                        }
                    },
                    { typeof(MultiEditorBrowsableTagHelper), true, new TagHelperDescriptor[0] }
                };
            }
        }

        [Theory]
        [MemberData(nameof(EditorBrowsableData))]
        public void CreateDescriptors_UnderstandsEditorBrowsableAttribute(
            Type tagHelperType,
            bool designTime,
            TagHelperDescriptor[] expectedDescriptors)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(tagHelperType),
                designTime,
                errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        public static TheoryData AttributeTargetData
        {
            get
            {
                var attributes = Enumerable.Empty<TagHelperAttributeDescriptor>();

                // tagHelperType, expectedDescriptors
                return new TheoryData<Type, IEnumerable<TagHelperDescriptor>>
                {
                    {
                        typeof(AttributeTargetingTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(AttributeTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class" })
                        }
                    },
                    {
                        typeof(MultiAttributeTargetingTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(MultiAttributeTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class", "style" })
                        }
                    },
                    {
                        typeof(MultiAttributeAttributeTargetingTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(MultiAttributeAttributeTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "custom" }),
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(MultiAttributeAttributeTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class", "style" })
                        }
                    },
                    {
                        typeof(InheritedAttributeTargetingTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(InheritedAttributeTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "style" })
                        }
                    },
                    {
                        typeof(RequiredAttributeTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                "input",
                                typeof(RequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class" })
                        }
                    },
                    {
                        typeof(InheritedRequiredAttributeTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                "div",
                                typeof(InheritedRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class" })
                        }
                    },
                    {
                        typeof(MultiAttributeRequiredAttributeTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                "div",
                                typeof(MultiAttributeRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class" }),
                            CreateTagHelperDescriptor(
                                "input",
                                typeof(MultiAttributeRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class" })
                        }
                    },
                    {
                        typeof(MultiAttributeSameTagRequiredAttributeTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                "input",
                                typeof(MultiAttributeSameTagRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "style" }),
                            CreateTagHelperDescriptor(
                                "input",
                                typeof(MultiAttributeSameTagRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class" })
                        }
                    },
                    {
                        typeof(MultiRequiredAttributeTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                "input",
                                typeof(MultiRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class", "style" })
                        }
                    },
                    {
                        typeof(MultiTagMultiRequiredAttributeTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                "div",
                                typeof(MultiTagMultiRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class", "style" }),
                            CreateTagHelperDescriptor(
                                "input",
                                typeof(MultiTagMultiRequiredAttributeTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class", "style" }),
                        }
                    },
                    {
                        typeof(AttributeWildcardTargetingTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(AttributeWildcardTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class*" })
                        }
                    },
                    {
                        typeof(MultiAttributeWildcardTargetingTagHelper),
                        new[]
                        {
                            CreateTagHelperDescriptor(
                                TagHelperDescriptorProvider.ElementCatchAllTarget,
                                typeof(MultiAttributeWildcardTargetingTagHelper).FullName,
                                AssemblyName,
                                attributes,
                                requiredAttributes: new[] { "class*", "style*" })
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(AttributeTargetData))]
        public void CreateDescriptors_ReturnsExpectedDescriptors(
            Type tagHelperType,
            IEnumerable<TagHelperDescriptor> expectedDescriptors)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(tagHelperType),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);

            // We don't care about order. Mono returns reflected attributes differently so we need to ensure order
            // doesn't matter by sorting.
            descriptors = descriptors.OrderBy(
                descriptor => CaseSensitiveTagHelperDescriptorComparer.Default.GetHashCode(descriptor)).ToArray();
            expectedDescriptors = expectedDescriptors.OrderBy(
                descriptor => CaseSensitiveTagHelperDescriptorComparer.Default.GetHashCode(descriptor)).ToArray();

            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        public static TheoryData HtmlCaseData
        {
            get
            {
                // tagHelperType, expectedTagName, expectedAttributeName
                return new TheoryData<Type, string, string>
                {
                    { typeof(SingleAttributeTagHelper), "single-attribute", "int-attribute" },
                    { typeof(ALLCAPSTAGHELPER), "allcaps", "allcapsattribute" },
                    { typeof(CAPSOnOUTSIDETagHelper), "caps-on-outside", "caps-on-outsideattribute" },
                    { typeof(capsONInsideTagHelper), "caps-on-inside", "caps-on-insideattribute" },
                    { typeof(One1Two2Three3TagHelper), "one1-two2-three3", "one1-two2-three3-attribute" },
                    { typeof(ONE1TWO2THREE3TagHelper), "one1two2three3", "one1two2three3-attribute" },
                    { typeof(First_Second_ThirdHiTagHelper), "first_second_third-hi", "first_second_third-attribute" },
                    { typeof(UNSuffixedCLASS), "un-suffixed-class", "un-suffixed-attribute" },
                };
            }
        }

        [Theory]
        [MemberData(nameof(HtmlCaseData))]
        public void CreateDescriptors_HtmlCasesTagNameAndAttributeName(
            Type tagHelperType,
            string expectedTagName,
            string expectedAttributeName)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(tagHelperType),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedTagName, descriptor.TagName, StringComparer.Ordinal);
            var attributeDescriptor = Assert.Single(descriptor.Attributes);
            Assert.Equal(expectedAttributeName, attributeDescriptor.Name);
        }

        [Fact]
        public void CreateDescriptors_OverridesAttributeNameFromAttribute()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var validProperty1 = typeof(OverriddenAttributeTagHelper).GetProperty(
                nameof(OverriddenAttributeTagHelper.ValidAttribute1));
            var validProperty2 = typeof(OverriddenAttributeTagHelper).GetProperty(
                nameof(OverriddenAttributeTagHelper.ValidAttribute2));
            var expectedDescriptors = new[]
            {
                CreateTagHelperDescriptor(
                    "overridden-attribute",
                    typeof(OverriddenAttributeTagHelper).FullName,
                    AssemblyName,
                    new[]
                    {
                        CreateTagHelperAttributeDescriptor("SomethingElse", validProperty1),
                        CreateTagHelperAttributeDescriptor("Something-Else", validProperty2)
                    })
            };

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(OverriddenAttributeTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_DoesNotInheritOverridenAttributeName()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var validProperty1 = typeof(InheritedOverriddenAttributeTagHelper).GetProperty(
                nameof(InheritedOverriddenAttributeTagHelper.ValidAttribute1));
            var validProperty2 = typeof(InheritedOverriddenAttributeTagHelper).GetProperty(
                nameof(InheritedOverriddenAttributeTagHelper.ValidAttribute2));
            var expectedDescriptors = new[]
            {
                CreateTagHelperDescriptor(
                    "inherited-overridden-attribute",
                    typeof(InheritedOverriddenAttributeTagHelper).FullName,
                    AssemblyName,
                    new[]
                    {
                        CreateTagHelperAttributeDescriptor("valid-attribute1", validProperty1),
                        CreateTagHelperAttributeDescriptor("Something-Else", validProperty2)
                    })
            };

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(InheritedOverriddenAttributeTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_AllowsOverridenAttributeNameOnUnimplementedVirtual()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var validProperty1 = typeof(InheritedNotOverriddenAttributeTagHelper).GetProperty(
                nameof(InheritedNotOverriddenAttributeTagHelper.ValidAttribute1));
            var validProperty2 = typeof(InheritedNotOverriddenAttributeTagHelper).GetProperty(
                nameof(InheritedNotOverriddenAttributeTagHelper.ValidAttribute2));
            var expectedDescriptors = new[]
            {
                CreateTagHelperDescriptor(
                    "inherited-not-overridden-attribute",
                    typeof(InheritedNotOverriddenAttributeTagHelper).FullName,
                    AssemblyName,
                    new[]
                    {
                        CreateTagHelperAttributeDescriptor("SomethingElse", validProperty1),
                        CreateTagHelperAttributeDescriptor("Something-Else", validProperty2)
                    })
            };

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(InheritedNotOverriddenAttributeTagHelper)),
                designTime: false,
                errorSink: errorSink);
            // Assert
            Assert.Empty(errorSink.Errors);
            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_BuildsDescriptorsWithInheritedProperties()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedDescriptor = CreateTagHelperDescriptor(
                "inherited-single-attribute",
                typeof(InheritedSingleAttributeTagHelper).FullName,
                AssemblyName,
                new[]
                {
                    new TagHelperAttributeDescriptor
                    {
                        Name = "int-attribute",
                        PropertyName = nameof(InheritedSingleAttributeTagHelper.IntAttribute),
                        TypeName = typeof(int).FullName
                    }
                });

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(InheritedSingleAttributeTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedDescriptor, descriptor, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_BuildsDescriptorsWithConventionNames()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var intProperty = typeof(SingleAttributeTagHelper).GetProperty(nameof(SingleAttributeTagHelper.IntAttribute));
            var expectedDescriptor = CreateTagHelperDescriptor(
                "single-attribute",
                typeof(SingleAttributeTagHelper).FullName,
                AssemblyName,
                new[]
                {
                    CreateTagHelperAttributeDescriptor("int-attribute", intProperty)
                });

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(SingleAttributeTagHelper)),
                designTime: false,
                errorSink: new ErrorSink());

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedDescriptor, descriptor, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_OnlyAcceptsPropertiesWithGetAndSet()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var validProperty = typeof(MissingAccessorTagHelper).GetProperty(
                nameof(MissingAccessorTagHelper.ValidAttribute));
            var expectedDescriptor = CreateTagHelperDescriptor(
                "missing-accessor",
                typeof(MissingAccessorTagHelper).FullName,
                AssemblyName,
                new[]
                {
                    CreateTagHelperAttributeDescriptor("valid-attribute", validProperty)
                });

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(MissingAccessorTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedDescriptor, descriptor, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_OnlyAcceptsPropertiesWithPublicGetAndSet()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var validProperty = typeof(NonPublicAccessorTagHelper).GetProperty(
                nameof(NonPublicAccessorTagHelper.ValidAttribute));
            var expectedDescriptor = CreateTagHelperDescriptor(
                "non-public-accessor",
                typeof(NonPublicAccessorTagHelper).FullName,
                AssemblyName,
                new[]
                {
                    CreateTagHelperAttributeDescriptor("valid-attribute", validProperty)
                });

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(NonPublicAccessorTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedDescriptor, descriptor, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_DoesNotIncludePropertiesWithNotBound()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedDescriptor = CreateTagHelperDescriptor(
                "not-bound-attribute",
                typeof(NotBoundAttributeTagHelper).FullName,
                AssemblyName,
                new[]
                {
                    new TagHelperAttributeDescriptor
                    {
                        Name = "bound-property",
                        PropertyName = nameof(NotBoundAttributeTagHelper.BoundProperty),
                        TypeName = typeof(object).FullName
                    }
                });

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(NotBoundAttributeTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedDescriptor, descriptor, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact(Skip = "#364")]
        public void CreateDescriptors_AddsErrorForTagHelperWithDuplicateAttributeNames()
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(DuplicateAttributeNameTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(descriptors);
            var error = Assert.Single(errorSink.Errors);
        }

        [Fact]
        public void CreateDescriptors_ResolvesMultipleTagHelperDescriptorsFromSingleType()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedDescriptors = new[]
            {
                CreateTagHelperDescriptor(
                    "div",
                    typeof(MultiTagTagHelper).FullName,
                    AssemblyName,
                    new[]
                    {
                        new TagHelperAttributeDescriptor
                        {
                            Name = "valid-attribute",
                            PropertyName = nameof(MultiTagTagHelper.ValidAttribute),
                            TypeName = typeof(string).FullName,
                            IsStringProperty = true
                        }
                    }),
                CreateTagHelperDescriptor(
                    "p",
                    typeof(MultiTagTagHelper).FullName,
                    AssemblyName,
                    new[]
                    {
                        new TagHelperAttributeDescriptor
                        {
                            Name = "valid-attribute",
                            PropertyName = nameof(MultiTagTagHelper.ValidAttribute),
                            TypeName = typeof(string).FullName,
                            IsStringProperty = true
                        }
                    })
            };

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(MultiTagTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);

            // We don't care about order. Mono returns reflected attributes differently so we need to ensure order
            // doesn't matter by sorting.
            descriptors = descriptors.OrderBy(descriptor => descriptor.TagName).ToArray();

            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_DoesNotResolveInheritedTagNames()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var validProp = typeof(InheritedMultiTagTagHelper).GetProperty(nameof(InheritedMultiTagTagHelper.ValidAttribute));
            var expectedDescriptor = CreateTagHelperDescriptor(
                    "inherited-multi-tag",
                    typeof(InheritedMultiTagTagHelper).FullName,
                    AssemblyName,
                    new[]
                    {
                        CreateTagHelperAttributeDescriptor("valid-attribute", validProp)
                    });

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(InheritedMultiTagTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            var descriptor = Assert.Single(descriptors);
            Assert.Equal(expectedDescriptor, descriptor, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_IgnoresDuplicateTagNamesFromAttribute()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedDescriptors = new[]
            {
                CreateTagHelperDescriptor(
                    "div",
                    typeof(DuplicateTagNameTagHelper).FullName,
                    AssemblyName),
                CreateTagHelperDescriptor(
                    "p",
                    typeof(DuplicateTagNameTagHelper).FullName,
                    AssemblyName)
            };

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(DuplicateTagNameTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);

            // We don't care about order. Mono returns reflected attributes differently so we need to ensure order
            // doesn't matter by sorting.
            descriptors = descriptors.OrderBy(descriptor => descriptor.TagName).ToArray();

            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        [Fact]
        public void CreateDescriptors_OverridesTagNameFromAttribute()
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedDescriptors = new[]
            {
                CreateTagHelperDescriptor(
                    "data-condition",
                    typeof(OverrideNameTagHelper).FullName,
                    AssemblyName),
            };

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(typeof(OverrideNameTagHelper)),
                designTime: false,
                errorSink: errorSink);

            // Assert
            Assert.Empty(errorSink.Errors);
            Assert.Equal(expectedDescriptors, descriptors, CaseSensitiveTagHelperDescriptorComparer.Default);
        }

        // name, expectedErrorMessages
        public static TheoryData<string, string[]> InvalidNameData
        {
            get
            {
                Func<string, string, string> onNameError =
                    (invalidText, invalidCharacter) => $"Tag helpers cannot target tag name '{ invalidText }' " +
                        $"because it contains a '{ invalidCharacter }' character.";
                var whitespaceErrorString = "Tag name cannot be null or whitespace.";

                var data = GetInvalidNameOrPrefixData(onNameError, whitespaceErrorString, onDataError: null);
                data.Add(string.Empty, new[] { whitespaceErrorString });

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(InvalidNameData))]
        public void ValidHtmlTargetElementAttributeNames_CreatesErrorOnInvalidNames(
            string name, string[] expectedErrorMessages)
        {
            // Arrange
            var errorSink = new ErrorSink();
            var attribute = new HtmlTargetElementAttribute(name);

            // Act
            TagHelperDescriptorFactory.ValidHtmlTargetElementAttributeNames(attribute, errorSink);

            // Assert
            var errors = errorSink.Errors.ToArray();
            Assert.Equal(expectedErrorMessages.Length, errors.Length);
            for (var i = 0; i < expectedErrorMessages.Length; i++)
            {
                Assert.Equal(0, errors[i].Length);
                Assert.Equal(SourceLocation.Zero, errors[i].Location);
                Assert.Equal(expectedErrorMessages[i], errors[i].Message, StringComparer.Ordinal);
            }
        }

        public static TheoryData ValidNameData
        {
            get
            {
                // name, expectedNames
                return new TheoryData<string, IEnumerable<string>>
                {
                    { "p", new[] { "p" } },
                    { " p", new[] { "p" } },
                    { "p ", new[] { "p" } },
                    { " p ", new[] { "p" } },
                    { "p,div", new[] { "p", "div" } },
                    { " p,div", new[] { "p", "div" } },
                    { "p ,div", new[] { "p", "div" } },
                    { " p ,div", new[] { "p", "div" } },
                    { "p, div", new[] { "p", "div" } },
                    { "p,div ", new[] { "p", "div" } },
                    { "p, div ", new[] { "p", "div" } },
                    { " p, div ", new[] { "p", "div" } },
                    { " p , div ", new[] { "p", "div" } },
                };
            }
        }

        [Theory]
        [MemberData(nameof(ValidNameData))]
        public void GetCommaSeparatedValues_OutputsCommaSeparatedListOfNames(
            string name,
            IEnumerable<string> expectedNames)
        {
            // Act
            var result = TagHelperDescriptorFactory.GetCommaSeparatedValues(name);

            // Assert
            Assert.Equal(expectedNames, result);
        }

        [Fact]
        public void GetCommaSeparatedValues_OutputsEmptyArrayForNullValue()
        {
            // Act
            var result = TagHelperDescriptorFactory.GetCommaSeparatedValues(text: null);

            // Assert
            Assert.Empty(result);
        }

        public static TheoryData InvalidTagHelperAttributeDescriptorData
        {
            get
            {
                var errorFormat = "Invalid tag helper bound property '{0}.{1}'. Tag helpers cannot bind to HTML " +
                    "attributes with name '{2}' because name starts with 'data-'.";

                // type, expectedAttributeDescriptors, expectedErrors
                return new TheoryData<Type, IEnumerable<TagHelperAttributeDescriptor>, string[]>
                {
                    {
                        typeof(InvalidBoundAttribute),
                        Enumerable.Empty<TagHelperAttributeDescriptor>(),
                        new[]
                        {
                            string.Format(
                                errorFormat,
                                typeof(InvalidBoundAttribute).FullName,
                                nameof(InvalidBoundAttribute.DataSomething),
                                "data-something")
                        }
                    },
                    {
                        typeof(InvalidBoundAttributeWithValid),
                        new[]
                        {
                            CreateTagHelperAttributeDescriptor(
                                "int-attribute",
                                typeof(InvalidBoundAttributeWithValid)
                                    .GetProperty(nameof(InvalidBoundAttributeWithValid.IntAttribute)))
                        },
                        new[]
                        {
                            string.Format(
                                errorFormat,
                                typeof(InvalidBoundAttributeWithValid).FullName,
                                nameof(InvalidBoundAttributeWithValid.DataSomething),
                                "data-something")
                        }
                    },
                    {
                        typeof(OverriddenInvalidBoundAttributeWithValid),
                        new[]
                        {
                            CreateTagHelperAttributeDescriptor(
                                "valid-something",
                                typeof(OverriddenInvalidBoundAttributeWithValid)
                                    .GetProperty(nameof(OverriddenInvalidBoundAttributeWithValid.DataSomething)))
                        },
                        new string[0]
                    },
                    {
                        typeof(OverriddenValidBoundAttributeWithInvalid),
                        Enumerable.Empty<TagHelperAttributeDescriptor>(),
                        new[]
                        {
                            string.Format(
                                errorFormat,
                                typeof(OverriddenValidBoundAttributeWithInvalid).FullName,
                                nameof(OverriddenValidBoundAttributeWithInvalid.ValidSomething),
                                "data-something")
                        }
                    },
                    {
                        typeof(OverriddenValidBoundAttributeWithInvalidUpperCase),
                        Enumerable.Empty<TagHelperAttributeDescriptor>(),
                        new[]
                        {
                            string.Format(
                                errorFormat,
                                typeof(OverriddenValidBoundAttributeWithInvalidUpperCase).FullName,
                                nameof(OverriddenValidBoundAttributeWithInvalidUpperCase.ValidSomething),
                                "DATA-SOMETHING")
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(InvalidTagHelperAttributeDescriptorData))]
        public void CreateDescriptors_DoesNotAllowDataDashAttributes(
            Type type,
            IEnumerable<TagHelperAttributeDescriptor> expectedAttributeDescriptors,
            string[] expectedErrors)
        {
            // Arrange
            var errorSink = new ErrorSink();

            // Act
            var descriptors = TagHelperDescriptorFactory.CreateDescriptors(
                AssemblyName,
                GetTypeInfo(type),
                designTime: false,
                errorSink: errorSink);

            // Assert
            var actualErrors = errorSink.Errors.ToArray();
            Assert.Equal(expectedErrors.Length, actualErrors.Length);

            for (var i = 0; i < actualErrors.Length; i++)
            {
                var actualError = actualErrors[i];
                Assert.Equal(0, actualError.Length);
                Assert.Equal(SourceLocation.Zero, actualError.Location);
                Assert.Equal(expectedErrors[i], actualError.Message, StringComparer.Ordinal);
            }

            var actualDescriptor = Assert.Single(descriptors);
            Assert.Equal(
                expectedAttributeDescriptors,
                actualDescriptor.Attributes,
                TagHelperAttributeDescriptorComparer.Default);
        }

        // tagTelperType, expectedAttributeDescriptors, expectedErrorMessages
        public static TheoryData<Type, IEnumerable<TagHelperAttributeDescriptor>, string[]> TagHelperWithPrefixData
        {
            get
            {
                Func<string, string, string> onError = (typeName, propertyName) =>
                    $"Invalid tag helper bound property '{ typeName }.{ propertyName }'. " +
                    $"'{ nameof(HtmlAttributeNameAttribute) }." +
                    $"{ nameof(HtmlAttributeNameAttribute.DictionaryAttributePrefix) }' must be null unless " +
                    "property type implements 'IDictionary<string, TValue>'.";

                // tagTelperType, expectedAttributeDescriptors, expectedErrorMessages
                return new TheoryData<Type, IEnumerable<TagHelperAttributeDescriptor>, string[]>
                {
                    {
                        typeof(DefaultValidHtmlAttributePrefix),
                        new[]
                        {
                            new TagHelperAttributeDescriptor
                            {
                                Name = "dictionary-property",
                                PropertyName = nameof(DefaultValidHtmlAttributePrefix.DictionaryProperty),
                                TypeName = typeof(IDictionary<string, string>).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "dictionary-property-",
                                PropertyName = nameof(DefaultValidHtmlAttributePrefix.DictionaryProperty),
                                TypeName = typeof(string).FullName,
                                IsIndexer = true
                            }
                        },
                        new string[0]
                    },
                    {
                        typeof(SingleValidHtmlAttributePrefix),
                        new[]
                        {
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name",
                                PropertyName = nameof(SingleValidHtmlAttributePrefix.DictionaryProperty),
                                TypeName = typeof(IDictionary<string, string>).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name-",
                                PropertyName = nameof(SingleValidHtmlAttributePrefix.DictionaryProperty),
                                TypeName = typeof(string).FullName,
                                IsIndexer = true
                            }
                        },
                        new string[0]
                    },
                    {
                        typeof(MultipleValidHtmlAttributePrefix),
                        new[]
                        {
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name1",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.DictionaryProperty),
                                TypeName = typeof(Dictionary<string, object>).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name2",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.DictionarySubclassProperty),
                                TypeName = typeof(DictionarySubclass).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name3",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.DictionaryWithoutParameterlessConstructorProperty),
                                TypeName = typeof(DictionaryWithoutParameterlessConstructor).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name4",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.GenericDictionarySubclassProperty),
                                TypeName = typeof(GenericDictionarySubclass<object>).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name5",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.SortedDictionaryProperty),
                                TypeName = typeof(SortedDictionary<string, int>).FullName
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name6",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.StringProperty),
                                TypeName = typeof(string).FullName,
                                IsStringProperty = true,
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-prefix1-",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.DictionaryProperty),
                                TypeName = typeof(object).FullName,
                                IsIndexer = true
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-prefix2-",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.DictionarySubclassProperty),
                                TypeName = typeof(string).FullName,
                                IsIndexer = true
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-prefix3-",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.DictionaryWithoutParameterlessConstructorProperty),
                                TypeName = typeof(string).FullName,
                                IsIndexer = true
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-prefix4-",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.GenericDictionarySubclassProperty),
                                TypeName = typeof(object).FullName,
                                IsIndexer = true
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-prefix5-",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.SortedDictionaryProperty),
                                TypeName = typeof(int).FullName,
                                IsIndexer = true
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "get-only-dictionary-property-",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.GetOnlyDictionaryProperty),
                                TypeName = typeof(int).FullName,
                                IsIndexer = true
                            },
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-prefix6",
                                PropertyName = nameof(MultipleValidHtmlAttributePrefix.GetOnlyDictionaryPropertyWithAttributePrefix),
                                TypeName = typeof(string).FullName,
                                IsIndexer = true
                            }
                        },
                        new string[0]
                    },
                    {
                        typeof(SingleInvalidHtmlAttributePrefix),
                        Enumerable.Empty<TagHelperAttributeDescriptor>(),
                        new[]
                        {
                            onError(
                                typeof(SingleInvalidHtmlAttributePrefix).FullName,
                                nameof(SingleInvalidHtmlAttributePrefix.StringProperty)),
                        }
                    },
                    {
                        typeof(MultipleInvalidHtmlAttributePrefix),
                        new[]
                        {
                            new TagHelperAttributeDescriptor
                            {
                                Name = "valid-name1",
                                PropertyName = nameof(MultipleInvalidHtmlAttributePrefix.LongProperty),
                                TypeName = typeof(long).FullName
                            }
                        },
                        new[]
                        {
                            onError(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntProperty)),
                            onError(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.ReadOnlyDictionaryProperty)),
                            onError(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.IntProperty)),
                            onError(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntSubclassProperty)),
                            onError(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryAttributePrefix)),
                            $"Invalid tag helper bound property '{ typeof(MultipleInvalidHtmlAttributePrefix).FullName }." +
                            $"{ nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryPropertyWithAttributeName) }'. " +
                            $"'{ typeof(HtmlAttributeNameAttribute).FullName }." +
                            $"{ nameof(HtmlAttributeNameAttribute.Name) }' must be null or empty if property has " +
                            "no public setter.",
                        }
                    },
                };
            }
        }

        public static TheoryData<string> ValidAttributeNameData
        {
            get
            {
                return new TheoryData<string>
                {
                    "data",
                    "dataa-",
                    "ValidName",
                    "valid-name",
                    "--valid--name--",
                    ",,--__..oddly.valid::;;",
                };
            }
        }

        [Theory]
        [MemberData(nameof(ValidAttributeNameData))]
        public void ValidateTagHelperAttributeDescriptor_WithValidName_ReturnsTrue(string name)
        {
            // Arrange
            var descriptor = new TagHelperAttributeDescriptor
            {
                Name = name,
                PropertyName = "ValidProperty",
                TypeName = "PropertyType"
            };
            var errorSink = new ErrorSink();

            // Act
            var result = TagHelperDescriptorFactory.ValidateTagHelperAttributeDescriptor(
                descriptor,
                GetTypeInfo(typeof(MultiTagTagHelper)),
                errorSink);

            // Assert
            Assert.True(result);
            Assert.Empty(errorSink.Errors);
        }

        public static TheoryData<string> ValidAttributePrefixData
        {
            get
            {
                return new TheoryData<string>
                {
                    string.Empty,
                    "data",
                    "dataa-",
                    "ValidName",
                    "valid-name",
                    "--valid--name--",
                    ",,--__..oddly.valid::;;",
                };
            }
        }

        [Theory]
        [MemberData(nameof(ValidAttributePrefixData))]
        public void ValidateTagHelperAttributeDescriptor_WithValidPrefix_ReturnsTrue(string prefix)
        {
            // Arrange
            var descriptor = new TagHelperAttributeDescriptor
            {
                Name = prefix,
                PropertyName = "ValidProperty",
                TypeName = "PropertyType",
                IsIndexer = true
            };
            var errorSink = new ErrorSink();

            // Act
            var result = TagHelperDescriptorFactory.ValidateTagHelperAttributeDescriptor(
                descriptor,
                GetTypeInfo(typeof(MultiTagTagHelper)),
                errorSink);

            // Assert
            Assert.True(result);
            Assert.Empty(errorSink.Errors);
        }

        // name, expectedErrorMessages
        public static TheoryData<string, string[]> InvalidAttributeNameData
        {
            get
            {
                Func<string, string, string> onNameError = (invalidText, invalidCharacter) => "Invalid tag helper " +
                    $"bound property '{ typeof(MultiTagTagHelper).FullName }.InvalidProperty'. Tag helpers cannot " +
                    $"bind to HTML attributes with name '{ invalidText }' because name contains a " +
                    $"'{ invalidCharacter }' character.";
                var whitespaceErrorString = "Invalid tag helper bound property " +
                    $"'{ typeof(MultiTagTagHelper).FullName }.InvalidProperty'. Tag helpers cannot bind to HTML " +
                    "attributes with a whitespace name.";
                Func<string, string> onDataError = invalidText => "Invalid tag helper bound property " +
                    $"'{ typeof(MultiTagTagHelper).FullName }.InvalidProperty'. Tag helpers cannot bind to HTML " +
                    $"attributes with name '{ invalidText }' because name starts with 'data-'.";

                return GetInvalidNameOrPrefixData(onNameError, whitespaceErrorString, onDataError);
            }
        }

        [Theory]
        [MemberData(nameof(InvalidAttributeNameData))]
        public void ValidateTagHelperAttributeDescriptor_WithInvalidName_AddsExpectedErrors(
            string name,
            string[] expectedErrorMessages)
        {
            // Arrange
            var descriptor = new TagHelperAttributeDescriptor
            {
                Name = name,
                PropertyName = "InvalidProperty",
                TypeName = "PropertyType"
            };
            var errorSink = new ErrorSink();

            // Act
            var result = TagHelperDescriptorFactory.ValidateTagHelperAttributeDescriptor(
                descriptor,
                GetTypeInfo(typeof(MultiTagTagHelper)),
                errorSink);

            // Assert
            Assert.False(result);

            var errors = errorSink.Errors.ToArray();
            Assert.Equal(expectedErrorMessages.Length, errors.Length);
            for (var i = 0; i < expectedErrorMessages.Length; i++)
            {
                Assert.Equal(0, errors[i].Length);
                Assert.Equal(SourceLocation.Zero, errors[i].Location);
                Assert.Equal(expectedErrorMessages[i], errors[i].Message, StringComparer.Ordinal);
            }
        }

        // prefix, expectedErrorMessages
        public static TheoryData<string, string[]> InvalidAttributePrefixData
        {
            get
            {
                Func<string, string, string> onPrefixError = (invalidText, invalidCharacter) => "Invalid tag helper " +
                    $"bound property '{ typeof(MultiTagTagHelper).FullName }.InvalidProperty'. Tag helpers cannot " +
                    $"bind to HTML attributes with prefix '{ invalidText }' because prefix contains a " +
                    $"'{ invalidCharacter }' character.";
                var whitespaceErrorString = "Invalid tag helper bound property " +
                    $"'{ typeof(MultiTagTagHelper).FullName }.InvalidProperty'. Tag helpers cannot bind to HTML " +
                    "attributes with a whitespace prefix.";
                Func<string, string> onDataError = invalidText => "Invalid tag helper bound property " +
                    $"'{ typeof(MultiTagTagHelper).FullName }.InvalidProperty'. Tag helpers cannot bind to HTML " +
                    $"attributes with prefix '{ invalidText }' because prefix starts with 'data-'.";

                return GetInvalidNameOrPrefixData(onPrefixError, whitespaceErrorString, onDataError);
            }
        }

        [Theory]
        [MemberData(nameof(InvalidAttributePrefixData))]
        public void ValidateTagHelperAttributeDescriptor_WithInvalidPrefix_AddsExpectedErrors(
            string prefix,
            string[] expectedErrorMessages)
        {
            // Arrange
            var descriptor = new TagHelperAttributeDescriptor
            {
                Name = prefix,
                PropertyName = "InvalidProperty",
                TypeName = "ValuesType",
                IsIndexer = true
            };
            var errorSink = new ErrorSink();

            // Act
            var result = TagHelperDescriptorFactory.ValidateTagHelperAttributeDescriptor(
                descriptor,
                GetTypeInfo(typeof(MultiTagTagHelper)),
                errorSink);

            // Assert
            Assert.False(result);

            var errors = errorSink.Errors.ToArray();
            Assert.Equal(expectedErrorMessages.Length, errors.Length);
            for (var i = 0; i < expectedErrorMessages.Length; i++)
            {
                Assert.Equal(0, errors[i].Length);
                Assert.Equal(SourceLocation.Zero, errors[i].Location);
                Assert.Equal(expectedErrorMessages[i], errors[i].Message, StringComparer.Ordinal);
            }
        }

        public static TheoryData<string, string[]> InvalidRestrictChildrenNameData
        {
            get
            {
                var nullOrWhiteSpaceError =
                    Resources.FormatTagHelperDescriptorFactory_InvalidRestrictChildrenAttributeNameNullWhitespace(
                        nameof(RestrictChildrenAttribute),
                        "SomeTagHelper");

                return GetInvalidNameOrPrefixData(
                    onNameError: (invalidInput, invalidCharacter) =>
                        Resources.FormatTagHelperDescriptorFactory_InvalidRestrictChildrenAttributeName(
                            nameof(RestrictChildrenAttribute),
                            invalidInput,
                            "SomeTagHelper",
                            invalidCharacter),
                    whitespaceErrorString: nullOrWhiteSpaceError,
                    onDataError: null);
            }
        }

        [Theory]
        [MemberData(nameof(InvalidRestrictChildrenNameData))]
        public void GetValidAllowedChildren_AddsExpectedErrors(string name, string[] expectedErrorMessages)
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedErrors = expectedErrorMessages.Select(
                message => new RazorError(message, SourceLocation.Zero, 0));

            // Act
            TagHelperDescriptorFactory.GetValidAllowedChildren(new[] { name }, "SomeTagHelper", errorSink);

            // Assert
            Assert.Equal(expectedErrors, errorSink.Errors);
        }

        public static TheoryData<string, string[]> InvalidParentTagData
        {
            get
            {
                var nullOrWhiteSpaceError =
                    Resources.FormatHtmlTargetElementAttribute_NameCannotBeNullOrWhitespace(
                        Resources.TagHelperDescriptorFactory_ParentTag);

                return GetInvalidNameOrPrefixData(
                    onNameError: (invalidInput, invalidCharacter) =>
                        Resources.FormatHtmlTargetElementAttribute_InvalidName(
                            Resources.TagHelperDescriptorFactory_ParentTag.ToLower(),
                            invalidInput,
                            invalidCharacter),
                    whitespaceErrorString: nullOrWhiteSpaceError,
                    onDataError: null);
            }
        }

        [Theory]
        [MemberData(nameof(InvalidParentTagData))]
        public void ValidateParentTagName_AddsExpectedErrors(string name, string[] expectedErrorMessages)
        {
            // Arrange
            var errorSink = new ErrorSink();
            var expectedErrors = expectedErrorMessages.Select(
                message => new RazorError(message, SourceLocation.Zero, 0));

            // Act
            TagHelperDescriptorFactory.ValidateParentTagName(name, errorSink);

            // Assert
            Assert.Equal(expectedErrors, errorSink.Errors);
        }

        private static TheoryData<string, string[]> GetInvalidNameOrPrefixData(
            Func<string, string, string> onNameError,
            string whitespaceErrorString,
            Func<string, string> onDataError)
        {
            // name, expectedErrorMessages
            var data = new TheoryData<string, string[]>
            {
                { "!", new[] {  onNameError("!", "!") } },
                { "hello!", new[] { onNameError("hello!", "!") } },
                { "!hello", new[] { onNameError("!hello", "!") } },
                { "he!lo", new[] { onNameError("he!lo", "!") } },
                {
                    "!he!lo!",
                    new[]
                    {
                        onNameError("!he!lo!", "!"),
                        onNameError("!he!lo!", "!"),
                        onNameError("!he!lo!", "!"),
                    }
                },
                { "@", new[] { onNameError("@", "@") } },
                { "hello@", new[] { onNameError("hello@", "@") } },
                { "@hello", new[] { onNameError("@hello", "@") } },
                { "he@lo", new[] { onNameError("he@lo", "@") } },
                {
                    "@he@lo@",
                    new[]
                    {
                        onNameError("@he@lo@", "@"),
                        onNameError("@he@lo@", "@"),
                        onNameError("@he@lo@", "@"),
                    }
                },
                { "/", new[] { onNameError("/", "/") } },
                { "hello/", new[] { onNameError("hello/", "/") } },
                { "/hello", new[] { onNameError("/hello", "/") } },
                { "he/lo", new[] { onNameError("he/lo", "/") } },
                {
                    "/he/lo/",
                    new[]
                    {
                        onNameError("/he/lo/", "/"),
                        onNameError("/he/lo/", "/"),
                        onNameError("/he/lo/", "/"),
                    }
                },
                { "<", new[] { onNameError("<", "<") } },
                { "hello<", new[] { onNameError("hello<", "<") } },
                { "<hello", new[] { onNameError("<hello", "<") } },
                { "he<lo", new[] { onNameError("he<lo", "<") } },
                {
                    "<he<lo<",
                    new[]
                    {
                        onNameError("<he<lo<", "<"),
                        onNameError("<he<lo<", "<"),
                        onNameError("<he<lo<", "<"),
                    }
                },
                { "?", new[] { onNameError("?", "?") } },
                { "hello?", new[] { onNameError("hello?", "?") } },
                { "?hello", new[] { onNameError("?hello", "?") } },
                { "he?lo", new[] { onNameError("he?lo", "?") } },
                {
                    "?he?lo?",
                    new[]
                    {
                        onNameError("?he?lo?", "?"),
                        onNameError("?he?lo?", "?"),
                        onNameError("?he?lo?", "?"),
                    }
                },
                { "[", new[] { onNameError("[", "[") } },
                { "hello[", new[] { onNameError("hello[", "[") } },
                { "[hello", new[] { onNameError("[hello", "[") } },
                { "he[lo", new[] { onNameError("he[lo", "[") } },
                {
                    "[he[lo[",
                    new[]
                    {
                        onNameError("[he[lo[", "["),
                        onNameError("[he[lo[", "["),
                        onNameError("[he[lo[", "["),
                    }
                },
                { ">", new[] { onNameError(">", ">") } },
                { "hello>", new[] { onNameError("hello>", ">") } },
                { ">hello", new[] { onNameError(">hello", ">") } },
                { "he>lo", new[] { onNameError("he>lo", ">") } },
                {
                    ">he>lo>",
                    new[]
                    {
                        onNameError(">he>lo>", ">"),
                        onNameError(">he>lo>", ">"),
                        onNameError(">he>lo>", ">"),
                    }
                },
                { "]", new[] { onNameError("]", "]") } },
                { "hello]", new[] { onNameError("hello]", "]") } },
                { "]hello", new[] { onNameError("]hello", "]") } },
                { "he]lo", new[] { onNameError("he]lo", "]") } },
                {
                    "]he]lo]",
                    new[]
                    {
                        onNameError("]he]lo]", "]"),
                        onNameError("]he]lo]", "]"),
                        onNameError("]he]lo]", "]"),
                    }
                },
                { "=", new[] { onNameError("=", "=") } },
                { "hello=", new[] { onNameError("hello=", "=") } },
                { "=hello", new[] { onNameError("=hello", "=") } },
                { "he=lo", new[] { onNameError("he=lo", "=") } },
                {
                    "=he=lo=",
                    new[]
                    {
                        onNameError("=he=lo=", "="),
                        onNameError("=he=lo=", "="),
                        onNameError("=he=lo=", "="),
                    }
                },
                { "\"", new[] { onNameError("\"", "\"") } },
                { "hello\"", new[] { onNameError("hello\"", "\"") } },
                { "\"hello", new[] { onNameError("\"hello", "\"") } },
                { "he\"lo", new[] { onNameError("he\"lo", "\"") } },
                {
                    "\"he\"lo\"",
                    new[]
                    {
                        onNameError("\"he\"lo\"", "\""),
                        onNameError("\"he\"lo\"", "\""),
                        onNameError("\"he\"lo\"", "\""),
                    }
                },
                { "'", new[] { onNameError("'", "'") } },
                { "hello'", new[] { onNameError("hello'", "'") } },
                { "'hello", new[] { onNameError("'hello", "'") } },
                { "he'lo", new[] { onNameError("he'lo", "'") } },
                {
                    "'he'lo'",
                    new[]
                    {
                        onNameError("'he'lo'", "'"),
                        onNameError("'he'lo'", "'"),
                        onNameError("'he'lo'", "'"),
                    }
                },
                { "hello*", new[] { onNameError("hello*", "*") } },
                { "*hello", new[] { onNameError("*hello", "*") } },
                { "he*lo", new[] { onNameError("he*lo", "*") } },
                {
                    "*he*lo*",
                    new[]
                    {
                        onNameError("*he*lo*", "*"),
                        onNameError("*he*lo*", "*"),
                        onNameError("*he*lo*", "*"),
                    }
                },
                { Environment.NewLine, new[] { whitespaceErrorString } },
                { "\t", new[] { whitespaceErrorString } },
                { " \t ", new[] { whitespaceErrorString } },
                { " ", new[] { whitespaceErrorString } },
                { Environment.NewLine + " ", new[] { whitespaceErrorString } },
                {
                    "! \t\r\n@/<>?[]=\"'*",
                    new[]
                    {
                        onNameError("! \t\r\n@/<>?[]=\"'*", "!"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", " "),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\t"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\r"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\n"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "@"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "/"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "<"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", ">"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "?"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "["),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "]"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "="),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\""),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "'"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "*"),
                    }
                },
                {
                    "! \tv\ra\nl@i/d<>?[]=\"'*",
                    new[]
                    {
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "!"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", " "),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\t"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\r"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\n"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "@"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "/"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "<"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", ">"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "?"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "["),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "]"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "="),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\""),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "'"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "*"),
                    }
                },
            };

            if (onDataError != null)
            {
                data.Add("data-", new[] { onDataError("data-") });
                data.Add("data-something", new[] { onDataError("data-something") });
                data.Add("Data-Something", new[] { onDataError("Data-Something") });
                data.Add("DATA-SOMETHING", new[] { onDataError("DATA-SOMETHING") });
            }

            return data;
        }

        protected static TagHelperDescriptor CreateTagHelperDescriptor(
            string tagName,
            string typeName,
            string assemblyName,
            IEnumerable<TagHelperAttributeDescriptor> attributes = null,
            IEnumerable<string> requiredAttributes = null)
        {
            return new TagHelperDescriptor
            {
                TagName = tagName,
                TypeName = typeName,
                AssemblyName = assemblyName,
                Attributes = attributes ?? Enumerable.Empty<TagHelperAttributeDescriptor>(),
                RequiredAttributes = requiredAttributes ?? Enumerable.Empty<string>()
            };
        }

        private static TagHelperAttributeDescriptor CreateTagHelperAttributeDescriptor(
            string name,
            PropertyInfo propertyInfo)
        {
            return new TagHelperAttributeDescriptor
            {
                Name = name,
                PropertyName = propertyInfo.Name,
                TypeName = propertyInfo.PropertyType.FullName,
                IsStringProperty = propertyInfo.PropertyType.FullName == typeof(string).FullName
            };
        }
    }
}