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
    }
}