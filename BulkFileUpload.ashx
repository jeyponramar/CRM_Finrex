<%@ WebHandler Language="C#" Class="FileUpload" %>
using System;
using System.Web;
using System.IO;
using System.Drawing;

public class FileUpload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.Files.Count == 0)
        {
        }
        else
        {
            if (!GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsRefuxLoggedIn"))
                && !GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsLoggedIn")))
                return;    
            HttpPostedFile uploadedfile = context.Request.Files[0];
            string folderName = "";
            folderName = Convert.ToString(context.Request.QueryString["folderpath"]);
            folderName = folderName.Replace('/', '\\');
            folderName = folderName.Replace("..", "~");

            if (!folderName.StartsWith("~")) folderName = folderName = "~/" + folderName;

            bool isguid = Convert.ToBoolean(context.Request.QueryString["isguid"]);

            string FileName = uploadedfile.FileName;
            if (FileName.Contains("\\"))
            {
                FileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);
            }
            else if (FileName.Contains("/"))
            {
                FileName = FileName.Substring(FileName.LastIndexOf("/") + 1);
            }
            
            string fileprefix = GlobalUtilities.ConvertToString(context.Request.QueryString["fileprefix"]);
            FileName = fileprefix + FileName;
            
            string FileType = uploadedfile.ContentType;
            string fileext = FileName.Substring(FileName.LastIndexOf(".")).ToLower();
            bool invalidfile = false;

            string GuidFileName = "";
            if (isguid == true)
            {
                Guid g;
                g = Guid.NewGuid();
                GuidFileName = g + fileext;
            }
            string filetype = "";

            if (fileext == ".jpg" || fileext == ".jpeg" || fileext == ".png" || fileext == ".gif" || fileext == ".bmp")
            {
                filetype = "image";
            }
            else if (fileext == ".mp3" || fileext == ".wav")
            {
                filetype = "song";
            }
            else if (fileext == ".avi" || fileext == ".wmv" || fileext == ".mov" || fileext == ".mpg" || fileext == ".vob" || fileext == ".3g2")
            {
                filetype = "video";
            }
            else if (fileext == ".doc" || fileext == ".docx")
            {
                filetype = "doc";
            }
            else if (fileext == ".txt")
            {
                filetype = "txt";
            }
            else if (fileext == ".pdf")
            {
                filetype = "pdf";
            }
            else if (fileext == ".xls" || fileext == ".xlsx")
            {
                filetype = "excel";
            }
            else if (fileext == ".ppt" || fileext == ".pptx")
            {
                filetype = "ppt";
            }
            else
            {
                filetype = "unknown";
            }

            if (fileext == ".exe" || fileext == ".bat")
            {
                invalidfile = true;
            }
            if (invalidfile)
            {
                context.Response.Write("{\"name\":\"" + FileName + "\",\"size\":\"Invalid File\"}");
                return;
            }
            //if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderName)))
            //{
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderName));
            //}
            string filePath = "";
            string size = GlobalUtilities.ConvertToString(context.Request.QueryString["size"]);
            if (size != "")
            {
                string tempPath = context.Server.MapPath("~/upload/temp/" + Guid.NewGuid() + ".jpg");
                uploadedfile.SaveAs(tempPath);
                Image img = Image.FromFile(tempPath);
                Array arr = size.Split('x');
                int width = Convert.ToInt32(arr.GetValue(0));
                int height = Convert.ToInt32(arr.GetValue(1));
                if (width == img.Width && height == img.Height)
                {
                }
                else
                {
                    context.Response.Write("ERROR : Invalid Image Size, Allowed Image is : " + width + "x" + height);
                    return;
                }
            }
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderName)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderName));
            }
            if (isguid == true)
            {
                if (!folderName.EndsWith("/")) folderName = folderName + "/";
                filePath = HttpContext.Current.Server.MapPath(folderName) + GuidFileName;
                uploadedfile.SaveAs(filePath);
                //FileSize = GetFileSize(folderName + GuidFileName);
                FileInfo fl = new FileInfo(HttpContext.Current.Server.MapPath(folderName + GuidFileName));

                context.Response.Write("{\"name\":\"" + FileName + "\",\"guidfilename\":\"" + GuidFileName + "\",\"filetype\":\"" + filetype + "\"}");
            }
            else
            {
                filePath = HttpContext.Current.Server.MapPath(folderName) + FileName;
                uploadedfile.SaveAs(filePath);
                //FileSize = GetFileSize(folderName + FileName);
                FileInfo fl = new FileInfo(HttpContext.Current.Server.MapPath(folderName + FileName));

                context.Response.Write("{\"name\":\"" + FileName + "\",\"filetype\":\"" + filetype + "\"}");
            }
            string resize = GlobalUtilities.ConvertToString(context.Request.QueryString["resize"]);
            
            if (resize != "")
            {
                //Common.ResizeMultiSizeImage(filePath, folderName, resize, 0);
                if (!folderName.Contains("temp"))
                {
                    Common.ResizeMultiSizeImageWithoutFolder(filePath, folderName, resize, 0);
                }
            }
            
        }
    }
    
    public static string GetFileSize(string path)
    {
        FileInfo zipfile = new FileInfo(HttpContext.Current.Server.MapPath(path));
        double l = (zipfile.Length / 1024.0);
        string length = "";
        if (l >= 1024.0)
        {
            length = string.Format("{0:0.00}", l / 1024.0) + " MB";
        }
        else
        {
            length = string.Format("{0:0.00}", l) + " KB";
        }
        return length;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
