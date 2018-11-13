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
                FirstGridViewRow();
            }
        }
        private void FirstGridViewRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Col1", typeof(string)));
            dt.Columns.Add(new DataColumn("Col2", typeof(string)));
            dt.Columns.Add(new DataColumn("Col3", typeof(string)));
            dr = dt.NewRow();
            dr["RowNumber"] = 1;
            dr["Col1"] = string.Empty;
            dr["Col2"] = string.Empty;
            dr["Col3"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;


            gvAssessmentTask.DataSource = dt;
            gvAssessmentTask.DataBind();


            TextBox txn = (TextBox)gvAssessmentTask.Rows[0].Cells[1].FindControl("txbAT");
            //txn.Focus();
            Button btnAdd = (Button)gvAssessmentTask.FooterRow.Cells[4].FindControl("ButtonAdd");
            Page.Form.DefaultFocus = btnAdd.ClientID;

        }
        private void AddNewRow()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TextBoxName = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                        TextBox TextBoxAge = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[2].FindControl("txbDueDate");
                        TextBox TextBoxAddress = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[3].FindControl("txbUnitRelate");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxName.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxAge.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxAddress.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

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
            SetPreviousData();
        }
        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox TextBoxName = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                        TextBox TextBoxAge = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[2].FindControl("txbDueDate");
                        TextBox TextBoxAddress = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[3].FindControl("txbUnitRelate");
                        // drCurrentRow["RowNumber"] = i + 1;

                        gvAssessmentTask.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        TextBoxName.Text = dt.Rows[i]["Col1"].ToString();
                        TextBoxAge.Text = dt.Rows[i]["Col2"].ToString();
                        TextBoxAddress.Text = dt.Rows[i]["Col3"].ToString();
                        rowIndex++;
                    }
                }
            }
        }
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }
        protected void grvAssessmentTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SetRowData();
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["CurrentTable"] = dt;
                    gvAssessmentTask.DataSource = dt;
                    gvAssessmentTask.DataBind();

                    for (int i = 0; i < gvAssessmentTask.Rows.Count - 1; i++)
                    {
                        gvAssessmentTask.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    SetPreviousData();
                }
            }
        }

        private void SetRowData()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TextBoxName = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[1].FindControl("txbAT");
                        TextBox TextBoxAge = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[2].FindControl("txbDueDate");
                        TextBox TextBoxAddress = (TextBox)gvAssessmentTask.Rows[rowIndex].Cells[3].FindControl("txbUnitRelate");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxName.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxAge.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxAddress.Text;
                        rowIndex++;
                    }

                    ViewState["CurrentTable"] = dtCurrentTable;
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
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SetRowData();
        //        DataTable table = ViewState["CurrentTable"] as DataTable;

        //        if (table != null)
        //        {
        //            foreach (DataRow row in table.Rows)
        //            {
        //                string txbAT = row.ItemArray[1] as string;
        //                string txbDueDate = row.ItemArray[2] as string;
        //                string txbUnitRelate = row.ItemArray[3] as string;


        //                if (txbAT != null ||
        //                    txbDueDate != null ||
        //                    txbUnitRelate != null)
        //                {
        //                    // Do whatever is needed with this data, 
        //                    // Possibily push it in database
        //                    // I am just printing on the page to demonstrate that it is working.
        //                    Response.Write(string.Format("{0} {1} {2} {3} {4}<br/>", txbAT, txbDueDate, txbUnitRelate));
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}