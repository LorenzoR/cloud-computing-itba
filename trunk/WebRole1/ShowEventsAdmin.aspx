<%@ Page Title="Events ABM" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="ShowEventsAdmin.aspx.cs" Inherits="WebRole1.ShowEventsAdmin" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div>
        <asp:GridView
        id="eventsView"
        DataSourceId="eventData"
        DataKeyNames="PartitionKey, RowKey"
        AllowPaging="False"
        AutoGenerateColumns="False"
        GridLines="Vertical"
        Runat="server" 
        BackColor="White" ForeColor="Black"
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <Columns>
            <asp:CommandField ShowDeleteButton="true"  />
            <asp:CommandField ShowEditButton="true"  />
            <asp:BoundField HeaderText="Artista" DataField="Artist" />
            <asp:BoundField HeaderText="Lugar" DataField="Place" />
            <asp:BoundField HeaderText="Descripcion" DataField="Description" />
            <asp:BoundField HeaderText="Fecha" DataField="EventDate" />
            <asp:BoundField HeaderText="#Visitas" DataField="VisitCounter" />
            <asp:HyperLinkField HeaderText="Ver Evento" Text="Ver" DataNavigateUrlFields="RowKey"
                        DataNavigateUrlFormatString="ShowEventDetailsAdmin.aspx?RowKey={0}" />
        </Columns>
        <RowStyle BackColor="#F7F7DE" />
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>    
        <br />        
    <asp:FormView
        id="frmAddComment"
        DataSourceId="eventData"
        DefaultMode="Insert"
        Runat="server">
        <InsertItemTemplate>
            <asp:Label
                    id="artistLabel"
                    Text="Artista:"
                    AssociatedControlID="artistBox"
                    Runat="server" />
            <asp:TextBox
                    id="artistBox"
                    Text='<%# Bind("Artist") %>'
                    Runat="server" />
            <br />
            <asp:Label
                    id="placeLabel"
                    Text="Lugar:"
                    AssociatedControlID="placeBox"
                    Runat="server" />
            <asp:TextBox
                    id="placeBox"
                    Text='<%# Bind("Place") %>'
                    Runat="server" />
            <br />
            <asp:Label
                    id="descriptionLabel"
                    Text="Descripcion:"
                    AssociatedControlID="descriptionBox"
                    Runat="server" />
            <asp:TextBox
                    id="descriptionBox"
                    Wrap="True"
                    TextMode="MultiLine"
                    Text='<%# Bind("Description") %>'
                    Runat="server" />
            <br />
            <asp:Label
                    id="DateLabel"
                    Text="Fecha:"
                    AssociatedControlID="dateBox"
                    Runat="server" />
            <asp:TextBox
                    id="dateBox"
                    Text='<%# Bind("EventDate") %>'
                    Runat="server" />
            <br />

            <asp:Button
                    id="insertButton"
                    Text="Enviar"
                    CommandName="Insert"
                    Runat="server"
                    onclick="btnTweet_Click"/>
        </InsertItemTemplate>
    </asp:FormView>
    <%-- Data Sources --%>
    <asp:ObjectDataSource runat="server" ID="eventData" 	TypeName="WebRole1.EventDataSource"
        DataObjectTypeName="WebRole1.EventDataModel" 
        SelectMethod="Select" DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update">    
    </asp:ObjectDataSource>

    </div>

    <ul>
        <li><a href="ShowEvents.aspx">Ver Eventos</a></li>
        <li><a href="ShowMostVisited.aspx">Los mas Visitados</a></li>    
    </ul>

</asp:Content>