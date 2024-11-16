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
using System.Text;
using System.Xml;
using System.IO;

public partial class utilities_print_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblCompanyName.Text = CustomSettings.CompanyName;
            lblDate.Text = GlobalUtilities.GetCurrentDateDDMMYYYY();
            lblTitle.Text = Request.QueryString["t"];
            BindData();
        }
    }
    private void BindData()
    {
        string query = GlobalUtilities.ConvertToString(Session["s_gridquery"]);
        if (query == "") return;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        //string module = Request.QueryString["m"];
        string reportModule = Request.QueryString["rm"];
        ltData.Text = BindGridData(dttbl, reportModule);
    }
    private string BindGridData(DataTable Data, string Module)
    {
        GridSettings gridSettings = new GridSettings();
        gridSettings.BindXml(Module);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' style='border:solid 1px #000;' cellspacing=0 class='grid-report'>");
        html.Append("<tr class='rowheader'>");
        html.Append("<td class='bold' style='border-right:solid 1px #000;border-bottom:solid 1px #000;'>Sr. No.</td>");
        for (int i = 0; i < gridSettings.arrColumns_HeaderText.Length; i++)
        {
            html.Append("<td class='bold' style='border-right:solid 1px #000;border-bottom:solid 1px #000;'>" + gridSettings.arrColumns_HeaderText.GetValue(i).ToString() + "</td>");
        }
        html.Append("</tr>");
        for (int i = 0; i < Data.Rows.Count; i++)
        {
            int row = 0;
            int prevRow = 0;
            int id = 0;
            bool showColumn = true;
            html.Append("<tr><td style='border-right:solid 1px #000;border-bottom:solid 1px #888;'>" + (i + 1) + "</td>");
            for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
            {
                string colName = gridSettings.xmlGridColumns[j].InnerText;
                XmlNode xmlNode = gridSettings.xmlGridColumns[j];

                if (showColumn)
                {
                    if (colName.Contains(" "))
                    {
                        colName = colName.Substring(colName.LastIndexOf(" ") + 1);
                    }
                    if (colName.Contains("."))
                    {
                        colName = colName.Substring(colName.IndexOf(".") + 1);
                    }
                    string headerText = gridSettings.xmlGridHeaders[j].InnerText;
                    string columnWidth = gridSettings.xmlGridColumnWidth[j].InnerText;
                    string val = Convert.ToString(Data.Rows[i][colName]);
                    string dataFormat = gridSettings.xmlGridColumnFormat[j].InnerText.ToLower();
                    string width = "";
                    string css = "";
                    string idval = "";
                    if (val == "01-01-2000 12:00:00 AM" || val == "01-01-1900 12:00:00 AM")
                    {
                        val = "";
                    }

                    if (dataFormat == "Amount" || dataFormat == "Quantity")
                    {
                        css = " right ";
                    }
                    else if (dataFormat == "date")
                    {
                        if (val.Contains("1900") || val.Contains("2000") || val == "")
                        {
                            val = "";
                        }
                        else
                        {
                            val = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(val));
                        }
                    }
                    else if (dataFormat == "datetime")
                    {
                        if (val.Contains("1900") || val.Contains("2000") || val == "")
                        {
                            val = "";
                        }
                        else
                        {
                            val = GlobalUtilities.ConvertToDateTime(val);
                        }
                    }
                    if (colName.EndsWith("cramount") || colName.EndsWith("dramount"))
                    {
                        if (GlobalUtilities.ConvertToDouble(val) == 0) val = "";
                    }
                    if (columnWidth != "")
                    {
                        width = " width='" + columnWidth + "'";
                    }
                    if (dataFormat == "image")
                    {
                        string imgPath = GlobalUtilities.ConvertToString(gridSettings.xmlGridColumnFormat[j].Attributes["imgpath"].Value);

                        if (imgPath != "")
                        {
                            string fileName = "default.jpg";
                            if (val.Trim() != "")
                            {
                                if (File.Exists(Server.MapPath("../" + imgPath + "/" + id + "." + val.Trim())))
                                {
                                    fileName = id + "." + val.Trim();
                                }
                            }
                            val = "<img src='../" + imgPath + "/" + fileName + "' height='30'/>";
                        }
                    }
                    if (val == "") val = "&nbsp;";
                    if (j == 0) css = " idval";
                    if (css != "") css = " class='" + css + "'";
                    html.Append("<td style='border-right:solid 1px #000;border-bottom:solid 1px #888;' " + width + css + idval + ">" + val + "</td>");

                    prevRow = row;
                }
            }
            if (gridSettings.GridType != "list")
            {
                html.Append("</tr>");
            }
        }
        html.Append("</table>");
        return html.ToString();
    }
}