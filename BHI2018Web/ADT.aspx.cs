using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BHI2018Web
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateADT_Click(object sender, EventArgs e)
        {

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            //create a HtmlDocument intance
            string Url = "https://training.gov.au/Training/Details/" + txbUnitCode.Text;
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(Url);

            //clear the form
            foreach (Control c in this.Controls)
            {
                if (c is ListBox)
                {
                    ListBox listBox = (ListBox)c;
                    listBox.Items.Clear();
                }
                else if (c is ListView)
                {
                    ListView listView = (ListView)c;
                    listView.Items.Clear();
                }
            }

            //split Unit Code and Name
            if (doc.DocumentNode.SelectSingleNode("//h1[text()='Unit of competency details']") != null)
            {
                var unitCodeName = doc.DocumentNode.SelectSingleNode("//h1[text()='Unit of competency details']/following::*[1]");
                String unitTitle = unitCodeName.InnerText;
                String[] ucn = unitTitle.Split('-');
                //lblUnitCode.Text = ucn[0].Trim();
                lblUnitNameDisplay.Text = ucn[1].Trim();
            }
            else
            {
                lblUnitNameDisplay.Text = "Make sure the unit code you entered is correct.";
            }
            //get Application
            //if the unit has Unit Sector, get nodes between Application and Unit Sector
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Application']") != null
                && doc.DocumentNode.SelectSingleNode("//h2[text()='Unit Sector']") != null)
            {
                //The Xpath expression below is used to get nodes between two given node. The formula is $ns1[count(.|$ns2) = count($ns2)], I am not fully understand this formula, but it works.
                var App = doc.DocumentNode.SelectNodes("//h2[text()='Application']/following-sibling::* [count(.|//h2[text()='Unit Sector']/preceding-sibling::*) = count(//h2[text()='Unit Sector']/preceding-sibling::*) ]").ToList();
                foreach (var items in App)
                {
                    //do not add the empty rows into ListBox
                    if (!String.IsNullOrWhiteSpace(items.InnerText.Trim()))
                    {
                        lsbApp.Items.Add(items.InnerText.Trim());
                    }
                }
            }
            //if the unit does not have Unit Sector, get nodes between Application and Elements and Performance Criteria
            else if (doc.DocumentNode.SelectSingleNode("//h2[text()='Unit Sector']") == null)
            {
                // if the unit code that user input is not exist, the result of Xpath expression below is NULL, then it will throw an execption. Using try..catch statement here to avoid the crash of application
                try
                {
                    var App = doc.DocumentNode.SelectNodes("//h2[text()='Application']/following-sibling::* [count(.|//h2[text()='Elements and Performance Criteria']/preceding-sibling::*) = count(//h2[text()='Elements and Performance Criteria']/preceding-sibling::*) ]").ToList();
                    foreach (var items in App)
                    {
                        if (!String.IsNullOrWhiteSpace(items.InnerText.Trim()))
                        {
                            lsbApp.Items.Add(items.InnerText.Trim());
                        }
                    }
                }
                catch (Exception ex) { lblUnitCode.Text = "There is something wrong with your input."; lblUnitName.Text = "The unit code you entered is not exist."; }
            }

            //
            //Get all the li nodes between Performance Evidence and Knowledge Evidence (there is an error with UnitCode:chcece020)
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Performance Evidence']") != null)
            {
                var pe = doc.DocumentNode.SelectNodes("//h2[text()='Performance Evidence']/following-sibling::*/li [count(.|//h2[text()='Knowledge Evidence']/preceding-sibling::*/li) = count(//h2[text()='Knowledge Evidence']/preceding-sibling::*/li) ]").ToList();
                foreach (var items in pe)
                {
                    lsbPE.Items.Add(items.InnerText);
                }
            }
            else
            {
                lsbPE.Items.Add("No Performance Evidence for this unit.");
            }

            //Get all the li nodes between Knowledge Evidence and Assessment Conditions
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Knowledge Evidence']") != null)
            {
                var kn = doc.DocumentNode.SelectNodes("//h2[text()='Knowledge Evidence']/following-sibling::*/li [count(.|//h2[text()='Assessment Conditions']/preceding-sibling::*/li) = count(//h2[text()='Assessment Conditions']/preceding-sibling::*/li) ]").ToList();
                foreach (var items in kn)
                {
                    lsbKE.Items.Add(items.InnerText);
                }
            }
            else
            {
                lsbKE.Items.Add("No Knowledge Evidence for this unit.");
            }

            //Get all the li nodes between Assessment Conditions and Link
            if (doc.DocumentNode.SelectSingleNode("//h2[text()='Assessment Conditions']") != null)
            {
                //var ass = doc.DocumentNode.SelectNodes("//h2[text()='Assessment Conditions']/following-sibling::*/li [count(.|//h2[text()='Links']/preceding-sibling::*/li) = count(//h2[text()='Links']/preceding-sibling::*/li) ]").ToList();
                var ass = doc.DocumentNode.SelectNodes("//h2[text()='Assessment Conditions']/following-sibling::*/li").ToList(); //some units do not have Link section, so get any li nodes after Assessment Conditions section instead
                foreach (var items in ass)
                {
                    lsbAC.Items.Add(items.InnerText);
                }
            }
            else
            {
                lsbAC.Items.Add("No Assessment Conditions for this unit.");
            }

        }
    }
}