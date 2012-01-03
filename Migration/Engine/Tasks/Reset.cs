using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    public class Reset : MigrationTask
    {
        [XmlAttribute()]
        public bool MonikerReset { get; set; }

        public override void Doit(IMigrationLog logger)
        {
            if (MonikerReset)
                MonikerStore.Clear();
        }
    }
}
