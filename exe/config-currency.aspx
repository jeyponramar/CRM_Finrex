<%@ Page Language="C#" MasterPageFile="~/exe/ExeMasterPage.master" AutoEventWireup="true" CodeFile="config-currency.aspx.cs" 
Inherits="exe_config_currency" Title="Finstation - Config Currency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function(){
        $(".expance-collapse-data").hide();
        $(".expance-collapse-data:first").show();
        $(".jq-expance-collapse:first").removeClass("expance-collapse-col").addClass("expance-collapse-exp");
        $(".jq-expance-collapse").click(function(){
            var div = $(this).closest(".jq-expand-collapse-panel");
            div.find(".jq-expance-collapse").removeClass("expance-collapse-exp").addClass("expance-collapse-col");
            if($(this).next().is(":visible"))
            {
                $(this).next().hide();
                $(this).removeClass("expance-collapse-exp").addClass("expance-collapse-col");
            }
            else
            {
                div.find(".expance-collapse-data").hide();
                $(this).next().show();
                $(this).removeClass("expance-collapse-col").addClass("expance-collapse-exp");
            }
        });
        $(".jq-save").click(function(){
            var currencyTypes = "";
            var currencies = "NA";
            var onlycurrencies = "";
            $("#divcurrencies").find(".jq-expance-collapse").each(function(){
                var ctypeId = $(this).attr("ctid");
                if(currencyTypes == "")
                {
                    currencyTypes = ctypeId;
                }
                else
                {
                    currencyTypes += "," + ctypeId;
                }
                var currency = "";
                $(this).next().find(".jq-chk-userconfig-currency").each(function(){
                    if($(this).is(":checked"))
                    {
                        if(currency == "")
                        {
                            currency = $(this).attr("cid");
                        }
                        else
                        {
                            currency += "," + $(this).attr("cid");
                        }
                        if(onlycurrencies == "")
                        {
                            onlycurrencies = $(this).attr("cid");
                        }
                        else
                        {
                            onlycurrencies += "," + $(this).attr("cid");
                        }
                    }
                });
                if(currencies == "NA")
                {
                    currencies = currency;
                }
                else
                {
                    currencies += "|" + currency;
                }
            });
            var arr = onlycurrencies.split(',');
            if(onlycurrencies == "")
            {
                alert("Please choose any item.");
                return false;
            }
            var maxCurrencyCount = parseInt($(".jq-expance-collapse:first").attr("maxcurrency"));
            if(arr.length > maxCurrencyCount)
            {
                alert("You can not choose more than "+maxCurrencyCount+" items.");
                return false;
            }
            var columns = "";
            var totcolumnsCount = parseInt($(".jq-currency-cols").attr("totcolumns"));
            $(".jq-chk-userconfig-currency-cols").each(function(){
                if($(this).is(":checked"))
                {
                    var c = $(this).attr("col");
                    if(columns == "")
                    {
                        columns = c;
                    }
                    else
                    {
                        columns += "," + c;
                    }
                }
            });
            var arrcols = columns.split(',');
            if(arrcols.length != totcolumnsCount)
            {
                alert("Choose " + totcolumnsCount + " columns!");
                return false;
            }
            $(".jq-columns").val(columns);
            currencies = currencyTypes + "~" + currencies;
            $(".jq-currencies").val(currencies);
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr>
        <td>
            <div style="width:100%;height:500px;overflow-y:scroll;padding-bottom:50px" id="divcurrencies">
                <asp:TextBox ID="hdncurrencies" runat="server" CssClass="hidden jq-currencies"></asp:TextBox>
                <asp:TextBox ID="hdncolumns" runat="server" CssClass="hidden jq-columns"></asp:TextBox>
                <table width="100%">
                    <tr><td><asp:Literal ID="ltcurrencies" runat="server"></asp:Literal></td></tr>
                    <tr><td align="center">
                        <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="cancelbtn" OnClick="btncancel_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnsubmit" runat="server" Text="Save" CssClass="btn jq-save" OnClick="btnsubmit_Click" />
                     </td></tr>
                </table>
            </div>
        </td>
    </tr>
</table>
</asp:Content>

