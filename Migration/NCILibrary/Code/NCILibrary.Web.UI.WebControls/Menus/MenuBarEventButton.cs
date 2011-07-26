using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.Menus
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MenuBarEventButton runat=server></{0}:MenuBarEventButton>")]
    public class MenuBarEventButton : MenuBarButton, IButtonControl
    {
        //This is for the button
        private LinkButton _linkButton = new LinkButton();

        //The buttons render out <li><a><span></span></a></li>
        //so we need a span tag and something that can manage its
        //viewstate.  So we will use a label.
        private Label _textLabel = new Label();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            _linkButton.ID = "ctxEventItemButton";
            _textLabel.ID = "label";
            base.CreateChildControls();
            this.Controls.Add(_linkButton);
            _linkButton.Controls.Add(_textLabel);

        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.RegisterRequiresRaiseEvent(_linkButton);

        }


        public override string Text
        {
            get
            {
                return _textLabel.Text;
            }
            set
            {
                EnsureChildControls();
                _textLabel.Text = value;
            }
        }

        #region LinkButton Passthroughs

        /// <summary>
        /// Gets or sets the client-side script that executes when a ContextMenuEventItem control's Click event is raised.
        /// <br/>
        /// Return Value <br/>
        /// The client-side script that executes when a ContextMenuEventItem control's Click event is raised.
        /// </summary>
        [Category("Behavior"), Themeable(false), Description("Button_OnClientClick"), DefaultValue("")]
        public override string OnClientClick
        {
            get
            {
                return _linkButton.OnClientClick;
            }
            set
            {
                EnsureChildControls();
                _linkButton.OnClientClick = value;
            }
        }

        #region IButtonControl Members

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the ContextMenuEventItem control is clicked.
        /// <br/>
        /// Return Value <br />
        /// true if validation is performed when the ContextMenuEventItem control is clicked; otherwise, false. The default value is true.
        /// </summary>
        [Category("Behavior"), Themeable(false), Description("Button_CausesValidation"), DefaultValue(true)]
        public virtual bool CausesValidation
        {
            get
            {
                return _linkButton.CausesValidation;
            }
            set
            {
                EnsureChildControls();
                _linkButton.CausesValidation = value;
            }
        }

        /// <summary>
        /// Occurs when the ContextMenuEventItem control is clicked.
        /// </summary>
        [Description("ContextMenuEventItem_OnClick"), Category("Action")]
        public event EventHandler Click
        {
            add
            {
                _linkButton.Click += value;
            }
            remove
            {
                _linkButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the ContextMenuEventItem control is clicked.
        /// </summary>
        [Description("ContextMenuEventItem_OnClick"), Category("Action")]
        public event CommandEventHandler Command
        {
            add
            {
                _linkButton.Command += value;
            }

            remove
            {
                _linkButton.Command -= value;
            }
        }

        /// <summary>
        /// Gets or sets an optional argument passed to the Command event handler along with the associated CommandName property.
        /// <br />
        /// Return Value <br />
        /// An optional argument passed to the Command event handler along with the associated CommandName property. The default value is Empty.
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Bindable(true), Description("WebControl_CommandArgument")]
        public string CommandArgument
        {
            get
            {
                return _linkButton.CommandArgument;
            }
            set
            {
                EnsureChildControls();
                _linkButton.CommandArgument = value;
            }
        }

        /// <summary>
        /// Gets or sets the command name associated with the ContextMenuEventItem control. This value is passed to the Command event handler along with the CommandArgument property.
        /// <br />
        /// Return Value <br />
        /// The command name of the ContextMenuEventItem control. The default value is Empty.
        /// </summary>        
        [DefaultValue(""), Description("WebControl_CommandName"), Themeable(false), Category("Behavior")]
        public string CommandName
        {

            get
            {
                return _linkButton.CommandName;
            }
            set
            {
                EnsureChildControls();
                _linkButton.CommandName = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the page to post to from the current page when the ContextMenuEventItem control is clicked.
        /// <br />
        /// Return Value <br />
        /// The URL of the Web page to post to from the current page when the ContextMenuEventItem control is clicked. The default value is an empty string (""), which causes the page to post back to itself.
        /// </summary>
        [Themeable(false), Category("Behavior"), Description("Button_PostBackUrl"), UrlProperty("*.aspx"), DefaultValue(""), Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public virtual string PostBackUrl
        {
            get
            {
                return _linkButton.PostBackUrl;
            }
            set
            {
                EnsureChildControls();
                _linkButton.PostBackUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the group of controls for which the ContextMenuEventItem control causes validation when it posts back to the server.
        /// <br />
        /// Return Value <br />
        /// The group of controls for which the ContextMenuEventItem control causes validation when it posts back to the server. The default value is an empty string ("").
        /// </summary>
        [Category("Behavior"), DefaultValue(""), Themeable(false), Description("PostBackControl_ValidationGroup")]
        public virtual string ValidationGroup
        {
            get
            {
                return _linkButton.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                _linkButton.ValidationGroup = value;
            }
        }


        #endregion

        #endregion

    }
}
