using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;

public partial class CP_convert_sp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string fromMainTable = txtMainTableName.Text.Trim();
        string fromSubTable = txtSubTableName.Text.Trim();
        string toMaintable = txtToMainTable.Text.Trim();
        string toSubtable = txtToSubTable.Text.Trim();
        string buttonText = txtButtonText.Text;

        if (fromMainTable == "")
        {
            lblMessage.Text = "Please enter main table";
            return;
        }
        if (toMaintable == "")
        {
            lblMessage.Text = "Please enter to main table";
            return;
        }
        
        string fromModule = GetModuleName(fromMainTable);
        string toModule = GetModuleName(toMaintable);
        string uspName = "usp_Convert" + fromModule.Replace(" ", "") + "To" + toModule;
        bool spExists = DbTable.IsDataExists("select * from sysobjects where name='" + uspName + "'");
        
        string fromMainTablePrefix = fromMainTable.Replace("tbl_", "");
        string toMainTablePrefix = toMaintable.Replace("tbl_", "");
        string fromSubTablePrefix = fromSubTable.Replace("tbl_", "");
        string toSubTablePrefix = toSubtable.Replace("tbl_", "");

        string sourceColumns = "";
        string destColumns = "";

        GetMatchedColumns(fromMainTable, toMaintable, out sourceColumns, out destColumns);
        lblMessage.Text = "";

        if (sourceColumns == "")
        {
            lblMessage.Text = "Source table columns not matching with destination table";
            return;
        }

        string sourceColumns_sub = "";
        string destColumns_sub = "";
        if (fromSubTable != "" && toSubtable != "")
        {
            GetMatchedColumns(fromSubTable, toSubtable, out sourceColumns_sub, out destColumns_sub);
            if (sourceColumns_sub == "")
            {
                lblMessage.Text += "<br/>Source sub table columns not matching with destination sub table";
            }
        }

        destColumns += "," + toMainTablePrefix + "_createdby," + toMainTablePrefix + "_createddate";
        sourceColumns+= ",@CreatedBy,GETDATE()";
        string spScript = "CREATE ";
        if (chkOverwriteStoredProcedure.Checked && spExists)
        {
            spScript = "ALTER ";
        }
        string spForCode = "";
        if (txtCodeColumn.Text.Trim() != "")
        {
            string codeColumn = txtCodeColumn.Text;
            if (!codeColumn.Contains("_"))
            {
                codeColumn = toMainTablePrefix + "_" + codeColumn;
            }
            spForCode = "\tDECLARE @Code AS VARCHAR(50)\n" +
                        "\tSELECT TOP 1 @Code = " + codeColumn + " FROM " + toMaintable + " ORDER BY " + toMainTablePrefix + "_" + toMainTablePrefix + "id DESC\n" +
                        "\tSET @Code = dbo.fn_FindCode(@Code,'" + txtCodePrefix.Text + "')\n\n";
            destColumns = codeColumn + "," + destColumns;
            sourceColumns = "@Code," + sourceColumns;
        }
        spScript += "PROCEDURE " + uspName + "\n(\n\t@ID INT,\n\t@CreatedBy INT,\n\t@NewId INT OUT\n)\nAS\nBEGIN\n\n"+
                    spForCode +
                    "\tINSERT INTO " + toMaintable + "(" + destColumns + ")\n" +
                     "\tSELECT " + sourceColumns + " FROM " + fromMainTable + "\n\tWHERE " + fromMainTablePrefix + "_" + fromMainTablePrefix + "id=@ID\n" +
                     "\tSET @NewId = @@IDENTITY\n\n";
        if (sourceColumns_sub != "")
        {
            destColumns_sub = toSubTablePrefix + "_" + toMainTablePrefix + "id," + destColumns_sub;
            spScript += "\tIF @NewId > 0\n" +
                        "\n\tBEGIN\n\n" +
                        "\t\tINSERT INTO " + toSubtable + "(" + destColumns_sub + ")\n" +
                        "\t\tSELECT @NewId," + sourceColumns_sub + " FROM " + fromSubTable + "\n\t\tWHERE " + fromSubTablePrefix + "_" + fromSubTablePrefix + "id=@ID\n" +
                        "\n\n\tEND\n";

        }
        spScript += "END\n";
        InsertUpdate obj = new InsertUpdate();
        if (obj.ExecuteQuery(spScript))
        {
            lblMessage.Text += "<br/>Stored procedure created and run successfully!, Stored procedure name is " + uspName;
        }
        else
        {
            lblMessage.Text += "<br/>Error occurred while executing stored procedure!, Stored procedure name is " + uspName + ", ERROR : " + obj._error;
        }
        txtStoredProcedure.Text = spScript;

        string designFilePath = Server.MapPath("~/" + fromMainTablePrefix + "/add.aspx");
        string designCode = GlobalUtilities.ReadFile(designFilePath);
        string buttonHtml = "";
        string buttonId = "btnConvert" + fromModule.Replace(" ", "") + "To" + toModule;
        string buttonClickEvent = buttonId + "_Click";

        if (buttonText.Trim() == "" || buttonText == "Convert")
        {
            buttonText = "Convert To " + toModule;
        }
        buttonHtml = "<asp:Button ID=\"" + buttonId + "\" runat=\"server\" visible=\"false\" CssClass=\"button\" Text=\"" + buttonText + "\" OnClick=\"" + buttonClickEvent + "\"></asp:Button>";
        if (designCode.Contains(buttonHtml))
        {
            lblMessage.Text += "<br/>Convert button design code already exists";
        }
        else
        {
            string existsButton="<input type=\"button\" value=\"Edit\" class=\"edit button dpage\"/>";
            if (designCode.Contains(existsButton))
            {
                buttonHtml = existsButton + "\n\t\t\t\t\t\t\t" + buttonHtml;
                designCode = designCode.Replace(existsButton, buttonHtml);
                GlobalUtilities.WriteFile(designFilePath, designCode);
                lblMessage.Text += "<br/>Button design coding added successfully!";
            }
            else
            {
                lblMessage.Text += "<br/><asp:Button ID=\"btnSubmit\" does not exists!, please check the design code";
            }
        }
        string codePath = Server.MapPath("~/" + fromMainTablePrefix + "/add.aspx.cs");
        string code = GlobalUtilities.ReadFile(codePath);
        if (code.Contains(buttonClickEvent))
        {
            lblMessage.Text += "<br/>Onclick event for convert already exists";
        }
        else
        {
            string onclickCode = @"protected void " + buttonClickEvent + "(object sender, EventArgs e)\n" +
                                  "\t{\n" +
                                  "\t\tConvertModuleDAO dao = new ConvertModuleDAO();\n" +
                                  "\t\tint NewId = dao.Convert(\"" + uspName + "\", GetId());\n" +
                                  "\t\tif (NewId == 0)\n" +
                                  "\t\t{\n" +
                                  "\t\t\tlblMessage.Text = \"Error occurred while converting to Invoice!\";\n" +
                                  "\t\t\tlblMessage.Visible = true;\n" +
                                  "\t\t}\n" +
                                  "\t\telse\n" +
                                  "\t\t{\n" +
                                  "\t\t\tResponse.Redirect(\"~/" + toModule.ToLower() + "/add.aspx?id=\" + NewId);\n" +
                                  "\t\t}\n" +
                                  "\t}";

            onclickCode += "\n\tprotected void Page_Load";
            code = code.Replace("protected void Page_Load", onclickCode);
            code = code.Replace("//Populate_START", buttonId + ".Visible = true;\n\t\t\t\t//Populate_START");
            GlobalUtilities.WriteFile(codePath, code);
            lblMessage.Text += "<br/>Onclick code written successfully";
        }
        
        lblMessage.Text += "<br/>Coding for convert generated successfully!";
    }
    private string GetModuleName(string tableName)
    {
        string moduleName = DbTable.GetOneColumnData("tbl_module", "module_modulename", "module_tablename='" + tableName + "'");
        return moduleName;
    }
    private void GetMatchedColumns(string fromTable,string toTable, out string sourceColumns, out string destColumns)
    {
        sourceColumns = "";
        destColumns = "";
        DataTable dttblFrom = DbTable.ExecuteSelect("select * from " + fromTable + " where 1=2");
        DataTable dttblTo = DbTable.ExecuteSelect("select * from " + toTable + " where 1=2");

        string fromMainTablePrefix = fromTable.Replace("tbl_", "");
        string toMainTablePrefix = toTable.Replace("tbl_", "");
        for (int i = 1; i < dttblFrom.Columns.Count; i++)
        {
            string sourceColumn = dttblFrom.Columns[i].ColumnName;
            sourceColumn = sourceColumn.Substring(sourceColumn.IndexOf("_") + 1);
            if (sourceColumn == "createdby" || sourceColumn == "createddate" || sourceColumn == "modifiedby" || sourceColumn == "modifieddate")
            {

            }
            else
            {
                for (int j = 0; j < dttblTo.Columns.Count; j++)
                {
                    string destColumn = dttblTo.Columns[j].ColumnName;
                    destColumn = destColumn.Substring(destColumn.IndexOf("_") + 1);
                    if (sourceColumn == destColumn)
                    {
                        if (sourceColumns == "")
                        {
                            sourceColumns = fromMainTablePrefix + "_" + sourceColumn;
                            destColumns = toMainTablePrefix + "_" + destColumn;
                        }
                        else
                        {
                            sourceColumns += "," + fromMainTablePrefix + "_" + sourceColumn;
                            destColumns += "," + toMainTablePrefix + "_" + destColumn;
                        }
                    }
                }
            }
        }
    }
}
