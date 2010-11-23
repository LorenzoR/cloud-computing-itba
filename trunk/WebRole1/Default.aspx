<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebRole1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView
        id="eventsView"
        DataSourceId="eventData"
        DataKeyNames="PartitionKey"
        AllowPaging="False"
        AutoGenerateColumns="True"
        GridLines="Vertical"
        Runat="server" 
        BackColor="White" ForeColor="Black"
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <Columns>
            <asp:CommandField ShowDeleteButton="true"  />
            <asp:CommandField ShowEditButton="true"  />
            <asp:BoundField HeaderText="Artist" DataField="Artist" />
            <asp:BoundField HeaderText="Place" DataField="Place" />
            <asp:BoundField HeaderText="Description" DataField="Description" />
            <asp:BoundField HeaderText="EventDate" DataField="EventDate" />
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
        id="frmAdd"
        DataSourceId="eventData"
        DefaultMode="Insert"
        Runat="server">
        <InsertItemTemplate>
            <asp:Label
                    id="artistLabel"
                    Text="Artist:"
                    AssociatedControlID="artistBox"
                    Runat="server" />
            <asp:TextBox
                    id="artistBox"
                    Text='<%# Bind("Artist") %>'
                    Runat="server" />
            <br />
            <asp:Label
                    id="placeLabel"
                    Text="Place:"
                    AssociatedControlID="placeBox"
                    Runat="server" />
            <asp:TextBox
                    id="placeBox"
                    Text='<%# Bind("Place") %>'
                    Runat="server" />
            <br />
            <asp:Label
                    id="descriptionLabel"
                    Text="Description:"
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
                    id="CounterLabel"
                    Text="Counter:"
                    AssociatedControlID="counterBox"
                    Runat="server" />
            <asp:TextBox
                    id="counterBox"
                    Text='<%# Bind("VisitCounter") %>'
                    Runat="server" />
            <br />
            <asp:Label
                    id="DateLabel"
                    Text="Date:"
                    AssociatedControlID="dateBox"
                    Runat="server" />
            <asp:TextBox
                    id="dateBox"
                    Text='<%# Bind("EventDate") %>'
                    Runat="server" />
            <br />

            <asp:Button
                    id="insertButton"
                    Text="Add"
                    CommandName="Insert"
                    Runat="server"/>
        </InsertItemTemplate>
    </asp:FormView>
    <%-- Data Sources --%>
    <asp:ObjectDataSource runat="server" ID="eventData" 	TypeName="WebRole1.EventDataSource"
        DataObjectTypeName="WebRole1.EventDataModel" 
        SelectMethod="Select" DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update">    
    </asp:ObjectDataSource>

    </div>
    </form>

    <a href="ShowEvents.aspx">Seccion para Usuarios</a>

</body>
</html>