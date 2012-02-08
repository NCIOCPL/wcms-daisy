using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonikerProviders
{
    public abstract class MonikerProvider  : ProviderBase, IMonikerStore
    {
        public abstract void Add(Moniker moniker);

        public abstract Moniker Get(string name);

        public abstract bool Contains(string name);

        public abstract void Delete(string name);

        public abstract void Clear();
    }
}
