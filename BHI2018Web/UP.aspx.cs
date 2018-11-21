using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BHI2018Web
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AssessmentTasksTable();
                UnitPlanTable();
            }
        }
        //initialize the assessment task table
        private void AssessmentTasksTable()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Col1", typeof(string)));
            dt.Columns.Add(new DataColumn("Col2", typeof(string)));
            dt.Columns.Add(new DataColumn("Col3", typeof(string)));
            dr = dt.NewRow();
            dr["RowNumber"] = 1;
            dr["Col1"] = string.Empty;
            dr["Col2"] = string.Empty;
            dr["Col3"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["ATTable"] = dt;


            gvAssessmentTask.DataSource = dt;
            gvAssessmentTask.DataBind();


            TextBox txn = (TextBox)gvAssessmentTask.Rows[0].Cells[1].FindControl("txbAT");
            //txn.Focus();
            Button btnAdd = (Button)gvAssessmentTask.FooterRow.Cells[4].FindControl("btnATAdd");
            Page.Form.DefaultFocus = btnAdd.ClientID;

        }
        //initialize the Unit Plan table
        private void UnitPlanTable()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Col1", typeof(string)));
            dt.Columns.Add(new DataColumn("Col2", typeof(string)));
            dt.Columns.Add(new DataColumn("Col3", typeof(string)));
            dt.Columns.Add(new DataColumn("Col4", typeof(string)));
            dt.Columns.Add(new DataColumn("Col5", typeof(string)));
            dt.Columns.Add(new DataColumn("Col6", typeof(string)));
            dr = dt.NewRow();
            dr["RowNumber"] = 1;
            dr["Col1"] = string.Empty;
            dr["Col2"] = string.Empty;
            dr["Col3"] = string.Empty;
            dr["Col4"] = string.Empty;
            dr["Col5"] = string.Empty;
            dr["Col6"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["UPTable"] = dt;


            gvUnitPlan.DataSource = dt;
            gvUnitPlan.DataBind();
        }
        //add row in the assessment task table.
        private void AddNewRowAT()
        {
            int rowIndex = 0;

            if (ViewState["ATTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["ATTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TextBoxAT = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                        TextBox TextBoxDueDate = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[2].FindControl("txbDueDate");
                        TextBox TextBoxUnitRelate = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[3].FindControl("txbUnitRelate");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxAT.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxDueDate.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxUnitRelate.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["ATTable"] = dtCurrentTable;

                    gvAssessmentTask.DataSource = dtCurrentTable;
                    gvAssessmentTask.DataBind();

                    TextBox txn = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                    txn.Focus();
                    // txn.Focus;
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            SetPreviousDataAT();
        }
        //add row in the Unit Plan table.
        private void AddNewRowUP()
        {
            int UProwIndex = 0;

            if (ViewState["UPTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["UPTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TextBoxTA = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[1].FindControl("txbTA");
                        TextBox TextBoxMapping = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[2].FindControl("txbMapping");
                        TextBox TextBoxLearning = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[3].FindControl("txbLearning");
                        TextBox TextBoxStrategies = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[4].FindControl("txbStrategies");
                        TextBox TextBoxAssessment = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[5].FindControl("txbAssessment");
                        TextBox TextBoxResources = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[6].FindControl("txbResources");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxTA.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxMapping.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxLearning.Text;
                        dtCurrentTable.Rows[i - 1]["Col4"] = TextBoxStrategies.Text;
                        dtCurrentTable.Rows[i - 1]["Col5"] = TextBoxAssessment.Text;
                        dtCurrentTable.Rows[i - 1]["Col6"] = TextBoxResources.Text;
                        UProwIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["UPTable"] = dtCurrentTable;

                    gvUnitPlan.DataSource = dtCurrentTable;
                    gvUnitPlan.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            SetPreviousDataUP();
        }
        //Method that handle the previous data in the Assessment task table
        private void SetPreviousDataAT()
        {
            int rowIndex = 0;
            if (ViewState["ATTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["ATTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox TextBoxAT = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                        TextBox TextBoxDueDate = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[2].FindControl("txbDueDate");
                        TextBox TextBoxUnitRelate = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[3].FindControl("txbUnitRelate");
                        // drCurrentRow["RowNumber"] = i + 1;

                        gvAssessmentTask.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        TextBoxAT.Text = dt.Rows[i]["Col1"].ToString();
                        TextBoxDueDate.Text = dt.Rows[i]["Col2"].ToString();
                        TextBoxUnitRelate.Text = dt.Rows[i]["Col3"].ToString();
                        rowIndex++;
                    }
                }
            }
        }
        //Method that handle the previous data in the Unit Plan table
        private void SetPreviousDataUP()
        {
            int UProwIndex = 0;
            if (ViewState["UPTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["UPTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox TextBoxTA = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[1].FindControl("txbTA");
                        TextBox TextBoxMapping = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[2].FindControl("txbMapping");
                        TextBox TextBoxLearning = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[3].FindControl("txbLearning");
                        TextBox TextBoxStrategies = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[4].FindControl("txbStrategies");
                        TextBox TextBoxAssessment = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[5].FindControl("txbAssessment");
                        TextBox TextBoxResources = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[6].FindControl("txbResources");
                        // drCurrentRow["RowNumber"] = i + 1;

                        gvUnitPlan.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        TextBoxTA.Text = dt.Rows[i]["Col1"].ToString();
                        TextBoxMapping.Text = dt.Rows[i]["Col2"].ToString();
                        TextBoxLearning.Text = dt.Rows[i]["Col3"].ToString();
                        TextBoxStrategies.Text = dt.Rows[i]["Col4"].ToString();
                        TextBoxAssessment.Text = dt.Rows[i]["Col5"].ToString();
                        TextBoxResources.Text = dt.Rows[i]["Col6"].ToString();
                        UProwIndex++;
                    }
                }
            }
        }
        //delete a row in assessment task table
        protected void grvAssessmentTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SetRowDataAT();
            if (ViewState["ATTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["ATTable"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["ATTable"] = dt;
                    gvAssessmentTask.DataSource = dt;
                    gvAssessmentTask.DataBind();

                    for (int i = 0; i < gvAssessmentTask.Rows.Count - 1; i++)
                    {
                        gvAssessmentTask.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    SetPreviousDataAT();
                }
            }
        }
        //delete a row in Unit Plan table
        protected void grvUnitPlan_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SetRowDataUP();
            if (ViewState["UPTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["UPTable"];
                DataRow drCurrentRow = null;
                int UProwIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[UProwIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["UPTable"] = dt;
                    gvUnitPlan.DataSource = dt;
                    gvUnitPlan.DataBind();

                    for (int i = 0; i < gvUnitPlan.Rows.Count - 1; i++)
                    {
                        gvUnitPlan.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    SetPreviousDataUP();
                }
            }
        }
        private void SetRowDataAT()
        {
            int rowIndex = 0;

            if (ViewState["ATTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["ATTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TextBoxAT = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                        TextBox TextBoxDueDate = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[2].FindControl("txbDueDate");
                        TextBox TextBoxUnitRelate = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[3].FindControl("txbUnitRelate");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxAT.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxDueDate.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxUnitRelate.Text;
                        rowIndex++;
                    }

                    ViewState["ATTable"] = dtCurrentTable;
                    //grvStudentDetails.DataSource = dtCurrentTable;
                    //grvStudentDetails.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //SetPreviousData();
        }
        private void SetRowDataUP()
        {
            int UProwIndex = 0;

            if (ViewState["UPTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["UPTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TextBoxTA = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[1].FindControl("txbTA");
                        TextBox TextBoxMapping = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[2].FindControl("txbMapping");
                        TextBox TextBoxLearning = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[3].FindControl("txbLearning");
                        TextBox TextBoxStrategies = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[3].FindControl("txbStrategies");
                        TextBox TextBoxAssessment = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[3].FindControl("txbAssessment");
                        TextBox TextBoxResources = (TextBox)gvUnitPlan.Rows[UProwIndex].Cells[3].FindControl("txbResources");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxTA.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxMapping.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxLearning.Text;
                        dtCurrentTable.Rows[i - 1]["Col4"] = TextBoxStrategies.Text;
                        dtCurrentTable.Rows[i - 1]["Col5"] = TextBoxAssessment.Text;
                        dtCurrentTable.Rows[i - 1]["Col6"] = TextBoxResources.Text;
                        UProwIndex++;
                    }

                    ViewState["UPTable"] = dtCurrentTable;
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //SetPreviousData();
        }
        protected void btnATAdd_Click(object sender, EventArgs e)
        {
            AddNewRowAT();
        }
        protected void btnUnitPlanAdd_Click(object sender, EventArgs e)
        {
            AddNewRowUP();
        }
        //useless, but delete this would cause an error
        protected void gvAssessmentTask_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //Generate 
        protected void btnCreateUP_Click(object sender, EventArgs e)
        {
            //object objMissing = System.Reflection.Missing.Value;
            String path = Environment.CurrentDirectory;
            //the code above may cause some problem because when you run the application, current directory is in C drive, it might be read only.
            //In web application, put the template in other drive to make sure it can be filled with data.
            path = "E:\\UP.docx";
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = false;
            Microsoft.Office.Interop.Word.Document document = wordApp.Documents.Open(path);
            try { 
                if (document == null)
                {
                        try
                        {
                            wordApp.Quit();
                            wordApp = null;
                        }
                        catch
                        {
                            lblMessageDisplay.Text = "wordApp did not quit.";
                        }

                        return;
                }
                //fill Informaiton About Your Unit
                Microsoft.Office.Interop.Word.Table informationAboutYourUnit = document.Tables[1];
                String yourQualification = txbCourseCodeTitle.Text;
                String yourUnits = txbUnitCodeTitle.Text;
                //separator topic by lines
                String topic = txbTopics.Text;
                String[] topicSeparators = new String[] { "\r\n" };
                String[] topicArray = topic.Split(topicSeparators, StringSplitOptions.None);

                String whatAreYouBingToClass = txbBring.Text;
                String DateStart = txbStart.Text;
                String DateEnd = txbFinish.Text;
                String classDateAndTime = txbDateTime.Text;
                String building = txbClassroom.Text;
                String learningProgram1 = txbLearningProgram1.Text;
                String learningProgram2 = txbLearningProgram2.Text;
                String learningProgram3 = txbLearningProgram3.Text;
                String learningProgram4 = txbLearningProgram4.Text;
                String learningProgram5 = txbLearningProgram5.Text;

                informationAboutYourUnit.Cell(3,2).Range.Text = yourQualification;
                informationAboutYourUnit.Cell(4, 2).Range.Text = yourUnits;
                //display topics in the cell
                String topics = null;
                foreach (string s in topicArray)
                {
                    topics = topics + s + Environment.NewLine;
                }
                informationAboutYourUnit.Cell(5, 2).Range.Text = topics;
                informationAboutYourUnit.Cell(6, 2).Range.Text = whatAreYouBingToClass;
                informationAboutYourUnit.Cell(7, 2).Range.Text = DateStart;
                informationAboutYourUnit.Cell(7, 4).Range.Text = DateEnd;
                informationAboutYourUnit.Cell(8, 2).Range.Text = classDateAndTime;
                informationAboutYourUnit.Cell(9, 2).Range.Text = building;
                informationAboutYourUnit.Cell(11, 2).Range.Text = learningProgram1;
                informationAboutYourUnit.Cell(12, 2).Range.Text = learningProgram2;
                informationAboutYourUnit.Cell(13, 2).Range.Text = learningProgram3;
                informationAboutYourUnit.Cell(14, 2).Range.Text = learningProgram4;
                informationAboutYourUnit.Cell(15, 2).Range.Text = learningProgram5;

                //fill teacher information
                Microsoft.Office.Interop.Word.Table teacherInformation = document.Tables[3];
                String teacherName = txbTeacherName.Text;
                String teacherEmail = txbEmail.Text;
                String teacherTel = txbTel.Text;
                //separator teacherAvailability by lines
                String teacherAvailability = txbAvailability.Text;
                String[] availabilitySeparators = new String[] { "\r\n" };
                String[] teacherAvailabilityArray = teacherAvailability.Split(availabilitySeparators, StringSplitOptions.None);
                teacherInformation.Cell(3, 1).Range.Text = teacherName;
                teacherInformation.Cell(3, 2).Range.Text = teacherEmail;
                teacherInformation.Cell(3, 3).Range.Text = teacherTel;
                //display teacherAvailability in the cell
                String teacherAvailabilities = null;
                foreach (string s in teacherAvailabilityArray)
                {
                    teacherAvailabilities = teacherAvailabilities + s + Environment.NewLine;
                }
                teacherInformation.Cell(3, 4).Range.Text = teacherAvailabilities;

                //fill Your Assessment Tasks
                Microsoft.Office.Interop.Word.Table yourAssessmentTasks = document.Tables[2];
                //the filled text stored in the ViewState, assign it to a DataTable
                DataTable filledAssessmentTaskTable = (DataTable)ViewState["ATTable"];
                Int32 assessmentTaskTableRowCount = filledAssessmentTaskTable.Rows.Count;
                //add rows. when generate UP, the table need to add additional empty row, otherwise the data in the last row will loss. That is why "rowCount - 2"
                for (int i = 0; i < assessmentTaskTableRowCount - 2; i++)
                { yourAssessmentTasks.Rows.Add(); }
                //fill data
                for (int row = 0; row < assessmentTaskTableRowCount - 1; row++)
                {
                    for (int column = 0; column < 3; column++)
                    {
                        yourAssessmentTasks.Cell(row + 3, column + 2).Range.Text = filledAssessmentTaskTable.Rows[row][column + 1].ToString();
                    }
                }

                //fill Unit Plan 2
                Microsoft.Office.Interop.Word.Table unitPlanPart2 = document.Tables[6];
                //the filled text stored in the ViewState, assign it to a DataTable
                DataTable filledUnitPlanPart2Table = (DataTable)ViewState["UPTable"];
                Int32 unitPlanPart2TableRowCount = filledUnitPlanPart2Table.Rows.Count;
                for (int i = 0; i < unitPlanPart2TableRowCount -2; i++)
                { unitPlanPart2.Rows.Add(); }
                //fill data
                for (int row = 0; row < unitPlanPart2TableRowCount - 1; row++)
                {
                    for (int column = 0; column < 6; column++)
                    {
                        unitPlanPart2.Cell(row + 3, column + 2).Range.Text = filledUnitPlanPart2Table.Rows[row][column + 1].ToString();
                    }
                }

                //Save file
                //the web application is running on the server, this is the path on the server side, not user side.
                object filename = "E:\\UP1.docx";
                document.SaveAs2(ref filename);
                lblMessageDisplay.Text = "File Generated on server, click download button to download on your PC.";
            }
            catch (Exception ex)
            {
                lblMessageDisplay.Text = ex.Message + Environment.NewLine + Environment.NewLine + ex.TargetSite.ToString();
            }
            finally
            {
                try
                {
                    document.Close(false);
                    document = null;
                    wordApp.Quit();
                    wordApp = null;
                }
                catch { }
            }
        }

        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            //Once user click on Generate button, download the document.
            Response.ContentType = "Application/docx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=UP1.docx");
            Response.TransmitFile("E:\\UP1.docx");
            Response.End();
        }
    }
}