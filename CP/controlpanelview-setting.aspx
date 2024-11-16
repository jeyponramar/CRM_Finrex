<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="controlpanelview-setting.aspx.cs" Inherits="controlpanelview_setting" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    var leftList;
    var rightList;
    $(document).ready(function() {
        leftList = $(".leftlist");
        rightList = $(".rightlist");

        $(".right-arr").click(function() {
            leftList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    rightList.append("<option value='" + $(this).attr("value") + "'>" + $(this).text() + "</option>");
                    $(this).remove();
                }
            });
            bindGridColumns();
        });
        $(".left-arr").click(function() {
            rightList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    leftList.append("<option value='" + $(this).attr("value") + "'>" + $(this).text() + "</option>");
                    $(this).remove();
                }
            });
            bindGridColumns();
        });
        $(".up-arr").click(function() {
            var selectedRow;
            var prevRow;
            rightList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    selectedRow = $(this);
                }
                else {
                    if (selectedRow == undefined) prevRow = $(this);
                }
            });
            if (selectedRow.attr("value") != prevRow.attr("value")) {
                selectedRow.insertBefore(prevRow);
            }            
        });
        $(".down-arr").click(function() {
            var selectedRow;
            var prevRow;
            rightList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    selectedRow = $(this);
                }
                else {
                    if (selectedRow != undefined && prevRow == undefined) prevRow = $(this);
                }
            });
            if (selectedRow.attr("value") != prevRow.attr("value")) {
                selectedRow.insertAfter(prevRow);
            }            
        });
        //left List
        $(".left_uparr").click(function() {
            var selectedRow;
            var prevRow;
            leftList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    selectedRow = $(this);
                }
                else {
                    if (selectedRow == undefined) prevRow = $(this);
                }
            });
            if (selectedRow.attr("value") != prevRow.attr("value")) {
                selectedRow.insertBefore(prevRow);
            }            
        });
        $(".left_downarr").click(function() {
            var selectedRow;
            var prevRow;
            leftList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    selectedRow = $(this);
                }
                else {
                    if (selectedRow != undefined && prevRow == undefined) prevRow = $(this);
                }
            });
            if (selectedRow.attr("value") != prevRow.attr("value")) {
                selectedRow.insertAfter(prevRow);
            }
        });
        //Left list End


        $(".save").click(function() {
            var gc = "";
            var gclabels = "";
            var gleftc = "";
            var gleftclabels = "";
            rightList.find("option").each(function() {
                gc = (gc != "") ? gc + "," + $(this).attr("value") : $(this).attr("value");
                gclabels += (gclabels != "") ? "," + $(this).text() : $(this).text();
            });
            leftList.find("option").each(function() {
                gleftc += (gleftc == "") ? $(this).attr("value") : "," + $(this).attr("value");
                gleftclabels += (gleftclabels == "") ? $(this).text() : "," + $(this).text();
            }); 
            $(".gridrightcolumns").val(gc);
            $(".gridrightcolumnlabels").val(gclabels);
            $(".gridleftcolumns").val(gleftc);
            $(".gridleftcolumnlabels").val(gleftclabels);

        });
    });      
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" style="background-color:#444444;color:White;">
    <tr>
        <td align="center">
            <asp:Label runat="server" ID="lblMessage" CssClass="error" Visible="false"></asp:Label>
        </td>
    </tr>
     <tr>        
        <td class="title" style="font-size:19px;font-weight:bold;border-bottom:1px dotted white" align="center">
            Page Setting
        </td>
     </tr>
     <tr>
         <td>
            <table width="100%">
                <tr>
                    <td><table><tr>                                        
                    <td style="vertical-align:top;padding:10px;">
                        <table cellpadding="10">
                            <tr><td><img src="../images/up-arrow-gr.png" class="left_uparr hand" title="Move Up"/></td></tr>
                            <tr><td><img src="../images/down-arrow-gr.png" class="left_downarr hand" title="Move Down"/></td></tr>
                        </table>
                    </td>
                    <td>
                        <asp:ListBox SelectionMode="Multiple" ID="lstLeft" CssClass="leftlist" runat="server" Height="350" Width="400"></asp:ListBox>
                        <asp:TextBox ID="hdnGridLeftColumns" runat="server" CssClass="gridleftcolumns hidden"/>
                        <asp:TextBox ID="hdnGridLeftColumnLabels" runat="server" CssClass="gridleftcolumnlabels hidden"/>
                    </td>
                    <td style="vertical-align:top;padding:10px;">
                        <table cellspacing="10">
                            <tr><td><img src="../images/arrow-right.png" class="right-arr hand" title="Move Right"/></td></tr>
                            <tr><td><img src="../images/arrow-left.png" class="left-arr hand" title="Move Left"/></td></tr>
                        </table>
                    </td>
                    <td>
                        <asp:ListBox SelectionMode="Multiple" ID="lstRight" CssClass="rightlist" runat="server" Height="350" Width="400"></asp:ListBox>
                        <asp:TextBox ID="hdnGridRightColumns" runat="server" CssClass="gridrightcolumns hidden"/>
                        <asp:TextBox ID="hdnGridRightColumnLabels" runat="server" CssClass="gridrightcolumnlabels hidden"/>
                    </td>
                    <td style="vertical-align:top;padding:10px;">
                    <table cellpadding="10">
                        <tr><td><img src="../images/up-arrow-gr.png" class="up-arr hand" title="Move Up"/></td></tr>
                        <tr><td><img src="../images/down-arrow-gr.png" class="down-arr hand" title="Move Down"/></td></tr>
                    </table>
                </td>
                </tr></table></td>
                </tr>                
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSubmit" CssClass="button save" runat="server" OnClick="btnSubmit_Click" Text="Submit" OnClientClick="PreSave()"/>
                    </td>
                </tr>
            </table>
         </td>
     </tr>
     
</table>     

</asp:Content>

