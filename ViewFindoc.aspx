<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewFindoc.aspx.cs" Inherits="ViewFindoc" Title="ViewFindoc Document " %>
<%@ Register Src="~/usercontrols/FinDocMenu.ascx" TagName="findocmenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style>
body{background-color:#fff !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table width='100%' cellpadding=0 cellspacing=0>
        <tr>
            <%--<td class="finmenubg">
               <uc:findocmenu id="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
            </td>--%>
             <td class='page-inner2' cellpadding="0" cellspacing="0">
                 <table width='100%'>
                       <tr>
                            <td class="page-title2">View Documents</td>
                       </tr>
                       <tr>
                            <td>
                                <table width='100%' cellpadding=0 cellspacing=0>
                                    <tr>
                                        <td style="padding-left:10px;">
                                            <a href="AddFindoc.aspx" class="add-link">Upload Document</a>
                                        </td>
                                        <td align="right" style="padding-right:10px;">
                                            <uc:findocmenu id="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                       </tr>
                       <tr>
                           <td>
                                <table cellspacing="10">
                                    <tr>
                                        <td class="bold">Date : From <asp:TextBox ID="txtfromdate"  Width="70" runat="server" CssClass="datepicker"></asp:TextBox>
                                            &nbsp;To <asp:TextBox ID="txttodate" Width="70" runat="server" CssClass="datepicker"></asp:TextBox>
                                        </td>
                                        <td class="bold">Department :
                                            <asp:TextBox ID="findocdepartment" style="width:120px !important;min-width:80px !important;" runat="server" m="findocdepartment" cn="departmentname" 
                                              CssClass="textbox ac txtac"></asp:TextBox>  
                                             <asp:TextBox id="txtfindocdepartmentid" Text="0" runat="server" class="hdnac" />
                                        </td>
                                        <td class="bold">Category :</td>
                                         <td>
                                            <asp:TextBox ID="findoccategory" style="width:120px !important;min-width:80px !important;" runat="server" m="findoccategory" cn="categoryname" 
                                              CssClass="textbox ac txtac"></asp:TextBox>  
                                             <asp:TextBox id="txtfindoccategoryid" Text="0" runat="server" class="hdnac" />
                                         </td>
                                         <td class="bold">Sub Category : </td>
                                         <td>
                                            <asp:TextBox ID="findocsubcategory" style="width:120px !important;min-width:80px !important;" runat="server" m="findocsubcategory" cn="subcategoryname" 
                                              CssClass="textbox ac txtac" acparent="findoccategoryid"></asp:TextBox>  
                                             <asp:TextBox id="txtfindocsubcategoryid" Text="0" runat="server" class="hdnac" />
                                         </td>
                                    
                                        <td><asp:Button ID="btnSubmit" CssClass="button" runat="server" Text="Search" OnClick="btnSubmit_Click" /></td>
                                    </tr>
                                
                        </table>
                           </td>
                       </tr>
                       
                       <tr>
                            <td>
                                <asp:Literal ID="ltdata" runat="server"></asp:Literal>
                            </td>
                        </tr>
                 </table>
             </td>
        </tr>
 </table>
</asp:Content>

