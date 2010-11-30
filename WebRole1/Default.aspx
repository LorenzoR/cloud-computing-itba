<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="_Default" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ASP.NET!
    </h2>

    <ul>
        <asp:Panel Visible="false" CssClass="secretContent" runat="server" ID="SecretContent">
            <li><a href="ShowEventsAdmin.aspx">Seccion para Administradores</a></li>
        </asp:Panel>
        <li><a href="ShowEvents.aspx">Seccion para Usuarios</a></li>
        <li><a href="ShowMostVisited.aspx">Los mas Visitados</a></li>    
    </ul>

</asp:Content>