using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for Pagefreezer
/// </summary>
public static class PageFreezer
{
    public static void freezePageByStatus(Button freezButton, Label lblmessage,Control c)
    {
        object strFreezMsg = "";
        object CurrentValue = getControlvalue(c);        
        object Targetvalue = getfreezButtonTargetValue(freezButton,ref strFreezMsg);
        if (getControlvalue(c).ToString() == getfreezButtonTargetValue(freezButton,ref strFreezMsg).ToString())
        {
            lblmessage.Text = strFreezMsg.ToString();
            lblmessage.Visible = true;
            freezButton.Visible = false;
        }              
    }
    public static void freezeButton(Button freezButton, Control c)
    {
        object CurrentValue = 0;
        object Targetvalue = 0;
        CurrentValue = getControlvalue(c);
        Targetvalue = getfreezButtonTargetValue(freezButton);
        if (CurrentValue.ToString() == Targetvalue.ToString())
        {
            freezButton.Visible = false;
        }
    }
    private static object getfreezButtonTargetValue(Button freezButton)
    {
        object FreezButtonMsg="";
        return getfreezButtonTargetValue(freezButton, ref FreezButtonMsg);
    }
    private static object getfreezButtonTargetValue(Button freezButton, ref object FreezButtonMsg)
    {
        string strFreezVal = "";
        strFreezVal = GlobalUtilities.ConvertToString(freezButton.Attributes["FreezTargetValue"]);
        FreezButtonMsg = GlobalUtilities.ConvertToString(freezButton.Attributes["FreezButtonMsg"]);
        return strFreezVal;
    }    
    public static object getControlvalue(Control c)
    {
        object CurrentValue = "";
        if (c is TextBox)
        {
            TextBox txt;
            txt = (TextBox)c;
            CurrentValue = txt.Text;
        }
        else if (c is DropDownList)
        {
            DropDownList ddl;
            ddl = (DropDownList)c;
            CurrentValue = ddl.SelectedValue;
        }
        else if (c is CheckBox)
        {
            CheckBox chk = (CheckBox)c;
            CurrentValue = (chk.Checked) ? 1 : 0;
        }
        return CurrentValue;

    }
}
