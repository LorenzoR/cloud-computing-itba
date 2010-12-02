<%@ Page Title="Events" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ShowEvents.aspx.cs" Inherits="WebRole1.ShowEvents" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div>
        <asp:GridView
        id="eventsView"
        DataSourceId="eventData"
        DataKeyNames="RowKey"
        AllowPaging="False"
        AutoGenerateColumns="False"
        GridLines="Vertical"
        Runat="server" 
        BackColor="White" ForeColor="Black"
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <Columns>
            <asp:BoundField HeaderText="Artista" DataField="Artist" />
            <asp:BoundField HeaderText="Lugar" DataField="Place" />
            <asp:BoundField HeaderText="Descripcion" DataField="Description" />
            <asp:BoundField HeaderText="Fecha" DataField="EventDate" />
            <asp:BoundField HeaderText="#Visitas" DataField="VisitCounter" />
            <asp:HyperLinkField HeaderText="View Event" Text="Ver" DataNavigateUrlFields="RowKey"
                        DataNavigateUrlFormatString="ShowEventDetails.aspx?RowKey={0}" />
        </Columns>
        <RowStyle BackColor="#F7F7DE" />
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>    
        <br />

    

    <%-- Data Sources --%>
    <asp:ObjectDataSource runat="server" ID="eventData" 	TypeName="WebRole1.EventDataSource"
        DataObjectTypeName="WebRole1.EventDataModel" 
        SelectMethod="Select">    
    </asp:ObjectDataSource>

    </div>

    <a href="Default.aspx">Inicio</a>

</asp:Content>