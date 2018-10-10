using System;
using System.Collections.Generic;

using Moq;
using Xunit;

using MigrationEngine;
using MigrationEngine.Descriptors;
using MigrationEngine.Utilities;
using Munger;

namespace Engine.Tests.Utilities.FieldHtmlRectifierTest
{
    public class RewriteHtml
    {
        /// <summary>
        /// Verify that all HTML fields are cleaned up.
        /// </summary>
        [Theory]
        [InlineData("bodyfield")]
        [InlineData("contact_text")]
        [InlineData("contact")]
        [InlineData("additional_information")]
        public void CleanupHtmlFields(string fieldName)
        {
            string original = "<p>This is text";
            string expected = "<p>This is text</p>";

            // Minimal setup.
            ContentDescriptionBase item = new FullItemDescription()
            {
                ContentType = "TestType",
                UniqueIdentifier = "17",
            };
            item.Fields.Add("sys_lang", "en-us");
            item.Fields.Add(fieldName, original);

            // Create a munger which doesn't rewrite anything.
            Mock<IUrlMunger> mockMunger = new Mock<IUrlMunger>();
            string mungeMessage = String.Empty;
            mockMunger.Setup( x => x.RewriteUrls(It.IsAny<string>(), It.IsAny<string>(), out mungeMessage)).Returns((string s, string t, string u) => s).Verifiable();
            
            Dictionary<string, string> cleanFields = FieldHtmlRectifier.RewriteHtml(item, null, mockMunger.Object);

            string actualValue = cleanFields[fieldName];
            Assert.Equal(expected, actualValue);

            mockMunger.Verify();
        }

        /// <summary>
        /// Confirm that HTML entities are converted to Unicode characters.
        /// </summary>
        [Theory]
        [InlineData("&apos;", "'")]
        [InlineData("&quot;", "\"")]
        [InlineData("&reg;", "®")]
        [InlineData("&copy;", "©")]
        [InlineData("&lsquo;", "‘")]
        [InlineData("&rsquo;", "’")]
        [InlineData("&ldquo;", "“")]
        [InlineData("&rdquo;", "”")]
        [InlineData("&dagger;", "†")]
        [InlineData("&Dagger;", "‡")]
        public void ConvertHtmlEntities(string entity, string unicode)
        {
            string original = "<p>Entity: '" + entity + "'</p>";
            string expected = "<p>Entity: '" + unicode + "'</p>";

            // Minimal setup.
            ContentDescriptionBase item = new FullItemDescription()
            {
                ContentType = "TestType",
                UniqueIdentifier = "17",
            };
            item.Fields.Add("sys_lang", "en-us");
            item.Fields.Add("bodyfield", original);

            // Create a munger which doesn't rewrite anything.
            Mock<IUrlMunger> mockMunger = new Mock<IUrlMunger>();
            string mungeMessage = String.Empty;
            mockMunger.Setup(x => x.RewriteUrls(It.IsAny<string>(), It.IsAny<string>(), out mungeMessage)).Returns((string s, string t, string u) => s).Verifiable();

            Dictionary<string, string> cleanFields = FieldHtmlRectifier.RewriteHtml(item, null, mockMunger.Object);

            string actualValue = cleanFields["bodyfield"];
            Assert.Equal(expected, actualValue);

            mockMunger.Verify();
        }
    }
}
