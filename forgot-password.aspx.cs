using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;

public partial class forgot_password : System.Web.UI.Page
{
    GlobalData globalData = new GlobalData("tbl_clientuser", "clientuserid");
    protected void Page_Load(object sender, EventArgs e)
    {

        //lblPageTitle.Text = "FinStation-Forgot Password";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strEmailId = global.CheckInputData(txtUserName.Text);
        string query = "select * from tbl_clientuser "+
                        "where clientuser_username='" + strEmailId + "'";

        DataRow dr = DbTable.ExecuteSelectRow(query);

        if (dr != null)
        {
            string struseremailid = GlobalUtilities.ConvertToString(dr["clientuser_username"]);
            string strMessage = "<table cellpadding=0 cellspacing=0 width='800'><tr><td><table width='100%'>" +
                                "<tr><td style='border-bottom:solid 5px #17365d;background-color:#17365d;color:#ffffff;font-size:15px;padding:10px 0px 10px 10px;font-family:Trebuchet MS;'>Forgot Password</td></tr>" +
                                "<tr><td style='background-color:#ebebeb;border-bottom:solid 1px #dad8d8;border-right:solid 1px #dad8d8;border-left:solid 1px #dad8d8;color:#484848;font-size:14px;padding-left:15px;padding-bottom:15px;'><table>" +
                                "<tr><td>Hi <b>" + Convert.ToString(dr["clientuser_name"]) + "</b>,</td></tr>" +
                                "<tr><td>&nbsp;</td></tr>" +
                                "<tr><td>Your UserName is <b>" + Convert.ToString(dr["clientuser_username"]) + "</td></tr>" +
                                "<tr><td>&nbsp;</td></tr>" +
                                 "<tr><td>Your Password is <b>" + Convert.ToString(dr["clientuser_password"]) + "</td></tr>" +
                                "<tr><td>Best Regards,</td></tr>" +
                                "<tr><td><b>Finrex</b></td></tr>" +
                            "</table></td></tr></table></td></tr></table>";

            string CcEmailid = GlobalUtilities.GetAppSetting("AdminEmailId");
            if (txtUserName.Text != "")
            {
                BulkEmail.SendMail(struseremailid, "Forgot Password - Finrex", strMessage, "");

                lblMessage.Text = "Password sent to your Email Id.";
            }
            
            txtUserName.Text = "";
            lblMessage.Visible = true;
        }
        else
        {
            lblMessage.Text = "Invalid UserName";
            lblMessage.Visible = true;
        }
            
    }
    
}
