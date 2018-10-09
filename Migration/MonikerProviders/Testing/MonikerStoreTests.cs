using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;

using Xunit;

using MonikerProviders;
using MonikerProviders.Configuration;

namespace MonikerProviders.Testing
{
    public class MonikerStoreTests
    {
        const string MONIKER_STORE_NAME = "MonikerStoreProvider";

        /// <summary>
        /// Initalization for the test fixture is done in the constructor.
        /// This differs from the [Setup] attribute used by XUnit.
        /// </summary>
        public MonikerStoreTests()
        {
            // Clear out any persisted data from previous runs.
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);
            store.Clear();
        }

        /// <summary>
        /// Can I load a provider? Any provider?
        /// </summary>
        [Fact]
        public void CreateMonikerStore()
        {
            MonikerStore store;

            store = new MonikerStore(MONIKER_STORE_NAME);
            Assert.NotNull(store);
        }

        [Fact]
        public void AddMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            // Test that these don't throw an exception.
            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);
        }

        [Fact]
        public void ContainsMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            Assert.True(store.Contains(moniker1.Name));
            Assert.True(store.Contains(moniker2.Name));
            Assert.True(store.Contains(moniker3.Name));

            Assert.False(store.Contains("moniker 4"));
        }

        [Fact]
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

            Assert.True(store.Contains(moniker1.Name));
            Assert.True(store.Contains(moniker3.Name));

            Assert.False(store.Contains(moniker2.Name));
        }

        [Fact]
        public void GetMoniker()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            Moniker testMoniker = store.Get(moniker1.Name);

            Assert.NotNull(testMoniker);

            Assert.True(testMoniker == moniker1);
            Assert.False(testMoniker == moniker2);
        }

        [Fact]
        public void ClearStore()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            store.Clear();

            Assert.False(store.Contains(moniker1.Name));
            Assert.False(store.Contains(moniker2.Name));
            Assert.False(store.Contains(moniker3.Name));

            Assert.False(store.Contains("moniker 4"));

        }

        [Fact]
        public void PersistenceWithoutContents()
        {
            MonikerStore store1 = new MonikerStore(MONIKER_STORE_NAME);
            store1.Clear();
            store1 = null;

            MonikerStore store2 = new MonikerStore(MONIKER_STORE_NAME);

            Assert.False(store2.Contains("moniker 2"));
        }

        [Fact]
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

            Assert.True(store2.Contains(moniker2.Name));

            Moniker testMoniker = store2.Get(moniker2.Name);
            Assert.True(testMoniker == moniker2);
        }

        [Fact]
        public void CaseSensitivity()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            string name1 = "MoNiKer 1";
            string name2 = "mOnIkeR 2";
            string name3 = "MoNiKer 3";

            store.Add(name1, 1, "testType");
            store.Add(name2, 2, "testType");
            store.Add(name3, 3, "testType");

            Assert.True(store.Contains(name1));
            Assert.True(store.Contains(name2));
            Assert.True(store.Contains(name3));

            Assert.True(store.Contains(name1.ToUpper()));
            Assert.True(store.Contains(name2.ToUpper()));
            Assert.True(store.Contains(name3.ToUpper()));

            Assert.True(store.Contains(name1.ToLower()));
            Assert.True(store.Contains(name2.ToLower()));
            Assert.True(store.Contains(name3.ToLower()));
        }

        [Fact]
        public void Duplicate()
        {
            MonikerStore store = new MonikerStore(MONIKER_STORE_NAME);

            Moniker moniker1 = new Moniker("moniker 1", 1, "testType");
            Moniker moniker2 = new Moniker("moniker 2", 2, "testType");
            Moniker moniker3 = new Moniker("moniker 3", 3, "testType");

            store.Add(moniker1);
            store.Add(moniker2);
            store.Add(moniker3);

            Assert.Throws<DuplicateMonikerException>(() => { store.Add(moniker1); });
            Assert.Throws<DuplicateMonikerException>(() => { store.Add(moniker2); });
            Assert.Throws<DuplicateMonikerException>(() => { store.Add(moniker3); });

            Assert.Throws<DuplicateMonikerException>(() => { store.Add("moniker 1", 4, "testType2"); });
            Assert.Throws<DuplicateMonikerException>(() => { store.Add("moniker 2", 5, "testType3"); });
            Assert.Throws<DuplicateMonikerException>(() => { store.Add("moniker 3", 6, "testType4"); });
        }
    }
}
