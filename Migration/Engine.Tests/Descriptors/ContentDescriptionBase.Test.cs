using MigrationEngine;
using MigrationEngine.Descriptors;
using Xunit;

namespace Engine.Tests.Descriptors
{
    public class ContentDescriptionBaseTest
    {
        /// <summary>
        /// Verify the Locale property works correctly.
        /// </summary>
        /// <param name="locale"></param>
        [Theory]
        [InlineData("en-us")]
        [InlineData("es-us")]
        [InlineData("zh-cn")]
        public void GetLocale(string locale)
        {
            // Minimal setup.
            ContentDescriptionBase item = new FullItemDescription() {
                ContentType = "TestType",
                UniqueIdentifier = "17",
            };
            item.Fields.Add("sys_lang", locale);

            Assert.Equal(locale, item.Locale);
        }

        /// <summary>
        /// Verify the Locale property fails when sys_lang isn't set.
        /// </summary>
        [Fact]
        public void GetMissingLocale()
        {
            // Minimal setup.
            ContentDescriptionBase item = new FullItemDescription()
            {
                ContentType = "TestType",
                UniqueIdentifier = "17",
            };

            Assert.Throws<DataFieldException>( () => {
                string locale = item.Locale;
            });
        }
    }
}
