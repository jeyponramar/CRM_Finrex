using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Xml;
using System.IO;
using WebComponent;

/// <summary>
/// Summary description for XMLNodeBinder
/// </summary>
public static class XMLNodeBinder
{
    public static XmlDocument _Doc;
    
    static XMLNodeBinder()
    {
        _Doc = getXMLDocument("add",Common.GetModuleName());
    } 
    public static void loadXMLDocument()
    {
        _Doc = getXMLDocument("add", Common.GetModuleName());
    }
    public static void loadXMLDocument(string ModuleName)
    {
        _Doc = getXMLDocument("add", ModuleName);
    }
    public static string getSingleNodeText(string strNodeName)
    {
        return getSingleNodeText(strNodeName, _Doc);
    }
    public static string getSingleNodeText(string strNodeName, XmlDocument doc)
    {
        if (doc == null) return "";
        strNodeName = (strNodeName.Contains("/setting")) ? strNodeName : "/setting/" + strNodeName;
        string strInnerText = "";
        XmlNode node = doc.SelectSingleNode(strNodeName);
        strInnerText = (node != null) ? node.InnerText : "";
        return strInnerText;
    }
    public static string getSingleNodeText(string strNodeName, string AddOrViewOrReport,string ModuleName)
    {
        return getSingleNodeText(strNodeName, getXMLDocument(AddOrViewOrReport,ModuleName));
    }
    public static string getSingleNodeText(string strNodeName, string AddOrViewOrReport)
    {
        return getSingleNodeText(strNodeName, getXMLDocument(AddOrViewOrReport));
    }
    public static Array getSingleNode(string strNodeName)
    {
        return getSingleNode(strNodeName, _Doc);
    }
    public static Array getSingleNode(string strNodeName, XmlDocument doc)
    {
        Array arr = getSingleNodeText(strNodeName, doc).Split(',');
        return arr;
    }
    public static ArrayList getMultipleNode(string strRootNodeName, XmlDocument doc)
    {
        ArrayList arrList = new ArrayList();
        XmlNodeList nodeList = doc.SelectNodes(strRootNodeName);
        foreach (XmlNode node in nodeList)
        {
            arrList.Add(getSingleNode(node.ToString(), doc));
        }
        return arrList;
    }
    //public static string getSingleNodeAttrText(string NodeAttrName, XmlDocument doc)
    //{
    //    NodeAttrName = (NodeAttrName.Contains("/setting")) ? NodeAttrName : "/setting/" + NodeAttrName;
    //    string strInnerText = "";
    //    XmlNode node = doc.SelectSingleNode(NodeAttrName);        
    //    strInnerText = (node != null) ? node.Attributes.ToString() : "";
    //    return strInnerText;
    //}
    public static string getXMLFilePath(string strModuleName)
    {
        string strPath = "add/" + strModuleName;
        return HttpContext.Current.Server.MapPath("~/xml/" + strPath + ".xml");
    }
    public static string getXMLFilePath(string strModuleName, string AddOrViewOrReport)
    {
        return HttpContext.Current.Server.MapPath("~/xml/" + AddOrViewOrReport + ".xml");
    }    
    public static XmlDocument getXMLDocument(string AddOrViewOrReport)
    {
        return getXMLDocument(AddOrViewOrReport,Common.GetModuleName());
    }
    public static XmlDocument getXMLDocument(string AddOrViewOrReport,string modulename)
    {
        XmlDocument doc = new XmlDocument();
        string filePath = HttpContext.Current.Server.MapPath("~/xml/" + AddOrViewOrReport + "/" + modulename + ".xml");
        if (File.Exists(filePath))
        {
            doc.Load(filePath);
            return doc;
        }
        else
        {
            return null;
        }
        
    }    
    public static string getFillDropDownSettings(string strAttrName,ref string TextFieldName,ref string ValueFieldName)
    {
        return getFillDropDownSettings("/setting/filldropdown[@name='" + strAttrName + "']", Common.GetModuleName(), ref TextFieldName, ref ValueFieldName);
    }
   
    public static string getFillDropDownSettings(string strAttrName,string ModuleName, ref string TextFieldName, ref string ValueFieldName)
    {        
        string strRootNodeName = (strAttrName.Contains("/setting/filldropdown")) ? strAttrName : "/setting/filldropdown[@name='" + strAttrName + "']";
        //XmlDocument doc = getXMLDocument("add",ModuleName);
        string query = getSingleNodeText(strRootNodeName + "/query",_Doc);
        TextFieldName = getSingleNodeText(strRootNodeName + "/datatextfield", _Doc);
        ValueFieldName = getSingleNodeText(strRootNodeName + "/datavaluefield", _Doc);
        return query;
    }
    public static void bindXmlNodeText(XmlNode node, string strNodevalue)
    {
        bindXmlNodeText(node, strNodevalue, false);
    }
    public static void bindXmlNodeText(XmlNode xmlNode, string strNodevalue, bool isXML)
    {
        if (xmlNode != null)
        {
            if (isXML)
            {
                xmlNode.InnerXml = strNodevalue;
            }
            else
            {
                xmlNode.InnerText = strNodevalue;
            }
        }
    }



   
}
