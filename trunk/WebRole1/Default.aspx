<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="SecurityTokenVisualizerControl" Namespace="Microsoft.Samples.DPE.Identity.Controls"
    TagPrefix="cc1" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Bienvenido a CuandoToca.com
    </h2>

    <ul>
        <li><a href="ShowEvents.aspx">Ver Eventos</a></li>
        <li><a href="ShowMostVisited.aspx">Los mas Visitados</a></li>    
    </ul>

</asp:Content>

