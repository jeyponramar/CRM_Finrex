<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="buy-sell-scrips-enquiry.aspx.cs" Inherits="buy_sell_scrips_enquiry" Title="Buy - Sell Scrips RODTEP/ROSCTL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">Buy - Sell Scrips RODTEP/ROSCTL</td>
            </tr>
            <tr><td style="text-align:center;"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
            <tr id="trform" runat="server">
                <td style="padding-left:50px;padding-bottom:50px;">
                    <table width="100%">
                        <tr>
                            <td>
                                <p>Finrex introduces BUY/SELL Duty Credit Scrips - RODTEP/ROSCTL service for our clients.</p>
                                <ul>
                                    <li>Buy or Sell Foreign Duty Credit Scrips - RODTEP/ROSCTL at best competitive rates.</li>
                                    <li>Immediate deal closures and payments realisation to exporters within minutes</li>
                                    <li>Technical guidance/support on scrips issuance.</li>
                                </ul>
                                <p>Exporters - Get the Best Rates for your export incentives - Rodtep/Rosctl scrips and immediate payments.</p>
                                <p>Importers - Immediate availability as per desired values for import duty payment utilising Duty Credit Scrips - RODTEP/ROSCTL</p>
                                <p>Call us or Whatsapp message on <b>7208824611</b></p>
                                <p><b>Kindly provide below details to proceed further to buy/sell.</b></p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:PlaceHolder ID="form" runat="server">
                                <table width="100%" cellpadding="5">
                                    <tr>
                                        <td class="label">Company Name <span class="error">*</span></td>
                                        <td><asp:Label ID="lblcompanyname" runat="server" CssClass="bold"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label">Person Name <span class="error">*</span></td>
                                        <td><asp:TextBox ID="txtpersonname" runat="server" CssClass="textbox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpersonname" ErrorMessage="Required Person Name" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label">Email Id <span class="error">*</span></td>
                                        <td><asp:TextBox ID="txtemailid" runat="server" CssClass="textbox val-email"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtemailid" ErrorMessage="Required Email Id" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label">Mobile <span class="error">*</span></td>
                                        <td><asp:TextBox ID="txtmobileno" runat="server" CssClass="textbox val-mobile" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtmobileno" ErrorMessage="Required Mobile No." ValidationGroup="vg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label">IEC Code</td>
                                        <td><asp:TextBox ID="txtIECCode" runat="server" CssClass="textbox"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="label">City</td>
                                        <td><asp:TextBox ID="txtCity" runat="server" CssClass="textbox"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="label"></td>
                                        <td class="rbtn">
                                            <asp:RadioButtonList ID="rbtnisexporter" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Exporter" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Importer" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label"></td>
                                        <td class="rbtn">
                                            <asp:RadioButtonList ID="rbtnisbuy" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Buy" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Sell" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label">Script Type</td>
                                        <td><asp:DropDownList ID="ddlbuysellscripttypeid" runat="server" CssClass="ddl">
                                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Script  Amount in Rs.</td>
                                        <td><asp:TextBox ID="txtscriptamount" runat="server" CssClass="mbox val-dbl"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Script Number</td>
                                        <td><asp:TextBox ID="txtscriptnumber" runat="server" CssClass="textbox"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Script Date</td>
                                        <td><asp:TextBox ID="txtscriptdate" runat="server" CssClass="textbox datepicker"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Port Code</td>
                                        <td><asp:TextBox ID="txtportcode" runat="server" CssClass="textbox"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><asp:Button ID="btnsubmit" CssClass="button" runat="server" Text="Submit" OnClick="btnsubmit_Click" ValidationGroup="vg"/></td>
                                    </tr>
                                </table>
                                </asp:PlaceHolder>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </td>
    </tr>
</table>
</asp:Content>

