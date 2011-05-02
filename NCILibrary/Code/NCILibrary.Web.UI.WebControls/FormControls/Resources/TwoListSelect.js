TwoListSelect = Class.create();
Object.extend(TwoListSelect,{

    // Root names for the components of a TwoListSelect
    _leftSelectRoot : "LeftSelect",
    _rightSelectRoot : "RightSelect",
    _addButtonRoot : "AddButton",
    _removeButtonRoot : "RemoveButton",
    _selectListRoot : "Selections",
    
    _nameSeparator : "_",


    /// <summary>
    /// Helper function to transfer items between two select lists.
    /// </summary>
    /// <param name="sourceList">The select list items are being removed from.</param>
    /// <param name="destinationList">The select list items are being added to.</param>
    TransferSelections : function (sourceList, destinationList)
    {
        var source = sourceList.options;
        var destination = destinationList.options;

        for(var i = 0; i < source.length; ++i)
        {
            if(source[i].selected == true)
            {
                var addition = new Option();
                addition.text = source[i].text;
                addition.value = source[i].value;
                destination.add(addition);
            }
        }

        // Remove selections starting at the end and moving up.
        for(var i = source.length - 1; i >= 0; --i)
        {
            if(source[i].selected == true)
                sourceList.remove(i);
        }
    },

    /// <summary>
    /// Helper function to update the control's hidden master selection list.
    /// </summary>
    /// <param name="selectList">The select list containing the actual selected items (as opposed to
    /// the non-selected items).</param>
    /// <param name="selectField">The hidden field containing the master selection list.</param>
    UpdateSelectionList : function (selectList, selectField)
    {
        var fieldValue = "";
        for(var i = 0; i < selectList.length; ++i)
        {
            if( i > 0 )
                fieldValue += '|';
            fieldValue += selectList.options[i].value;
        }
        selectField.value = fieldValue;
    }
});

Object.extend(TwoListSelect.prototype, {

    _leftSelectElement : false,     // Two list boxes and a hidden field.
    _rightSelectElement : false,
    _selectListElement : false,
    
    initialize : function(controlID)
    {
        this._leftSelectElement = $(controlID + TwoListSelect._nameSeparator + TwoListSelect._leftSelectRoot);
        this._rightSelectElement = $(controlID + TwoListSelect._nameSeparator + TwoListSelect._rightSelectRoot);
        this._selectListElement = $(controlID + TwoListSelect._nameSeparator + TwoListSelect._selectListRoot);

        var addButton = $(controlID + TwoListSelect._nameSeparator + TwoListSelect._addButtonRoot);
        var removeButton = $(controlID + TwoListSelect._nameSeparator + TwoListSelect._removeButtonRoot);
    
        if(addButton)
            addButton.observe("click", this.Add.bindAsEventListener(this));
        if(removeButton)
            removeButton.observe("click", this.Remove.bindAsEventListener(this));
    },

    /// <summary>
    /// Event handler for the control's Add (Select) button.
    /// </summary>
    Add : function (event)
    {
        Event.stop(event);

        TwoListSelect.TransferSelections(this._leftSelectElement, this._rightSelectElement);
        TwoListSelect.UpdateSelectionList(this._rightSelectElement, this._selectListElement);
    },


    /// <summary>
    /// Event handler for the control's Remove (Unselect) button.
    /// </summary>
    Remove : function (event)
    {
        Event.stop(event);

        TwoListSelect.TransferSelections(this._rightSelectElement, this._leftSelectElement);
        TwoListSelect.UpdateSelectionList(this._rightSelectElement, this._selectListElement);
    }
});

