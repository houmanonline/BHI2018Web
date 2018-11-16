<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BHI2018Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Welcome</h1>
        <p class="lead">This is a website that can generate Unit plan and ADT.</p>
        <p><img src="Rescource/logo.png"> </p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Create ADT document</h2>
            <p>
                Using this function can help you grabbing data from Training.gov.au and generating ADT document automaticlly.
            </p>
            <p>
                <a class="btn btn-default" href="ADT">Go to ADT Page &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Create Unit Plan document</h2>
            <p>
                Using this function can help you fill the unit plan template and store the information you filled into database.
            </p>
            <p>
                <a class="btn btn-default" href="UP">Go to UP Page &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Help</h2>
            <p>
                If you have trouble to use this website, you can download user manual by clicking the link below
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">User Manual &raquo;</a>
            </p>
        </div>
    </div>
</asp:Content>