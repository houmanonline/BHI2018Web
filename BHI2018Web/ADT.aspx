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
            <asp:Label ID="lblUnitName" runat="server" Text="Unit Name: "></asp:Label>  <asp:Label ID="lblUnitNameDisplay" runat="server" Text="Show unit name here..."></asp:Label><p></p>
            <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
        </div>
        <div class="col-md-4">

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
    <div class="col">
        <p>Elements and Performance Criteria:</p>
        <asp:GridView ID="GridView2" runat="server"></asp:GridView>
    </div>
    <div class="col">
        <p>Foundation Skills:</p>
        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    </div>
        
  </div>
</asp:Content>
