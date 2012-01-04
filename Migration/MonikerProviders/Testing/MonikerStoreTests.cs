using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;

using NUnit.Framework;

using MonikerProviders;
using MonikerProviders.Configuration;

namespace MonikerProviders.Testing
{
    [TestFixture]
    public class MonikerStoreTests
    {
        const string MONIKER_STORE_NAME = "MonikerStoreProvider";

        [SetUp]
        public void TestPrep()
        {
            // Clear out any persisted data from previous runs.
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);
            store.Clear();
        }

        /// <summary>
        /// Can I load a provider? Any provider?
        /// </summary>
        [Test]
        public void CreateMonikerStore()
        {
            MonikerStore store;
            Assert.DoesNotThrow(delegate { store = new MonikerStore(MONIKER_STORE_NAME); });
        }

        [Test]
        public void AddMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            Assert.DoesNotThrow(delegate { store.Add(moniker1); });
            Assert.DoesNotThrow(delegate { store.Add(moniker2); });
            Assert.DoesNotThrow(delegate { store.Add(moniker3); });
        }

        [Test]
        public void ContainsMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            Assert.IsTrue(store.Contains(moniker1.Name));
            Assert.IsTrue(store.Contains(moniker2.Name));
            Assert.IsTrue(store.Contains(moniker3.Name));

            Assert.IsFalse(store.Contains("moniker 4"));
        }

        [Test]
        public void DeleteMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            store.Delete(moniker2.Name);

            Assert.IsTrue(store.Contains(moniker1.Name));
            Assert.IsTrue(store.Contains(moniker3.Name));

            Assert.IsFalse(store.Contains(moniker2.Name));
        }

        [Test]
        public void GetMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            Moniker testMoniker = null;
            Assert.DoesNotThrow(delegate { testMoniker = store.Get(moniker1.Name); });

            Assert.IsNotNull(testMoniker);

            Assert.IsTrue(testMoniker == moniker1);
            Assert.IsFalse(testMoniker == moniker2);
        }

        [Test]
        public void ClearStore()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            Assert.DoesNotThrow(delegate { store.Clear(); });

            Assert.IsFalse(store.Contains(moniker1.Name));
            Assert.IsFalse(store.Contains(moniker2.Name));
            Assert.IsFalse(store.Contains(moniker3.Name));

            Assert.IsFalse(store.Contains("moniker 4"));

        }

        [Test]
        public void PersistenceWithoutContents()
        {
            MonikerStore store1 = new MonikerStore(MONIKER_STORE_NAME);
            store1.Clear();
            store1 = null;

            MonikerStore store2 = new MonikerStore(MONIKER_STORE_NAME);

            Assert.IsFalse(store2.Contains("moniker 2"));
        }

        [Test]
        public void PersistenceWithContents()
        {
            MonikerStore store1 = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store1.Add(moniker1);
            store1.Add(moniker2);
            store1.Add(moniker3);

            store1 = null;

            MonikerStore store2 = new MonikerStore(MONIKER_STORE_NAME);

            Assert.IsTrue(store2.Contains(moniker2.Name));

            Moniker testMoniker = store2.Get(moniker2.Name);
            Assert.IsTrue(testMoniker == moniker2);
        }

        [Test]
        public void CaseSensitivity()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            string name1 = "MoNiKer 1";
            string name2 = "mOnIkeR 2";
            string name3 = "MoNiKer 3";

            store.Add(name1, 1, "testType");
            store.Add(name2, 2, "testType");
            store.Add(name3, 3, "testType");

            Assert.IsTrue(store.Contains(name1));
            Assert.IsTrue(store.Contains(name2));
            Assert.IsTrue(store.Contains(name3));

            Assert.IsTrue(store.Contains(name1.ToUpper()));
            Assert.IsTrue(store.Contains(name2.ToUpper()));
            Assert.IsTrue(store.Contains(name3.ToUpper()));

            Assert.IsTrue(store.Contains(name1.ToLower()));
            Assert.IsTrue(store.Contains(name2.ToLower()));
            Assert.IsTrue(store.Contains(name3.ToLower()));
        }
    }
}
