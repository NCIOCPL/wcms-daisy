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
        public void VerifyAllHtmlFields(string fieldName)
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


            Mock<IUrlMunger> mockMunger = new Mock<IUrlMunger>();
            string mungeMessage = String.Empty;
            mockMunger.Setup( x => x.RewriteUrls(It.IsAny<string>(), It.IsAny<string>(), out mungeMessage)).Returns((string s, string t, string u) => s);
            

            Dictionary<string, string> cleanFields = FieldHtmlRectifier.ConvertToXHtml(item, null, mockMunger.Object);
        }
    }
}
