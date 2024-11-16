using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Formatter
/// </summary>
public static class Formatter
{
    public static string FormatAmount(double dblAmount) 
    {        
        if (dblAmount == 0) return "0.00";
        return String.Format("{0:#.00}", dblAmount);
    }
    public static string FormatAmount(string strAmount)
    {
        if (strAmount.Trim() == "") return "0.00";
        return String.Format("{0:#.00}", Convert.ToDouble(strAmount));
    }
    public static string formatAmountWithCommaSeparator(double Amount)
    {
        try
        {
            return Convert.ToDecimal(Amount).ToString("#,##0.00");
        }
        catch (Exception ex) { return Amount.ToString(); }
    }    
    public static string ConvertToShortDate(object date)
    {
        try
        {
            return String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(date));
        }
        catch (Exception ex) { return Convert.ToString(date); }
    }
    public static string ConvertToMonthDate(object date)
    {
        try
        {
            return String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(date));
        }
        catch (Exception ex)
        {
            return Convert.ToString(date);
        }
    }

    ///<param name="mForMonth_dForday_dmForDayAndMonth">
    ///m for Month 01-Jan - 2014
    ///d for Day Mon - 01-2014
    ///dm for dayAndMonth Mon-Jan-2014
    ///</param>
    ///
    ///</summary>
    ///
    public static string ConvertDateAsStringFormat(DateTime DateVal, string mForMonth_dForday_dmForDayAndMonth)
    {
        string format = "";
        try
        {            
            if (mForMonth_dForday_dmForDayAndMonth == "m")
            {
                format = "dd-MMM-yyyy";    // 01-Jan-2014       
            }
            else if (mForMonth_dForday_dmForDayAndMonth == "d")
            {
                format = "ddd-MM-yyyy";   // Sun-01-2014
            }
            else if (mForMonth_dForday_dmForDayAndMonth == "dm")
            {
                format = "ddd-MMM-yyyy";  // Sun-Jan-2014  
            }
           return DateVal.ToString(format);
        }
        catch (Exception ex) { }
        return (Convert.ToString(DateVal));
    }
    ///<param name="mForMonth_dForday_dmForDayAndMonth">
    ///m for Month 01-Jan - 2014
    ///d for Day Mon - 01-2014
    ///dm for dayAndMonth Mon-Jan-2014
    ///</param>
    ///<summary>TEST</summary>

    public static string ConvertDateAsStringFormat(object date, string mForMonth_dForday_dmForDayAndMonth)
    {
        string format = "";
        if (mForMonth_dForday_dmForDayAndMonth == "m")
        {
            format = "{0:dd-MMM-yyyy}";    // 01-Jan-2014       
        }
        else if (mForMonth_dForday_dmForDayAndMonth == "d")
        {
            format = "{0:ddd-MM-yyyy}";   // Sun-01-2014
        }
        else if (mForMonth_dForday_dmForDayAndMonth == "dm")
        {
            format = "{0:ddd-MMM-yyyy}";  // Sun-Jan-2014  
        }
        try
        {
            String.Format(format, Convert.ToDateTime(date));
        }
        catch (Exception ex) { }
        return Convert.ToString(date);
    }
    public static string ConvertToMonthDateTime(object date)
    {
        try
        {
            return String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(date));
        }
        catch (Exception ex) { return Convert.ToString(date); }
    }

}
