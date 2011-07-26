using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("")]
    [ToolboxData("<{0}:MultiTemplatedRepeater runat=server></{0}:MultiTemplatedRepeater>")]
    [ParseChildren(true), PersistChildren(false)]
    public class MultiTemplatedRepeater : MultiTemplatedDataBoundControl, INamingContainer
    {
        private TemplateItemCollection _itemTemplates = new TemplateItemCollection();
        private TemplateItemCollection _alternatingItemTemplates = new TemplateItemCollection();
        private ITemplate _emptyItemTemplate;
        private ITemplate _headerTemplate;
        private ITemplate _footerTemplate;

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TemplateItemCollection ItemTemplates
        {
            get
            {
                return _itemTemplates;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TemplateItemCollection AlternatingItemTemplates
        {
            get
            {
                return _alternatingItemTemplates;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate EmptyItemTemplate
        {
            get
            {
                return _emptyItemTemplate;
            }
            set
            {
                _emptyItemTemplate = value;
            }
        }

        [DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(RepeaterItem)), Browsable(false)]
        public virtual ITemplate HeaderTemplate
        {
            get
            {
                return this._headerTemplate;
            }
            set
            {
                this._headerTemplate = value;
            }
        }

        [DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(RepeaterItem)), Browsable(false)]
        public virtual ITemplate FooterTemplate
        {
            get
            {
                return this._footerTemplate;
            }
            set
            {
                this._footerTemplate = value;
            }
        }


        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {

            if (HeaderTemplate != null)
            {
                //Add header
                RepeaterItem item = new RepeaterItem(-1, ListItemType.Header);
                this.Controls.Add(item);
                HeaderTemplate.InstantiateIn(item);
            }

            int itemCount = 0;
            
            if (dataSource != null)
            {

                foreach (ITemplatedDataItem item in dataSource)
                {
                    TemplateItemContainer container = null;

                    //We alternate every other item.  So even numbers will be the alternating template.
                    bool isAlternating = (((itemCount + 1) % 2) == 0);

                    if (dataBinding)
                    {
                        container = new TemplateItemContainer(item.ItemType, item.Data);                        
                    }
                    else
                    {
                        container = new TemplateItemContainer(TemplateTypes[itemCount]);
                    }

                    if (container != null)
                    {
                        this.Controls.Add(container);
                        TemplateItem template = null;

                        if (isAlternating)
                        {
                            if (_alternatingItemTemplates.ContainsType(container.TemplateType))
                                template = _alternatingItemTemplates[container.TemplateType];
                            else if (_itemTemplates.ContainsType(container.TemplateType))
                                template = _itemTemplates[container.TemplateType];
                            else
                                throw new Exception("The template collection does not contain a template of type " + container.TemplateType);
                        }
                        else
                        {
                            if (_itemTemplates.ContainsType(container.TemplateType))
                                template = _itemTemplates[container.TemplateType];
                            else
                                throw new Exception("The template collection does not contain a template of type " + container.TemplateType);
                        }

                        template.InstantiateIn(container);

                        if (dataBinding)
                            container.DataBind();
                    }

                    itemCount++;
                }
            }

            if (itemCount == 0)
            {
                if (EmptyItemTemplate != null)
                {
                    RepeaterItem item = new RepeaterItem(-1, ListItemType.Separator);
                    this.Controls.Add(item);
                    _emptyItemTemplate.InstantiateIn(item);
                }
            }

            if (FooterTemplate != null)
            {
                //Add header
                RepeaterItem item = new RepeaterItem(-1, ListItemType.Footer);
                this.Controls.Add(item);
                FooterTemplate.InstantiateIn(item);
            }

            return itemCount;
        }
    }
}
