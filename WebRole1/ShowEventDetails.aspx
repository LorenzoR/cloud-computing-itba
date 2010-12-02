<%@ Page Title="Event Details" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  
    CodeBehind="ShowEventDetails.aspx.cs" Inherits="WebRole1.ShowEventDetails" %>

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

    <hr />

    <div>
    
        <h3>Comentarios</h3>

       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DataList 
                   ID="commentData1" 
                   runat="server" 
                   DataSourceID="ObjectDataSource1">
                    <ItemTemplate>
                        <div class="signature">
                            <div class="signatureImage">
                                <a href="<%# Eval("PhotoUrl") %>" target="_blank">
                                    <img src="<%# Eval("ThumbnailUrl") %>" alt="" style="border:0px"/>
                                </a>
                            </div>
                            <div class="signatureDescription">
                                <div class="signatureName">
                                    <%# Eval("Username") %>
                                </div>
                                <div class="signatureSays">
                                    dice
                                </div>
                                <div class="signatureDate">
                                    <%# ((DateTime)Eval("Timestamp")).ToString("dd-MM-yyyy HH:mm:ss") %>
                                </div>
                                <div class="signatureMessage">
                                    "<%# Eval("Message") %>"
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <asp:Timer 
                    ID="Timer1" 
                    runat="server"
                    Interval="15000"
                    OnTick="Timer1_Tick">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />  
               
    <%-- Data Sources --%>
    <asp:ObjectDataSource runat="server" ID="ObjectDataSource1" 	TypeName="WebRole1.CommentDataSource"
        DataObjectTypeName="WebRole1.EventDataModel" 
        SelectMethod="Select">    
        <SelectParameters>
            <asp:QueryStringParameter Name="RowKey" QueryStringField="RowKey" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    


    </div>

    

    <div>
    
    <dl>
                <dd>
                    <asp:TextBox 
                       ID="NameTextBox" 
                       runat="server" 
                       Visible="false"
                       CssClass="field"/>
                    <asp:RequiredFieldValidator 
                      ID="NameRequiredValidator" 
                      runat="server" 
                      ControlToValidate="NameTextBox"
                      Text="* Campo obligatorio" />
                </dd>
                <dt>
                    <label for="MessageLabel">Nombre:</label>
                </dt>
                <dd>
                    <asp:TextBox 
                       ID="UsernameTextBox" 
                       runat="server" 
                       CssClass="field" />
                    <asp:RequiredFieldValidator 
                       ID="UsernameRequiredValidator" 
                       runat="server" 
                       ControlToValidate="UsernameTextBox"
                       Text="* Campo obligatorio" />
                </dd>
                <dt>
                    <label for="MessageLabel">Mensaje:</label>
                </dt>
                <dd>
                    <asp:TextBox 
                       ID="MessageTextBox" 
                       runat="server" 
                       TextMode="MultiLine" 
                       CssClass="field" />
                    <asp:RequiredFieldValidator 
                       ID="MessageRequiredValidator" 
                       runat="server" 
                       ControlToValidate="MessageTextBox"
                       Text="*" />
                </dd>
                <dt>
                    <label for="FileUpload1">Foto: (opcional)</label></dt>
                <dd>
                    <asp:FileUpload 
                        ID="FileUpload1" 
                        runat="server" 
                        size="16" />
                    <asp:RegularExpressionValidator 
                        ID="PhotoRegularExpressionValidator"
                        runat="server"
                        ControlToValidate="FileUpload1" 
                        ErrorMessage="Only .jpg or .png files are allowed"
                        ValidationExpression="([a-zA-Z\\].*(.jpg|.JPG|.png|.PNG)$)" />
                </dd>
            
                       <asp:Button
                    ID="addButton" 
                    Text="Enviar"
                    onclick="SignButton_Click" 
                    Runat="server"/>
     
            </dl>

    </div>

    <a href="ShowEvents.aspx">Volver</a>
    

</asp:Content>