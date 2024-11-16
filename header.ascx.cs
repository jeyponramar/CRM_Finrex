using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WebComponent;
using System.Data;
using System.IO;

public partial class header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMenu();
            int userId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            if (File.Exists(Server.MapPath("~/upload/user/" + userId + ".jpg")))
            {
                imgLoggedInUser.ImageUrl = "~/upload/user/" + userId + ".jpg";
            }
            else
            {
                imgLoggedInUser.ImageUrl = "~/upload/user/default.jpg";
            }
            EnableLiveChat();
        }
        lblLoginUserName.Text = GlobalUtilities.ConvertToString(CustomSession.Session("Login_FullName"));
    }
    private void EnableLiveChat()
    {
        string query = "select * from tbl_user where user_userid="+Common.UserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr["user_agentid"]) > 0)
        {
            tdLiveChat.Visible = true;
        }
        else
        {
            tdLiveChat.Visible = false;
        }
    }
    private void BindMenu()
    {
        InsertUpdate obj = new InsertUpdate();
        int roleID = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId"));
        string strExtraJoin = "";
        if (roleID != 1)
        {
            strExtraJoin = "JOIN tbl_menurights ON (menurights_submenuid=submenu_submenuid AND menurights_roleid=" + roleID + ")" +
                            "OR (menurights_roleid=" + roleID + " AND menurights_menuid=menu_menuid)";
        }
        string exludeMenu = "";
        //if (AppConstants.IsLive)
        //{
            exludeMenu = " AND menu_menuname <> 'REFUX'";
        //}
        string query = "select m.*,s.* from tbl_submenu s " +
                        "join tbl_menu m on m.menu_menuid = s.submenu_menuid " + strExtraJoin +
                        "where submenu_isvisible=1" + exludeMenu + " order by menu_sequence,submenu_sequence";
        DataTable dttbl = obj.ExecuteSelect(query);
        StringBuilder menu = new StringBuilder();
        StringBuilder submenu = new StringBuilder();
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            int prevMenuId = Convert.ToInt32(dttbl.Rows[0]["menu_menuid"]);
            int menuId = 0;
            string subMenuName = "";
            string url = "";
            string menuName = "";
            int pMenuId = 0;
            string submenutype = "";
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                subMenuName = Convert.ToString(dttbl.Rows[i]["submenu_submenuname"]);
                
                menuName = Convert.ToString(dttbl.Rows[i]["menu_menuname"]);
                menuId = Convert.ToInt32(dttbl.Rows[i]["menu_menuid"]);

                if (pMenuId != menuId)
                {
                    menu.Append("<td class=\"menu\">" + Convert.ToString(dttbl.Rows[i]["menu_menuname"]) + "</td><td class=\"menu-sep\"></td>");
                    submenu.Append("<div class='submenu hidden' style='position:absolute;left:100px;top:45px;' target='" + menuName + "'><table>");
                    DataRow[] drs = dttbl.Select("menu_menuid=" + menuId);
                    submenu.Append("<tr><td style='padding:0px;width:250px;vertical-align:top;'><table cellspacing=0 cellpadding=0>");
                    int mod = 15;
                    if (drs.Length > 15 && drs.Length < 30) mod = 10;
                    for (int j = 0; j < drs.Length; j++)
                    {
                        DataRow dr = (DataRow)drs.GetValue(j);
                        int subMenuId = GlobalUtilities.ConvertToInt(dr["submenu_submenuid"]);
                        subMenuName = GlobalUtilities.ConvertToString(dr["submenu_submenuname"]);
                        submenutype = GlobalUtilities.ConvertToString(dr["submenu_menutype"]);
                        url = Convert.ToString(dr["submenu_url"]);
                        if (j % mod == 0 && j > 0)
                        {
                            submenu.Append("</table></td><td style='padding:0px;width:250px;vertical-align:top;'><table cellspacing=0 cellpadding=0>");
                        }
                        if (GlobalUtilities.ConvertToBool(dr["submenu_isnewwindow"]))
                        {
                            submenu.Append("<tr><td in='1' class='lnksubmenu smenu-" + submenutype + "' href='" + url + "'>" + subMenuName + "</td></tr>");
                        }
                        else
                        {
                            submenu.Append("<tr><td class='lnksubmenu smenu-" + submenutype + "' href='" + url + "'>" + subMenuName + "</td></tr>");
                        }
                    }
                    
                    submenu.Append("</table></td></tr>");
                    submenu.Append("</table></div>");
                }

                prevMenuId = menuId;
                pMenuId = menuId;
            }
        }
        ltMenu.Text = menu.ToString();
        ltSubMenu.Text = submenu.ToString();
    }

}
