<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="configure-viewpage.aspx.cs" Inherits="CP_configure_viewpage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        var Include_View = "";
        var Include_search = "";
        var Include_search_Header = "";
        var Include_View_Header = "";
        var IncludeAdv_Search = "";
        var IncludeAdv_SearchHeader = "";
        var IncludeView_Sequenc = "";
        var isValid = true;
        $(".updateviewcofig").click(function() {

            var TotalCount_AdvSearch = 10;
            $(".chkincludesearch").each(function() {
                if ($(this).is(':checked')) {
                    var viewColNme = $(this).attr("columnname");
                    var viewColNmeHeader = $(".txt_" + viewColNme).val();
                    Include_search += (Include_search == "") ? viewColNme : "," + viewColNme;
                    Include_search_Header += (Include_search_Header == "") ? viewColNmeHeader : "," + viewColNmeHeader;

                }
            });

            $(".chkincludeadvsearch").each(function() {
                if ($(this).is(':checked')) {
                    TotalCount_AdvSearch--;
                    if (TotalCount_AdvSearch < 0) {
                        alert("Advance Search Column Counts Exceeds the limit,Total Limit is:10");
                        isValid = false;
                        return false;
                    }
                    var viewColNme = $(this).attr("columnname");
                    var viewColNmeHeader = $(".txt_" + viewColNme).val();
                    IncludeAdv_Search += (IncludeAdv_Search == "") ? viewColNme : "," + viewColNme;
                    IncludeAdv_SearchHeader += (IncludeAdv_SearchHeader == "") ? viewColNmeHeader : "," + viewColNmeHeader;

                }
            });
            $(".chkincludeview").each(function() {
                //if ($(".chkincludeview").attr("checked")) {                    
                if ($(this).is(':checked')) {
                    var viewColNme = $(this).attr("columnname");
                    var viewColNmeHeader = $(".txt_" + viewColNme).val();
                    var viewColSequence = $(".txtsequence_" + viewColNme).val();
                    Include_View += (Include_View == "") ? viewColNme : "," + viewColNme;
                    Include_View_Header += (Include_View_Header == "") ? viewColNmeHeader : "," + viewColNmeHeader;
                    //IncludeView_Sequenc += (IncludeView_Sequenc == "") ? viewColSequence : "," + viewColSequence;
                } 
            });
            $(".txtincludesearch").val(Include_search);
            $(".txtincludeview").val(Include_View);
            $(".txtincludesearchHeader").val(Include_search_Header);
            $(".txtincludeviewHeader").val(Include_View_Header);
            $(".txtincludeAdvanceSearch").val(IncludeAdv_Search);
            $(".txtincludeAdvanceSearchHeader").val(IncludeAdv_SearchHeader);
            //$(".txtIncludeView_Sequence").val(IncludeView_Sequenc);
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
        $(".hideshowquery").click(function() {
            $(".query").toggle();
            $(this).text(($(this).text() == "Hide Query") ? "Show Query" : "Hide Query");
        });


    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0" width="100%" style="background-color:#444444;color:White;">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblmessage" CssClass="error" Visible="false"></asp:Label>
            <asp:TextBox CssClass="hidden txtincludesearch" Width="900" runat="server" ID="txtincludesearch"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeview" Width="900" runat="server" ID="txtincludeview"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludesearchHeader" Width="900" runat="server" ID="hdnincludesearchHeader"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeviewHeader" Width="900" runat="server" ID="hdnincludeviewHeader"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeAdvanceSearch" Width="900" runat="server" ID="hdnIncludeAdvSearch"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtincludeAdvanceSearchHeader" Width="900" runat="server" ID="hdnincludeAdvanceSearchHeader"></asp:TextBox> 
            <asp:TextBox CssClass="hidden txtIncludeView_Sequence" Width="900" runat="server" ID="hdnIncludeView_Sequence"></asp:TextBox> 
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
            <table width="100%" runat="server" id="NonEditableGridSettings" visible="false">
               <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlNonEditableGridName"></asp:DropDownList>
                        <asp:RequiredFieldValidator ValidationGroup="vgsettings" ControlToValidate="ddlNonEditableGridName" runat="server" ID="rfv111" InitialValue="0" ErrorMessage="Please Select Grid Name"></asp:RequiredFieldValidator>
                        &nbsp;<asp:Button CssClass="button" runat="server" Text="Bind Settings" Width="150" ValidationGroup="vgsettings" ID="btnBindSettings" OnClick="btnBindSettings_Click" />
                        <asp:Button CssClass="button" runat="server" Text="Create New Settings" Width="150" ID="btnNewSettings" OnClick="btnBindSettings_Click" />
                    </td>
                </tr>   
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" runat="server" id="tdSetings">
                <tr>
                    <td style="border:1px solid black">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:TextBox Width="800" CssClass="query" Height="200" runat="server" TextMode="MultiLine" placeholder="WRITE QUERY" ID="txtquery"></asp:TextBox>&nbsp;<div style="font-size:14px;font-weight:bold;padding-left:10px;cursor:pointer;color:Black;background-color:#00FF00;border-radius:20px;width:110px;" class="hideshowquery">Hide Query</div>
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
                                <td><asp:Literal runat="server" ID="ltSettings"></asp:Literal> </td>                                      
                            </tr>
                        </table>
                    </td>        
                </tr>
            </table>
        </td>
    </tr>
     <tr>
        <td>
            <table runat="server" id="tdNonEditSettings" visible="false">
                <tr>
                     <td>
                    <asp:TextBox runat="server" ID="txtTableName" Visible="false" placeholder="Enter table Name to be Update" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="vg" runat="server" ControlToValidate="txtTableName" ErrorMessage="please Enter table Name"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtGridName" CssClass="gridname" placeholder="Enter GridName" Width="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="vg" runat="server" ControlToValidate="txtGridName" CssClass="gridname" ErrorMessage="please Enter Grid Name"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td  align="center">
            <table width="100%">
                <tr>
                     <td align="center">
                        <asp:Button runat="server" ID="btnUpdateSettings"  Width="150" Text="Update Settings" OnClick="btnUpdateSettings_Click" CssClass="button updateviewcofig" />
                    </td>
                </tr>
            </table>
        </td>
       
    </tr>
</table>
</asp:Content>

