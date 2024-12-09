﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFindoc.aspx.cs" Inherits="AddFindoc" Title="AddFindocdocument" %>
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
            <td class='page-inner2'>
             <table width="100%">
                
                 <tr><td class="page-title2">Upload Document</td></tr>
                 <tr>
                    <td align="right" style="padding-right:10px;">
                        <uc:findocmenu id="findocmenu1" runat="server" IsAdminPage="false"></uc:findocmenu>
                        
                    </td>
                </tr>
                 <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="500">
                                    <table cellpadding='5' cellspacing='5'>
                                         <tr><td colspan="2"><asp:Label ID="lblmessage" runat="server" CssClass="error"></asp:Label></td></tr>
                                         <tr>
                                             <td>Department</td>
                                             <td>
                                                <asp:TextBox ID="findocdepartment" MaxLength="100" runat="server" m="findocdepartment" cn="departmentname" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindocdepartmentid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                          <tr>
                                             <td>Category</td>
                                             <td>
                                                <asp:TextBox ID="findoccategory" MaxLength="100" runat="server" m="findoccategory" cn="categoryname" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindoccategoryid" dcn="findocdocument_findoccategoryid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                          <tr>
                                             <td>Sub Category</td>
                                             <td>
                                                <asp:TextBox ID="findocsubcategory" MaxLength="100" runat="server" m="findocsubcategory" cn="subcategoryname" 
                                                  CssClass="textbox ac txtac" acparent="findoccategoryid"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindocsubcategoryid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                          <tr>
                                             <td>Document Type </td>
                                             <td>
                                                <asp:TextBox ID="findocdocumenttype" MaxLength="100" runat="server" m="findocdocumenttype" cn="documenttype" 
                                                  CssClass="textbox ac txtac"></asp:TextBox>  
                                                 <asp:TextBox id="txtfindocdocumenttypeid" Text="0" runat="server" class="hdnac" />
                                                 <img src="images/down-arrow.png" class="quick-new epage" />
                                             </td>
                                         </tr>
                                         <tr>
											<td>Attachment <span class="error">*</span></td>
											<td><uc:MultiFileUpload ID="mfuattachment" IsPopulateFiles="false" IsMultiple="true" FileType="any" 
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
                                              <asp:TextBox ID="txtsubject" MaxLength="100" runat="server" class="textbox"></asp:TextBox>
                                              </td>
                                         </tr>
                                         <tr>
				                             <td>Remarks</td>
				                             <td><asp:TextBox TextMode="MultiLine" ID="txtremarks" MaxLength="300" runat="server" CssClass="textarea" style="height:70px;"></asp:TextBox></td>
			                             </tr>
                                         <tr>
                                             <td></td>
                                             <td>
                                                <asp:Button ID="btnSubmit" runat="server"  Text="Save" CssClass="save button" OnClick="btnSubmit_Click" />&nbsp;
                                                <asp:Button ID="btnDelete" runat="server"  Text="Delete" CssClass="delete button" OnClick="btnDelete_Click" Visible="false"/>
                                             </td>
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
                                            <td><asp:Label ID="lbluploaddate" Format="DateTime" runat="server" CssClass="bold"></asp:Label></td>
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

