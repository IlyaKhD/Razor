// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    /// <summary>
    /// A class used to run <see cref="ITagHelper"/>s.
    /// </summary>
    public class TagHelperRunner
    {
        /// <summary>
        /// Calls the <see cref="ITagHelper.ProcessAsync"/> method on <see cref="ITagHelper"/>s.
        /// </summary>
        /// <param name="executionContext">Contains information associated with running <see cref="ITagHelper"/>s.
        /// </param>
        /// <returns>Resulting <see cref="TagHelperOutput"/> from processing all of the
        /// <paramref name="executionContext"/>'s <see cref="ITagHelper"/>s.</returns>
        public async Task<TagHelperOutput> RunAsync(TagHelperExecutionContext executionContext)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            var tagHelperContext = new TagHelperContext(
                executionContext.AllAttributes,
                executionContext.Items,
                executionContext.UniqueId,
                executionContext.GetChildContentAsync);
            var tagHelperOutput = new TagHelperOutput(
                executionContext.TagName,
                executionContext.HTMLAttributes)
            {
                TagMode = executionContext.TagMode,
            };
            var orderedTagHelpers = executionContext.TagHelpers.OrderBy(tagHelper => tagHelper.Order);

            foreach (var tagHelper in orderedTagHelpers)
            {
                await tagHelper.ProcessAsync(tagHelperContext, tagHelperOutput);
            }

            return tagHelperOutput;
        }
    }
}