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
using WebComponent;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

public partial class DbBackup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string backupDate = Common.GetSetting("Last DB Backup");
            if (backupDate != "")
            {
                lblLastBackup.Text = backupDate;
            }
        }
    }
    protected void btnBackup_Click(object sender, EventArgs e)
    {
        string filePath = ConfigurationManager.ConnectionStrings["DbBackupSQLPath"].ConnectionString;
        string strDataBaseName = Common.GetSetting("Db Name");
        string actualFilePath = Server.MapPath("~/upload/DBBackup/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year + ".bak");
        if (filePath.EndsWith("/"))
        {
            filePath = filePath + strDataBaseName + ".bak";
        }
        else
        {
            filePath = filePath + "/"+strDataBaseName + ".bak";
        }
        
        string query = "BACKUP DATABASE "+strDataBaseName+" TO DISK = '" + filePath + "'";
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            if (File.Exists(actualFilePath)) File.Delete(actualFilePath);
            File.Move(filePath, actualFilePath);
            InsertUpdate obj = new InsertUpdate();
            obj.ExecuteQuery("update tbl_Setting set setting_settingvalue=getdate() where setting_settingname='Last DB Backup'");
            lblMessage.Text = "Database backup completed successfully.";
            btnCancel.Text = "OK";
            btnBackup.Visible = false;
            
        }
        catch(Exception ex)
        {
            lblMessage.Text = "Error Occurred while taking database backup.</br>"+ex.Message;
            lblMessage.Visible = true;
        }
        //BackUpAllAttachedFiles();
        //lblMessage.Text=lblMessage.Text + ": All Files Are BackedUp Sucessfully!!!!";
        //lblMessage.Visible = true;
    }
    private void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
    {
        try
        {            
            if (!source.Exists)
            {
                return;
            }
            if (!destination.Exists)
            {
                destination.Create();
            }

            // Copy all files.
            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(destination.FullName,
                    file.Name), true);                
            }
            // Process subdirectories.
            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // Get destination directory.
                string destinationDir = Path.Combine(destination.FullName, dir.Name);
                // Call CopyDirectory() recursively.
                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }
        catch (Exception ex)
        {
          
        }

    }
    private void BackUpAllAttachedFiles()
    {
        DirectoryInfo Sourcedir = new DirectoryInfo(Server.MapPath("../upload"));
        string strDestDir = Server.MapPath("BackupData//Files-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
        //if(Directory.Exists(strDestDir))
        DirectoryInfo Destdir = new DirectoryInfo(strDestDir);
        CopyDirectory(Sourcedir, Destdir);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin.aspx");
    }

}
