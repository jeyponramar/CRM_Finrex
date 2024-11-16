using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class share : System.Web.UI.UserControl
{
    public string FileType = "";
    public string FolderPath = "";
     
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Enctype = "multipart/form-data";

        Page.Form.Attributes.Add("folder", "followup");
        if (Request.QueryString["type"] == "image")
        {
            FileType = "image";
            FolderPath = "upload/photos/" + Request.QueryString["id"] + "/";
        }
        else if (Request.QueryString["type"] == "song")
        {
            FileType = "song";
            FolderPath = "upload/songs/" + Request.QueryString["id"] + "/";
        }
        else if (Request.QueryString["type"] == "videos")
        {
            FileType = "video";
            FolderPath = "upload/videos/" + Request.QueryString["id"] + "/";
        }
        if (!IsPostBack)
        {
            BindShare();
        }
    }
    private void BindShare()
    {
        string query = "select * from tbl_share s "+
                        "join tbl_user u on u.user_userid=s.share_createdby "+
                        "left join tbl_sharecomment sc on sc.sharecomment_shareid = s.share_shareid "+
                        "left join tbl_user u2 on u2.user_userid = sc.sharecomment_createdby order by share_shareid desc";
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = new DataTable();
        dttbl = obj.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        int prevShareId = 0;
        int i = 0;
        while (i < dttbl.Rows.Count)
        {
            string message = Convert.ToString(dttbl.Rows[i]["share_message"]);
            int userid = Convert.ToInt32(dttbl.Rows[i]["user_userid"]);
            int shareid = Convert.ToInt32(dttbl.Rows[i]["share_shareid"]);
            string firstName = Convert.ToString(dttbl.Rows[i]["user_firstname"]);
            string lastName = Convert.ToString(dttbl.Rows[i]["user_lastname"]);
            string photoPath = "../images/user/" + userid + ".jpg";
            string date = Convert.ToString(dttbl.Rows[i]["share_createddate"]);
            string attachment = Convert.ToString(dttbl.Rows[i]["share_attachment"]);
            bool iscomment = false;
            string attachmentHtml = "";

            if (!System.IO.File.Exists(Server.MapPath(photoPath)))
            {
                photoPath = "../images/user/0.jpg";
            }
            if (attachment != "")
            {
                Array arrfiles = attachment.Split('|');
                for (int j = 0; j < arrfiles.Length; j++)
                {
                    string fileFullName = arrfiles.GetValue(j).ToString();
                    Array arrf = fileFullName.Split('=');
                    if (arrf.Length > 1)
                    {
                        string fileName = arrf.GetValue(0).ToString();
                        string actualFileName = arrf.GetValue(1).ToString();
                        string ext = fileName.Substring(fileName.ToLower().LastIndexOf("."));
                        int width = 25;
                        string icon = "";
                        string folder = "../upload/followup";
                        if (ext == ".doc" || ext == ".docx")
                        {
                            icon = "../images/icon/doc.png";
                        }
                        else if (ext == ".bmp" || ext == ".jpg" || ext == ".gif"
                            || ext == ".png" || ext == ".tif")
                        {
                            icon = folder + "/" + actualFileName;
                            width = 50;
                        }
                        else if (ext == ".pdf")
                        {
                            icon = "../images/icon/pdf.png";
                        }
                        else if (ext == ".zip")
                        {
                            icon = "../images/icon/zip.png";
                        }
                        else if (ext == ".txt")
                        {
                            icon = "../images/icon/text.png";
                        }
                        else
                        {
                            icon = "../images/icon/unknown.png";
                        }
                        string actualFilePath = folder + "/" + actualFileName;
                        if (System.IO.File.Exists(Server.MapPath(actualFilePath)))
                        {
                            attachmentHtml += "<a href='../download.aspx?f=folloup&id=" + actualFileName + "' target='_blank' style='color:#444444;text-decoration:none;'>" +
                                                "<img src='" + icon + "' width='" + width + "'/>" +
                                                "&nbsp;" +
                                                fileName + "&nbsp;" + GlobalUtilities.GetFileSize(actualFilePath) +
                                            "</a>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;";
                                                    
                        }
                    }
                }
            }
            if (shareid != prevShareId)
            {
                
                html.Append("<tr>" +
                               "<td width='70' class='valign'><img src='" + photoPath + "' width='50'/></td>" + 
                               "<td style='border-top:solid 1px #efefef;'>" +
                                   "<table width='100%'>" +
                                       "<tr>"+
                                            "<td class='valign'>" +
                                                "<table width='100%' cellpadding=0 cellspacing=0>" +
                                                   "<tr>" +
                                                        "<td class='valign'>" +
                                                           "<table width='100%' cellpadding=0 cellspacing=0>" +
                                                               "<tr><td><b>" + firstName + " " + lastName + "</b></td>" +
                                                                    "<td class='right date'>" + date + "</td>" +
                                                               "</tr>" +
                                                            "</table>" +
                                                         "</td>" +
                                                    "</tr>" +
                                                   "<tr><td>" + message + "</td></tr>" +
                                                 "</table>" +
                                             "</td>" +
                                           "</tr>" +
                                     "</table>" +
                                   "</td>" +
                                "</tr>");
                if (attachmentHtml != "")
                {
                    attachmentHtml = "<tr><td>&nbsp;</td><td>" + attachmentHtml + "</td></tr>";
                    html.Append(attachmentHtml);
                }
            }
            while (i < dttbl.Rows.Count)
            {
                //for commment
                if (dttbl.Rows[i]["user_userid1"] == DBNull.Value)
                {
                    break;
                }
                userid = Convert.ToInt32(dttbl.Rows[i]["user_userid1"]);
                message = Convert.ToString(dttbl.Rows[i]["sharecomment_comment"]);
                firstName = Convert.ToString(dttbl.Rows[i]["user_firstname1"]);
                lastName = Convert.ToString(dttbl.Rows[i]["user_lastname1"]);
                photoPath = "../images/user/" + userid + ".jpg";
                date = Convert.ToString(dttbl.Rows[i]["sharecomment_createddate"]);

                if (!System.IO.File.Exists(Server.MapPath(photoPath)))
                {
                    photoPath = "../images/user/0.jpg";
                }
                html.Append("<tr><td width=70>&nbsp;</td>" +
                               "<td style='background-color:#f0f2f6;'>" +
                                   "<table width='100%'>" +
                                       "<tr><td width='70' class='valign'><img src='" + photoPath + "' width='50'/></td>" +
                                            "<td class='valign'>" +
                                                "<table width='100%' cellpadding=0 cellspacing=0>" +
                                                   "<tr>" +
                                                        "<td class='valign'>" +
                                                           "<table width='100%' cellpadding=0 cellspacing=0>" +
                                                               "<tr><td><b>" + firstName + " " + lastName + "</b></td>" +
                                                                    "<td class='right date'>" + date + "</td>" +
                                                               "</tr>" +
                                                            "</table>" +
                                                         "</td>" +
                                                    "</tr>" +
                                                   "<tr><td>" + message + "</td></tr>" +
                                                 "</table>" +
                                             "</td>" +
                                           "</tr>" +
                                    "</table>" +
                                  "</td>" +
                             "</tr>");
                i++;
            }
            html.Append("<tr>" +
                            "<td><input type='text' value='" + shareid + "' class='hidden d_share_comment_" + i.ToString() + "' name='shareid'/></td>" +
                            "<td><textarea name='comment' cols='50' class='comment watermark savesharecomment d_share_comment_" + i.ToString() + "' dtarget='d_share_comment_" + i.ToString() + "' " +
                                 "wm='Write a comment' m='sharecomment'>Write a comment</textarea></td>" +
                        "</tr>");
            
            prevShareId = shareid;
            i++;
        }
        ltshare.Text = html.ToString();
    }

}
