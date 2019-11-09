using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using JsonExtractor.Models;

namespace JsonExtractor.Logic
{


    public class JsonItem
    {
        public Hotel hotel { get; set; }
        public List<HotelRates> hotelRates { get; set; }
    }
    public class Hotel
    {
       public string hotelID { get; set; }
       public int classification { get; set; }
       public string name { get; set; }
       public float reviewscore { get; set; }
  
    }

    public class HotelRates
    {
        public int adults { get; set; }
        public int los { get; set; }
        public Price price { get; set; }
        public string rateDescription { get; set; }
        public int rateID { get; set; }
        public string rateName { get; set; }
        public List<RateTags> rateTags { get; set; }
        public DateTime targetDay { get; set; }
        public Booking ToBooking()
        {
            return new Booking()
            {
                ARRIVAL_DATE = targetDay.Date.ToString("dd.MM.yy"),
                DEPARTURE_DATE = targetDay.AddDays(los).Date.ToString("dd.MM.yy"),
                PRICE = price.numericFloat,
                CURRENCY = price.currency,
                RATENAME = rateName,
                ADULTS = adults,
                BREAKFAST_INCLUDED = rateTags.Where(tag => tag.name == "breakfast").Select(tag => tag.shape).FirstOrDefault() ? 1 : 0
            };
        }
    }

    public class RateTags
    {
        public string name { get; set; }
        public bool shape { get; set; }
    }

    public class Price
    {
        public string currency { get; set; }
        public float numericFloat { get; set; }
        public int numericInteger { get; set; }
    }


    public static class TaskLogic
    {
   
        public static string RenderExcelContent(IEnumerable<Booking> bookings)
        {
            try
            {

                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = bookings;
                grid.DataBind();
                
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);

                return sw.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<Booking> ReadJason(string jsonContent)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                JsonItem item = serializer.Deserialize<JsonItem>(jsonContent);
                if (item != null)
                {
                    List<Booking> list = new List<Booking>();
                    if (item.hotelRates != null && item.hotelRates.Count > 0)
                    {

                        foreach (HotelRates hotelRate in item.hotelRates)
                        {
                            list.Add(hotelRate.ToBooking());
                        }
                    }
                    else
                    {
                        throw new Exception("No Hotel Rates could be found.");
                    }
                    return list;

                }
                else
                {
                    throw new Exception("Uploaded file does not contain expected format.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }


       
    }


   
}