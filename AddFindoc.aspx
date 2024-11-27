<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFindoc.aspx.cs" Inherits="AddFindoc" Title="AddFindocdocument" %>
<%@ Register Src="~/usercontrols/FinDocMenu.ascx" TagName="findocmenu" TagPrefix="uc" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    

<script src="js/upload/jquery.fileupload.js?v=<%=VersionNo %>"></script>
<script src="js/upload/jquery.fileupload-ui.js?v=<%=VersionNo %>"></script>
<script src="js/upload/multifileupload.js?v=<%=VersionNo %>"></script>
    
<style>
body{background-color:#fff !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:PlaceHolder ID="form" runat="server">
     <table width='100%' cellpadding=0 cellspacing=0>
      <tr>
        <td class="finmenubg">
               <uc:findocmenu id="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
         </td>
         <td class='page-inner2'>
             <table width="100%">
                 <tr><td class="page-title2">Add FinDoc Document</td></tr>
                 <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="500">
                                    <table cellpadding='5' cellspacing='5'>
                                         <tr><td colspan="2"><asp:Label ID="lblmessage" runat="server"></asp:Label></td></tr>
                                         <tr>
                                             <td>FinDoc Department</td>
                                             <td>
                                                <asp:TextBox ID="findocdepartment" MaxLength="100" runat="server" m="findocdepartment" cn="departmentname" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindocdepartmentid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                          <tr>
                                             <td>FinDoc Category</td>
                                             <td>
                                                <asp:TextBox ID="findoccategory" MaxLength="100" runat="server" m="findoccategory" cn="categoryname" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindoccategoryid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                          <tr>
                                             <td>FinDoc Sub Category</td>
                                             <td>
                                                <asp:TextBox ID="findocsubcategory" MaxLength="100" runat="server" m="findocsubcategory" cn="subcategoryname" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindocsubcategoryid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                          <tr>
                                             <td>FinDoc Document Type </td>
                                             <td>
                                                <asp:TextBox ID="findocdocumenttype" MaxLength="100" runat="server" m="findocdocumenttype" cn="documenttype" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindocdocumenttypeid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                         <tr>
											<td>Attachment <span class="error">*</span></td>
											<td><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="any" 
											FolderPath="upload/client/findoc" runat="server" CssClass="textbox "></uc:MultiFileUpload>
											</td>
										</tr>
										<tr>
										    <td></td>
										    <td><asp:Literal ID="ltuploadedfiled" runat="server"></asp:Literal></td>
										</tr>
                                         <tr>
                                              <td>Subject</td>
                                              <td>
                                              <asp:TextBox ID="txtsubject"  runat="server" class="textbox"></asp:TextBox>
                                              </td>
                                         </tr>
                                         <tr>
				                             <td>Remarks</td>
				                             <td><asp:TextBox TextMode="MultiLine" ID="txtremarks"   runat="server" CssClass="textarea" style="height:70px;"></asp:TextBox></td>
			                             </tr>
                                         <tr>
                                             <td></td>
                                             <td><asp:Button ID="btnSubmit" runat="server"  Text="Save" CssClass="save button" OnClick="btnSubmit_Click" /></td>
                                         </tr>
                                  </table>
                                </td>
                                <td valign="top" style="padding-top:20px;">
                                    <table cellpadding="5" id="tbluploadby" runat="server" visible="false">
                                        <tr>
                                            <td>Uploaded By</td>
                                            <td><asp:Label ID="lblname" runat="server" CssClass="bold"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Uploaded On</td>
                                            <td><asp:Label ID="lbluploaddate" runat="server" CssClass="bold"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                 </tr>
             </table> 
          </td>
      </tr>
    </table>        
                      
    </asp:PlaceHolder>
</asp:Content>

