<%@ Page Title="Event Details Admin" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  
    CodeBehind="ShowEventDetailsAdmin.aspx.cs" Inherits="WebRole1.ShowEventDetailsAdmin" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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
        SelectMethod="Select" DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update">    
        <SelectParameters>
          <asp:QueryStringParameter Name="RowKey" QueryStringField="RowKey" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>

    </div>

    <div>
    
       <asp:GridView
        id="commentsView"
        DataSourceId="commentData"
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
            <asp:BoundField HeaderText="Nombre" DataField="Username" />
            <asp:BoundField HeaderText="Mensaje" DataField="Message" />
            <asp:BoundField HeaderText="URL Foto" DataField="PhotoUrl" />
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
    <asp:ObjectDataSource runat="server" ID="commentData" 	TypeName="WebRole1.CommentDataSource"
        DataObjectTypeName="WebRole1.CommentDataModel"
        SelectMethod="Select" DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update">    
        <SelectParameters>
            <asp:QueryStringParameter Name="RowKey" QueryStringField="RowKey" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    


    </div>

    <a href="ShowEventsAdmin.aspx">Volver</a>

</asp:Content>