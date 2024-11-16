using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using WebComponent;


public partial class displaycontrol : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayControl();
    }
    private void DisplayControl()
    {
        StringBuilder html = new StringBuilder();
        int pageid = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        string query = "SELECT * FROM tbl_content where content_pageid=" + pageid + " ORDER BY content_sequence ASC";
        InsertUpdate obj = new InsertUpdate();
        DataTable content = obj.ExecuteSelect(query);
        if (content.Rows.Count > 0)
        {
            for (int i = 0; i < content.Rows.Count; i++)
            {
                int templatetype = GlobalUtilities.ConvertToInt(content.Rows[i]["content_templatetypeid"]);
                string description = GlobalUtilities.ConvertToString(content.Rows[i]["content_content"]);
                int contentid = GlobalUtilities.ConvertToInt(content.Rows[i]["content_contentid"]);
                string ext = GlobalUtilities.ConvertToString(content.Rows[i]["content_extension"]);
                int sequence = GlobalUtilities.ConvertToInt(content.Rows[i]["content_sequence"]);
                if (templatetype == 1)
                {
                    html.Append("<table cellspacing='0' cellpadding='0' width='100%' style='padding-top:20px;'");
                    html.Append("<tr>");
                    html.Append("<td><a href='../content/add.aspx?id=" + contentid + "&pageid=" + pageid + "'>Edit</a></td>");
                    html.Append("<td><a href='../content/add.aspx?pageid=" + pageid + "&sequence=" + sequence + "'>Insert Above</a></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td><img src='../upload/page/content/" + contentid + ext + "' /></td>");
                    html.Append("<td style='vertical-align:top;padding-left:20px;'>" + description + "</td>");
                    html.Append("</tr>");
                    html.Append("</table>");
                }
                else if (templatetype == 2)
                {
                    html.Append("<table cellspacing='0' cellpadding='0' width='100%' style='padding-top:20px;'");
                    html.Append("<tr>");
                    html.Append("<td><a href='../content/add.aspx?id=" + contentid + "&pageid=" + pageid + "'>Edit</a></td>");
                    html.Append("<td><a href='../content/add.aspx?pageid=" + pageid + "&sequence=" + sequence + "'>Insert Above</a></td>");

                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td style='vertical-align:top;padding-right:20px;'>" + description + "</td>");
                    html.Append("<td><img src='../upload/page/content/" + contentid + ext + "' /></td>");
                    html.Append("</tr>");
                    html.Append("</table>");
                }
                else if (templatetype == 3)
                {
                    html.Append("<table cellspacing='0' cellpadding='0' width='100%' style='padding-top:20px;'");
                    html.Append("<tr>");
                    html.Append("<td><a href='../content/add.aspx?id=" + contentid + "&pageid=" + pageid + "'>Edit</a></td>");
                    html.Append("<td><a href='../content/add.aspx?pageid=" + pageid + "&sequence=" + sequence + "'>Insert Above</a></td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td style='text-align:center;padding-bottom:20px;'><img src='../upload/page/content/" + contentid + ext + "' /></td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td style='text-align:center;'>" + description + "</td>");
                    html.Append("</tr>");
                    html.Append("</table>");
                }
                else if (templatetype == 4)
                {
                    html.Append("<table cellspacing='0' cellpadding='0' width='100%' style='padding-top:20px;'");
                    html.Append("<tr>");
                    html.Append("<td><a href='../content/add.aspx?id=" + contentid + "&pageid=" + pageid + "'>Edit</a></td>");
                    html.Append("<td><a href='../content/add.aspx?pageid=" + pageid + "&sequence=" + sequence + "'>Insert Above</a></td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td style='text-align:center;'>" + description + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td style='text-align:center;padding-top:20px;'><img src='../upload/page/content/" + contentid + ext + "' /></td>");
                    html.Append("</tr>");
                    html.Append("</table>");
                }
                ltdisplaycontrol.Text = html.ToString();
            }
        }
    }
    protected void btn_AddContent_Click(object sender, EventArgs e)
    {
        int pageid = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        Response.Redirect("../content/add.aspx?pageid=" + pageid);
    }
}
