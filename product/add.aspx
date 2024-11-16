 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Product_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
            <td align="right"> 
                <uc:NextPrevDetail ID="NextPrevDetail" runat="server" />
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
				<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Product Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtproductname"  IsUnique="true"  search='true'  MaxLength="100" runat="server"  dcn="product_productname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Product Name" ValidationGroup="vg" ControlToValidate="txtproductname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Product Category</td>
											<td ti='1'><asp:TextBox ID="productcategory"  dcn="productcategory_categoryname" MaxLength="100" runat="server" m="productcategory" cn="categoryname" CssClass="textbox txtqa ac txtac"></asp:TextBox><asp:TextBox id="txtproductcategoryid"  dcn="product_productcategoryid"  Text="0" runat="server" class=" hdnac hdnqa"/><img src="../images/down-arr1.jpg" class="quick-menu epage"/></td>
										</tr>
										<tr>
											<td class="label">HSN</td>
											<td ti='2'><asp:TextBox ID="hsn"  search='true'  dcn="hsn_hsn" MaxLength="100" runat="server" m="hsn" cn="hsn" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txthsnid"  dcn="product_hsnid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Price</td>
											<td ti='3'><asp:TextBox ID="txtprice"  dcn="product_price" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Opening Stock</td>
											<td ti='4'><asp:TextBox ID="txtopeningstock"  dcn="product_openingstock" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Unit</td>
											<td ti='5'><asp:DropDownList ID="ddlunitid"  dcn="product_unitid"  search='true' runat="server" m="unit" cn="unit" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Minimum Stock</td>
											<td ti='6'><asp:TextBox ID="txtminimumstock"  dcn="product_minimumstock" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Product Specification</td>
											<td ti='7'><asp:TextBox TextMode="MultiLine" ID="txtproductspecification"  Width="600" Height="150"  dcn="product_productspecification" runat="server" CssClass="htmleditor"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Product Image</td>
											<td ti='8'><uc:MultiFileUpload ID="mfuproductimage"  IsMutiple="false" FileType="Image" ReSize="200x200" SaveExt="true" FolderPath="upload/product" SaveAs="jpg" runat="server" CssClass="textbox "></uc:MultiFileUpload>
							<asp:TextBox ID="txtproductimage" runat="server" CssClass="hidden"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Maximum Stock</td>
											<td ti='9'><asp:TextBox ID="txtmaximumstock"  dcn="product_maximumstock" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
					<!--CONTROLS_END-->
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
					<asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="save button" ValidationGroup="vg"/>
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
				<asp:Button ID="btnSubmitAndView" runat="server" Visible="false"/>
					<!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
