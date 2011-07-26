using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{  
#region Implemented in DataControlField
/*
    public class CheckBoxBoundField : CheckBoxField
    {
        public const string CheckBoxID = "DefaultCheckBoxButton";

        public CheckBoxBoundField()
        {
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            base.InitializeDataCell(cell, rowState);

            // Add a checkbox anyway, if not done already
            if (cell.Controls.Count == 0)
            {
                CheckBox chk = new CheckBox();
                chk.ID = CheckBoxBoundField.CheckBoxID;
                cell.Controls.Add(chk);
            }
        }

    }
    */
    #endregion

    /// <summary>
    /// Serves as the checkbox data control field types, which represent a column of data in tabular data-bound controls such as DetailsView and GridView.
    /// </summary>
    public class CheckBoxBoundField : DataControlField
    {
        #region private variables
        public const string CheckBoxID = "DefaultCheckBoxButton";
        #endregion

        #region public constructor
        /// <summary>
        /// Initializes a new instance of the CheckBoxBoundField class.
        /// </summary>
        public CheckBoxBoundField()
        {
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Overridden. Creates an empty CheckBoxBoundField object
        /// </summary>
        /// <returns></returns>
        protected override DataControlField CreateField()
        {
            return new CheckBoxBoundField();
        }
        #endregion

        #region public method
        /// <summary>
        /// Adds a checkbox control to a cell's controls collection
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {

            base.InitializeCell(cell, cellType, rowState, rowIndex);

            // Add a checkbox anyway, if not done already
            if (cell.Controls.Count == 0 && cellType == DataControlCellType.DataCell)
            {
                CheckBox chk = new CheckBox();
                chk.ID = CheckBoxBoundField.CheckBoxID;
                cell.Controls.Add(chk);
            }
        }
        #endregion

    }
}
