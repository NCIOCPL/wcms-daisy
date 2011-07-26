using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Used inside DropDownBoundField For GridView.
    /// +++++++++++++++++++
    /// This control cannot participate in request processing unless it is added to the control tree. 
    /// When This control is created declaratively on a page, the page parser adds it to the control tree. 
    /// If you create This control dynamically, you are responsible for adding it to the control tree yourself.
    /// +++++++++++++++++++
    /// Usage: 
    /// 1. In ASPX page 
    /// <Columns>
    ///         <sk:DropDownBoundField HeaderText="CountryID" DataField="CountryID">
    ///             <Items>
    ///                 <sk:DropDownListItem text="223" value="223"  Default="true" />
    ///                 <sk:DropDownListItem text="2" value="2"   />
    ///                 <sk:DropDownListItem text="1" value="1" />
    ///             </Items>
    ///         </sk:DropDownBoundField>
    /// </Columns>
    /// 2. Inside cs.
    /// 2.1 Declare GridView and instansciate it right away. 
    /// private CustomSortingGridView GridView1 = new CustomSortingGridView();
    /// 2.2 When create gridview, always add GridView to Form in Init
    /// protected void Page_Init(object sender, EventArgs e)
    ///{
    ///    this.form1.Controls.Add(GridView1);
    ///    CreateGridView();
    ///        GridView1.DataKeyNames = new string[] { "ConfUserID" };
    ///}
    /// 2.3 Inside Page Load, databind the gridview and define DataKeyNames 
    ///  protected void Page_Load(object sender, EventArgs e)
    ///{
    ///    if (!IsPostBack)
    ///    {
    ///        this.BindData();
    ///    }
    ///}
    /// 2.4 Define columsn for Gridview inside CreateGridView()
    ///  private void CreatePanel()
    /// {
    ///    GridView1.ID = "test";
    ///   
    ///     DropDownBoundField db = new DropDownBoundField();
    ///    db.DataField = "MedicalSpecialtyID";
    ///    db.HeaderText = "MedicalSpecialtyID";
    ///    GridView1.Columns.Add(db);
    ///    db.Items.Add(new DropDownListItem("a", "1"));
    ///    db.Items.Add(new DropDownListItem("aa", "2"));
    ///    db.Items.Add(new DropDownListItem("aaa", "3"));
    /// }
    ///  Note: It must follow this sequence. Create control first, add it to parent control collection in Init and 
    ///     then add child items. Otherwise, the children's viewstates won't work.
    /// </summary>
    public class DropDownBoundField : BoundField
    {
        #region private variables
        public const string DropDownID = "DropDownID";

        private DropDownListItemCollection items;
        private PropertyDescriptor _boundFieldDesc;
        #endregion

        #region public method
        /// <summary>
        /// Initializes a new empty instance of the DropDownBoundField class
        /// </summary>
        public DropDownBoundField()
        {
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets DropDown List Item Collection
        /// </summary>
        [DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public DropDownListItemCollection Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new DropDownListItemCollection();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)this.items).TrackViewState();
                    }
                }
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        #endregion

        #region protected methods 
        /// <summary>
        /// Called when a data bound control needs to get the value back out of the control(s) 
        /// created by this cell. Extract values from cell (presumably in edit mode)
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="cell"></param>
        /// <param name="rowState"></param>
        /// <param name="includeReadOnly"></param>
        public override void ExtractValuesFromCell(System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
            object value = null;

            if (cell.Controls.Count > 0)
            {
                DropDownList dropDownList = cell.Controls[0] as DropDownList;
                if (dropDownList == null)
                {
                    throw new InvalidOperationException("DropDownField could not extract control.");
                }
                else
                    value = dropDownList.SelectedValue;
            }
            // Add the value to the dictionary
            if (dictionary.Contains(DataField))
            {
                dictionary[DataField] = value;
            }
            else
            {
                dictionary.Add(DataField, value);
            }
        }

        /// <summary>
        ///  Called when a data bound control need to get the value for this cell when is is being data bound
        /// </summary>
        /// <param name="controlContainer"></param>
        /// <returns></returns>
        protected virtual object GetValue(Control controlContainer)
        {
            object data = null;
            object dataItem = null;
            string boundField = DataField;

            if (controlContainer == null)
            {
                throw new ArgumentNullException("controlContainer");
            }

            // Get the DataItem from the container
            dataItem = DataBinder.GetDataItem(controlContainer);

            if (dataItem == null && !DesignMode)
            {
                throw new InvalidOperationException("No data item");
            }
            // Get value of field in data item
            if (_boundFieldDesc == null)
            {
                if (!boundField.Equals(null))
                {
                    _boundFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(boundField, true);
                    if ((_boundFieldDesc == null) && !DesignMode)
                    {
                        throw new InvalidOperationException("The field '" + boundField + "' was not found on the data item");
                    }
                }
            }

            if (_boundFieldDesc != null && dataItem != null)
            {
                data = _boundFieldDesc.GetValue(dataItem);
            }
            else
            {
                if (DesignMode)
                {
                    data = GetDesignTimeValue();
                }
                else
                {
                    data = dataItem;
                }
            }

            return data;
        }

        /// <summary>
        /// Called when the child controls for this cell need to be created
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="rowState"></param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            DropDownList childControl = null;
            DropDownList boundControl = null;

            // If we're in edit/insert mode...
            DataControlRowState state = rowState & DataControlRowState.Edit;
            if ((!ReadOnly && (state != DataControlRowState.Normal)) || rowState == DataControlRowState.Insert)
            {
                DropDownList editor = new DropDownList();

                int count = 0;
                foreach (DropDownListItem item in Items)
                {
                    editor.Items.Add(item.ToListItem());
                    if (item.Default)
                        count++;
                }

                if (count > 1)
                    throw new Exception("There are two default items");

                editor.ID = DropDownID + this.DataField;
                editor.ToolTip = HeaderText;
                childControl = editor;
                cell.Controls.Add(editor);

                // Save the control to use for binding (edit/insert mode)
                if (DataField.Length != 0 && (rowState & DataControlRowState.Edit) != 0)
                {
                    boundControl = editor;
                }
            }
            else if (DataField.Length != 0)
            {
                DropDownList editor = new DropDownList();
                int count = 0;
                foreach (DropDownListItem item in Items)
                {
                    editor.Items.Add(item.ToListItem());
                    if (item.Default)
                        count++;
                }

                if (count > 1)
                    throw new Exception("There are two default items");

                editor.ID = DropDownID + this.DataField;
                childControl = editor;
                boundControl = editor;
            }

            if (childControl != null)
            {
                MethodInfo mi = typeof(Control).GetMethod("ClearChildState", (BindingFlags)(-1));
                mi.Invoke(childControl, new object[0]);
                cell.Controls.Add(childControl);
            }

            // If the column is visible, trigger the binding process
            if (boundControl != null)
            {
                boundControl.DataBinding += new EventHandler(OnDataBindField);
            }
        }

        /// <summary>
        /// Called when the cell is being data bound.
        /// Created selected value and if no selected, set default value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDataBindField(object sender, EventArgs e)
        {
            Control boundControl = (Control)sender;
            Control controlContainer = boundControl.NamingContainer;

            string data = GetValue(controlContainer).ToString();
            DropDownList ddl = (DropDownList)boundControl;

            if (!string.IsNullOrEmpty(data))
            {
                foreach (ListItem item in ddl.Items)
                {
                    if (item.Value == data)
                    {
                        item.Selected = true;
                        return;
                    }
                }
            }
            else
                ddl.SelectedValue = this.Items.GetDefaultValue();
        }

        /// <summary>
        /// Save View Steate. It needs to do this to add child controls viewstate into its viewstate.
        /// Object one is the base view state. Obj2 is the item's view state
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object obj1 = base.SaveViewState();


            object obj2 = (this.items != null) ? ((IStateManager)this.items).SaveViewState() : null;

            if (((obj1 == null) && (obj2 == null)))
            {
                return null;
            }
            return new object[] { obj1, obj2 };

        }

        /// <summary>
        /// Load base and Item state
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;
                if (objArray[0] != null)
                {
                    base.LoadViewState(objArray[0]);
                }
                if (objArray[1] != null)
                {
                    ((IStateManager)this.Items).LoadViewState(objArray[1]);
                }
            }
        }
        #endregion
    }
}
