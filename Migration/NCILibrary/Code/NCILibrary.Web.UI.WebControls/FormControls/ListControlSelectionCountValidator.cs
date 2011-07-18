using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.FormControls
{
    public class ListControlSelectionCountValidator : BaseValidator
    {
        private ListControl _listControl;

        /// <summary>
        /// Gets and Sets the minimum number of selected items in order for the ListControl to be valid.
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(1), Description("The minimum number of selected items in order for the ListControl to be valid")]
        public int MinNumItems
        {
            get
            {
                object i = ViewState["MinNumItems"];
                return (i == null) ? 1 : (int)i;
            }
            set
            {
                ViewState["MinNumItems"] = value;
            }
        }

        /// <summary>
        /// Gets and Sets the maximum number of selected items in order for the ListControl to be valid.
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(1), Description("The maximum number of selected items in order for ListControl to be valid")]
        public int MaxNumItems
        {
            get
            {
                object i = ViewState["MaxNumItems"];
                return (i == null) ? 1 : (int)i;
            }
            set
            {
                ViewState["MaxNumItems"] = value;
            }
        }

        public ListControlSelectionCountValidator()
        {
            this.EnableClientScript = false;
        }

        /// <summary>
        /// Determines whether the control specified by the ControlToValidate property is a valid control.
        /// </summary>
        /// <returns>true if the control specified by ControlToValidate is a valid control; otherwise, false.</returns>
        /// <exception cref="System.Web.HttpException">No value is specified for the ControlToValidate property.
        /// <br />- or -
        /// <br />The input control specified by the ControlToValidate property is not found on the page.
        /// <br />- or -
        /// <br />The input control specified by the ControlToValidate property does not have a ValidationPropertyAttribute attribute associated with it; therefore, it cannot be validated with a validation control.
        /// </exception>
        protected override bool ControlPropertiesValid()
        {
            if (string.IsNullOrEmpty(this.ControlToValidate))
                throw new HttpException("No value is specified for the ControlToValidate property.");

            Control ctrl = FindControl(ControlToValidate);

            if (ctrl != null && ctrl is ListControl)
            {
                _listControl = (ListControl)ctrl;
                return (true);
            }
            else
            {
                if (ctrl == null)
                    throw new HttpException("The input control with the ID, '" + this.ControlToValidate + "' does not exist.");
                else
                    throw new HttpException("The input control with the ID, '" + this.ControlToValidate + "' cannot be validated using this validator.");
            }
        }


        /// <summary>
        /// Determines whether the number of selected items of the ListControl is the boundries MinNumItems and MaxNumItems.
        /// </summary>
        /// <returns>true if the value in the input control is valid; otherwise, false.</returns>
        protected override bool EvaluateIsValid()
        {
            int selectedCount = 0;

            //Count the selected items.  No need to check if _listControl is null since we would throw
            //an exception earlier if that were the case.
            foreach (ListItem item in _listControl.Items)
                if (item.Selected)
                    selectedCount++;

            return (selectedCount >= MinNumItems && selectedCount <= MaxNumItems);
        }
    }
}
