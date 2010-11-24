<%@ Page Title="Most Visited" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="ShowMostVisited.aspx.cs" Inherits="WebRole1.ShowMostVisited" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div>
        <asp:GridView
        id="eventsView"
        DataSourceId="eventData"
        DataKeyNames="PartitionKey"
        AllowPaging="False"
        AutoGenerateColumns="False"
        GridLines="Vertical"
        Runat="server" 
        BackColor="White" ForeColor="Black"
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <Columns>
            <asp:BoundField HeaderText="Artist" DataField="Artist" />
            <asp:BoundField HeaderText="Place" DataField="Place" />
            <asp:BoundField HeaderText="Description" DataField="Description" />
            <asp:BoundField HeaderText="EventDate" DataField="EventDate" />
            <asp:HyperLinkField HeaderText="View Event" Text="View" DataNavigateUrlFields="PartitionKey"
                        DataNavigateUrlFormatString="ShowEventDetails.aspx?PartitionKey={0}" />
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
        SelectMethod="SelectMostVisited">    
    </asp:ObjectDataSource>

    </div>

    <a href="Default.aspx">Inicio</a>

</asp:Content>