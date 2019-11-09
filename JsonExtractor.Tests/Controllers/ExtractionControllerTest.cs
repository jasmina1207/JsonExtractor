using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonExtractor;
using JsonExtractor.Controllers;
using JsonExtractor.Models;
using System.Web;
using Moq;
using System.IO;
using System.Web.Routing;
using System.Threading.Tasks;

namespace JsonExtractor.Tests.Controllers
{
    [TestClass]
    public class ExtractionControllerTest
    {
        [TestMethod]
        public async Task UploadPass()
        {
            // Arrange
            
            ExtractionController controller = new ExtractionController();
         
            string filePath = Path.GetFullPath(@"testfile\hotelrates.json");
            StreamReader fileStream = new StreamReader(filePath);

            //// Act
            
            IEnumerable<Booking> bookings = await controller.GetBookingsFromFile(fileStream);

            // Assert
            Assert.IsNotNull(bookings);
            Assert.IsNotNull(bookings.Count()==104);

        }

    }
}
