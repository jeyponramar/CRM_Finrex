<%@ Page Language="C#" MasterPageFile="~/chat/ChatClientMaster.master" AutoEventWireup="true" 
CodeFile="chatfeedback.aspx.cs" Inherits="chat_chatfeedback"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function(){
        $(".imgrating").click(function(){
            $(".hdnrating").val($(this).attr("val"));
            var rid = $(this).attr("val");
            $(".imgrating").css("background-image","url(images/star-gray.png)");
            for(i=1;i<=rid;i++)
            {
                $("#imgrating"+rid).css("background-image","url(images/star.png)");
            }
        });
        $(".imgrating").hover(function(){
            var rid = $(this).attr("val");
            $(".imgrating").css("background-image","url(images/star-gray.png)");
            for(i=1;i<=rid;i++)
            {
                $("#imgrating"+i).css("background-image","url(images/star.png)");
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table cellpadding="0" cellspacing="0" width="100%"> 
     <tr>
        <td style="background-color:#201f1f;height:35px; 
            width:100%;text-align:center;color:#f5f5f5; font-size:14px;">
            Your Rating
            <asp:TextBox ID="hdnRating" runat="server" CssClass="hidden hdnrating" Text="0"></asp:TextBox>
        </td>
    </tr>
    <tr id="trFeedback" runat="server">
        <td style="background-color:#000;color:#fff;padding:5px;">
            <table width="90%">
                <tr>
                    <td>Please provide your valuable rating/feedback to improve our support!</td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>Rating</td>
                </tr>
                <tr><td>
                    <div>
                        <div class="imgrating" val="1" id="imgrating1"></div>
                        <div class="imgrating" val="2" id="imgrating2"></div>
                        <div class="imgrating" val="3" id="imgrating3"></div>
                        <div class="imgrating" val="4" id="imgrating4"></div>
                        <div class="imgrating" val="5" id="imgrating5"></div>
                    </div>
                </td></tr>
                <tr>
                    <td>Feedback</td>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txtFeedback" MaxLength="500" runat="server" TextMode="MultiLine" Width="300" Height="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="center"><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" /></td>
                </tr>

            </table>
        </td>
    </tr>
    <tr id="trMessage" runat="server" visible="false">
        <td align="center" style="padding-top:20px;"><asp:Label ID="lblMessage" runat="server" CssClass="error" Text="Thanks for your rating/feedback!"></asp:Label></td>
    </tr>
    </table>    
</asp:Content>

