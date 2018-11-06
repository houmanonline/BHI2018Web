using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BHI2018Web
{
    public partial class Contact : Page
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
                //lblUnitCode.Text = ucn[0].Trim();
                //lblUnitName.Text = ucn[1].Trim();
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
                        lsbApp.Items.Add(item);
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
                    lsbAC.Items.Add(items.InnerText);
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
                    lsbPE.Items.Add(items.InnerText);
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
                    lsbKE.Items.Add(items.InnerText);
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

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Create data storage variables
            String application, assessmentConditions;
            String[] unitDetail;
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
                unitDetail = getUnitCodeAndTitle(doc);
                lblUnitNameDisplay.Text = unitDetail[1].Trim();
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
        }
    }
}