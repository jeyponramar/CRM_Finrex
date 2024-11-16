using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using WebComponent;
using System.Collections;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for ExportExposurePortal
/// </summary>
public class FinstationPortal
{
    public void BindRate(int sectionId, Literal lt, string columns)
    {
        BindRate(sectionId, lt, columns, "");
    }
    public void BindRate(int sectionId, Literal lt, string columns, string excludeRows)
    {
        BindRate(sectionId, lt, columns, excludeRows, "");
    }
    public void BindRate(int sectionId, Literal lt, string columns, string excludeRows, string excludeCols)
    {
        BindRate(sectionId, lt, columns, excludeRows, excludeCols, 0);
    }
    public void BindRate(int sectionId, Literal lt, string columns, string excludeRows, string excludeCols, int showMoreIndex)
    {
        BindRate(sectionId, lt, columns, excludeRows, excludeCols, showMoreIndex, "");
    }
    public void BindRate(int sectionId, Literal lt, string columns, string excludeRows, string excludeCols, int showMoreIndex, string includeRows)
    {
        BindRate(sectionId, lt, columns, excludeRows, excludeCols, showMoreIndex, includeRows, "");
    }
    public void BindRate(int sectionId, Literal lt, string columns, string excludeRows, string excludeCols, int showMoreIndex, string includeRows, string includeCurrencies)
    {
        BindRate(sectionId, lt, columns, excludeRows, excludeCols, showMoreIndex, includeRows, includeCurrencies, false);
    }
    public void BindRate(int sectionId, Literal lt, string columns, string excludeRows, string excludeCols, int showMoreIndex, string includeRows, string includeCurrencies,
        bool isShowCountry)
    {
        string html = GetRateHtml(sectionId, columns, excludeRows, excludeCols, showMoreIndex, includeRows, includeCurrencies, isShowCountry);
        lt.Text = html;
    }
    public DataTable GetRateDataTable(int sectionId)
    {
        string query = "select * from tbl_liveratesection where liveratesection_liveratesectionid=" + sectionId;
        DataRow drSection = DbTable.ExecuteSelectRow(query);
        string rows = GlobalUtilities.ConvertToString(drSection["liveratesection_rows"]);
        string cols = GlobalUtilities.ConvertToString(drSection["liveratesection_columns"]);
        int colCount = 0;
        bool isint = Int32.TryParse(cols, out colCount);
        if (!isint)
        {
            colCount = cols.Split(',').Length;
        }
        DataTable dttbldata = new DataTable();
        for (int i = 0; i < colCount + 1; i++)
        {
            dttbldata.Columns.Add("col" + i);
        }
        query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                liverate_calculation,liverate_istick,liverate_liveratesectionid,liverate_decimalplaces 
                from tbl_liverate
                WHERE liverate_liveratesectionid=" + sectionId;
        query += " order by liverate_row,liverate_column";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        Array arrRows = rows.Split(',');
        dttbl = CorrectRateData(dttbl, sectionId);
        dttbl = Finstation.CorrectLiveRateValues(dttbl);
        int rowCounter = 0;
        int prevRow = 0;
        int startIndex = 0;
        for (int i = 0; i < arrRows.Length; i++)
        {
            string liverateName = arrRows.GetValue(i).ToString();
            //liverateName = liverateName.Replace(" ", "&nbsp;");

            for (int k = startIndex; k < dttbl.Rows.Count; k++)
            {
                int r = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]);
                if (prevRow != r)
                {
                    rowCounter = r;
                    prevRow = r;
                    startIndex = k;
                    break;
                }
            }
            //bool isexcluded = false;
            //if (excludeRows != "")
            //{
            //    for (int k = 0; k < arrExcludeRow.Length; k++)
            //    {
            //        if (Convert.ToInt32(arrExcludeRow.GetValue(k)) == rowCounter)
            //        {
            //            isexcluded = true;
            //            break;
            //        }
            //    }
            //}
            //if (isexcluded) continue;
            DataRow dr = dttbldata.NewRow();
            dr[0] = liverateName;
            for (int j = 0; j < colCount; j++)
            {
                string currentRate = "";
                string prevRate = "";
                int liverateId = 0;
                string rtdCode = "";
                string calculation = "";
                int rowNo = 0;
                int colNo = 0;
                double dblCurrentRate = 0;
                double dblPrevRate = 0;
                bool isnumber = false;
                int istick = 0;
                for (int k = startIndex; k < dttbl.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]) == rowCounter && //(i + 1) &&
                        GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]) == (j + 1))
                    {
                        currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_currentrate"]);
                        prevRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_prevrate"]);
                        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_liverateid"]);
                        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_rtdcode"]);
                        calculation = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_calculation"]);
                        rowNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]);
                        colNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]);
                        istick = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_istick"]);
                        isnumber = Double.TryParse(currentRate, out dblCurrentRate);
                        Double.TryParse(prevRate, out dblPrevRate);
                        break;
                    }
                }
                dr[j + 1] = currentRate;
            }
            dttbldata.Rows.Add(dr);
        }

        return dttbldata;
    }
    public string GetRateHtml(int sectionId, string columns, string excludeRows)
    {
        return GetRateHtml(sectionId, columns, excludeRows, "", 0);
    }
    public string GetRateHtml(int sectionId, string columns, string excludeRows, string excludeCols)
    {
        return GetRateHtml(sectionId, columns, excludeRows, excludeCols, 0);
    }
    public string GetRateHtml(int sectionId, string columns, string excludeRows, string excludeCols, int showMoreIndex)
    {
        return GetRateHtml(sectionId, columns, excludeRows, excludeCols, showMoreIndex, "","", false);
    }
    public string GetRateHtml(int sectionId, string columns, string excludeRows, string excludeCols, int showMoreIndex, 
        string includeRows, string includeCurrencies, bool isShowCountry)
    {
        string query = "select * from tbl_liveratesection where liveratesection_liveratesectionid=" + sectionId;
        DataRow drSection = DbTable.ExecuteSelectRow(query);
        string rows = GlobalUtilities.ConvertToString(drSection["liveratesection_rows"]);
        string cols = GlobalUtilities.ConvertToString(drSection["liveratesection_columns"]);
        string sectionCode = GlobalUtilities.ConvertToString(drSection["liveratesection_code"]);
        //if (extrarows != "") rows = rows + "," + rows;
        //DataTable dttblOtherData = new DataTable();
        if (columns != "") cols = columns;
        //if (sectionId == 57)
        //{
        //    cols += ",Open,High,Low";
        //    dttblOtherData = GetRateDataTable(61);
        //}
        string url = HttpContext.Current.Request.Url.ToString().ToLower();
        bool isHomePage = false;
        if (url.EndsWith("/index.aspx")) isHomePage = true;
        Array arrCols = cols.Split(',');
        int colCount = arrCols.Length;
        int tempCount = 0;
        bool isColFound = true;
        Array arrExcludeRow = excludeRows.Split(',');
        Array arrExcludeCol = excludeCols.Split(',');
        Array arrIncludeRows = includeRows.Split(',');
        if (Int32.TryParse(cols, out tempCount))
        {
            colCount = tempCount;
            isColFound = false;
        }
        if (showMoreIndex == 0) showMoreIndex = -1;
        StringBuilder html = new StringBuilder();
        //html.Append("<table class='repeater' border=1 style='width:100%;height:100%;'>");
        html.Append("<table class='repeater' border=1 style='width:100%;'>");
        if (isColFound)
        {
            if (sectionId == 50)
            {
                html.Append("<tr class='title'>");
            }
            else
            {
                html.Append("<tr class='repeater-header'>");
            }
            if (!columns.StartsWith("~"))
            {
                if (isHomePage && (sectionId == 1 || sectionId == 2 || sectionId == 50))
                {
                    int currencyTypeId = sectionId;
                    if (sectionId == 50) currencyTypeId = 3;
                    if (sectionId == 50)
                    {
                        html.Append("<td style='white-space:nowrap'><img src='images/plus-add-white.png' title='Configure Currencies' width='15' class='hand jq-config-liverate' ctype='" + currencyTypeId + "'/> OFFSHORE</td>");
                    }
                    else
                    {
                        html.Append("<td><img src='images/plus-add-white.png' title='Configure Currencies' width='15' class='hand jq-config-liverate' ctype='" + currencyTypeId + "'/></td>");
                    }
                }
                else
                {
                    html.Append("<td>&nbsp;</td>");
                }
                //colCount--;
            }
            if (isShowCountry)
            {
                html.Append("<td align='center' style='width:130px;'>Country</td>");
            }
            int colLength = arrCols.Length;
            
            //if (sectionId == 1) colLength = 6;
            for (int i = 0; i < colLength; i++)
            {
                string name = arrCols.GetValue(i).ToString().Replace("~", "");
                //if ((sectionId == 1 && i == 5) || (sectionId == 61 && i == 2 && HttpContext.Current.Request.Url.ToString().ToLower().EndsWith("index.aspx")))
                if(showMoreIndex == i + 1)
                {
                    //arrow_expand_right.png
                    html.Append("<td align='center'>" + name + "&nbsp;<img src='images/arrow_expand_right.png' target='jq-liverate-more-data-" + sectionId + "' class='jq-liverate-expand hand' title='Show more'/></td>");
                }
                //else if ((sectionId == 1 && i > 5) || (sectionId == 61 && i > 2))
                else if (showMoreIndex > 0 && i + 1 > showMoreIndex)
                {
                    html.Append("<td class='hidden jq-liverate-more-data-" + sectionId + "' align='center'>" + name + "</td>");
                }
                else
                {
                    html.Append("<td align='center'>" + name + "</td>");
                }
            }
            html.Append("</tr>");
        }
        string countryJoin = "";
        string sectionIds = "=" + sectionId.ToString();
        string rowcolumn = "liverate_row";
        string sortby = "liverate_row";
        if (sectionId <= 3)
        {
            sortby = rowcolumn = "currencymaster_currencymasterid";
        }
        if (!isHomePage)
        {
            if (isShowCountry)
            {
                sortby = "country_country";
                rowcolumn = "country_countryid";
            }
        }
        if (sectionId == 2)
        {
            sectionIds = " in(2,3)";
        }
        if (isShowCountry)
        {
            countryJoin = "left join tbl_country on country_countryid=currencymaster_countryid";
        }
        query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                liverate_calculation,liverate_istick,liverate_liveratesectionid,liverate_decimalplaces,
                currencymaster_currencymasterid,currencymaster_currency,liverate_isapirate,liverate_isexcelupdate";
        if (isShowCountry) query += ",country_countryid,country_country";
        query += @"
                from tbl_liverate
                left join tbl_currencymaster on currencymaster_currencymasterid=liverate_currencymasterid
                " + countryJoin + @"
                WHERE liverate_liveratesectionid" + sectionIds;
        if (includeCurrencies != "") query += " and currencymaster_currencymasterid in(" + includeCurrencies + ")";
        //if (sectionId == 50)
        //{
        //    query += @" and ((liverate_currencymasterid = 1 AND liverate_column in(1,5,6)) OR (liverate_currencymasterid > 1 AND liverate_column in(1,4,5)))";
        //}
        query += " order by " + sortby + ",liverate_column";
        
        DataTable dttbl = DbTable.ExecuteSelect(query);
        dttbl = CorrectRateData(dttbl, sectionId);
        dttbl = Finstation.CorrectLiveRateValues(dttbl);
        int rowCounter = 0;
        int prevRow = 0;
        int startIndex = 0;
        bool isCurrencyBasedData = false;
        if (sectionId <= 3 || sectionId == 50) isCurrencyBasedData = true;
        ArrayList arrcurrency = new ArrayList();
        ArrayList arrcurrencyid = new ArrayList();
        ArrayList arrcountry = new ArrayList();
        if (isCurrencyBasedData)
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                int row1 = GlobalUtilities.ConvertToInt(dttbl.Rows[i][rowcolumn]);
                if (row1 != prevRow)
                {
                    string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["currencymaster_currency"]);
                    int currencyid = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["currencymaster_currencymasterid"]);
                    string country = "";
                    if (isShowCountry) country = GlobalUtilities.ConvertToString(dttbl.Rows[i]["country_country"]);
                    arrcurrency.Add(currency);
                    arrcountry.Add(country);
                    arrcurrencyid.Add(currencyid);
                }
                prevRow = row1;
            }
        }
        else
        {
            Array arrRows = rows.Split(',');
            for (int i = 0; i < arrRows.Length; i++)
            {
                arrcurrency.Add(arrRows.GetValue(i));
            }
        }
        for (int i = 0; i < arrcurrency.Count; i++)
        {
            int currencyId = 0;
            string liverateName = "";
            string country = "";
            if (isCurrencyBasedData)
            {
                currencyId = Convert.ToInt32(arrcurrencyid[i]);
                liverateName = arrcurrency[i].ToString();
                country = arrcountry[i].ToString();

                if (sectionId == 50)
                {
                    if (currencyId == 1)//USDINR
                    {
                        excludeCols = "2,3,4";
                        arrExcludeCol = excludeCols.Split(',');
                    }
                    else
                    {
                        excludeCols = "2,3";
                        arrExcludeCol = excludeCols.Split(',');
                    }
                }
            }
            else
            {
                liverateName = arrcurrency[i].ToString().Replace(" ", "&nbsp;");
            }
            //if (liverateName == "USDINR")
            //{
            //    liverateName = liverateName;
            //}
            for (int k = startIndex; k < dttbl.Rows.Count; k++)
            {
                int r = GlobalUtilities.ConvertToInt(dttbl.Rows[k][rowcolumn]);
                if (prevRow != r)
                {
                    rowCounter = r;
                    prevRow = r;
                    startIndex = k;
                    break;
                }
            }
            bool isexcluded = false;
            if (excludeRows != "")
            {
                for (int k = 0; k < arrExcludeRow.Length; k++)
                {
                    if (Convert.ToInt32(arrExcludeRow.GetValue(k)) == rowCounter)
                    {
                        isexcluded = true;
                        break;
                    }
                }
            }
            if (isexcluded == false && includeRows != "")
            {
                isexcluded = true;
                for (int k = 0; k < arrIncludeRows.Length; k++)
                {
                    if (Convert.ToInt32(arrIncludeRows.GetValue(k)) == rowCounter)
                    {
                        isexcluded = false;
                        break;
                    }
                }
            }
            if (isexcluded) continue;
            string leftcss = "";
            if (sectionId == 1 && i < 4 && isHomePage)
            {
                leftcss = " liverate-chart-icon";
            }
            //if (sectionId == 4 || sectionId == 60)
            //{
            //    currencyId = Finstation.SaveCurrency(liverateName, Enum_CurrencyType.IndicesAndCommodities);
            //}
            html.Append("<tr><td class='repeater-header-left" + leftcss + "' cid='" + currencyId + "'>" + liverateName + "</td>");
            if (isShowCountry)
            {
                html.Append("<td style='font-size:14px;padding-left:5px;'>" + country + "</td>");
            }
            int colCounter = 1;
            for (int j = 0; j < colCount; j++)
            {
                string currentRate = "";
                string prevRate = "";
                int liverateId = 0;
                string rtdCode = "";
                string calculation = "";
                int rowNo = 0;
                int colNo = 0;
                double dblCurrentRate = 0;
                double dblPrevRate = 0;
                bool isnumber = false;
                int istick = 0;
                bool isexcludedcol = false;
                bool isapirate = false;
                bool isexcelrate = false;
                if (excludeCols != "")
                {
                    //isexcludedcol = false;
                    //for (int k = 0; k < arrExcludeCol.Length; k++)
                    //{
                    //    if (Convert.ToInt32(arrExcludeCol.GetValue(k)) == colCounter)
                    //    {
                    //        isexcludedcol = true;
                    //        colCounter++;
                    //        break;
                    //    }
                    //}
                    //if (isexcludedcol) continue;
                    isexcludedcol = true;
                    while (isexcludedcol)
                    {
                        isexcludedcol = false;
                        for (int k = 0; k < arrExcludeCol.Length; k++)
                        {
                            if (Convert.ToInt32(arrExcludeCol.GetValue(k)) == colCounter)
                            {
                                isexcludedcol = true;
                                colCounter++;
                                break;
                            }
                        }
                        if (!isexcludedcol) break;
                        //if (colCounter > colCount) break;
                    }
                }
                for (int k = startIndex; k < dttbl.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttbl.Rows[k][rowcolumn]) == rowCounter && //(i + 1) &&
                        GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]) == colCounter)
                    {
                        currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_currentrate"]);
                        prevRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_prevrate"]);
                        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_liverateid"]);
                        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_rtdcode"]);
                        calculation = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_calculation"]);
                        rowNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k][rowcolumn]);
                        colNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]);
                        istick = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_istick"]);
                        isapirate = GlobalUtilities.ConvertToBool(dttbl.Rows[k]["liverate_isapirate"]);
                        isexcelrate = GlobalUtilities.ConvertToBool(dttbl.Rows[k]["liverate_isexcelupdate"]);
                        isnumber = Double.TryParse(currentRate, out dblCurrentRate);
                        Double.TryParse(prevRate, out dblPrevRate);
                        break;
                    }
                }
                //if (sectionId == 4 || sectionId == 60)
                //{
                //    Finstation.UpdateLiverateCurrency(liverateId, currencyId);
                //}
                colCounter++;
                string cls = "";
                string calc = "";
                string code = sectionCode + "_" + rowNo + "_" + colNo;

                if (isnumber && istick == 1)
                {
                    if (dblCurrentRate > dblPrevRate)
                    {
                        cls = "rate-up";
                    }
                    else
                    {
                        cls = "rate-down";
                    }
                }
                if (calculation != "")
                {
                    cls += " calc";
                    calc = " calc='" + calculation + "'";
                }
                if (calculation == "" || calculation.Contains("self"))
                {
                    if (isapirate)
                    {
                        cls += " apirate";
                    }
                    else if (isexcelrate)
                    {
                        cls += " xlrate";
                    }
                }
                cls += " " + code;

                string tdcss = "";
                //if ((sectionId == 1 && j > 5) || (sectionId == 61 && j > 2 && HttpContext.Current.Request.Url.ToString().ToLower().EndsWith("index.aspx")))
                if (showMoreIndex > 0 && j >= showMoreIndex)
                {
                    tdcss = "hidden jq-liverate-more-data-" + sectionId;
                }
                if (sectionId == 5 && liverateId == 181)
                {
                    html.Append("<td class='rate'>CASH/SPOT</td>");
                }
                else if (rtdCode != "")
                {
                    string alertattr = "";
                    if (liverateId == 1 || liverateId == 2 || liverateId == 10 || liverateId == 11 || liverateId == 19 || liverateId == 20
                         || liverateId == 28 || liverateId == 29)
                    {
                        cls += " set-alert";
                        alertattr = " title='Set live rate alert'";
                    }
                    html.Append("<td class='" + tdcss + "'><div sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' col='"+colNo+"' class='" + cls + " rate liverate'" + alertattr + " rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "' istick='" + istick + "' prate='" + dblPrevRate + "' rate='" + currentRate + "'>" + currentRate + "</div></td>");
                }
                else
                {
                    if (calculation == "")
                    {
                        html.Append("<td sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' col='" + colNo + "' rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "' class='liverate " + tdcss + "' istick='" + istick + "' rate='" + currentRate + "'>" + currentRate + "</td>");
                    }
                    else
                    {
                        html.Append("<td><div sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' col='" + colNo + "' class='calc liverate " + tdcss + "' rid='" + liverateId + "' rc='" + rtdCode + "' calc='" + calculation + "' c='" + code + "' istick='" + istick + "'></div></td>");
                    }
                }
                //if (sectionId == 1 || sectionId == 2 || sectionId == 3)
                //{
                //    int currencyTypeId = sectionId;
                //    query = "if not exists(select 1 from tbl_currencymaster where currencymaster_currency='" + liverateName +
                //            "') insert into tbl_currencymaster(currencymaster_currency,currencymaster_currencytypeid) values('" + liverateName + "'," + currencyTypeId + ")";
                //    DbTable.ExecuteQuery(query);
                //    query = "update tbl_liverate set liverate_currencymasterid=(select currencymaster_currencymasterid from tbl_currencymaster where currencymaster_currency='" +
                //                liverateName + "') where liverate_liverateid=" + liverateId;
                //    DbTable.ExecuteQuery(query);
                //}
            }
            
            html.Append("</tr>");
        }

        html.Append("</table>");

        return html.ToString();
    }
    
    public static DataTable CorrectRateData(DataTable dttbl, int sectionId)
    {
        if (sectionId == 5 || sectionId == 51 || sectionId == 54 //monthend
            || sectionId == 27 || sectionId == 8 || sectionId == 11 || sectionId == 14 || sectionId == 17 || sectionId == 20 || sectionId == 23)//monthwise
        {
            int row = 0;
            bool isHistory = false;
            if (dttbl.Columns.Contains("dailyhistoricalliverate_currentrate"))
            {
                isHistory = true;
            }
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                if (GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_row"]) == "2")
                {
                    string rate = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
                    if (isHistory)
                    {
                        rate = GlobalUtilities.ConvertToString(dttbl.Rows[i]["dailyhistoricalliverate_currentrate"]);
                    }
                    if (rate.Contains("-"))
                    {
                        if (row == 0)
                        {
                            if (sectionId == 5 || sectionId == 51 || sectionId == 54) //monthend
                            {
                                dttbl.Rows[row]["liverate_currentrate"] = "CASH/SPOT";
                            }
                            else
                            {
                                dttbl.Rows[row]["liverate_currentrate"] = "ON";
                            }
                        }
                        else
                        {
                            if (sectionId == 5 || sectionId == 51 || sectionId == 54) //monthend
                            {
                                Array arr = rate.Split('-');
                                rate = arr.GetValue(1).ToString().ToUpper() + "-" + arr.GetValue(2).ToString();
                                dttbl.Rows[row]["liverate_currentrate"] = rate;//dttbl.Rows[i]["liverate_liverateid"];
                            }
                            else
                            {
                                dttbl.Rows[row]["liverate_currentrate"] = row + "M";
                            }
                        }
                        if (isHistory)
                        {
                            dttbl.Rows[row]["dailyhistoricalliverate_currentrate"] = dttbl.Rows[row]["liverate_currentrate"];
                        }
                        row++;
                    }
                }
                else
                {
                    continue;
                }

            }
        }
        return dttbl;
    }
    //public string GetRateHtml_History(int sectionId, string columns, string excludeRows, string date)
    //{
    //    return GetRateHtml_History(sectionId, columns, excludeRows, date, 0);
    //}
    public string GetRateHtml_History(int sectionId, string columns, string excludeRows, string date)
    {
        string query = "select * from tbl_liveratesection where liveratesection_liveratesectionid=" + sectionId;
        DataRow drSection = DbTable.ExecuteSelectRow(query);
        string rows = GlobalUtilities.ConvertToString(drSection["liveratesection_rows"]);
        string cols = GlobalUtilities.ConvertToString(drSection["liveratesection_columns"]);
        string sectionCode = GlobalUtilities.ConvertToString(drSection["liveratesection_code"]);
        if (cols == "61") cols = "13";
        //if (extrarows != "") rows = rows + "," + rows;
        if (columns != "") cols = columns;

        Array arrCols = cols.Split(',');
        int colCount = arrCols.Length;
        int tempCount = 0;
        bool isColFound = true;
        Array arrExcludeRow = excludeRows.Split(',');
        if (Int32.TryParse(cols, out tempCount))
        {
            colCount = tempCount;
            isColFound = false;
        }
        StringBuilder html = new StringBuilder();
        html.Append("<table class='repeater' border=1 style='width:100%;height:100%;'>");
        if (isColFound)
        {
            html.Append("<tr class='repeater-header'>");
            if (!columns.StartsWith("~"))
            {
                html.Append("<td>&nbsp;</td>");
                //colCount--;
            }

            int colLength = arrCols.Length;
            //if (sectionId == 1) colLength = 6;
            for (int i = 0; i < colLength; i++)
            {
                string name = arrCols.GetValue(i).ToString().Replace("~", "");
                if (sectionId == 1 && i == 5)
                {
                    //arrow_expand_right.png
                    html.Append("<td align='center'>" + name + "&nbsp;<img src='images/arrow_expand_right.png' class='jq-liverate-expand hand' title='Show more'/></td>");
                }
                else if (sectionId == 1 && i > 5)
                {
                    html.Append("<td class='jq-liverate-more-rate' align='center'>" + name + "</td>");
                }
                else
                {
                    html.Append("<td align='center'>" + name + "</td>");
                }
            }
            html.Append("</tr>");
        }
        date = global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(date));
        query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                liverate_calculation,liverate_istick,dailyhistoricalliverate_currentrate,liverate_decimalplaces 
                from tbl_liverate
                join tbl_dailyhistoricalliverate on dailyhistoricalliverate_liverateid=liverate_liverateid
                WHERE liverate_liveratesectionid=" + sectionId + " and cast(dailyhistoricalliverate_date as date)=cast('" + date + "' as date)";
        query += " and isnull(liverate_isfuturerate,0)=0";
        query += " order by liverate_row";
        Array arrRows = rows.Split(',');
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (dttbl.Rows.Count == 0)
        {
            return "";
        }
        dttbl = CorrectRateData(dttbl, sectionId);
        dttbl = Finstation.CorrectLiveRateValues(dttbl);

        int rowCounter = 0;
        int prevRow = 0;
        int startIndex = 0;
        for (int i = 0; i < arrRows.Length; i++)
        {
            string liverateName = arrRows.GetValue(i).ToString();
            liverateName = liverateName.Replace(" ", "&nbsp;");

            for (int k = startIndex; k < dttbl.Rows.Count; k++)
            {
                int r = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]);
                if (prevRow != r)
                {
                    rowCounter = r;
                    prevRow = r;
                    startIndex = k;
                    break;
                }
            }
            bool isexcluded = false;
            if (excludeRows != "")
            {
                for (int k = 0; k < arrExcludeRow.Length; k++)
                {
                    if (Convert.ToInt32(arrExcludeRow.GetValue(k)) == rowCounter)
                    {
                        isexcluded = true;
                        break;
                    }
                }
            }
            if (isexcluded) continue;
            html.Append("<tr><td class='repeater-header-left'>" + liverateName + "</td>");
            for (int j = 0; j < colCount; j++)
            {
                string currentRate = "";
                string prevRate = "";
                int liverateId = 0;
                string rtdCode = "";
                string calculation = "";
                int rowNo = 0;
                int colNo = 0;
                double dblCurrentRate = 0;
                double dblPrevRate = 0;
                bool isnumber = false;
                int istick = 0;
                for (int k = startIndex; k < dttbl.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]) == rowCounter && //(i + 1) &&
                        GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]) == (j + 1))
                    {
                        currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["dailyhistoricalliverate_currentrate"]);
                        prevRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["dailyhistoricalliverate_currentrate"]);
                        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_liverateid"]);
                        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_rtdcode"]);
                        calculation = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_calculation"]);
                        rowNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]);
                        colNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]);
                        istick = 0;// GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_istick"]);
                        isnumber = Double.TryParse(currentRate, out dblCurrentRate);
                        Double.TryParse(prevRate, out dblPrevRate);
                        break;
                    }
                }
                double d = 0;
                if (calculation == "self/100")
                {
                    if (double.TryParse(currentRate, out d))
                    {
                        currentRate = Convert.ToString(Convert.ToDouble(currentRate) / 100);
                    }
                }
                else if (calculation == "self/1000")
                {
                    if (double.TryParse(currentRate, out d))
                    {
                        currentRate = Convert.ToString(Convert.ToDouble(currentRate) / 1000);
                    }
                }
                //fix for Annualised Premium % showing NaN in history
                calculation = calculation.Replace("#USDINR[1,3]", "#USDINRPremiumPaisaMonthEnd[2,1]");
                calculation = calculation.Replace("#EURINR[1,3]", "#EURINRPremiumPaisaMonthEnd[2,1]");
                calculation = calculation.Replace("#GBPINR[1,3]", "#GBPINRPremiumPaisaMonthEnd[2,1]");
                
                //if (divFactor > 0)
                //{
                //    double d = 0;
                //    if (double.TryParse(currentRate, out d))
                //    {
                //        currentRate = Convert.ToString(Convert.ToDouble(currentRate) / divFactor);
                //    }
                //}
                string cls = "";
                string calc = "";
                string code = sectionCode + "_" + rowNo + "_" + colNo;

                if (isnumber && istick == 1)
                {
                    if (dblCurrentRate > dblPrevRate)
                    {
                        cls = "rate-up";
                    }
                    else
                    {
                        cls = "rate-down";
                    }
                }
                if (calculation != "")
                {
                    cls += " calc";
                    calc = " calc='" + calculation + "'";
                }
                cls += " " + code;

                string tdcss = "";
                if (sectionId == 1 && j > 5)
                {
                    tdcss = "jq-liverate-more-rate";
                }
                if (sectionId == 5 && liverateId == 181)
                {
                    html.Append("<td class='rate'>CASH/SPOT</td>");
                }
                else if (rtdCode != "")
                {
                    string alertattr = "";
                    if (liverateId == 1 || liverateId == 2 || liverateId == 10 || liverateId == 11 || liverateId == 19 || liverateId == 20
                         || liverateId == 28 || liverateId == 29)
                    {
                        cls += " set-alert";
                        alertattr = " title='Set live rate alert'";
                    }
                    html.Append("<td class='" + tdcss + "'><div sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' class='" + cls + " rate'" + alertattr + " rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "' istick='" + istick + "' prate='" + dblPrevRate + "'>" + currentRate + "</div></td>");
                }
                else
                {
                    if (calculation == "")
                    {
                        html.Append("<td sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "' class='liverate " + tdcss + "' istick='" + istick + "'>" + currentRate + "</td>");
                    }
                    else
                    {
                        html.Append("<td><div sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' class='calc " + tdcss + "' rid='" + liverateId + "' rc='" + rtdCode + "' calc='" + calculation + "' c='" + code + "' istick='" + istick + "'></div></td>");
                    }
                }
            }
            html.Append("</tr>");
        }

        html.Append("</table>");

        return html.ToString();
    }
}