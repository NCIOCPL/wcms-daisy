using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

namespace MonikerProviders.Testing
{
    public class MonikerObject
    {
        [Fact]
#pragma warning disable 1718 // Disable warning about comparing the same variable
        public void MonikerEquivalance()
        {
            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker1a = new Moniker("moniker 1", 1, "testType");

            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");

            Assert.True(moniker1 == moniker1);
            Assert.True(moniker1 == moniker1a);
            Assert.False(moniker1 == moniker2);
        }
#pragma warning restore 1718


        [Fact]
        public void PropertyValues()
        {
            string name = "moniker";
            long contentID = 12345;
            string contentType = "ATestType";

            Moniker moniker = new Moniker(name, contentID, contentType);

            Assert.True(name == moniker.Name);
            Assert.True(contentID == moniker.ContentID);
            Assert.True(contentType == moniker.ContentType);
        }

    }
}
