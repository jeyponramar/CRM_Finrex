<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditAuditControl.ascx.cs" Inherits="usercontrols_AddEditAuditControl" %>
<table width='100%' cellspacing="0" cellpadding="0">
    <tr>
        <td width="200" style="vertical-align:top;background-color:#fff;border-right:solid 1px #eee;">
            <table width="100%">
                 <tr id="trsteps" runat="server">
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="ltsteps" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align:top;">
            <table width="100%">
                 <tr>
                    <td class='page-inner2' style="padding-bottom:100px;padding-top:10px;">
                    <table width='100%'><tr><td class='page-title2'><asp:Label ID="lbltitle" runat="server" Text="New BankScan"></asp:Label></td></tr>
                        <tr><td align="center">
                            <div class="jq-lblmessage"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></div>

                            <asp:TextBox ID="hdnid" runat="server" CssClass="jq-id hidden" Text="0"></asp:TextBox>
                            <asp:TextBox ID="hdnispagemodified" runat="server" CssClass="jq-ispagemodified hidden" Text="0"></asp:TextBox>
                            <asp:TextBox ID="hdnissaveenabled" runat="server" CssClass="jq-issaveenabled hidden" Text="1"></asp:TextBox>
                            <asp:TextBox ID="hdnstage" runat="server" CssClass="jq-stage hidden" Text="0"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trhomelink" runat="server" visible="false"><td align="center"><a href="index.aspx">Home</a></td></tr>
                        <tr id="trpagecontent" runat="server">
                            <td>
                            <asp:PlaceHolder ID="plstep1" runat="server" Visible="false">
                            <table width="100%" cellspacing="10">
                                <tr id="trcode" runat="server" visible="false">
                                    <td>Code</td><td><asp:Label ID="lblCode" runat="server" CssClass="bold"></asp:Label></td>
                                </tr>
                                <tr id="trdate" runat="server" visible="false">
                                    <td class="label">Date</td><td><asp:Label ID="lbldate" runat="server"></asp:Label></td>
                                </tr>
                                <tr id="trclient" runat="server" visible="false">
                                    <td>Company</td><td><asp:Label ID="lblcompany" runat="server" CssClass="bold"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Customer</td>
                                    <td>    
                                        <asp:TextBox ID="client" runat="server" CssClass="ac txtac" m="client" cn="customername"></asp:TextBox>
                                        <asp:TextBox ID="txtclientid" runat="server" CssClass="hdnac"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="label">Business Into</td><td><asp:TextBox ID="txtbuesinessinto" runat="server" CssClass="textbox"></asp:TextBox></td>
                                </tr>--%>
                                <tr>
                                    <td>Bank Name</td><td><asp:DropDownList ID="ddlbank" runat="server" CssClass="ddl"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Branch</td><td><asp:TextBox ID="txtbankbranch" runat="server" CssClass="textbox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Currencies</td>
                                    <td>
                                        <asp:DropDownList ID="ddlcurrencies" runat="server" CssClass="ddl ddlmultiselect"></asp:DropDownList>
                                        <asp:TextBox ID="hdncurrencyids" runat="server" CssClass="hdn"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trstatus" runat="server" visible="false"><td>Status</td><td><asp:Literal ID="ltstatus" runat="server"></asp:Literal></td></tr>
                                <tr id="trisbanklettersent" runat="server" visible="false">
                                    <td>Bank Letter Sent Date</td>
                                    <td><asp:Label ID="lblbanklettersentdate" runat="server" CssClass="bold"></asp:Label></td>
                                </tr>
                                <tr id="trclosedon" runat="server" visible="false">
                                    <td>Closed Date</td>
                                    <td><asp:Label ID="lblclosedon" runat="server" CssClass="bold"></asp:Label></td>
                                </tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr>
                                    <td></td><td><asp:Button ID="btnaddaudit" runat="server" Text="Continue" CssClass="button" OnClick="btnaddaudit_Click"/>
                                    &nbsp;<asp:Button ID="btnReopen" runat="server" Text="ReOpen" CssClass="button" Visible="false"/>
                                    </td>
                                </tr>
                            </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plstep2" runat="server" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="10">
                                                <tr><td>Industry</td><td><asp:TextBox ID="txtindustry" runat="server" CssClass="textbox"></asp:TextBox></td></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="10">
                                                <tr>
                                                    <td class="page-stitle" colspan="2">Last 3 Years Turnover in Rs. in Cr</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Literal ID="ltyearlyturnover" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="10">
                                                <tr>
                                                    <td class="page-stitle" colspan="2">Last Year Annual Turnover Currency-wise</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Literal ID="ltyearlyrunovercurrency" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align="center" cellspacing="20">
                                                <tr>
                                                    <td><asp:Button ID="btnprevstep2" runat="server" Text="Previous" CssClass="button" OnClick="btnprevstep2_Click"/></td>       
                                                    <td><asp:Button ID="btnsave2" runat="server" Text="Save" CssClass="button jq-btnsavebankaudit"/></td>       
                                                    <td><asp:Button ID="btnnextstep2" runat="server" Text="Next" CssClass="button jq-savebankaudit" OnClick="btnnextstep2_Click"/></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plstep3" runat="server" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="10">
                                                <tr>
                                                    <td class="page-stitle" colspan="2">Total Banking Cost Last Year </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Literal ID="ltyearlybankingcost" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color:#fafafa;padding:10px;">
                                                        <asp:Literal ID="ltbankingdetails" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="page-stitle" colspan="2">Bank Conversion Margin/Charges: </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Literal ID="ltbankingmargin" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table align="center" cellspacing="20">
                                                            <tr>
                                                                <td><asp:Button ID="btnprevstep3" runat="server" Text="Previous" CssClass="button" OnClick="btnprevstep3_Click"/></td>       
                                                                <td><asp:Button ID="btnsave3" runat="server" Text="Save" CssClass="button jq-btnsavebankaudit"/></td>       
                                                                <td><asp:Button ID="btnnextstep3" runat="server" Text="Save and Continue" CssClass="button jq-savebankaudit" OnClick="btnnextstep3_Click"/></td>       
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                 </table>
                             </asp:PlaceHolder>
                             <asp:PlaceHolder ID="plstep4" runat="server" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="10">
                                                <tr>
                                                    <td><table><tr><td><asp:Literal ID="ltquestionairetabs" runat="server"></asp:Literal></td></tr></table></td>
                                                </tr>
                                                <tr><td>&nbsp;</td></tr>
                                                <tr>
                                                    <td class="page-stitle" colspan="2"><asp:Label ID="lblquestiontitle" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Literal ID="ltquestionnarie" runat="server"></asp:Literal></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table align="center" cellspacing="20">
                                                            <tr>
                                                                <td><asp:Button ID="btnprevstep4" runat="server" Text="Previous" CssClass="button" OnClick="btnprevstep4_Click"/></td>       
                                                                <td><asp:Button ID="btnsave4" runat="server" Text="Save" CssClass="button jq-btnsavebankaudit"/></td>       
                                                                <td><asp:Button ID="btnnextstep4" runat="server" Text="Save and Continue" CssClass="button jq-savebankaudit" OnClick="btnnextstep4_Click"/></td>       
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                             </table>
                                         </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plstep5" runat="server" Visible="false">
                                <table width="100%" cellspacing="10">
                                    <tr>
                                        <td><asp:Literal ID="ltdocuments" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align="center" cellspacing="20">
                                                <tr>
                                                    <td width="150"><asp:Button ID="btnprevstep5" runat="server" Text="Previous" CssClass="button" OnClick="btnprevstep5_Click"/></td>       
                                                    <td><asp:Button ID="btnnextstep5" runat="server" Text="Next" CssClass="button" OnClick="btnnextstep5_Click"/></td>       
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"><asp:Button ID="btnsendforreview_documents" style="background-color:#F58730;" Visible="false" runat="server" Text="Send for Review" 
                                        CssClass="button jq-btnsendforreview" OnClick="btnsendforreview_fromdoc_Click"/></td>   
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plstep6" runat="server" Visible="false">
                                <table width="100%" cellspacing="10">
                                    <tr>
                                        <td><asp:Literal ID="ltbankletter" runat="server"></asp:Literal></td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <table align="center" cellspacing="20">
                                                <tr>
                                                    <td><asp:Button ID="btnprevstep6" runat="server" Text="Previous" CssClass="button" OnClick="btnprevstep6_Click"/></td>       
                                                    <td><asp:Button ID="btnsendbankletter" style="background-color:#039e65;" Visible="false" runat="server" Text="Send Bank Letter" confirmmsg='Are you sure you want to send bank letter?' 
                                                                CssClass="confirm button" OnClick="btnsendbankletter_Click"/></td>     
                                                    <td><asp:Button ID="btnnextstep6" runat="server" Text="Next" CssClass="button" OnClick="btnnextstep6_Click"/></td>       
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plstep7" runat="server" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="10">
                                                <tr>
                                                    <td>
                                                        <table class='grid-ui' border='1' cellpadding='7' style='width:auto;'>
                                                            <tr class='grid-ui-header'>
                                                                <td>SUMMARY</td><td>Expected Savings</td><td>Remarks</td>
                                                            </tr>
                                                            <tr>
                                                                <td>Forex Conversion</td>
                                                                <td><asp:TextBox runat="server" ID="txtforexexpectedsaving" CssClass="mbox" ></asp:TextBox></td>
                                                                <td><asp:TextBox runat="server" ID="txtforexremarks" TextMode="MultiLine" CssClass="textarea" ></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Forward Contract</td>
                                                                <td><asp:TextBox runat="server" ID="txtforwardcontractexpectedsaving" CssClass="mbox" ></asp:TextBox></td>
                                                                <td><asp:TextBox runat="server" ID="txtforwardcontractremarks" TextMode="MultiLine" CssClass="textarea" ></asp:TextBox></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trclientramrks" runat="server" visible="false">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>Advisor Last Remarks : </td>
                                                                <td><asp:Label ID="lblfinrexremarks" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr><td>&nbsp;</td></tr>
                                                            <tr>
                                                                <td style="border: solid 1px #aaa;padding:10px;background-color:#ffcece;" colspan="2">
                                                                    <table>
                                                                        <tr>
                                                                            <td>Your overall Remarks</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtclientremarks" runat="server" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trfinrexremarks" runat="server" visible="false">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>Client Last Remarks : </td>
                                                                <td><asp:Label ID="lblclientremarks" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr><td>&nbsp;</td></tr>
                                                            <tr>
                                                                <td style="border: solid 1px #aaa;padding:10px;background-color:#a1fcd9;" colspan="2">
                                                                    <table>
                                                                        <tr>
                                                                            <td>Your overall Remarks</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtfinrexremarks" runat="server" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table align="center" cellpadding="10">
                                                            <tr>
                                                                <td><asp:Button ID="btnprevstep7" runat="server" Text="Previous" CssClass="button" OnClick="btnprevstep7_Click"/></td>       
                                                                <td><asp:Button ID="btnfinalsave" runat="server" Text="Save" CssClass="button" OnClick="btnfinalsave_Click"/></td>       
                                                                <td><asp:Button ID="btnsendforreview" style="background-color:#F58730;" Visible="false" runat="server" Text="Send for Review" CssClass="button jq-btnsendforreview" OnClick="btnsendforreview_Click"/></td>   
                                                                <td><asp:Button ID="btnsubmitreview" style="background-color:#F58730;" Visible="false" runat="server" Text="Submit Review" CssClass="button jq-btnsubmitreview" OnClick="btnsubmitreview_Click"/></td>     
                                                                <td><asp:Button ID="btncompleteaudit" style="background-color:#039e65;" Visible="false" runat="server" Text="Close Audit" CssClass="button jq-btncompleteaudit" OnClick="btncompleteaudit_Click"/></td>     
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr><td><asp:Literal ID="ltcomment" runat="server"></asp:Literal></td></tr>
                                             </table>
                                         </td>
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

