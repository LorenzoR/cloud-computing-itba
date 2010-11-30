<%@ Page Title="Show Token" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ShowToken.aspx.cs" Inherits="WebRole1.ShowToken" %>


<%@ Register Assembly="SecurityTokenVisualizerControl" Namespace="Microsoft.Samples.DPE.Identity.Controls"
    TagPrefix="cc1" %>



<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <cc1:SecurityTokenVisualizerControl ID="SecurityTokenVisualizerControl1" runat="server" />

</asp:Content>