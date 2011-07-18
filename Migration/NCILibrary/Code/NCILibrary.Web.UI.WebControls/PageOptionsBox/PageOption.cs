using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace NCI.Web.UI.WebControls
{
    public abstract class PageOption : IStateManager
    {

        private StateBag _statebag;
        private bool _trackViewState = false;

        internal event EventHandler OptionChanged;


        public virtual string LinkText
        {
            get
            {
                return (string)this.ViewState["LinkText"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["LinkText"]))
                {
                    this.ViewState["LinkText"] = value;
                    this.OnOptionChanged();
                }
            }
        }

        public virtual string CssClass
        {
            get
            {
                return (string)this.ViewState["CssClass"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["CssClass"]))
                {
                    this.ViewState["CssClass"] = value;
                    this.OnOptionChanged();
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

        protected PageOption()
        {
            this._statebag = new StateBag();
        }

        protected internal PageOption CloneOption()
        {
            PageOption newOption = this.CreateOption();
            this.CopyProperties(newOption);
            return newOption;
        }

        protected virtual void CopyProperties(PageOption newOption)
        {
            //Copy properties that are in the statebag
            newOption.LinkText = this.LinkText;
            newOption.CssClass = this.CssClass;

            //Copy other local member vars here. (If any)
        }

        //Is this needed???
        protected abstract PageOption CreateOption();

        //Initialize?

        protected virtual void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;

                if (objArray[0] != null)
                    ((IStateManager)this.ViewState).LoadViewState(objArray[0]);
            }
        }

        protected virtual object SaveViewState()
        {
            //This is standard code for this kind of thing, the reason it is done
            //this way is if the PageOption ever has some kind of sub IStateManager items
            //like say, OptionStyle, that would become obj2 and we would call that OptionStyle's
            //SaveViewState's method.  Then when we load we get index 1 from the objArray that
            //gets passed to the OptionStyle's LoadViewState method.
            object obj1 = ((IStateManager)this.ViewState).SaveViewState();

            if (obj1 == null)
                return null;

            return new object[] { obj1 };
        }

        protected virtual void OnOptionChanged()
        {
            if (this.OptionChanged != null)
            {
                this.OptionChanged(this, EventArgs.Empty);
            }
        }

        //Override to string?

        protected virtual void TrackViewState()
        {
            this._trackViewState = true;
            ((IStateManager)this.ViewState).TrackViewState();

            //If there are any sub IStateManager items, start tracking view state for them as well.
        }

        internal void SetDirty()
        {
            this._statebag.SetDirty(true);

            //If there are any sub IStateManager items, set them dirty as well.
        }

        //Validate Supports Callback??


        #region IStateManager Members

        /// <summary>
        /// When implemented by a class, gets a value indicating whether a server control is tracking its view state changes.
        /// </summary>
        /// <value></value>
        /// <returns>true if a server control is tracking its view state changes; otherwise, false.
        /// </returns>
        public bool IsTrackingViewState
        {
            get { return this._trackViewState; }
        }

        /// <summary>
        /// When implemented by a class, loads the server control's previously saved view state to the control.
        /// </summary>
        /// <param name="state">An <see cref="T:System.Object"/> that contains the saved view state values for the control.</param>
        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        /// <summary>
        /// When implemented by a class, instructs the server control to track changes to its view state.
        /// </summary>
        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        #endregion
    }
}
