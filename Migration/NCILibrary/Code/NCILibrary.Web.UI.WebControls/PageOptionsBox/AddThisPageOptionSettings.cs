using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace NCI.Web.UI.WebControls
{
        public class AddThisPageOptionSettings : IStateManager
        {
            private StateBag _statebag;
            private bool _trackViewState = false;

            public string UserName
            {
                get
                {
                    return (string)this.ViewState["UserName"] ?? string.Empty;
                }
                set
                {
                    //If the text is the same, just ignore and don't set dirtyness
                    //or fire any events.
                    if (!object.Equals(value, this.ViewState["UserName"]))
                    {
                        this.ViewState["UserName"] = value;
                        //this.OnOptionChanged();
                    }
                }
            }

            public string CompactServicesList
            {
                get
                {
                    return (string)this.ViewState["CompactServicesList"] ?? string.Empty;
                }
                set
                {
                    //If the text is the same, just ignore and don't set dirtyness
                    //or fire any events.
                    if (!object.Equals(value, this.ViewState["CompactServicesList"]))
                    {
                        this.ViewState["CompactServicesList"] = value;
                        //this.OnOptionChanged();
                    }
                }
            }

            public string ExpandedServicesList
            {
                get
                {
                    return (string)this.ViewState["ExpandedServicesList"] ?? string.Empty;
                }
                set
                {
                    //If the text is the same, just ignore and don't set dirtyness
                    //or fire any events.
                    if (!object.Equals(value, this.ViewState["ExpandedServicesList"]))
                    {
                        this.ViewState["ExpandedServicesList"] = value;
                        //this.OnOptionChanged();
                    }
                }
            }

            public string Language
            {
                get
                {
                    return (string)this.ViewState["Language"] ?? string.Empty;
                }
                set
                {
                    //If the text is the same, just ignore and don't set dirtyness
                    //or fire any events.
                    if (!object.Equals(value, this.ViewState["Language"]))
                    {
                        this.ViewState["Language"] = value;
                        //this.OnOptionChanged();
                    }
                }
            }

            protected StateBag ViewState
            {
                get
                {
                    return this._statebag;
                }
            }

            public AddThisPageOptionSettings()
            {
                this._statebag = new StateBag();
            }

            public void CopyProperties(AddThisPageOptionSettings newSettings)
            {
                newSettings.UserName = this.UserName;
                newSettings.CompactServicesList = this.CompactServicesList;
                newSettings.ExpandedServicesList = this.ExpandedServicesList;
                newSettings.Language = this.Language;
            }

            #region IStateManager Members

            /// <summary>
            /// Gets a value indicating whether this instance is tracking view state.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is tracking view state; otherwise, <c>false</c>.
            /// </value>
            bool IStateManager.IsTrackingViewState
            {
                get { return this._trackViewState; }
            }

            /// <summary>
            /// Loads the state of the view.
            /// </summary>
            /// <param name="savedState">State of the saved.</param>
            void IStateManager.LoadViewState(object savedState)
            {
                if (savedState != null)
                {
                    object[] objArray = (object[])savedState;

                    if (objArray[0] != null)
                        ((IStateManager)this.ViewState).LoadViewState(objArray[0]);
                }
            }

            /// <summary>
            /// Saves the state of the view.
            /// </summary>
            /// <returns></returns>
            object IStateManager.SaveViewState()
            {
                object obj1 = ((IStateManager)this.ViewState).SaveViewState();

                if (obj1 == null)
                    return null;

                return new object[] { obj1 };
            }

            /// <summary>
            /// Tracks the state of the view.
            /// </summary>
            void IStateManager.TrackViewState()
            {
                this._trackViewState = true;
                ((IStateManager)this.ViewState).TrackViewState();
            }

            #endregion
        }
}
