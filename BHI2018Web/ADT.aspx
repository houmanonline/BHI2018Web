<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ADT.aspx.cs" Inherits="BHI2018Web.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
  <h1 class="display-4">Create an ADT</h1>
  <p class="lead">Input Unit Code to create an ADT file.</p>
  <hr class="my-4">
</div>

    <div class="row">
        <div class="col-md-4">
            <asp:Label ID="lblUnitCode" runat="server" Text="Unit Code: "></asp:Label>  &nbsp<asp:TextBox ID="txbUnitCode" runat="server"></asp:TextBox><p></p>
            <asp:Label ID="lblUnitName" runat="server" Text="Unit Name: "></asp:Label>  <asp:Label ID="lblUnitNameDisplay" runat="server" Text="Show unit name here..."></asp:Label><p>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
            </p>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblMessage" runat="server" Text="System Message: "></asp:Label><p></p>
            <asp:Label ID="lblMessageDisplay" runat="server" BackColor="#FFCC99"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:Button ID="btnCreateADT" runat="server" Text="Create ADT" OnClick="btnCreateADT_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <p>Application:</p>
            <asp:ListBox ID="lsbApp" runat="server" Height="150px" Width="300px"></asp:ListBox>
        </div>
        <div class="col-md-3">
            <p>Performance Evidence:</p>
            <asp:ListBox ID="lsbPE" runat="server" Height="150px" Width="300px"></asp:ListBox>
        </div>
        <div class="col-md-3">
           <p>Knowledge Evidence:</p>
            <asp:ListBox ID="lsbKE" runat="server" Height="150px" Width="300px"></asp:ListBox>
        </div>
        <div class="col-md-3">
           <p>Assessment Conditions:</p>
            <asp:ListBox ID="lsbAC" runat="server" Height="150px" Width="300px"></asp:ListBox>
        </div>
    </div>
    <div class="row">
    <div class="col-md-4">
        <p>Elements and Performance Criteria:</p>
        <asp:GridView ID="gvEP" runat="server" AutoGenerateColumns="False" DataKeyNames="Element ID" DataSourceID="Elements">
            <Columns>
                <asp:BoundField DataField="Element ID" HeaderText="Element ID" InsertVisible="False" ReadOnly="True" SortExpression="Element ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Unit_Unit Code" HeaderText="Unit_Unit Code" SortExpression="Unit_Unit Code" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="Elements" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Elements]"></asp:SqlDataSource>
    </div>
    <div class="col-md-4">
        <p>Foundation Skills:</p>
        <asp:GridView ID="gvFS" runat="server" AutoGenerateColumns="False" DataKeyNames="Foundation Skill ID" DataSourceID="FoundationSkills">
            <Columns>
                <asp:BoundField DataField="Foundation Skill ID" HeaderText="Foundation Skill ID" InsertVisible="False" ReadOnly="True" SortExpression="Foundation Skill ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Performance Criterias" HeaderText="Performance Criterias" SortExpression="Performance Criterias" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="Unit_Unit Code" HeaderText="Unit_Unit Code" SortExpression="Unit_Unit Code" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="FoundationSkills" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Foundation Skills]"></asp:SqlDataSource>
    </div>
        
  </div>
</asp:Content>
