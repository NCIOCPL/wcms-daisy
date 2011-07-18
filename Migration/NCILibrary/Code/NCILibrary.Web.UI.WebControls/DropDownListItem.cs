using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Used inside DropDownBoundField For GridView.
    /// Usage:
    ///   <sk:DropDownListItem text="223" value="223"  Default="true" />
    /// </summary>
    public class DropDownListItem : IStateManager
    {
        #region private variables
        private StateBag _statebag;
        private bool _isTrackingViewState = false;
        
        #endregion

        #region public Properties

        /// <summary>
        /// Gets or sets text
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// Gets or sets default
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool Default
        {
            get
            {
                Object s = ViewState["Default"] as Object;
                return ((s == null) ? false : (bool)s);
            }

            set
            {
                ViewState["Default"] = value;
            }
        }

        /// <summary>
        /// Gets or sets value
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Value
        {
            get
            {
                String s = (String)ViewState["Value"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Value"] = value;
            }
        }

        /// <summary>
        /// Gets viewstate. Because this does not derive from Control, it does not 
        /// have access to an inherited view state StateBag object.
        /// </summary>
        protected StateBag ViewState
        {
            get
            {
                if (_statebag == null)
                {
                    _statebag = new StateBag(false);
                    if (_isTrackingViewState)
                    {
                        ((IStateManager)_statebag).TrackViewState();
                    }
                }
                return this._statebag;
            }
        }

        #endregion

        #region public Constructors
        /// <summary>
        /// Initializes a new empty instance of the DropDownListItem class.
        /// </summary>
        public DropDownListItem() { }

        /// <summary>
        /// Initializes a new instance of the DropDownListItem class.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public DropDownListItem(string text, string value)
        {
            Text = text;
            Value = value;
        }
        #endregion

        #region internal methods
        /// <summary>
        /// Tells view state that the item has changed. 
        /// </summary>
        internal void SetDirty()
        {
            ViewState.SetDirty(true);
        }

        #endregion

        #region public method
        /// <summary>
        /// Returns a new list item
        /// </summary>
        /// <returns></returns>
        public ListItem ToListItem()
        {
            return new ListItem(this.Text, this.Value);
        }
        #endregion

        #region IStateManager Members

        /// <summary>
        /// Gets IsTrackingViewState
        /// </summary>
        protected virtual bool IsTrackingViewState
        {
            get
            {
                return this._isTrackingViewState;
            }
        }

        /// <summary>
        /// Gets IStateManager's IsTrackingViewState
        /// </summary>
        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return this.IsTrackingViewState;
            }
        }

        /// <summary>
        /// In SaveViewState, an array of one element is created.
        /// Therefore, if the array passed to LoadViewState has 
        /// more than one element, it is invalid.
        /// </summary>
        /// <param name="savedState"></param>
        protected virtual void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;
                if (objArray.Length != 1)
                {
                    throw new ArgumentException("Invalid DropDownListItem View State");
                }

                ((IStateManager)this.ViewState).LoadViewState(objArray[0]);
            }
        }
        /// <summary>
        /// Implements the LoadViewState method by
        /// calling the LoadViewState method of the base class's 
        /// </summary>
        /// <param name="state"></param>
        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        /// <summary>
        /// Save the view state by calling the StateBag's SaveViewState
        /// method.
        /// </summary>
        /// <returns></returns>
        protected virtual object SaveViewState()
        {
            object obj = ((IStateManager)this.ViewState).SaveViewState();

            if (obj == null)
            {
                return null;
            }
            return new object[] { obj };
        }

        /// <summary>
        /// Implements the SaveViewState method by
        /// calling the SaveViewState method of the base class's 
        /// </summary>
        /// <returns></returns>
        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        /// <summary>
        ///  Begin tracking view state. Check the private variable, because 
        /// if the view state has not been accessed or set, then it is not  
        /// being used and there is no reason to store any view state
        /// </summary>
        protected virtual void TrackViewState()
        {
            this._isTrackingViewState = true;

            if (this.ViewState != null)
                ((IStateManager)this.ViewState).TrackViewState();
        }

        /// <summary>
        /// Implements the TrackViewState method for this class by
        /// calling the TrackViewState method of the base class's 
        /// </summary>
        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        #endregion
    }

}
