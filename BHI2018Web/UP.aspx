<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UP.aspx.cs" Inherits="BHI2018Web.About" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link href="content/UnitPlan.css" rel="stylesheet">
<div class="jumbotron">
  <h1 class="display-4">Create an Unit Plan</h1>
  <p class="lead">Fill all the text fields too create a unit plan.</p>
  <hr class="my-4">
</div>
<div class="Studentinformation">
    <h2>Unit Plan Part 1 Student information</h2>
    <h3>Information about your Unit</h3>
    <table class="table">
        <tr>
            <td>Your Qualification: Code and Title</td>
            <td><asp:TextBox ID="txbCourseCodeTitle" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>            
        </tr>
        <tr>
            <td>Your Unit/s : Code and Title</td>
            <td><asp:TextBox ID="txbUnitCodeTitle" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>            
        </tr>
        <tr>
            <td>Topics you will be learning</td>
            <td><asp:TextBox ID="txbTopics" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>            
        </tr>
        <tr>
            <td>What do you need to bring to class?</td>
            <td><asp:TextBox ID="txbBring" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>            
        </tr>
        <tr>
            <td>Date your unit/s starts</td>
            <td><asp:TextBox ID="txbStart" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Date your unit/s finishes</td>
            <td><asp:TextBox ID="txbFinish" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Class dates and times for this unit/s</td>
            <td><asp:TextBox ID="txbDateTime" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>            
        </tr>
        <tr>
            <td>Building and Room for your unit/s</td>
            <td><asp:TextBox ID="txbClassroom" Cssclass ="Textboxes1" runat="server"></asp:TextBox></td>            
        </tr>
    </table>
</div>
      <hr class="my-4">
<div class="LearningHours">
    <table class="table">
        <tr>
            <td><b>Your Learning Program</b></td>
            <td><b>Hours</b></td>
        </tr>
        <tr>
            <td>Timetabled classes/tutorials with a teacher</td>
            <td><asp:TextBox ID="txbLearningProgram1" Cssclass ="Textboxes2" runat="server"></asp:TextBox></td> 
        </tr>
        <tr>
            <td>Timetabled in class or timetabled workplace based assessment</td>
            <td><asp:TextBox ID="txbLearningProgram2" Cssclass ="Textboxes2" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Timetabled online student support</td>
            <td><asp:TextBox ID="txbLearningProgram3" Cssclass ="Textboxes2" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Practical placement/Practicum/Workplace based training</td>
            <td><asp:TextBox ID="txbLearningProgram4" Cssclass ="Textboxes2" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Self Directed learning and assessment</td>
            <td><asp:TextBox ID="txbLearningProgram5" Cssclass ="Textboxes2" runat="server"></asp:TextBox></td>
        </tr>
        </table>
    </div>
    <hr class="my-4">
    <div class="AssessmentTasks">
        <h3>Your Assessment Tasks</h3>
        <asp:GridView ID="gvAssessmentTask" runat="server" ShowFooter="True" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDeleting="grvAssessmentTask_RowDeleting"
                 Width="97%" Height="16px" Style="text-align: left" OnSelectedIndexChanged="gvAssessmentTask_SelectedIndexChanged" >
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="Task NO." />
                    <asp:TemplateField HeaderText="Assessment Task">
                        <ItemTemplate>
                            <asp:TextBox ID="txbAT" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Due Date for Assessment">
                        <ItemTemplate>
                            <asp:TextBox ID="txbDueDate" runat="server" MaxLength="12" Width="100px"></asp:TextBox>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit/s that relate to this assessment">
                        <ItemTemplate>
                            <asp:TextBox ID="txbUnitRelate" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>

                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <asp:Button ID="btnATAdd" runat="server" Text="Add New Row" OnClick="btnATAdd_Click" />
                        </FooterTemplate>
                    </asp:TemplateField>                       
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
                <FooterStyle BackColor="#c8e011" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#c8e011" Font-Bold="True" ForeColor="Black" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
        <hr class="my-4">
        <div class="TeacherInformation">
        <h3>Your Teacher Contact Details</h3>
            <table class="table">
        <tr>
            <td><b>Teacher Name</b></td>
            <td><b>Email Contact</b></td>
            <td><b>Telephone</b></td>
            <td><b>Availability</b></td>
        </tr>
        <tr>
            <td><asp:TextBox ID="txbTeacherName" runat="server" ></asp:TextBox></td>
            <td><asp:TextBox ID="txbEmail" runat="server" ></asp:TextBox></td>
            <td><asp:TextBox ID="txbTel" runat="server" ></asp:TextBox></td>
            <td><asp:TextBox ID="txbAvailability" runat="server" textMode="MultiLine"></asp:TextBox></td>
        </tr>
         </table>
        </div>
            <hr class="my-4">
    <div class="UnitPlan">
        <h3>Unit Plan Part 2 - Teacher Information </h3>
        <asp:GridView ID="gvUnitPlan" runat="server" ShowFooter="True" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDeleting="grvUnitPlan_RowDeleting"
                 Width="97%" Height="16px" Style="text-align: left" >
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="Session NO." />
                    <asp:TemplateField HeaderText="Time allocated">
                        <ItemTemplate>
                            <asp:TextBox ID="txbTA" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                           
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mapping to the unit/s">
                        <ItemTemplate>
                            <asp:TextBox ID="txbMapping" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Learning Topics">
                        <ItemTemplate>
                            <asp:TextBox ID="txbLearning" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Teaching and Learning Strategies">
                        <ItemTemplate>
                            <asp:TextBox ID="txbStrategies" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                           
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Assessment">
                        <ItemTemplate>
                            <asp:TextBox ID="txbAssessment" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Resources">
                        <ItemTemplate>
                            <asp:TextBox ID="txbResources" runat="server" Height="55px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                    
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <asp:Button ID="btnUnitPlanAdd" runat="server" Text="Add New Row" OnClick="btnUnitPlanAdd_Click" />
                        </FooterTemplate>
                    </asp:TemplateField>                       
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
                <FooterStyle BackColor="#c8e011" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#c8e011" Font-Bold="True" ForeColor="Black" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
                <hr class="my-4">
    <div class="row">
        <div class="col-md-4">
            <asp:Label ID="lblMessage" runat="server" Text="System Message: "></asp:Label><p></p>
            <asp:Label ID="lblMessageDisplay" runat="server" BackColor="#FFCC99"></asp:Label>
        </div>
        <div class="col-md-4">

        </div>
        <div class="col-md-4">
            <asp:Button ID="btnSave" runat="server" Text="Store in Database" Width="200px"  /><p></p>
            <asp:Button ID="btnLoad" runat="server" Text="Load from Database" Width="200px" /><p></p>
            <asp:Button ID="btnCreateADT" runat="server" Text="Create Unit Plan" Width="200px"  /><p></p>
            <asp:Button ID="btnDownLoad" runat="server" Text="Download" Width="200px" />
        </div>
    </div>
</asp:Content>
