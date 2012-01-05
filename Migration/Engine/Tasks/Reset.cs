using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    public class Reset : MigrationTask
    {
        /// <summary>
        /// Controls whether the Reset task will clear the moniker store.
        /// If not set, the store is not cleared.
        /// </summary>
        /// <value><c>true</c> - Clear the moniker store; <c>false</c> - Preserve the moniker store.</value>
        [XmlAttribute()]
        public bool MonikerReset { get; set; }

        public override void Doit(IMigrationLog logger)
        {
            if (MonikerReset)
                MonikerStore.Clear();
        }
    }
}
