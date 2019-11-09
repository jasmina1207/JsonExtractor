using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using JsonExtractor.Models;

namespace JsonExtractor.Controllers
{
    public class ExtractionController : Controller
    {
        public ActionResult Extract()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Extract(HttpPostedFileBase file)
        {
            try
            {
                IEnumerable<Booking> bookings;
                if (file != null && file.ContentLength > 0)
                {
                    using (StreamReader stream = new StreamReader(file.InputStream))
                    {

                        bookings = await GetBookingsFromFile(stream);

                        string content = Logic.TaskLogic.RenderExcelContent(bookings);
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                        Response.ContentType = "application / vnd.openxmlformats - officedocument.spreadsheetml.sheet";
                        Response.Output.Write(content);

                        Response.Charset = "";
                        Response.Flush();
                        Response.End();
                    }

                }
                else
                {
                    throw new Exception("File not found or empty.");
                }
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("../Shared/Error");
            }


        }

        public async Task<IEnumerable<Booking>> GetBookingsFromFile(StreamReader stream)
        {
            try
            {
                string jsonContent = await stream.ReadToEndAsync();
                return Logic.TaskLogic.ReadJason(jsonContent);
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}