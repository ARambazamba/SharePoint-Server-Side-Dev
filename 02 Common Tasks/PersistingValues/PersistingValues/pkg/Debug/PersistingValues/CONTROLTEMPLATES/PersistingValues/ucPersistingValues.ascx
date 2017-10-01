<%@ Assembly Name="PersistValues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e2bf21b6205b4482" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPersistingValues.ascx.cs" Inherits="PersistingValues.CONTROLTEMPLATES.PersistingValues.ucPersistingValues" %>
<table style="width: 100%">
    <tr>
        <td><asp:Label ID="lUser" runat="server" Text="User"></asp:Label></td>
        <td><asp:TextBox ID="txtUser" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lPw" runat="server" Text="Passwort"></asp:Label></td>
        <td><asp:TextBox ID="txtPassword" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lPw0" runat="server" Text="ListParameter"></asp:Label></td>
        <td><asp:TextBox ID="txtListParameter" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:LinkButton ID="bSubmit" OnClick="SaveTextboxValue" runat="server">Speichern</asp:LinkButton></td>

    </tr>
</table>

