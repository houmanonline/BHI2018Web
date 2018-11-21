using HtmlAgilityPack;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BHI2018Web
{
    public partial class Contact : System.Web.UI.Page
    {
        //Connection String
        String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //Return an array that contain unit code and name
        private String[] getUnitCodeAndTitle(HtmlAgilityPack.HtmlDocument doc)
        {
            //split Unit Code and Name
            if (doc.DocumentNode.SelectSingleNode("//h1[text()='Unit of competency details']") != null)
            {
                var unitCodeName = doc.DocumentNode.SelectSingleNode("//h1[text()='Unit of competency details']/following::*[1]");
                String unitTitle = unitCodeName.InnerText;
                String[] ucn = unitTitle.Split('-');
                Int32 count = ucn.Count();
                if(count > 2)
                    ucn[1] = ucn[1] + ucn[2];
                return ucn;
            }
            else
            {
                return null;
            }
        }
        //Return Application as a string
        private String getApplication(HtmlAgilityPack.HtmlDocument doc)
        {
            String application = "";

            //if the unit has Unit Sector, get nodes between Application and Unit Sector
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Application']") != null
                && doc.DocumentNode.SelectSingleNode("//h2[text()='Unit Sector']") != null)
            {
                //The Xpath expression below is used to get nodes between two given node. The formula is $ns1[count(.|$ns2) = count($ns2)], I am not fully understand this formula, but it works.
                var App = doc.DocumentNode.SelectNodes("//h2[text()='Application']/following-sibling::* [count(.|//h2[text()='Unit Sector']/preceding-sibling::*) = count(//h2[text()='Unit Sector']/preceding-sibling::*) ]").ToList();
                foreach (var items in App)
                {
                    string item = items.InnerText.Trim();

                    //do not add the empty rows into ListBox
                    if (!String.IsNullOrWhiteSpace(item))
                    {
                        //lsbApp.Items.Add(item);
                        application += item + Environment.NewLine + Environment.NewLine;
                    }
                }
            }
            //if the unit does not have Unit Sector, get nodes between Application and Elements and Performance Criteria
            else if (doc.DocumentNode.SelectSingleNode("//h2[text()='Unit Sector']") == null)
            {
                var App = doc.DocumentNode.SelectNodes("//h2[text()='Application']/following-sibling::* [count(.|//h2[text()='Elements and Performance Criteria']/preceding-sibling::*) = count(//h2[text()='Elements and Performance Criteria']/preceding-sibling::*) ]").ToList();
                foreach (var items in App)
                {
                    string item = items.InnerText.Trim();

                    if (!String.IsNullOrWhiteSpace(item))
                    {
                        //lsbApp.Items.Add(item);
                        application += item + Environment.NewLine + Environment.NewLine;
                    }
                }
            }
            //
            else
            {
                lblMessageDisplay.Text = "No Application for this unit.";
            }

            return application.Trim();
        }
        //Return Assessment Conditions as a string
        private String getAssessmentConditions(HtmlAgilityPack.HtmlDocument doc)
        {
            string assessmentConditions = "";

            //Get all the li nodes between Assessment Conditions and Link
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Assessment Conditions']") != null)
            {
                //var ass = doc.DocumentNode.SelectNodes("//h2[text()='Assessment Conditions']/following-sibling::*/li [count(.|//h2[text()='Links']/preceding-sibling::*/li) = count(//h2[text()='Links']/preceding-sibling::*/li) ]").ToList();
                var ass = doc.DocumentNode.SelectNodes("//h2[text()='Assessment Conditions']/following-sibling::*/li").ToList(); //some units do not have Link section, so get any li nodes after Assessment Conditions section instead
                foreach (var items in ass)
                {
                    //lsbAC.Items.Add(items.InnerText);
                    assessmentConditions += items.InnerText + Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                lblMessageDisplay.Text = "No Assessment Conditions for this unit.";
            }

            return assessmentConditions.Trim();
        }

        private void getElementsAndPerformanceCriteria(HtmlAgilityPack.HtmlDocument doc, ref ArrayList elems, ref ArrayList perfCriteria)
        {
            //get Elements and Performance Criteria
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Elements and Performance Criteria']") != null)
            {
                var elements = doc.DocumentNode.SelectNodes("//h2[text()='Elements and Performance Criteria']/following-sibling::*[1]/tr/td").ToList();
                //delete the first two rows of the table
                elements.RemoveRange(0, 4);
                int i = 0;
                int row = 1;//Row number of performance criteria

                //remove the empty node (eg.UnitCode:hltwhs003. There are some empty nodes without this section of code)
                for (int j = elements.Count - 1; j >= 0; j--)
                {
                    if (String.IsNullOrWhiteSpace(elements[j].InnerText))
                    {
                        elements.RemoveAt(j);
                    }
                }
                foreach (var items in elements)
                {
                    //elements
                    if (i % 2 == 0)
                    {
                        string elem = items.InnerText.Trim();
                        elems.Add(elem);
                    }
                    //performance criteria
                    else
                    {
                        //Split criteria 
                        String criteria = items.InnerText;
                        String[] pattern = { row.ToString() + "." };
                        String[] criteriaArray = criteria.Split(pattern, StringSplitOptions.None);
                        //Convert array to ArrayList then delete the first element
                        ArrayList criteriaArrayList = new ArrayList(criteriaArray);
                        criteriaArrayList.RemoveAt(0);
                        criteriaArray = (string[])criteriaArrayList.ToArray(typeof(string));
                        foreach (String c in criteriaArray)
                        {
                            string crit = row + "." + c.Trim();
                            perfCriteria.Add(crit);
                        }
                        row++;
                    }
                    i++;
                }
            }
            else
            {
                lblMessageDisplay.Text = "No Elements and Performance Criteria for this unit.";
            }

        }

        private void getFoundationSkills(HtmlAgilityPack.HtmlDocument doc, ref ArrayList[] foundationSkills)
        {
            /*
             * There is a empty table when grab content of ICTGAM401, the switch statement can fix the bug which the application cannot grab foundation skills of ictgam401. 
             * I know the code is terrible, but the web structure of TGA is terrible too.
             */
            Int32 swtichCase = 0;
            //Because of the variable scope of switch statement, it need to declare variable here.
            System.Collections.Generic.List<HtmlNode> foundation;
            // Normally case, the foundation skills is the second table
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Foundation Skills']/following-sibling::*[2]/tr/td") != null)
            { swtichCase = 1; }
            // ICTGAM401's case, there is a empty table, so the foundation skills is the third table
            else if (doc.DocumentNode.SelectSingleNode("//h2[text()='Foundation Skills']/following-sibling::*[3]/tr/td") != null) { swtichCase = 2; }
            switch (swtichCase)
            {
                case 1:
                    foundation = doc.DocumentNode.SelectNodes("//h2[text()='Foundation Skills']/following-sibling::*[2]/tr/td").ToList();
                    break;
                case 2:
                    foundation = doc.DocumentNode.SelectNodes("//h2[text()='Foundation Skills']/following-sibling::*[3]/tr/td").ToList();
                    break;
                default:
                    foundation = null;
                    lblMessageDisplay.Text = "Cannot grab foundation skills from web";
                    break;
            }
            //delete the first row of the table
            foundation.RemoveRange(0, 3);
            int i = 0;
            string tempString = "";
            //add each row into ListView
            for (i = 0; i < foundation.Count(); i++)
            {
                if (i % 3 == 0)
                {
                    tempString = foundation[i].InnerText.Trim();
                    foundationSkills[i % 3].Add(tempString);
                    i++;
                    tempString = foundation[i].InnerText.Trim();;
                    foundationSkills[i % 3].Add(tempString);
                    i++;
                    tempString = foundation[i].InnerText.Trim();
                    foundationSkills[i % 3].Add(tempString);
                }
            }
        }

        private void getPerformanceEvidence(HtmlAgilityPack.HtmlDocument doc, ref ArrayList performanceEvidence)
        {
            //Get all the li nodes between Performance Evidence and Knowledge Evidence (there is an error with UnitCode:chcece020)
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Performance Evidence']") != null)
            {
                var pe = doc.DocumentNode.SelectNodes("//h2[text()='Performance Evidence']/following-sibling::*/li [count(.|//h2[text()='Knowledge Evidence']/preceding-sibling::*/li) = count(//h2[text()='Knowledge Evidence']/preceding-sibling::*/li) ]").ToList();
                foreach (var items in pe)
                {
                    //lsbPE.Items.Add(items.InnerText);
                    performanceEvidence.Add(items.InnerText);
                }
            }
            else
            {
                lblMessageDisplay.Text = "No Performance Evidence for this unit.";
            }
        }

        private void getKnowlegeEvidence(HtmlAgilityPack.HtmlDocument doc, ref ArrayList knowlegeEvidence)
        {
            //Get all the li nodes between Knowledge Evidence and Assessment Conditions
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Knowledge Evidence']") != null)
            {
                var kn = doc.DocumentNode.SelectNodes("//h2[text()='Knowledge Evidence']/following-sibling::*/li [count(.|//h2[text()='Assessment Conditions']/preceding-sibling::*/li) = count(//h2[text()='Assessment Conditions']/preceding-sibling::*/li) ]").ToList();
                foreach (var items in kn)
                {
                    //lsbKE.Items.Add(items.InnerText);
                    knowlegeEvidence.Add(items.InnerText);
                }
            }
            else
            {
                lblMessageDisplay.Text = "No Knowledge Evidence for this unit.";
            }
        }

        private SqlCommand populateUnitTable(SqlConnection myConn, String[] unitDetails, String application, String assessmentConditions)
        {
            SqlCommand cmd = null;

            String SQL = "EXEC dbo.Save_Unit @UnitCode = @UC, @UnitTitle = @UT";
            if (!String.IsNullOrWhiteSpace(application))
            {
                SQL += ",@Application = @App";
            }
            if (!String.IsNullOrWhiteSpace(assessmentConditions))
            {
                SQL += ",@AssessmentConditions = @AC";
            }
            SQL += ";";
            cmd = new SqlCommand(SQL, myConn);

            // unit code and title
            cmd.Parameters.AddWithValue("@UC", unitDetails[0]);
            cmd.Parameters.AddWithValue("@UT", unitDetails[1]);
            //cmd.Parameters.Add("@UC", SqlDbType.VarChar, 20).Value = unitDetails[0];
            //cmd.Parameters.Add("@UT", SqlDbType.VarChar, 256).Value = unitDetails[1];

            if (!String.IsNullOrWhiteSpace(application))
            {
                // Application
                cmd.Parameters.AddWithValue("@App", application);
                //cmd.Parameters.Add("@App", SqlDbType.VarChar, 2048).Value = application;
            }
            if (!String.IsNullOrWhiteSpace(assessmentConditions))
            {
                // Assessment Conditions
                cmd.Parameters.AddWithValue("@AC", assessmentConditions);
                //cmd.Parameters.Add("@AC", SqlDbType.VarChar, 2048).Value = assessmentConditions;
            }

            return cmd;
        }

        private SqlCommand populateElements(SqlConnection myConn, String unitCode, String element)
        {
            SqlCommand cmd = null;

            String SQL = "EXEC [dbo].[Save_Element] @UC, @Desc;";

            cmd = new SqlCommand(SQL, myConn);

            cmd.Parameters.AddWithValue("@UC", unitCode);
            cmd.Parameters.AddWithValue("@Desc", element);
            //cmd.Parameters.Add("@UC", SqlDbType.VarChar, 20).Value = unitCode;
            //cmd.Parameters.Add("@Desc", SqlDbType.VarChar, 256).Value = element;

            return cmd;
        }

        private SqlCommand populatePerformanceCriteria(SqlConnection myConn, Int32 eleIndex, String perfCriteria)
        {
            SqlCommand cmd = null;

            String SQL = "EXEC [dbo].[Save_PerformanceCriteria] @EID, @Desc;";

            cmd = new SqlCommand(SQL, myConn);

            cmd.Parameters.AddWithValue("@EID", eleIndex);
            cmd.Parameters.AddWithValue("@Desc", perfCriteria);
            //cmd.Parameters.Add("@EID", SqlDbType.Int).Value = eleIndex;
            //cmd.Parameters.Add("@Desc", SqlDbType.VarChar, 1024).Value = perfCriteria;

            return cmd;
        }

        private SqlCommand populateFoundationSkills(SqlConnection myConn, String unitCode, String name, String perfCriterias, String description)
        {
            SqlCommand cmd = null;

            String SQL = "EXEC [dbo].[Save_FoundationSkills] @UC, @Name, @perfCrit, @Desc;";

            cmd = new SqlCommand(SQL, myConn);

            cmd.Parameters.AddWithValue("@UC", unitCode);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@perfCrit", perfCriterias);
            cmd.Parameters.AddWithValue("@Desc", description);
            //cmd.Parameters.Add("@UC", SqlDbType.VarChar, 20).Value = unitCode;
            //cmd.Parameters.Add("@Name", SqlDbType.VarChar, 45).Value = name;
            //cmd.Parameters.Add("@perfCrit", SqlDbType.VarChar, 45).Value = perfCriterias;
            //cmd.Parameters.Add("@Desc", SqlDbType.VarChar, 1024).Value = description;

            return cmd;
        }

        private SqlCommand populatePerformance_KnowlegeEvidence(SqlConnection myConn, String unitCode, String description, Boolean knowlege)
        {
            SqlCommand cmd = null;

            String SQL = "EXEC [dbo].[Save_Performance_KnowlegeEvidence] @UC, @know, @Desc;";

            cmd = new SqlCommand(SQL, myConn);

            cmd.Parameters.AddWithValue("@UC", unitCode);
            cmd.Parameters.AddWithValue("@know", knowlege);
            cmd.Parameters.AddWithValue("@Desc", description);
            //cmd.Parameters.Add("@UC", SqlDbType.VarChar, 20).Value = unitCode;
            //cmd.Parameters.Add("@know", SqlDbType.Bit).Value = knowlege;
            //cmd.Parameters.Add("@Desc", SqlDbType.VarChar, 1024).Value = description;

            return cmd;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateADT_Click(object sender, EventArgs e)
        {
            //object objMissing = System.Reflection.Missing.Value;
            String path = Environment.CurrentDirectory;
            //the code above may cause some problem because when you run the application, current directory is in C drive, it might be read only.
            //In web application, put the template in other drive to make sure it can be filled with data.
            path = "E:\\ADT.doc";
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = false;
            Document document = wordApp.Documents.Open(path);
            try
            {
                if (document == null)
                {
                    try
                    {
                        wordApp.Quit();
                        wordApp = null;
                    }
                    catch {
                        lblMessageDisplay.Text = "wordApp did not quit.";
                    }

                    return;
                }
                // get Unit Data
                String SQL;
                SqlCommand cmd;
                using (SqlConnection myConn = new SqlConnection(connectionString))
                {
                    myConn.Open();

                    // add Unit Code, Unit Title, Application, Assessment Conditions                
                    //add unit code and name
                    Microsoft.Office.Interop.Word.Table unitCode = document.Tables[2];
                    // add Application
                    Microsoft.Office.Interop.Word.Table applicationTable = document.Tables[1];
                    //add Assessment Conditions
                    Microsoft.Office.Interop.Word.Table TableAC = document.Tables[7];

                    SQL = "EXEC [dbo].[Load Unit for ADT] @UC";
                    cmd = new SqlCommand(SQL, myConn);

                    cmd.Parameters.AddWithValue("@UC", txbUnitCode.Text);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            // unit code and name
                            unitCode.Rows.Add();
                            unitCode.Cell(2, 1).Range.Text = reader[0].ToString();
                            unitCode.Cell(2, 2).Range.Text = reader[1].ToString();
                            unitCode.Rows[2].Cells.Shading.BackgroundPatternColor = WdColor.wdColorWhite;
                            unitCode.Rows[2].Range.Font.Bold = 0;
                            

                            // application
                            applicationTable.Cell(1, 2).Range.Text = reader[2].ToString();
                            applicationTable.Cell(1, 2).Range.Font.Color = WdColor.wdColorBlack;
                            applicationTable.Cell(1, 2).Range.ListFormat.ApplyBulletDefault(Type.Missing);


                            // assessment conditions
                            TableAC.Cell(1, 2).Range.Text = reader[3].ToString();

                            //display in the web page
                            lblUnitNameDisplay.Text = reader[1].ToString();
                            lsbApp.Items.Add(reader[2].ToString());
                            lsbAC.Items.Add(reader[3].ToString());

                        }
                    }

                    //add ELEMENT and PERFORMANCE CRITERIA
                    Microsoft.Office.Interop.Word.Table tableEP = document.Tables[3];

                    SQL = "EXEC [dbo].[Load Elements and PerformanceCriteria for ADT] @UC";
                    cmd.CommandText = SQL;

                    using (var reader = cmd.ExecuteReader())
                    {
                        Row newRow = null;
                        int oldEleID = -1;
                        int readCount = 1;
                        int mergeLoc = 2;
                        string element = null;

                        while (reader.Read())
                        {
                            readCount++;

                            // adds a new row, and formats it
                            newRow = tableEP.Rows.Add();

                            // add performance criteria
                            tableEP.Cell(readCount, 2).Range.Text = reader[2].ToString();

                            // add elements     
                            

                            if (oldEleID > -1)
                            {
                                if ((int)reader[0] == oldEleID)
                                {
                                    // merge
                                    tableEP.Cell(mergeLoc, 1).Merge(tableEP.Cell(readCount, 1));
                                }
                                else
                                {
                                    // input text
                                    tableEP.Cell(mergeLoc, 1).Range.Text = element;
                                    mergeLoc = readCount;
                                    element = null;
                                }
                            }
                            else
                            {
                                newRow.Cells.Shading.BackgroundPatternColor = WdColor.wdColorWhite;
                                newRow.Range.Font.Bold = 0;
                            }
                            element = reader[1].ToString();
                            oldEleID = (int)reader[0];
                        }

                        if (!String.IsNullOrWhiteSpace(element))
                        {
                            tableEP.Cell(mergeLoc, 1).Range.Text = element;
                        }

                    }

                    //add Foundation Skills
                    Microsoft.Office.Interop.Word.Table tableFS = document.Tables[4];

                    SQL = "EXEC [dbo].[Load FoundationSkills for ADT] @UC";
                    cmd.CommandText = SQL;

                    using (var reader = cmd.ExecuteReader())
                    {
                        Row newRow;
                        while (reader.Read())
                        {
                            newRow = tableFS.Rows.Add();                            
                            newRow.Range.Font.Bold = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                newRow.Cells[i + 1].Range.Text = reader[i].ToString();
                            }
                        }
                        for (int i = 0; i < tableFS.Rows.Count; i++)
                        {
                            tableFS.Cell(i + 3, 3).Range.ListFormat.ApplyBulletDefault(Type.Missing);
                        }
                    }

                    //add Performance Evidence
                    Microsoft.Office.Interop.Word.Table tablePE = document.Tables[5];
                    //add Knowledge Evidence
                    Microsoft.Office.Interop.Word.Table tableKE = document.Tables[6];

                    SQL = "EXEC [dbo].[Load EvidenceIndicators for ADT] @UC";
                    cmd.CommandText = SQL;

                    using (var reader = cmd.ExecuteReader())
                    {
                        Row newRow;
                        while (reader.Read())
                        {
                            // if knowlege evidence flag is true
                            if ((bool)reader[0])
                            {
                                newRow = tableKE.Rows.Add();
                                lsbKE.Items.Add(reader[1].ToString()); // display in web page
                            }
                            else
                            {
                                newRow = tablePE.Rows.Add();
                                lsbPE.Items.Add(reader[1].ToString()); // display in web page
                            }

                            newRow.Cells.Shading.BackgroundPatternColor = WdColor.wdColorWhite;
                            newRow.Range.Font.Bold = 0;

                            newRow.Cells[1].Range.Text = reader[1].ToString();
                        }
                    }
                    //close the connection
                    myConn.Close();
                }

                //Save file
                //the web application is running on the server, this is the path on the server side, not user side.
                object filename = "E:\\ADT1.doc";
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Create data storage variables
            String application = null, assessmentConditions =null;
            String[] unitDetails = null;
            ArrayList elements = new ArrayList(), performanceCriteria = new ArrayList(), performanceEvidence = new ArrayList(), knowlegeEvidence = new ArrayList();
            ArrayList[] foundationSkills = new ArrayList[3];
            //Initialize foundationSkills
            for (int i = 0; i < 3; i++)
            {
                foundationSkills[i] = new ArrayList();
            }

            //create a HtmlDocument intance
            string Url = "https://training.gov.au/Training/Details/" + txbUnitCode.Text;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            //check if the unitcode exists
            if (doc.DocumentNode.SelectSingleNode("//h1[text()='Unit of competency details']") != null)
            {
                unitDetails = getUnitCodeAndTitle(doc);
                lblUnitNameDisplay.Text = unitDetails[1].Trim();
            }
            else
            {
                lblUnitNameDisplay.Text = "Make sure the unit code you entered is correct.";
            }

            //get parameter data
            try
            {
                //get Application
                application = getApplication(doc);

                //Assessment Conditions
                assessmentConditions = getAssessmentConditions(doc);

                //get Elements and Performance Criteria
                getElementsAndPerformanceCriteria(doc, ref elements, ref performanceCriteria);

                //get Foundation Skills            
                getFoundationSkills(doc, ref foundationSkills);

                // Performance Evidence
                getPerformanceEvidence(doc, ref performanceEvidence);

                // Knowledge Evidence
                getKnowlegeEvidence(doc, ref knowlegeEvidence);
            }
            catch (Exception ex) { lblMessageDisplay.Text = ex.ToString(); ; }

            // SQL Insert Transaction starts from here
            String SQL = null;
            SqlCommand cmd = null;
            using (SqlConnection myConn = new SqlConnection(connectionString))
            {
                
                try
                {
                    myConn.Open();

                        SQL = "BEGIN TRAN";
                        cmd = new SqlCommand(SQL, myConn);
                        cmd.ExecuteNonQuery();

                        // populates Unit table into the database
                        cmd = populateUnitTable(myConn, unitDetails, application, assessmentConditions);
                        cmd.ExecuteNonQuery();

                        // counts the elements we're up to
                        int i = 1;

                        // populates elements and perf criteria into the database
                        foreach (String element in elements)
                        {
                            cmd = populateElements(myConn, unitDetails[0], element);

                            // ExecuteScalar() is the same as ExecuteNonQuery(), except it can be used to return the Primary Key index that was just created
                            Int32 eleIndex = (Int32)cmd.ExecuteScalar();

                            //lblMessageDisplay.Text = "eleIndex: " + eleIndex;

                            foreach (String perfCriteria in performanceCriteria)
                            {
                                if (perfCriteria.StartsWith(i + "."))
                                {
                                    cmd = populatePerformanceCriteria(myConn, eleIndex, perfCriteria);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            i++;
                        }

                        // populates foundation skills into the database
                        for (int x = 0; x < foundationSkills[0].Count; x++)
                        {
                            cmd = populateFoundationSkills(myConn, unitDetails[0],
                                foundationSkills[0][x].ToString(),
                                foundationSkills[1][x].ToString(),
                                foundationSkills[2][x].ToString());
                            cmd.ExecuteNonQuery();
                        }

                        // populate performance and knowlege evidence
                        foreach (String perfEv in performanceEvidence)
                        {
                            cmd = populatePerformance_KnowlegeEvidence(myConn, unitDetails[0], perfEv, false);
                            cmd.ExecuteNonQuery();
                        }
                        foreach (String knowEv in knowlegeEvidence)
                        {
                            cmd = populatePerformance_KnowlegeEvidence(myConn, unitDetails[0], knowEv, true);
                            cmd.ExecuteNonQuery();
                        }

                        SQL = "COMMIT TRAN";
                        cmd = new SqlCommand(SQL, myConn);
                        cmd.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
                    try
                    {
                        lblMessageDisplay.Text = cmd.CommandText + Environment.NewLine + ex.Message;

                        SQL = "ROLLBACK TRAN";
                        cmd = new SqlCommand(SQL, myConn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception exx)
                    {
                        try
                        {
                            lblMessageDisplay.Text = "Rollback failed too, :(" + Environment.NewLine + cmd.CommandText + Environment.NewLine + exx.Message;
                        }
                        catch { }
                    }
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                    myConn.Dispose();
                }
            }
        }

        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            //Once user click on Generate button, download the doc.
            Response.ContentType = "Application/doc";
            Response.AppendHeader("Content-Disposition", "attachment; filename=ADT1.doc");
            Response.TransmitFile("E:\\ADT1.doc");
            Response.End();
        }
    }
}