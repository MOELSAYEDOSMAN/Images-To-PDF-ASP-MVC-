using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _ConvertImagesToPDF.Models;
using System.Data;
using System.IO;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Web;
using System;

namespace _ConvertImagesToPDF.Controllers
{
 
    public class _CovertController : Controller
    {
        private CovertPdfEntities db = new CovertPdfEntities();
        #region Start Convert
        // GET: _Covert
        public ActionResult Index()
        {
            return View();
        }
        //Post
        [HttpPost]
        
        public FileContentResult Index(List<HttpPostedFileBase> Files,string NameFile)
        {
            #region start Save Photo
            LocalReport local = new LocalReport();
            local.ReportPath = Server.MapPath("~/Reports/Report1.rdlc");
            ReportDataSource reportData = new ReportDataSource();
            reportData.Name = "DataSet2";
            int i = 1;
            int x = Files.Count;
            List<Jpg_To_Pdf> ls = new List<Jpg_To_Pdf>();
            foreach(HttpPostedFileBase fle in Files)
            {
                #region covert image To PDf
                byte[] fileByte = new byte[fle.ContentLength];
                fle.InputStream.Read(fileByte,0,fle.ContentLength);
                string base64string = Convert.ToBase64String(fileByte);
                byte[] newByFile = Convert.FromBase64String(base64string);
                #endregion
                Jpg_To_Pdf newJpj = new Jpg_To_Pdf
                {
                    id = i,
                    Images = newByFile
                };
                ls.Add(newJpj);
                i++;
            }
            reportData.Value = ls;
            local.DataSources.Add(reportData);
            string mimType, encoding, fileNameExtion = "application/pdf", type= "PDF";
            string[] strems;
            Warning[] warnings;
            byte[] renderByte;
            renderByte = local.Render(type, "", out mimType, out encoding, out fileNameExtion, out strems, out warnings);
            Response.AddHeader("Content-disposition", "attachment;filename="+ NameFile + ".pdf");
            ls.Clear();
            return File(renderByte, System.Net.Mime.MediaTypeNames.Application.Octet);

            #endregion

        }


        #endregion
    }
}