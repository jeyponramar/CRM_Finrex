using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.IO;
using System.Collections;

public partial class CP_addons : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_module","moduleid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlModule, "tbl_module", "module_modulename", "");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Request.Url.ToString().ToLower().Contains("demoonline.in"))
        {
            if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId")) > 1)
            {
                lblMessage.Text="This feature is not available in demo version!";
                return;
            }
        }
        bool saved = SaveFollowup();
        SaveModuleDetail();
        if (saved)
        {
            lblMessage.Text = "Add-ons added successfully";
        }
    }
    private void SaveModuleDetail()
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("isfollowup", chkFollowup.Checked);
        hstbl.Add("iscomment", chkComments.Checked);
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_module",GlobalUtilities.ConvertToInt(ddlModule.SelectedValue));
    }
    private bool SaveFollowup()
    {
        DataRow dr = gblData.GetTableRow("tbl_module", GlobalUtilities.ConvertToInt(ddlModule.SelectedValue));
        string moduleName = Convert.ToString(dr["module_modulename"]);
        string module = moduleName.Replace(" ", "").ToLower();
        string followupDesign = "";
        string designHtml = "";
        string controlRegister = "";
        string designFile = Server.MapPath("~/" + module + "/add.aspx");
        if (File.Exists(designFile))
        {
            designHtml = GlobalUtilities.ReadFile(designFile);
        }
        else
        {
            lblMessage.Text = "This module is currently not available.";
            return false;
        }

        if (chkFollowup.Checked)
        {
            followupDesign = "<tr>\n" +
                                "\t<td colspan=\"2\">\n" +
                                    "\t\t<uc:Followups id=\"Followups\" runat=\"server\" Module=\"followups_" + moduleName.ToLower().Replace(" ", "") + "\"/>" +
                                "\n\t</td>" +
                            "\n\t</tr>";
            if (!designHtml.Contains("Followups.ascx"))
            {
                controlRegister = "<%@ Register Src=\"~/Followups.ascx\" TagName=\"Followups\" TagPrefix=\"uc\" %>";
                designHtml = AddControlRegisterCode(designHtml, "CONTROLREGISTER", controlRegister);
            }
        }
        string commentsDesign = "";
        if (chkComments.Checked)
        {
            commentsDesign = "<tr>\n" +
                                "\t<td colspan=\"2\">\n" +
                                    "\t\t<uc:Chatter id=\"chatter\" runat=\"server\" Module=\"" + moduleName.ToLower().Replace(" ", "") + "\"/>" +
                                "\n\t</td>" +
                            "\n\t</tr>";
            if (!designHtml.Contains("Chatter.ascx"))
            {
                controlRegister = "<%@ Register Src=\"~/Chatter.ascx\" TagName=\"Chatter\" TagPrefix=\"uc\" %>";
                designHtml = AddControlRegisterCode(designHtml, "CONTROLREGISTER", controlRegister);
            }
        }
        designHtml = GlobalUtilities.ReplaceStartEnd(designHtml, "FOLLOWUP", followupDesign);
        designHtml = GlobalUtilities.ReplaceStartEnd(designHtml, "COMMENTS", commentsDesign);
        GlobalUtilities.WriteFile(designFile, designHtml);

        //string templateXmlFile = Server.MapPath("template/followup.xml");
        //string targetxmlFile = Server.MapPath("~/xml/view/" + "followups_" + module + ".xml");
        //if (!File.Exists(targetxmlFile))
        //{
        //    File.Copy(templateXmlFile, targetxmlFile);
        //    string xml = GlobalUtilities.ReadFile(targetxmlFile);
        //    xml = xml.Replace("$Module$", moduleName.Replace(" ", ""));
        //    GlobalUtilities.WriteFile(targetxmlFile, xml);
        //}
        return true;
    }
    public static string AddControlRegisterCode(string designHtml, string replacePrefix, string replaceHtml)
    {
        int intStart = designHtml.IndexOf("<%--" + replacePrefix + "_START--%>");
        if (intStart > 0)
        {
            string end = "<%--" + replacePrefix + "_END--%>";
            int intEnd = designHtml.IndexOf(end);
            if (intEnd > 0)
            {
                string replaceString = designHtml.Substring(intStart, intEnd - intStart + end.Length);
                replaceHtml = replaceString.Replace("<%--" + replacePrefix + "_END--%>", "\n" + 
                                                replaceHtml + "\n<%--" + replacePrefix + "_END--%>");
                designHtml = designHtml.Replace(replaceString, replaceHtml);
            }
        }
        return designHtml;
    }
    protected void ddlModule_Changed(object sender, EventArgs e)
    {
        DataRow dr = gblData.GetTableRow(GlobalUtilities.ConvertToInt(ddlModule.SelectedValue));
        chkFollowup.Checked = GlobalUtilities.ConvertToBool(dr["module_isfollowup"]);
        chkComments.Checked = GlobalUtilities.ConvertToBool(dr["module_iscomment"]);
    }
}
