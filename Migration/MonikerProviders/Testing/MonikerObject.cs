using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MonikerProviders.Testing
{
    [TestFixture]
    public class MonikerObject
    {
        [Test]
#pragma warning disable 1718 // Disable warning about comparing the same variable
        public void MonikerEquivalance()
        {
            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker1a = new Moniker("moniker 1", 1, "testType");

            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");

            Assert.IsTrue(moniker1 == moniker1);
            Assert.IsTrue(moniker1 == moniker1a);
            Assert.IsFalse(moniker1 == moniker2);
        }
#pragma warning restore 1718


        [Test]
        public void PropertyValues()
        {
            string name = "moniker";
            long contentID = 12345;
            string contentType = "ATestType";

            Moniker moniker = new Moniker(name, contentID, contentType);

            Assert.IsTrue(name == moniker.Name);
            Assert.IsTrue(contentID == moniker.ContentID);
            Assert.IsTrue(contentType == moniker.ContentType);
        }

    }
}
