using System;
using System.Collections.Generic;

using Moq;
using Xunit;

using MigrationEngine;
using MigrationEngine.Descriptors;
using MigrationEngine.Utilities;
using Munger;
using AngleSharp.Dom;

namespace Engine.Tests.Utilities.FieldHtmlRectifierTest
{
    public class RewriteHtml
    {
        // Fake logger.
        Mock<IMigrationLog> mockLogger = new Mock<IMigrationLog>();

        // Munger which doesn't rewrite anything.
        Mock<IUrlMunger> mockMunger = new Mock<IUrlMunger>();

        public RewriteHtml()
        {
            // Set up the fake munger.
            string mungeMessage = String.Empty;
            mockMunger.Setup(x => x.RewriteUrls(It.IsAny<IDocument>(), It.IsAny<string>(), out mungeMessage)).Verifiable();
        }


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
            mockMunger.Setup( x => x.RewriteUrls(It.IsAny<IDocument>(), It.IsAny<string>(), out mungeMessage)).Verifiable();
            
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

            Dictionary<string, string> cleanFields = FieldHtmlRectifier.RewriteHtml(item, mockLogger.Object, mockMunger.Object);

            string actualValue = cleanFields["bodyfield"];
            Assert.Equal(expected, actualValue);

            mockMunger.Verify();
        }

        /// <summary>
        /// Confirm that HTML parsing errors are caught.
        /// </summary>
        /// <remarks>FieldHtmlRectifier.RewriteHtml expects fields to contain HTML *fragments*, not full documents.</remarks>
        /// <param name="snippet">HTML snippet containing an error.</param>
        /// <param name="expectedCount">The number of errors the fragment contains.</param>
        [Theory]
        [InlineData("<p>Tags <em><b>closed in wrong order</em></b>", 2)]
        [InlineData("<p>Tags <b>Unclosed tag</p>", 1)]
        [InlineData("<p>Tags <b>broken closing tag </b</p>", 4)]
        public void HtmlWithParseErrors(string snippet, int expectedCount)
        {
            // Minimal setup.
            ContentDescriptionBase item = new FullItemDescription()
            {
                ContentType = "TestType",
                UniqueIdentifier = "17",
            };
            item.Fields.Add("sys_lang", "en-us");
            item.Fields.Add("bodyfield", snippet);

            // logger to get warnings
            String warnings = String.Empty;
            Mock<IMigrationLog> warningLogger = new Mock<IMigrationLog>();
            warningLogger.Setup(x => x.LogTaskItemWarning(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Callback((string uniqueID, string message, Dictionary<string,string> fields) => { warnings = message; });

            Dictionary<string, string> cleanFields = FieldHtmlRectifier.RewriteHtml(item, warningLogger.Object, mockMunger.Object);

            Array list = warnings.Split('\n');
            Assert.Equal(expectedCount, list.Length);
            warningLogger.Verify();
        }

        /// <summary>
        /// Confirm that HTML without parsing errors goes through correctly.
        /// </summary>
        /// <remarks>FieldHtmlRectifier.RewriteHtml expects fields to contain HTML *fragments*, not full documents.</remarks>
        /// <param name="snippet">HTML snippet containing an error.</param>
        /// <param name="expectedCount">The number of errors the fragment contains.</param>
        [Theory]
        [InlineData("<p>Tags <em><b>closed in correct order</b></em>")]
        public void HtmlWithNoParseErrors(string snippet)
        {
            // Minimal setup.
            ContentDescriptionBase item = new FullItemDescription()
            {
                ContentType = "TestType",
                UniqueIdentifier = "17",
            };
            item.Fields.Add("sys_lang", "en-us");
            item.Fields.Add("bodyfield", snippet);

            // logger to get warnings
            String warnings = String.Empty;
            Mock<IMigrationLog> warningLogger = new Mock<IMigrationLog>();
            warningLogger.Setup(x => x.LogTaskItemWarning(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Callback((string uniqueID, string message, Dictionary<string, string> fields) => { warnings = message; });

            Dictionary<string, string> cleanFields = FieldHtmlRectifier.RewriteHtml(item, warningLogger.Object, mockMunger.Object);

            Assert.Equal(String.Empty, warnings);
            warningLogger.Verify();
        }
    }
}
