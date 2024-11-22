<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFindoc.aspx.cs" Inherits="AddFindoc" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:PlaceHolder ID="form" runat="server">
     
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
                  <td>Subject</td>
                  <td>
                  <asp:TextBox ID="txtsubject"  runat="server" class="textbox"></asp:TextBox>
                  </td>
             </tr>
             <tr>
				 <td class="label">Remarks</td>
				 <td><asp:TextBox TextMode="MultiLine" ID="txtremarks"   runat="server" CssClass="textarea"></asp:TextBox></td>
			 </tr>
             <tr>
                 <td></td>
                 <td><asp:Button ID="btnSubmit" runat="server"  Text="Save" CssClass="save button" OnClick="btnSubmit_Click" /></td>
             </tr>
      </table>  
                  
                      
    </asp:PlaceHolder>
</asp:Content>