<div id="divaddcustomlabel-annualturnover" class="dialog" style="width:400px;height:200px;">
    <table width="100%">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>Header Label</td>
                        <td><input type="text" class="textbox jq-txtcustomlabel-annualturnover" name="txtcustomlabel-annualturnover"/></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td></td>
                        <td><input type="button" id="jq-btnsavecustomlebal" value="Save" class="button"/></td>
                    </tr>
                </table>
            </td>
            
        </tr>
    </table>
</div>
<div id="divaddbankcost-currencylabel" class="dialog" style="width:400px;height:200px;">
    <table width="100%">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>Currency-wise</td>
                        <td><input type="text" class="textbox jq-txtcurrencywiselabel-bankcostdetails" name="txtcurrencywiselabel-bankcostdetails"/></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td></td>
                        <td><input type="button" id="jq-btnsavecurrencywiselabelbankcost" value="Save" class="button"/></td>
                    </tr>
                </table>
            </td>
            
        </tr>
    </table>
</div>
<div id="divbankscantabdialog" class="dialog" style="color:#000;width:600px;height:300px;">
    <table width="100%" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table align="center">
                    <tr>
                        <td>
                            <div style="border:solid 2px #eee;border-radius:5px;padding:10px;">
                                <table>
                                    <tr>
                                        <td><div style="background-color: #cbedcb;padding: 10px 30px;font-weight: bold;">Completed tabs</div></td>
                                        <td><div style="background-color: #ffe0ce;padding: 10px 30px;font-weight: bold;margin-left:10px;margin-right:10px;">Current tab</div></td>
                                        <td><div style="background-color: #eee;padding: 10px 30px;font-weight: bold;">Yet to complete tabs</div></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding:10px;"><i><u>Note :</u> Green tick <img src="images/tick2.png" width="10"/> denotes the particular upload/step/process is been successfully done/completed.</i></td>
        </tr>
        <tr>
            <td align="right" style="padding:20px;"><input type="button" class="button closedialog" value="Got it" /></td>
        </tr>
    </table>
</div>