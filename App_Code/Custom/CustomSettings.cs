using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebComponent;

/// <summary>
/// Summary description for CustomSettings
/// </summary>
public class CustomSettings : ICustomSettings
{
    public bool ShowRatingOnListingPage = false;
    public int MaxProductNameLength = 10;
    public static int SareesSubSubCategoryId = 15;
    public static string ParentURL = "https://finrex.in/";
    public static string CompanyName = "Finrex Treasury Advisors LLP"; 
    public static int m_TotalSMS = 500000;
    public static int SMS_WarningAfter_Count = 500000;
    public static bool IsSMSPackageEnable = true;
    //public static string SMS_URL = "http://smslane.com/vendorsms/pushsms.aspx?user=jeyponramar@gmail.com&password=jeyapaul&msisdn=MobileNo&sid=REFUXS&msg=Message&fl=0&gwid=2";
    public static string SMS_URL = "http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=finrextra&password=Fin@1234&type=0&dlr=1&destination=MobileNo&source=FINREX&message=Message";
    public static string RefuxSupportEmailId = "jeyponramar@gmail.com";
    public static bool IsSMStoAllAdditonalContact = false;
    public static bool IsEmailtoAllAdditonalContact = false;

    public static string GetDbConstant(string name)
    {
        string query = "select *  from tbl_dbconstants where dbconstants_name='" + name + "'";
        DataRow drColor = DbTable.ExecuteSelectRow(query);
        string val = GlobalUtilities.ConvertToString(drColor["dbconstants_value"]);

        return val;
    }

    #region ICustomSettings Members

    public string CompanyName_Prop
    {
        get { return CompanyName; }
    }

    public bool IsSMSPackageEnabled_Prop
    {
        get { return IsSMSPackageEnable; }
    }

    public string RefuxSupportEmail_Prop
    {
        get
        {
            if (RefuxSupportEmailId == "") return "jeyponramar@gmail.com";
            return RefuxSupportEmailId;
        }
    }

    public int SMSWarningAfter_Prop
    {
        get { return SMS_WarningAfter_Count; }
    }

    public string SMS_URL_Prop
    {
        get { return SMS_URL; }
    }

    public int TotalSMS_Prop
    {
        get { return m_TotalSMS; }
    }
    public bool IsSMStoAllAdditonalContact_Prop
    {
        get { return IsSMStoAllAdditonalContact; }
    }
    public bool IsEmailtoAllAdditonalContact_Prop
    {
        get { return IsEmailtoAllAdditonalContact; }
    }

    #endregion
}
