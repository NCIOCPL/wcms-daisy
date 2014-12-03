using System;
namespace MonikerProviders
{
    public interface IMonikerStore
    {
        void Add(Moniker moniker);
        bool Contains(string name);
        void Delete(string name);
        Moniker Get(string name);
        void Clear();
    }
}
