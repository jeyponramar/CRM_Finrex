<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="config-module.aspx.cs" Inherits="CP_config_module" EnableEventValidation="false" ValidateRequest="false"%>
<%@ Register Src="~/Grid.ascx" TagName="GridData" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".lbl").change(function() {
            var label = $(this).val().trim().toLowerCase().replace(/\W/g, "");
            var colname = label;
            if (colname.indexOf("_") < 0) {
                colname = $(".prefix").val() + colname;
            }
            $(".columnname").val(colname);
            $(".gridcolumnname").val($(this).val().trim());
            $(".isgenerate").attr("checked", true);
        });
        $(".lbl").blur(function() {
            var label = $(this).val().toLowerCase();
            if (label.indexOf("status") >= 0) {
                alert("ALERT : For Status Master: Status column name should end with status eg.complaintstatus_status\n\n" +
                          "Status master should have columns _textcolor,_backgroundcolor to display color in view page.");
            }
            else if (label == "textcolor" || label == "backgroundcolor") {
                $(".size").val("6");
            }
        });
        $(".modulename").change(function() {
            $(".tablename").val("tbl_" + $(this).val().trim().toLowerCase().replace(/\W/g, ""));
            $(".addtitle").val("Add " + $(this).val());
            $(".edittitle").val("Edit " + $(this).val());
            $(".viewtitle").val("View " + $(this).val());
        });
        $(".dropdownmodule").blur(function() {
            if ($(".dropdownmoduleid").val() == "") {
                $(".dropdowncolumn").val("");
                $(".dropdownvalues").val("");
                return;
            }
            var url = "getddlcol.ashx?id=" + $(".dropdownmoduleid").val();
            var columnName = RequestData(url);
            $(".dropdowncolumn").val(columnName);
            var idcol = "";
            if (columnName != "") {
                arr = columnName.split('_');
                idcol = $(".prefix").val() + arr[0] + "id";
            }
            $(".columnname").val(idcol);
        });
        $(".ddlcontrol").change(function() {
            var control = $(this).val();
            $(".size").val("100");
            if (control == "Amount") {
                $(".ddldatatype").val("numeric");
            }
            else if (control == "Number") {
                $(".ddldatatype").val("int");
            }
            else if (control == "Number") {
                $(".ddldatatype").val("int");
            }
            else if (control == "Multi Line") {
                $(".size").val("300");
            }
            else if (control == "Html Editor") {
                $(".size").val("1000");
            }
            else if (control == "Html Editor") {
                $(".attributes").val("Width=\"100%\" Height=\"300\"");
            }
            else if (control == "Multi Checkbox") {
                $(".attributes").val("Module=\"\" Column=\"\"");
            }
            else if (control == "Mobile No") {
                $(".css").val("val-mobile");
            }
            else if (control == "Email Id") {
                $(".css").val("val-email");
            }
            else {
                $(".attributes").val("");
            }
        });
        $(".repeater-row,.repeater-alt").click(function() {
            var id = $(this).find(".idval").attr("idval");
            window.location = 'config-module.aspx?mid=<%=Request.QueryString["mid"]%>&id=' + id;
        });
        $(".isvisibleinadd").change(function() {
            if ($(this).find("input").attr("checked") == "checked") {
                if ($(".trattributes").val() == "") {
                }
                else {
                    var val = $(".trattributes").val();
                    val = val.replace('Visible="false"', '').replace('Visible="true"', '');
                    $(".trattributes").val(val);
                }
            }
            else {
                if ($(".trattributes").val() == "") {
                    $(".trattributes").val('Visible="false"');
                }
                else {
                    var val = $(".trattributes").val();
                    val = val.replace('Visible="false"', '').replace('Visible="true"', '');
                    $(".trattributes").val(val + ' Visible="false"');
                }
            }
        });
        $(".help").click(function() {
            if ($(".trhelp").css("display") == "none") {
                $(".trhelp").show();
            }
            else {
                $(".trhelp").hide();
            }
        });
        $(".save").click(function() {
            var control = $(".ddlcontrol").val();
            if (control == "Auto Complete") {
                if ($(".dropdowncolumn").val() == "") {
                    alert("Please select dropdown module");
                    $(".dropdownmodule").focus();
                    return false;
                }
            }
        });
        $(".ddlgoto").change(function() {
            var m = $(this).val();
            if (m == "0") return;

            var url = m + ".aspx?mid=" + QueryString("mid") + "&mname=" + $(".modulename").val();
            var cid = QueryString("id");
            if (cid != "") url += "&cid=" + cid;

            if (m == "configure-viewpage-view") {
                url = "configure-viewpage.aspx?mid=" + QueryString("mid") + "&mname=" + $(".modulename").val();
            }
            else if (m == "controlpanelview-setting") {
            }
            else if (m == "configure-viewpage") {
                url += "&isSubGrid=true";
            }
            else if (m == "Configure-UpdatableGrid") {
                url += "&isUpdatebleGrid=true";
            }
            else if (m == "populate-on-add") {
            }
            window.open(url);
        });
        $(".ismobilecol").click(function() {
            if ($(this).find("input").prop("checked")) {
                if ($(".mobilesetting").val().indexOf("ismobile") == -1) {
                    $(".mobilesetting").val('ismobile="true"');
                }
            }
        });
    });
    
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
  <tr>
    <td>
        <table width="100%">
            <tr>
                <td class="title"><asp:Label id="lblTitle" runat="server"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="../images/help.png" title="Help" class="help hand"/>
                    <asp:TextBox ID="h_modulename" runat="server" CssClass="hidden modulename"></asp:TextBox>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>Go to 
                            <asp:DropDownList runat="server" ID="ddlGotoConfigPageSetting" CssClass="ddlgoto">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="configure-viewpage-view">Go To View Page Settings</asp:ListItem>
                            <asp:ListItem Value="controlpanelview-setting">Go To Quick Page Alignment</asp:ListItem>
                            <asp:ListItem Value="configure-viewpage">Go To Auto Bind Sub Grid</asp:ListItem>
                            <asp:ListItem Value="Configure-UpdatableGrid">Go To updatable Sub Grid</asp:ListItem>
                            <asp:ListItem Value="populate-on-add">Go To Set Populate on Add</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            <td>Switch To</td>
                            <td>
                                <asp:DropDownList ID="module" runat="server" AutoPostBack="true" OnSelectedIndexChanged="module_Changed"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
        </table>
    </td>
  </tr>
  <tr class="trhelp hidden">
  <td style="background-color:#ffffff;padding:10px;color:#000000">
    <table width="100%" border="1">
        <tr>
            <td class="bold" width="200">File Upload Properties</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">Attributes</td></tr>
                    <tr><td>IsMultiple="true" / IsMultiple="false"</td></tr>
                    <tr><td>FileType="Image" / FileType="Doc" / FileType="Audio" / FileType="Video" / FileType="Any"</td></tr>
                    <tr><td>Size="200x150"</td></tr>
                    <tr><td>ReSize="large=800x800,medium=300x250,thumb=50x50"</td></tr>
                    <tr><td>SaveExt="true" / SaveExt="false"</td></tr>
                    <tr><td>FolderPath="upload/foldername"</td></tr>
                    <tr><td>SaveAs="jpg"</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Row HTML</td>
            <td>
                <table width="100%">
                    <tr><td>Write html in settings for the particular row.</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Sub Grid</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">Attributes</td><td>ShowTotal="false"</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Auto Complete</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">Populate Auto complete On Add</td></tr>
                    <tr><td>In Settings=> PopAcOnAdd="mid" //Here mid is a querystring value </td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Calculte Columns</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">Settings</td></tr>
                    <tr><td>Calculate=columnName1*columnName2</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">JS Automate</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">Populate Same Data</td></tr>
                    <tr><td>If you enter value in one textbox automatically data will be populated in other textbox</td></tr>
                    <tr><td>Set css in parent textbox as popsamedata. and set attributes - popsamedata_target='CLASS OF CHILD TEXTBOX'</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Action Button</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">Css</td></tr>
                    <tr><td>btnaction</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Fill Dropdown</td>
            <td>
                <table width="100%">
                    <tr><td class="bold">settings</td></tr>
                    <tr><td>where=YOUR WHERE CONDITION</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr><td>Column name should end with status, eg. complaintstatus_status</td></tr>
                    <tr><td>Color columns : _textcolor,_backgroundcolor wi</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Multi Checkbox</td>
            <td>
                <table width="100%">
                    <tr><td>Attributes: Column="DisplayColumnName" Columns="NoOfColumns" Height=""</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Radio button list</td>
            <td>
                <table width="100%">
                    <tr><td>Settings: value="comma seperated values"</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="bold" width="200">Unique</td>
            <td>
                <table width="100%">
                    <tr><td>Settings: IsDbUnique="false"</td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr><td>Mobile Setting</td></tr>
                    <tr><td>ismobile="true" colspan="2" class="bold"</td></tr>
                </table>
            </td>
        </tr>
    </table>
  </td>
  </tr>
  <tr><td>
    <asp:TextBox class="nodisplay prefix" ID="h_prefix" runat="server"/>
    <asp:PlaceHolder ID="form" runat="server">
    <table>
    <tr>
      <td class="slabel">Control</td>
      <td>
        <asp:DropDownList ID="ddlControl" runat="server" CssClass="ddlcontrol">
            <asp:ListItem Text="Text Box" Value="Text Box"></asp:ListItem>
            <asp:ListItem Text="Label" Value="Label"></asp:ListItem>
            <asp:ListItem Text="Auto Complete" Value="Auto Complete"></asp:ListItem>
            <asp:ListItem Text="Email Id" Value="Email Id"></asp:ListItem>
            <asp:ListItem Text="Mobile No" Value="Mobile No"></asp:ListItem>
            <asp:ListItem Text="Phone No" Value="Phone No"></asp:ListItem>
            <asp:ListItem Text="Amount" Value="Amount"></asp:ListItem>
            <asp:ListItem Text="Number" Value="Number"></asp:ListItem>
            <asp:ListItem Text="Date" Value="Date"></asp:ListItem>
            <asp:ListItem Text="Date Time" Value="Date Time"></asp:ListItem>
            <asp:ListItem Text="Multi Line" Value="Multi Line"></asp:ListItem>
            <asp:ListItem Text="Html Editor" Value="Html Editor"></asp:ListItem>
            <asp:ListItem Text="Dropdown" Value="Dropdown"></asp:ListItem>
            <asp:ListItem Text="Checkbox" Value="Checkbox"></asp:ListItem>
            <asp:ListItem Text="Multi Checkbox" Value="Multi Checkbox"></asp:ListItem>
            <asp:ListItem Text="Radio Button List" Value="Radio Button List"></asp:ListItem>
            <asp:ListItem Text="File Upload" Value="File Upload"></asp:ListItem>
            <asp:ListItem Text="Section" Value="Section"></asp:ListItem>
            <asp:ListItem Text="Sub Grid" Value="Sub Grid"></asp:ListItem>
            <asp:ListItem Text="Non Editable Grid" Value="Non Editable Grid"></asp:ListItem>
            <asp:ListItem Text="Updateable Grid" Value="Updateable Grid"></asp:ListItem>
            <asp:ListItem Text="Button" Value="Button"></asp:ListItem>
            <asp:ListItem Text="Literal" Value="Literal"></asp:ListItem>
            <asp:ListItem Text="Row HTML" Value="Row HTML"></asp:ListItem>
        </asp:DropDownList>
      </td>    
      <td class="slabel">Label</td>
      <td><asp:TextBox ID="txtlbl" runat="server" CssClass="textbox lbl"/></td>
      <td class="slabel">Column Name</td>
      <td><asp:TextBox ID="txtcolumnname" CssClass="textbox columnname" runat="server"/></td>
      <td>Data Type</td>
      <td>
        <asp:TextBox ID="txtdatatype" runat="server"></asp:TextBox>
      </td>
      <td>Size</td>
      <td><asp:TextBox CssClass="stextbox val-i size" ID="txtsize" runat="server" Text="100" /></td>
      <td>Default Value</td>
      <td><asp:TextBox CssClass="textbox defaultvalue" ID="txtDefaultValue" runat="server"/></td>
    </tr>
    <tr>
      <td class="slabel">Dropdown Module</td>
      <%--<td><asp:DropDownList ID="ddlDropdownmoduleid" runat="server" CssClass="dropdownmoduleid"></asp:DropDownList></td>--%>
      <td><asp:TextBox ID="dropdownmodule" runat="server" CssClass="ac dropdownmodule" m="module" cn="module_modulename"></asp:TextBox>
        <asp:TextBox ID="txtdropdownmoduleid" runat="server" CssClass="hdnac dropdownmoduleid"></asp:TextBox>
      </td>
      <td>Dropdown Module Column</td>
      <td><asp:TextBox ID="txtdropdowncolumn" CssClass="textbox dropdowncolumn" runat="server"/></td>
      <td class="slabel">Dropdown Values</td>
      <td><asp:TextBox CssClass="textbox dropdownvalues" runat="server" ID="txtdropdownvalues"/></td>
      <td class="slabel">Sequence</td>
      <td><asp:TextBox runat="server" CssClass="stextbox sequence val-i" ID="txtsequence"/></td>
      <td class="slabel">Grid Column</td>
      <td><asp:TextBox runat="server" CssClass="textbox gridcolumnname" ID="chkgridcolumnname" runat="server"/></td>
    </tr>
    <tr>
      <td class="slabel">Code Format</td>
      <td><asp:TextBox runat="server" CssClass="textbox codeformat" ID="txtcodeformat" runat="server"/></td>
      <td class="slabel" id="lblcss">CSS</td>
      <td><asp:TextBox runat="server" CssClass="textbox css" ID="txtcss" runat="server"/></td>
      <td class="slabel">Attributes</td>
      <td><asp:TextBox runat="server" CssClass="textbox attributes" ID="txtattributes" runat="server"/></td>
      <td class="slabel">Colspan</td>
      <td>
        <asp:DropDownList ID="ddlcolspan" runat="server">
            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
            <asp:ListItem Text="4" Value="4"></asp:ListItem>
        </asp:DropDownList>
      </td>
      <td class="slabel">Grid Module</td>
      <td><asp:DropDownList ID="ddlsubmoduleid" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="slabel">Row Attributes</td>
      <td><asp:TextBox runat="server" CssClass="textbox trattributes" ID="txttrattributes" runat="server"/></td>
      <td class="slabel" id="lbltooltip">Tooltip</td>
      <td><asp:TextBox runat="server" CssClass="textbox tooltip" ID="chktooltip" runat="server"/></td>
      <td class="slabel">Under Section</td>
      <td><asp:DropDownList ID="ddlsubsectionid" runat="server"></asp:DropDownList></td>
      <td class="slabel">Settings</td>
      <td><asp:TextBox TextMode="MultiLine" runat="server" CssClass="textbox settings" ID="txtsettings" runat="server"/></td>
      <td class="slabel">Mobile Settings</td>
      <td><asp:TextBox TextMode="MultiLine" runat="server" CssClass="textbox mobilesetting" ID="txtmobilesetting" runat="server"/></td>
    </tr>
    <tr>
        <td colspan="10">
            <table>
                <tr>
                    <td><asp:CheckBox CssClass="isleftcolumn" ID="chkisleftcolumn" Text="Is Left Side?" runat="server"/></td>
                    <td><asp:CheckBox CssClass="isrequired" ID="chkisrequired" Text="Is Required?" runat="server"/></td>
                    <td><asp:CheckBox CssClass="isunique" ID="chkisunique" runat="server" Text="Is Unique?"/></td>
                    <td><asp:CheckBox CssClass="ispassword" ID="chkispassword" runat="server" Text="Password?"/></td>
                    <td><asp:CheckBox CssClass="isviewpage" ID="chkisviewpage" runat="server" Text="View Page?"/></td>
                    <td><asp:CheckBox CssClass="isquickadd" ID="chkisquickadd" runat="server" Text="Quick Add?"/></td>
                    <td><asp:CheckBox CssClass="isautocomplete" ID="chkisautocomplete" runat="server" Text="Auto Complete?"/></td>
                    <td><asp:CheckBox CssClass="issearchfield" ID="chkissearchfield" runat="server" Text="Search?"/></td>
                    <td><asp:CheckBox CssClass="isgenerate" ID="chkisgenerate" Checked="true" runat="server" Text="Generate?"/></td>
                    <td><asp:CheckBox CssClass="isgeneratelabel" ID="chkisgeneratelabel" Checked="true" runat="server" Text="Generate Label?"/></td>
                    <td><asp:CheckBox CssClass="isvisibleinedit" ID="chkisvisibleinedit" runat="server" Text="Visible in edit?"/></td>
                    <td><asp:CheckBox CssClass="isenableinedit" ID="chkisenableinedit" runat="server" Text="Enable in edit?"/></td>
                    <td><asp:CheckBox CssClass="ismobilecol" ID="ismobilecol" runat="server" Text="Is Mobile Column?"/></td>
                </tr>
            </table>
            
        </td>
    </tr>
    </table>
    </asp:PlaceHolder>
  </td></tr>
  <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
  <tr><td><asp:Label ID="lblWarning" runat="server" CssClass="error"></asp:Label></td></tr>
  <tr>
        <td align="center">
            <table width="100%">
                <tr>
                    <td width="300"><asp:CheckBox ID="chisoverwritexml" runat="server" Text="Overwrite XML"/></td>
                    <td>
                        <table>
                            <tr>
                                <td><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="button save"/>&nbsp;</td>
                                <td><asp:Button ID="btnSaveAndGenerate" runat="server" OnClick="btnSaveAndGenerate_Click" Text="Save and Generate" CssClass="button save"/>&nbsp;</td>
                                <td><asp:Button ID="btnGenerate" runat="server" OnClick="btnGenerate_Click" Text="Generate" CssClass="button"/>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete This Column" CssClass="button delete"/>&nbsp;</td>
                                <td><asp:HyperLink ID="lnkConvert" runat="server" Text="Set Convert" Target="_blank"></asp:HyperLink></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right"><asp:Button ID="btnDeleteModule" runat="server" OnClick="btnDeleteModule_Click" Text="Delete This Module" CssClass="button delete"/></td>
                </tr>
            </table>
        </td>
        
    </tr>  
  <tr>
    <td>
        <uc:GridData ID="gridData" runat="server" Module="columns" EnablePaging="true"/>
    </td>
  </tr>
</table>    
    
</asp:Content>

