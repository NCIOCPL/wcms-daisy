using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonikerProviders
{
    class XmlPersistedMonikerProvider : MonikerProvider
    {
        const string DEFAULT_FILE_NAME = "Monikers.xml";

        private string _monikerFileName;

        MonikerMap _monikerCollection = new MonikerMap();

        #region Methods required for Provider pattern

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if ((config == null) || (config.Count == 0))
                throw new MonikerConfigurationException(string.Format("No configuration settings found for {0}.", name));

            //Let ProviderBase perform the basic initialization
            base.Initialize(name, config);

            //Perform feature-specific provider initialization here

            _monikerFileName = config["filename"];
            if (String.IsNullOrEmpty(_monikerFileName))
                _monikerFileName = DEFAULT_FILE_NAME;

            LoadFromDisk();
        }

        #endregion

        public override void Add(Moniker moniker)
        {
            if (_monikerCollection.ContainsKey(moniker.Name))
            {
                Moniker old = _monikerCollection[moniker.Name];
                String fmt = "Attempt to add a moniker with the same name as an existing entry. Old: {0}, New: {1}.";
                throw new DuplicateMonikerException(string.Format(fmt, old, moniker));
            }

            _monikerCollection.Add(moniker.Name, moniker);
            PersistToDisk();
        }

        public override Moniker Get(string name)
        {
            if (!_monikerCollection.ContainsKey(name))
            {
                string fmt = "Unable to locate a moniker named \"{0}\".";
                string message = string.Format(fmt, name);
                throw new MonikerNotFoundException(message);
            }

            return _monikerCollection[name];
        }

        public override bool Contains(string name)
        {
            return _monikerCollection.ContainsKey(name);
        }

        public override void Delete(string name)
        {
            _monikerCollection.Remove(name);
            PersistToDisk();
        }

        public override void Clear()
        {
            _monikerCollection.Clear();
            PersistToDisk();
        }

        private void PersistToDisk()
        {
            MonikerMap.PersistToFile(_monikerCollection, _monikerFileName);
        }

        private void LoadFromDisk()
        {
            _monikerCollection = MonikerMap.LoadFromFile(_monikerFileName);
        }
    }
}
