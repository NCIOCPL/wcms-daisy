using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.UI.WebControls
{
    public class PageOptionsCollection : StateManagedCollection
    {
        //These are the types of page options that can be in this collection.
        //If there needs to be a new page option type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(LinkPageOption), 
            typeof(AddThisPageOption)
        };

        public event EventHandler OptionsChanged;

        [Browsable(false)]
        public PageOption this[int index]
        {
            get
            {
                return (this[index] as PageOption);
            }
        }

        public void Add(PageOption option)
        {
            ((IList)this).Add(option);
        }

        public PageOptionsCollection CloneFields()
        {
            PageOptionsCollection options = new PageOptionsCollection();
            foreach (PageOption option in this)
            {
                options.Add(option.CloneOption());
            }
            return options;
        }

        public bool Contains(PageOption option)
        {
            return ((IList)this).Contains(option);
        }

        public void CopyTo(PageOption[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(PageOption option)
        {
            return ((IList)this).IndexOf(option);
        }

        public void Insert(int index, PageOption option)
        {
            ((IList)this).Insert(index, option);
        }

        public void Removed(PageOption option)
        {
            ((IList)this).Remove(option);
        }

        public void RemoveAt(int index)
        {
            ((IList)this).RemoveAt(index);
        }


        protected override void OnClearComplete()
        {
            this.OnOptionsChanged();
        }

        protected override object CreateKnownType(int index)
        {
            switch (index)
            {
                case 0 :
                    return new LinkPageOption();
                case 1 :
                    return new AddThisPageOption();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            PageOption option = value as PageOption;
            if (option != null)
            {
                option.OptionChanged += new EventHandler(this.OnOptionChanged);
            }

            this.OnOptionsChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is PageOption))
            {
                throw new ArgumentException("Invalid type");
            }
        }

        protected override Type[] GetKnownTypes()
        {
            return knownTypes;
        }

        protected override void SetDirtyObject(object o)
        {
            ((PageOption)o).SetDirty();
        }

        private void OnOptionChanged(object sender, EventArgs e)
        {
            this.OnOptionsChanged();
        }

        private void OnOptionsChanged()
        {
            if (this.OptionsChanged != null)
            {
                this.OptionsChanged(this, EventArgs.Empty);
            }
        }

    }
}
