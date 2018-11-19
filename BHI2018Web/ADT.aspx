<%@ Page Title="ADT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ADT.aspx.cs" Inherits="BHI2018Web.Contact" %>

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
            <asp:Button ID="btnDownLoad" runat="server" Text="Download" OnClick="btnDownLoad_Click" />
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
        <asp:GridView ID="gvEP" runat="server" AutoGenerateColumns="False" DataKeyNames="Element ID" DataSourceID="RetrieveElements">
            <Columns>
                <asp:BoundField DataField="Element ID" HeaderText="Element ID" InsertVisible="False" ReadOnly="True" SortExpression="Element ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="RetrieveElements" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="Load Elements and PerformanceCriteria for ADT" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txbUnitCode" DefaultValue="" Name="UnitCode" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div class="col-md-4">
        <p>Foundation Skills:</p>
        <asp:GridView ID="gvFS" runat="server" AutoGenerateColumns="False" DataSourceID="Retrieve">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Performance Criterias" HeaderText="Performance Criterias" SortExpression="Performance Criterias" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="Retrieve" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="Load FoundationSkills for ADT" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txbUnitCode" Name="UnitCode" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
        
  </div>
</asp:Content>
