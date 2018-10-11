using Xunit;

using MigrationEngine.Utilities;

namespace Engine.Tests.Utilities
{
    /// <summary>
    /// Tests for MigrationEngine.Utilities.ParseErrorCollection
    /// </summary>
    public class ParseErrorCollectionTest
    {
        const string ONE_MESSAGE = "one message";
        const string TWO_MESSAGE = "two messages";
        const string THREE_MESSAGE = "three messages";

        /// <summary>
        /// Verify that IsEmpty returns correctly when the collection is empty.
        /// </summary>
        [Fact]
        public void IsEmptyWhenEmpty()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            Assert.True(collection.IsEmpty);
        }

        /// <summary>
        /// Verify that IsEmpty returns correctly when the collection has one message.
        /// </summary>
        [Fact]
        public void IsEmptyWithOneEntry()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);

            Assert.False(collection.IsEmpty);
        }

        /// <summary>
        /// Verify that IsEmpty returns correctly when the collection has multiple messages.
        /// </summary>
        [Fact]
        public void IsEmptyWithMultipleEntries()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);
            collection.Add(TWO_MESSAGE);
            collection.Add(THREE_MESSAGE);

            Assert.False(collection.IsEmpty);
        }

        /// <summary>
        /// Verify that Count returns correctly when the collection is empty.
        /// </summary>
        [Fact]
        public void CountWhenEmpty()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            Assert.Equal(0, collection.Count);
        }

        /// <summary>
        /// Verify Add() and Count for a single message.
        /// </summary>
        [Fact]
        public void CountWithOneEntry()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);

            Assert.Equal(1, collection.Count);
        }

        /// <summary>
        /// Verify Add() and Count for multiple messages.
        /// </summary>
        [Fact]
        public void CountWithMultipleEntries()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);
            collection.Add(TWO_MESSAGE);
            collection.Add(THREE_MESSAGE);

            Assert.Equal(3, collection.Count);
        }

        /// <summary>
        /// Verify Clear empty list.  (You know, that it doesn't blow up or anything.)
        /// </summary>
        [Fact]
        public void ClearWithEmptyList()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Clear();

            Assert.Equal(0, collection.Count);
        }

        /// <summary>
        /// Verify Clear for One messages.
        /// </summary>
        [Fact]
        public void ClearWithOneEntry()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);

            collection.Clear();

            Assert.Equal(0, collection.Count);
        }

        /// <summary>
        /// Verify Clear for multiple messages.
        /// </summary>
        [Fact]
        public void ClearWithMultipleEntries()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);
            collection.Add(TWO_MESSAGE);
            collection.Add(THREE_MESSAGE);

            collection.Clear();

            Assert.Equal(0, collection.Count);
        }

        /// <summary>
        /// Verify ToString() for empty list.
        /// </summary>
        [Fact]
        public void ToStringWithEmptyList()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            string list = collection.ToString();

            Assert.Equal(string.Empty, list);
        }

        /// <summary>
        /// Verify ToString() for One messages.
        /// </summary>
        [Fact]
        public void ToStringWithOneEntry()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);

            string list = collection.ToString();

            Assert.Equal(ONE_MESSAGE, list);
        }

        /// <summary>
        /// Verify ToString() for multiple messages.
        /// </summary>
        [Fact]
        public void ToStringWithMultipleEntries()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);
            collection.Add(TWO_MESSAGE);
            collection.Add(THREE_MESSAGE);

            string expected = string.Format("{0}\n{1}\n{2}", ONE_MESSAGE, TWO_MESSAGE, THREE_MESSAGE);

            string list = collection.ToString();

            Assert.Equal(expected, list);
        }

        /// <summary>
        /// Verify ToArray() for empty list.
        /// </summary>
        [Fact]
        public void ToArrayWithEmptyList()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            string[] list = collection.ToArray();

            Assert.Empty(list);
        }

        /// <summary>
        /// Verify ToString() for One messages.
        /// </summary>
        [Fact]
        public void ToArrayWithOneEntry()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);

            string[] list = collection.ToArray();

            Assert.Single(list);
            Assert.Equal(ONE_MESSAGE, list[0]);
        }

        /// <summary>
        /// Verify ToArray() for multiple messages.
        /// </summary>
        [Fact]
        public void ToArrayWithMultipleEntries()
        {
            ParseErrorCollection collection = new ParseErrorCollection();

            collection.Add(ONE_MESSAGE);
            collection.Add(TWO_MESSAGE);
            collection.Add(THREE_MESSAGE);

            string[] list = collection.ToArray();

            Assert.Equal(3, list.Length);
            Assert.Collection(list,
                item => Assert.Equal(ONE_MESSAGE, item),
                item => Assert.Equal(TWO_MESSAGE, item),
                item => Assert.Equal(THREE_MESSAGE, item)
                );
        }
    }
}
