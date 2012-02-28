using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PanizoMVC;
using PanizoMVC.Controllers;
using System.Globalization;

namespace PanizoMVC.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Prueba()
        {
            String fecha = "02/27/2012";
            DateTime date;
            
            date = DateTime.ParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            date = DateTime.Parse(fecha);
        }
    }
}
