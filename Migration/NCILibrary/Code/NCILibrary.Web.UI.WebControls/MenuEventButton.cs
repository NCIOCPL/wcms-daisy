using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [ToolboxData("<{0}:EventButton runat=server></{0}:EventButton>")]
    public class EventButton : MenuButton
    {
        private LinkButton _button = new LinkButton();
        public event EventHandler Click; //Defines the event handler for OUR OnClick.

        protected override void OnInit(EventArgs e)
        {
            _button.ID = "MenuButtonLink";
            _button.Click += new EventHandler(Button_Click);
            this.Controls.Add(_button);
            base.OnInit(e);
        }

        //Defines our OnClick event.
        protected void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        //The is the event handler for the LinkButton's OnClick Event
        void Button_Click(object sender, EventArgs e)
        {
            //Raise our event.
            OnClick(e);
        }

        protected override void OnLoad(EventArgs e)
        {            
            _button.Text = "<span>" + ButtonText + "<span>";
            base.OnLoad(e);
        }
    }
}
