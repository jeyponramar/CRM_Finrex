<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeFile="Configure-UpdatableGrid.aspx.cs" Inherits="CP_Configure_UpdatableGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        var Include_View = "";
        var Include_View_Header = "";
        var TextBoxOptionType = "";
        var TextBoxOptionTypeColumns = ""; 
        var isValid = true;
        $(".updateviewcofig").click(function() {

            $(".chkincludeview").each(function() {
                //if ($(".chkincludeview").attr("checked")) {                    
                if ($(this).is(':checked')) {
                    var viewColNme = $(this).attr("columnname");
                    var viewColNmeHeader = $(".txt_" + viewColNme).val();
                    Include_View += (Include_View == "") ? viewColNme : "," + viewColNme;
                    Include_View_Header += (Include_View_Header == "") ? viewColNmeHeader : "," + viewColNmeHeader;
                }
            });
            $(".ddltextboxoptiontype").each(function() {
               // if ($(this).is(':selected')) {
                    var val = $(this).val();
                    var columnnm = $(this).attr("columnname");
                    if (val > 0) {
                        TextBoxOptionType += (TextBoxOptionType != "") ? "," + $(this).val() : $(this).val();
                        TextBoxOptionTypeColumns += (TextBoxOptionTypeColumns != "") ? +"," + TextBoxOptionTypeColumns : columnnm;
                    }
                //}
            });
            $(".txtincludeview").val(Include_View);
            $(".txtincludeviewHeader").val(Include_View_Header);
            $(".txtGridTextboxType").val(TextBoxOptionType);
            $(".txtGridTextboxTypeColumns").val(TextBoxOptionTypeColumns);
            return isValid;
        });
        $("input:checkbox").click(function() {
            var viewColNme = $(this).attr("columnname");
            var _thisView = ("chkview_" + viewColNme);
            var _thisSearch = ("chksearch_" + viewColNme);
            var _thisadvSearch = ("chkadvsearch_" + viewColNme);

            if ($("." + _thisView).is(':checked') || $("." + _thisSearch).is(':checked') || $("." + _thisadvSearch).is(':checked')) {
                $(".txt_" + viewColNme).css('background-color', '#00FF00')
            } else {
                $(".txt_" + viewColNme).css('background-color', '#ffffff');
            }
            //return;
            if ($("." + _thisView).is(':checked')) {
                $("." + _thisView).css('color', '#00FF00');
            }
            else {
                $("." + _thisView).css('color', '#ffffff');
            }
            if ($("." + _thisSearch).is(':checked')) {
                $("." + _thisSearch).css('color', '#FF384C');
            }
            else {
                $("." + _thisSearch).css('color', '#ffffff');
            }

            if ($("." + _thisadvSearch).is(':checked')) {
                $("." + _thisadvSearch).css('color', '#FF384C');
            }
            else {
                $("." + _thisadvSearch).css('color', '#ffffff');
            }
        });
        $(".ddltextboxoptiontype").change(function() {
            //if ($(this).is(':selected')) {
            var val = $(this).val();
            if (val > 0) {
                $(this).css('background-color', '#FF384C');
                $(this).css('color', '#ffffff');
            }
            else {
                $(this).css('background-color', '#ffffff');
                $(this).css('color', '#000000');
            }
            // }
        });
        $(".hideshowquery").click(function() {
            $(".query").toggle();
            $(this).text(($(this).text() == "Hide Query") ? "Show Query" : "Hide Query");
        });


    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblmessage" CssClass="error" Visible="false"></asp:Label>
            <asp:TextBox CssClass="hidden txtincludesearch" Width="900" runat="server" ID="txtincludesearch"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeview" Width="900" runat="server" ID="txtincludeview"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludesearchHeader" Width="900" runat="server" ID="hdnincludesearchHeader"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeviewHeader" Width="900" runat="server" ID="hdnincludeviewHeader"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeAdvanceSearch" Width="900" runat="server" ID="hdnIncludeAdvSearch"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeAdvanceSearchHeader" Width="900" runat="server" ID="hdnincludeAdvanceSearchHeader"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtGridTextboxType" Width="900" runat="server" ID="hdnGridTextboxType"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtGridTextboxTypeColumns" Width="900" runat="server" ID="hdnGridTextboxTypeColumns"></asp:TextBox> 
            <asp:TextBox CssClass="hidden " runat="server" ID="hdnModuleName"></asp:TextBox> 
            <asp:TextBox CssClass="hidden " runat="server" ID="hdnReportId"></asp:TextBox> 
            <asp:TextBox CssClass="hidden " runat="server" ID="hdnModuleId"></asp:TextBox> 
            <asp:TextBox Width="900" CssClass="hidden" runat="server" ID="txttargetFile"></asp:TextBox> 
        </td> 
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblModuleName"></asp:Label>
        </td>
    </tr> 
    <tr>
        <td>
            <asp:DropDownList runat="server" ID="ddlUpdatableGridName"></asp:DropDownList>
            <asp:RequiredFieldValidator ValidationGroup="vgsettings" ControlToValidate="ddlUpdatableGridName" runat="server" ID="rfv111" InitialValue="0" ErrorMessage="Please Select Grid Name"></asp:RequiredFieldValidator>
            &nbsp;<asp:Button CssClass="button" runat="server" Text="Bind Settings" Width="150" ValidationGroup="vgsettings" ID="btnBindSettings" OnClick="btnBindSettings_Click" />
            <asp:Button CssClass="button" runat="server" Text="Create New Settings" Width="150" ID="btnNewSettings" OnClick="btnBindSettings_Click" />
        </td>
    </tr>   
    <tr>
        <td>
            <table width="100%" runat="server" id="tdSettings" visible="false">
                <tr>
                    <td style="border:1px solid black">
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox Width="800" CssClass="query" Height="150" runat="server" TextMode="MultiLine" placeholder="WRITE QUERY" ID="txtquery"></asp:TextBox>&nbsp;<div style="font-size:14px;font-weight:bold;padding-left:10px;cursor:pointer;color:Black;background-color:#00FF00;border-radius:20px;width:110px;" class="hideshowquery">Hide Query</div>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnGenerate" Width="150" Text="Generate Columns" OnClick="generateColumns_Click" CssClass="button" />
                                </td>                    
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <table>
                            <tr>
                                 <td>
                                <asp:TextBox runat="server" ID="txtTableName" placeholder="Enter table Name to be Update" Width="300"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="vg" runat="server" ControlToValidate="txtTableName" ErrorMessage="please Enter table Name"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtGridName" placeholder="Enter GridName" Width="200"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="vg" runat="server" ControlToValidate="txtGridName" ErrorMessage="please Enter Grid Name"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr>   
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColumns" AutoPostBack="true" OnSelectedIndexChanged="ddlColumns_ChangeCommitted" ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtColumnLbl" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlScriptInputOptionId">
                                        <asp:ListItem Selected="True" Value="0">Select Input Option</asp:ListItem>
                                        <asp:ListItem  value='1'>label</asp:ListItem>
                                        <asp:ListItem value='2'>Numer TextBox</asp:ListItem>
                                        <asp:ListItem value='3'>TextBox</asp:ListItem>
                                        <asp:ListItem value='4'>Text Area</asp:ListItem>
                                        <asp:ListItem value='5'>Hidden Field</asp:ListItem>
                                        <asp:ListItem value='6'>Amount TextBox</asp:ListItem>
                                        <asp:ListItem value='7'>Auto Complete</asp:ListItem> 
                                    </asp:DropDownList>
                                </td>        
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="Generate HTML Script" Width="200" ValidationGroup="vg" CssClass="button" OnClick="btnGenerateScript_Click" />
                                </td>
                             </tr>
                             <tr>
                                <td colspan="4"  width="100%" align="center"><asp:TextBox runat="server" TextMode="MultiLine" Height="300" Width="800" ID="txtHtmlScript"></asp:TextBox></td>        
                            </tr>
                        </table>
                    </td>
                    
                </tr>
               
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td><asp:Literal Visible="false" runat="server" ID="ltSettings"></asp:Literal> </td>                                      
                            </tr>
                        </table>
                    </td>        
                </tr>
               
                <tr>
                    <td  align="center">
                        <table width="100%">
                            <tr>
                                 <td align="center">
                                    <asp:Button runat="server" ID="btnUpdateSettings" Width="150" Text="Update Settings" OnClick="btnUpdateSettings_Click" ValidationGroup="vg" CssClass="button updateviewcofig" />
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

