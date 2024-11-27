using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using WebComponent;

public partial class MultiFileUpload : System.Web.UI.UserControl
{
    public string _folderPath = "";
    public bool _isGuid = false;
    private string _Size = "";
    private string _ReSize = "";
    private bool _IsMultiple = false;
    private string _FileType = "";
    private string _saveAs = "";
    private string _filePath = "";
    private bool _isDetail = false;
    private bool _saveExt = false;
    private bool _IsPopulateFiles = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initMultiUpload();
        }
        //Page.Form.Enctype = "multipart/form-data";

    }
    private void initMultiUpload()
    {
        if (_folderPath == "" && _filePath == "") return;

        if (Request.QueryString["id"] != null)
        {
            if (IsMultiple)
            {
                //PopulateFiles("~/" + _folderPath + "/" + GetId());
                PopulateFiles("~/" + _folderPath + "/");
            }
            else
            {
                PopulateFiles("~/" + _folderPath + "/");
            }
        }

        if (_folderPath.EndsWith("\\"))
        {
            _folderPath = _folderPath.Substring(0, _folderPath.Length - 1) + "/";
        }
        else
        {
            _folderPath += "/";
        }
        if (IsMultiple == false)
        {
            _isGuid = true;
            _folderPath = _folderPath + "temp/";
        }
        else
        {
            if (Request.QueryString["id"] == null)
            {
                //_folderPath = _folderPath + "temp/" + Guid.NewGuid() + "/";
                _folderPath = _folderPath + "temp/";
                ViewState["TempFolderPath"] = _folderPath;
                ViewState["TempFiles"] = "";
            }
            else
            {
               // _folderPath = _folderPath + GetId() + "/";
            }
        }
        if (_folderPath == "/temp/") _folderPath = "upload/temp/";

        //string script = "$(document).ready(function(){bulidFileUpload(\"" + _folderPath + "\",\"" + fileNames + "\",'" + _isGuid + "','" + h_FileName.ClientID + "'," +
                        //"'" + IsMultiple + "','" + uploadfilediv.ClientID + "','" + FileType.ToLower() + "');});";
        //this.Page.ClientScript.RegisterClientScriptBlock(typeof(Page), this.ID, "<script>" + script + "</script>");

        SetAttributes();

        if (_isDetail)
        {
            trUpload.Visible = false;
        }
    }
    public void SetTempFolderPath(string folderPath)
    {
        ViewState["TempFolderPath"] = folderPath;
    }
    private void SetAttributes()
    {
        string filePrefix = Guid.NewGuid().ToString() + "_";
        uploadfilediv.Attributes.Add("fileprefix", filePrefix);
        uploadfilediv.Attributes.Add("ismultiple", _IsMultiple.ToString());
        uploadfilediv.Attributes.Add("isguid", _isGuid.ToString());
        uploadfilediv.Attributes.Add("folder", _folderPath);
        //uploadfilediv.Attributes.Add("files", fileNames);
        uploadfilediv.Attributes.Add("filetype", _FileType.ToLower());
        uploadfilediv.Attributes.Add("size", _Size);
        uploadfilediv.Attributes.Add("resize", _ReSize);
    }
    private string GetFileExt()
    {
        if (_saveAs != "")
        {
            return _saveAs;
        }
        DataRow dr = Common.GetCurrentModuleRow();
        if (dr == null) return "";
        string ext = GlobalUtilities.ConvertToString(dr[Common.GetModuleName() + "_" + this.ID.Replace("mfu", "")]);
        return ext;
    }
    public string fileNames
    {
        get 
        {
            string tempFolderPath = Convert.ToString(ViewState["TempFolderPath"]);
            if (tempFolderPath == "") return "";
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/" + tempFolderPath));
            string files = "";
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (files == "")
                    {
                        files = file.Name;
                    }
                    else
                    {
                        files += "," + file.Name;
                    }
                }
            }
            return files;
        }
        set 
        { 
            h_FileName.Text = value; 
        }
    }
    public string fileNamesFullPath
    {
        get
        {
            string strFolder = "~/" + Convert.ToString(ViewState["TempFolderPath"]);
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath(strFolder));
            string files = "";
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (files == "")
                    {
                        files = strFolder + file.Name;
                    }
                    else
                    {
                        files += "," + strFolder + file.Name;
                    }
                }
            }
            return files;
        }
        set
        {
            h_FileName.Text = value;
        }
    }
    public string FolderPath
    {
        get { return _folderPath; }
        set 
        { 
            _folderPath = value;
        }
    }
    public string FilePath
    {
        get { return _filePath; }
        set { _filePath = value; }
    }
    public bool IsMultiple
    {
        get { return _IsMultiple; }
        set { _IsMultiple= value; }
    }
    public string FileType
    {
        get { return _FileType; }
        set { _FileType = value; }
    }
    public string Size
    {
        get { return _Size; }
        set { _Size = value; }
    }
    public string ReSize
    {
        get { return _ReSize; }
        set { _ReSize = value; }
    }    
    public bool IsGuid
    {
        get { return _isGuid; }
        set { _isGuid = value; }
    }
    public string SaveAs
    {
        get { return _saveAs; }
        set { _saveAs = value.Replace(".",""); }
    }
    public bool IsDetail
    {
        set { _isDetail = value; }
        get { return _isDetail; }
    }
    public bool SaveExt
    {
        set { _saveExt = value; }
        get { return _saveExt; }
    }
    public bool IsPopulateFiles
    {
        get { return _IsPopulateFiles; }
        set { _IsPopulateFiles = value; }
    }
    public void SaveFiles(int id)
    {
        string folderPath = _folderPath;
        if (folderPath.EndsWith("\\"))
        {
            folderPath = folderPath.Substring(0, folderPath.Length - 1) + "/";
        }
        if (!folderPath.EndsWith("/")) folderPath += "/";

        if (!folderPath.StartsWith("~")) folderPath = "~/" + folderPath;
        if (IsMultiple)
        {
            string saveFolderPath = folderPath;
            string tempFolderPath = "~/" + Convert.ToString(ViewState["TempFolderPath"]);
            SaveFiles(saveFolderPath, tempFolderPath, id);
            SaveFileNameInDB(h_FileName.Text, id);
        }
    }
    public void SaveFileNameInDBByModule(string m, int id)
    {
        SaveFileNameInDB(h_FileName.Text, m, id);
    }
    public void Save(int id)
    {
        string folderPath = _folderPath;
        if (folderPath.EndsWith("\\"))
        {
            folderPath = folderPath.Substring(0, folderPath.Length - 1) + "/";
        }
        if (!folderPath.EndsWith("/")) folderPath += "/";

        if (!folderPath.StartsWith("~")) folderPath = "~/" + folderPath;
        if (IsMultiple)
        {
            if (Request.QueryString["id"] == null)
            {
                string saveFolderPath = folderPath;// +id;
                string tempFolderPath = "~/" + Convert.ToString(ViewState["TempFolderPath"]);
                SaveFiles(saveFolderPath, tempFolderPath, id);

                if (_ReSize != "")
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath(saveFolderPath));
                    for (int i = 0; i < filePaths.Length; ++i)
                    {
                        string path = filePaths[i];
                        Common.ResizeMultiSizeImage(path, saveFolderPath, _ReSize, id);
                    }
                }
            }
            else
            {
                //no need to save because file will be saved in id folder only
                SaveFileNameInDB(h_FileName.Text, id);
            }
            PopulateFiles("~/" + _folderPath + "/" + id);
        }
        else
        {
            SaveSingleFile(folderPath, id);
            PopulateFiles("~/" + _folderPath);
        }
        if (Request.QueryString["id"] == null)
        {
            Clear();
        }
    }
    public void SaveSingleFile(string saveFolder, int id)
    {
        if (h_FileName.Text == "") return;
        string fileName = h_FileName.Text;
        if(fileName.Contains(","))
        {
            fileName = fileName.Split(',')[1];
        }
        string sourceFile = "";
        Array arr = fileName.Split('.');
        string fileExt = Convert.ToString(arr.GetValue(arr.Length - 1));
        if (_saveAs != "") fileExt = _saveAs;
        string destFile = Server.MapPath(saveFolder + id + "." + fileExt);

        if (FilePath.IndexOf("$id$") > 0)
        {
            destFile = Server.MapPath("~/" + FilePath.Replace("$id$", id.ToString()));
        }
        if (FolderPath.EndsWith("\\"))
        {
            FolderPath = FolderPath.Substring(0, FolderPath.Length - 1) + "/";
        }
        if (!FolderPath.EndsWith("/")) FolderPath += "/";

        sourceFile = "~/" + FolderPath + "temp/" + fileName;
        if (FolderPath == "/")
        {
            sourceFile = "~/upload/temp/" + fileName;
        }
        
        sourceFile = Server.MapPath(sourceFile);

        try
        {
            if (File.Exists(destFile)) File.Delete(destFile);

            Common.CreateFolderForFile(destFile);

            File.Move(sourceFile, destFile);
            if (_ReSize != "")
            {
                //Common.ResizeMultiSizeImage(destFile, "~/" + FolderPath, _ReSize, id);
                Common.ResizeMultiSizeImageWithoutFolder(destFile, "~/" + FolderPath, _ReSize, id);
            }
        }
        catch { }
        SaveExtInDB(fileExt, id);
    }
    public void SaveFile(string destFile)
    {
        string fileName = h_FileName.Text;
        if (fileName.Contains(","))
        {
            fileName = fileName.Split(',')[1];
        }
        string sourceFile = "";
        Array arr = fileName.Split('.');
        string fileExt = Convert.ToString(arr.GetValue(arr.Length - 1));
        if (_saveAs != "") fileExt = _saveAs;

        if (FolderPath.EndsWith("\\"))
        {
            FolderPath = FolderPath.Substring(0, FolderPath.Length - 1) + "/";
        }
        if (!FolderPath.EndsWith("/")) FolderPath += "/";

        sourceFile = "~/" + FolderPath + "temp/" + fileName;
        if (FolderPath == "/")
        {
            sourceFile = "~/upload/temp/" + fileName;
        }

        sourceFile = Server.MapPath(sourceFile);

        try
        {
            if (File.Exists(destFile)) File.Delete(destFile);

            Common.CreateFolderForFile(destFile);

            File.Move(sourceFile, destFile);
          
        }
        catch { }


    }
    private void SaveExtInDB(string ext, int id)
    {
        if (!_saveExt) return;
        string module = Common.GetModuleName();
        string query = "update tbl_" + module + " set " + module + "_" + this.ID.Replace("mfu", "") + "='" + global.CheckInputData(ext) + "' WHERE " + module + "_" + module + "id=" + id;
        DbTable.ExecuteQuery(query);
    }
    private void SaveFileNameInDB(string fileNames, int id)
    {
        string module = Common.GetModuleName();
        SaveFileNameInDB(fileNames, module, id);
    }
    public void SaveFileNameInDB(string fileNames, string module, int id)
    {
        string query = "update tbl_" + module + " set " + module + "_" + this.ID.Replace("mfu", "") + "='" + global.CheckInputData(fileNames) + "' WHERE " + module + "_" + module + "id=" + id;
        DbTable.ExecuteQuery(query);
    }
    public void SaveFiles(string savefolderpath, string sourceFolder, int id)
    {
        string tempFiles = h_FileName.Text;
        string sourcePath = Server.MapPath(sourceFolder);
        string destPath = Server.MapPath(savefolderpath);

        Array arrFiles = tempFiles.Split('|');
        for (int i = 0; i < arrFiles.Length; i++)
        {
            string fileName = Convert.ToString(arrFiles.GetValue(i));
            if (fileName != "")
            {
                string sourceFilePath = sourcePath + fileName;
                string destFilePath = destPath + fileName;
                if (File.Exists(sourceFilePath))
                {
                    if (File.Exists(destFilePath))
                    {
                        try
                        {
                            File.Delete(destFilePath);
                        }
                        catch { }
                    }
                    File.Move(sourceFilePath, destFilePath);
                    if (_ReSize != "")
                    {
                       Common.ResizeMultiSizeImageWithoutFolder(destFilePath, "~/" + savefolderpath, _ReSize, id);
                    }
                }
            }
        }
        SaveFileNameInDB(h_FileName.Text, id);
        /*string filetype = FileType;
        string destPath = Server.MapPath(savefolderpath);
        if (Directory.Exists(destPath))
        {
            //Directory.Delete(destPath);
        }
       
        string sourcePath = Server.MapPath(sourceFolder);
        //if (Directory.Exists(sourcePath))
        //    Directory.Move(sourcePath, destPath);

        string sourcepath = "";
        string destinationpath = "";
        if (!Directory.Exists(Server.MapPath(sourceFolder))) return;

        string[] filePaths = Directory.GetFiles(Server.MapPath(sourceFolder));

        for (int i = 0; i < filePaths.Length; ++i)
        {
            string path = filePaths[i];
            string filename = Path.GetFileName(path);
            sourcepath = Server.MapPath(sourceFolder + "/" + filename);
            destinationpath = Server.MapPath(savefolderpath + "/" + filename);
            try
            {
                if (File.Exists(destinationpath))
                {
                    File.Delete(destinationpath);
                }
            }
            catch { }
            File.Move(sourcepath, destinationpath);
            //File.Delete(sourcepath);
        }*/
    }
    public void Clear()
    {
        h_FileName.Text = "";
        ltFiles.Text = "";

        if (IsMultiple)
        {
            //delete prev files
            string strFolderPath = FolderPath;
            if (!strFolderPath.EndsWith("/")) strFolderPath = strFolderPath + "/";
            _folderPath = strFolderPath + "temp/" + Guid.NewGuid() + "/";
            ViewState["TempFolderPath"] = _folderPath;
            SetAttributes();
        }
    }
    public string CurrentFolder
    {
        get
        {
            return Convert.ToString(ViewState["TempFolderPath"]);
        }
    }
    private int GetId()
    {
        return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
    }
    
    public void PopulateFiles(string folderpath)
    {
        if (!_IsPopulateFiles) return;
        ArrayList arrFiles = new ArrayList();
        string singleFilePath = "";
        if (IsMultiple)
        {
            if (!Directory.Exists(Server.MapPath(folderpath))) return;
            string m = Common.GetModuleName();
            string colName = GlobalUtilities.ConvertToString(m + "_" + this.ID.Replace("mfu", ""));
            string query = "select " + colName + " from tbl_" + m + " WHERE " + m + "_" + m + "id=" + GetId();
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return;
            string files = GlobalUtilities.ConvertToString(dr[colName]);
            h_FileName.Text = files;
            if (files == "") return;
            Array arrFiles1 = files.Split('|');
            for (int i = 0; i < arrFiles1.Length; i++)
            {
                if (Convert.ToString(arrFiles1.GetValue(i)) != "")
                {
                    arrFiles.Add(folderpath + Convert.ToString(arrFiles1.GetValue(i)));
                }
            }

        }
        else
        {
            string ext = GetFileExt().Trim().Replace("undefined|","").Replace("undefined","");
            if (ext == "") return;

            if (FilePath == "")
            {
                arrFiles.Add(Server.MapPath(folderpath + GetId() + "." + ext));
                if (!File.Exists(Convert.ToString(arrFiles[0]))) return;
            }
            else
            {
                arrFiles.Add(Server.MapPath("~/" + FilePath.Replace("$id$", GetId().ToString())));
                if (!File.Exists(Convert.ToString(arrFiles[0]))) return;
                singleFilePath = FilePath.Replace("$id$", GetId().ToString());
            }

        }
        StringBuilder html = new StringBuilder();
        for (int i = 0; i < arrFiles.Count; ++i)
        {
            string path = Convert.ToString(arrFiles[i]);
            string filename = Path.GetFileName(path);
            if (filename.Contains("."))
            {
                string fileext = filename.Substring(filename.LastIndexOf('.')).ToLower();
                string imgsrc = "";
                string deleteimgsrc = "";
                string fullFileUrl = folderpath.Replace("~", "..");
                if (singleFilePath == "")
                {
                    if (fullFileUrl.EndsWith("/"))
                    {
                        fullFileUrl = fullFileUrl + filename;
                    }
                    else
                    {
                        fullFileUrl = fullFileUrl + "/" + filename;
                    }
                }
                else
                {
                    fullFileUrl = "../" + singleFilePath;
                }
                if (singleFilePath == "")
                {
                    if (folderpath.EndsWith("/"))
                    {
                        deleteimgsrc = folderpath + filename;
                    }
                    else
                    {
                        deleteimgsrc = folderpath + "/" + filename;
                    }
                }
                else
                {
                    deleteimgsrc = "~/" + singleFilePath;
                }
                deleteimgsrc = deleteimgsrc.Replace("..", "~");

                if (fileext == ".jpg" || fileext == ".png" || fileext == ".gif" || fileext == ".bmp")
                {
                    imgsrc = fullFileUrl;// folderpath + "/" + filename;
                }
                else if (fileext == ".mp3" || fileext == ".wav")
                {
                    imgsrc = "../images/song-icon.png";
                }
                else if (fileext == ".avi" || fileext == ".wmv" || fileext == ".mov" || fileext == ".mpg" || fileext == ".vob" || fileext == ".3g2")
                {
                    imgsrc = "../images/video-icon.png";
                }
                else if (fileext == ".doc" || fileext == ".docx")
                {
                    imgsrc = "../images/doc-icon.png";
                }
                else if (fileext == ".txt")
                {
                    imgsrc = "../images/txt-icon.png";
                }
                else if (fileext == ".pdf")
                {
                    imgsrc = "../images/pdf-icon.png";
                }
                else if (fileext == ".zip")
                {
                    imgsrc = "../images/icon/zip.png";
                }
                else if (fileext == ".xls" || fileext == ".xlsx")
                {
                    imgsrc = "../images/xl-icon.png";
                }
                else if (fileext == ".ppt")
                {
                    imgsrc = "../images/ppt-icon.png";
                }
                else
                {
                    imgsrc = "../images/unknown.png";
                }
                string actualFileName = filename;
                if (IsMultiple)
                {
                    int index = filename.IndexOf('_');
                    if (index > 0)
                    {
                        filename = filename.Substring(index + 1);
                    }
                }
                html.Append("<tr><td align='center'><img src=\'" + imgsrc + "' width='25px'/>" +
                    "</td><td><a href='" + fullFileUrl + "' target='_blank'>" + filename + "</a></td>");
                if (_isDetail == false)
                {
                    html.Append("<td><img src='../images/delete.png' class='deletefile hand' val='" + deleteimgsrc + "' fn='" + actualFileName + "' title='Delete'></td>");
                }
                html.Append("</tr>");
            }
        }
        ltFiles.Text = html.ToString();
    }
    public void BindMultiFiles(ArrayList arrFiles, string folderpath)
    {
        StringBuilder html = new StringBuilder();
        string slash = "../";
        if (folderpath.Contains("/client/"))
        {
            slash = "";
        }
        for (int i = 0; i < arrFiles.Count; ++i)
        {
            string path = Convert.ToString(arrFiles[i]);
            string filename = Path.GetFileName(path);
            if (filename.Contains("."))
            {
                string fileext = filename.Substring(filename.LastIndexOf('.')).ToLower();
                string imgsrc = "";
                string deleteimgsrc = "";
                string fullFileUrl = folderpath.Replace("~", "..");
                if (fullFileUrl.EndsWith("/"))
                {
                    fullFileUrl = fullFileUrl + filename;
                }
                else
                {
                    fullFileUrl = fullFileUrl + "/" + filename;
                }
                if (h_FileName.Text == "")
                {
                    h_FileName.Text = filename;
                }
                else
                {
                    h_FileName.Text += "|" + filename;
                }
                if (folderpath.EndsWith("/"))
                {
                    deleteimgsrc = folderpath + filename;
                }
                else
                {
                    deleteimgsrc = folderpath + "/" + filename;
                }

                deleteimgsrc = deleteimgsrc.Replace("..", "~");

                if (fileext == ".jpg" || fileext == ".png" || fileext == ".gif" || fileext == ".bmp")
                {
                    imgsrc = fullFileUrl;// folderpath + "/" + filename;
                }
                else if (fileext == ".mp3" || fileext == ".wav")
                {
                    imgsrc = slash + "images/song-icon.png";
                }
                else if (fileext == ".avi" || fileext == ".wmv" || fileext == ".mov" || fileext == ".mpg" || fileext == ".vob" || fileext == ".3g2")
                {
                    imgsrc = slash + "images/video-icon.png";
                }
                else if (fileext == ".doc" || fileext == ".docx")
                {
                    imgsrc = slash + "images/doc-icon.png";
                }
                else if (fileext == ".txt")
                {
                    imgsrc = slash + "images/txt-icon.png";
                }
                else if (fileext == ".pdf")
                {
                    imgsrc = slash + "images/pdf-icon.png";
                }
                else if (fileext == ".zip")
                {
                    imgsrc = slash + "images/icon/zip.png";
                }
                else if (fileext == ".xls" || fileext == ".xlsx")
                {
                    imgsrc = slash + "images/xl-icon.png";
                }
                else if (fileext == ".ppt")
                {
                    imgsrc = slash + "images/ppt-icon.png";
                }
                else
                {
                    imgsrc = slash + "images/unknown.png";
                }
                string actualFileName = filename;
                if (IsMultiple)
                {
                    int index = filename.IndexOf('_');
                    if (index > 0)
                    {
                        filename = filename.Substring(index + 1);
                    }
                }
                html.Append("<tr><td align='center'><img src=\'" + imgsrc + "' width='50px'/>" +
                    "</td><td><a href='" + fullFileUrl + "' target='_blank'>" + filename + "</a></td>");
                if (_isDetail == false)
                {
                    html.Append("<td><img src='"+slash+"images/delete.png' class='deletefile hand' val='" + deleteimgsrc + "' fn='" + actualFileName + "' title='Delete'></td>");
                }
                html.Append("</tr>");
            }
        }
        ltFiles.Text = html.ToString();
    }


}


   