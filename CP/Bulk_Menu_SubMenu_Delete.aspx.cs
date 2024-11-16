using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class CP_Bulk_Menu_SubMenu_Delete : System.Web.UI.Page
{
    GlobalData globalData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            globalData.FillDropdown(ddlMenuId, "tbl_menu", "menu_menuname", "menu_menuid", "", "menu_menuname");            
        }
    }
    protected void btnBind_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.Text == "Bind All Menu")
        {
            BindMenu_SubMenu(true);
        }
        else
        {
            BindMenu_SubMenu(false);
        }
    }
    protected void ddlMenuId_Changed(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToInt(ddlMenuId.SelectedValue) > 0)
        {
            BindMenu_SubMenu(false);
            btnAllMenu.Visible = false;
        }
        else
        {
            btnAllMenu.Visible = true;
        }
    }
    private void BindMenu_SubMenu(Boolean isMenu)
    {
        if (!isMenu)
        {
            if (GlobalUtilities.ConvertToInt(ddlMenuId.SelectedValue) == 0)
            {
                lblMessage.Text = "Please select Menu";
                lblMessage.Visible = true;
                return;
            }
        }
        StringBuilder html = new StringBuilder();
        int MenuId = (!isMenu) ? GlobalUtilities.ConvertToInt(ddlMenuId.SelectedValue) : 0;
        ViewState["MenuID"] = MenuId;                
        DataTable dt = new DataTable();
        string query = (MenuId > 0) ? "SELECT * FROM tbl_submenu WHERE submenu_menuid=" + MenuId : "SELECT * FROM tbl_menu";
        string strType = ((MenuId == 0)) ? "menu_menuname" : "submenu_submenuname";
        string strIdType = ((MenuId == 0)) ? "menu_menuid" : "submenu_submenuid";
        query = query + " ORDER BY " + strType;
        dt = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dt))
        {
            html.Append("<table  class='repeater' border='1' cellspacing='0'><tr  class='repeater-header'><td colspan='3'>" + Convert.ToString((MenuId > 0) ? "Sub Menu" : "Menu") + "</td></tr>");
            string rcls = "repeater-alt";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rcls = "repeater-alt";
                string strMenuName = GlobalUtilities.ConvertToString(dt.Rows[i][strType]);
                int menu_subMenuId = GlobalUtilities.ConvertToInt(dt.Rows[i][strIdType]);
                if (i % 2 == 0)                
                    rcls = "repeater-row";                
                if (i % 3 == 0)
                {
                    html.Append("<tr class='" + rcls + "'>");
                }
                html.Append("<td><input type='checkbox' class='chk_menusubmenu' menu_subMenuId=" + menu_subMenuId + " name='" + strMenuName + "'/>" + strMenuName + " </td>");
                if ((i + 1) % 3 == 0)
                {
                    html.Append("</tr>");
                }
            }
            html.Append("</table>"); 
            btnSubmit.Visible = true;
            selectall.Visible = true;
            ltMenu_SubMenu.Text = html.ToString();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Menu_SubMneuId = GlobalUtilities.ConvertToString(hdnMenuSubMenuId.Text);
        if (Menu_SubMneuId != "")
        {
            string strType = (GlobalUtilities.ConvertToBool(Request.QueryString["IsMenu"])) ? "menu_menuid" : "submenu_submenuid";
            int MenuId = GlobalUtilities.ConvertToInt(ViewState["MenuID"]);
            DataTable dt = new DataTable();
            string query = (MenuId== 0) ? "DELETE FROM tbl_menu WHERE menu_menuid IN(" + Menu_SubMneuId + ")" : "DELETE FROM tbl_submenu  WHERE submenu_submenuid IN(" + Menu_SubMneuId + ")";
            if (DbTable.ExecuteQuery(query))
            {
                lblMessage.Text = "Menu's deleted sucessfully!!!!!!!!!!!";
                lblMessage.Visible =true;
                ltMenu_SubMenu.Text = "";
            }
        }
        else
        {
            lblMessage.Text = "Invalid Selection!!!!!!!";
            lblMessage.Visible = true;
        }


    }
}
