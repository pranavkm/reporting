using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Report([FromServices] IHostingEnvironment hostingEnv)
        {
            var rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Path.Combine(hostingEnv.ContentRootPath, "Reports", "Report1.rdlc");
            var table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Name");
            table.Rows.Add(1);
            table.Rows.Add(2);
            var ds = new ReportDataSource("DataSet1", table);
            rv.LocalReport.DataSources.Add(ds);
            rv.LocalReport.Refresh();

            string mimeType;
            string encoding;
            string filenameExtension;
            string[] streams;
            Warning[] warnings;

            var streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

            return File(streamBytes, mimeType, "TestReport.pdf");
        }
    }
}
