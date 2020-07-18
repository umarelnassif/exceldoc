using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Web.Mvc;
using System;

namespace exceldoc.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        OleDbConnection Econ;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

            string filepath = "/excelfolder/" + filename;

            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));

            InsertExceldata(filepath, filename);

            return View();
        }

        private void ExcelConn(string filepath)

        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);

            Econ = new OleDbConnection(constr);
        }
        private void InsertExceldata(string fileepath, string filename)

        {

            string fullpath = Server.MapPath("/excelfolder/") + filename;

            ExcelConn(fullpath);

            string query = string.Format("Select * from [{0}]", "Sheet1$");

            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            Econ.Open();
            DataSet ds = new DataSet();

            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);

            Econ.Close();

            oda.Fill(ds);

            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            objbulk.DestinationTableName = "User";

            objbulk.ColumnMappings.Add("Date", "Date");

            objbulk.ColumnMappings.Add("Region", "Region");

            objbulk.ColumnMappings.Add("Person", "Person");

            objbulk.ColumnMappings.Add("Item", "Item");

            objbulk.ColumnMappings.Add("Units", "Units");

            objbulk.ColumnMappings.Add("UnitCost", "UnitCost");

            objbulk.ColumnMappings.Add("Total", "Total");

            con.Open();

            objbulk.WriteToServer(dt);

            con.Close();

        }
    }
}