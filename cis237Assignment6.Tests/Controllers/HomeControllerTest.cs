using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cis237Assignment6;
using cis237Assignment6.Controllers;

namespace cis237Assignment6.Tests.Controllers
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
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your Centralized Beverage Managment Resource", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //Test Beverage Create Page
        [TestClass]
        public class BeveragesControllerTest
        {
            [TestMethod]
            public void BeverageCreate()
            {
                // Arrange
                BeveragesController controller = new BeveragesController();

                // Act
                ViewResult result = controller.Create() as ViewResult;

                // Assert
                // Assert.AreEqual("Beverage List", result.ViewBag.Title);
                Assert.IsNotNull(result);
            }
        }

        //Test add phone number page
        [TestClass]
        public class ManageControllerTest
        {
            [TestMethod]
            public void AddPhoneNumber()
            {
                // Arrange
                ManageController controller = new ManageController();

                // Act
                ViewResult result = controller.AddPhoneNumber() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
            }
        }
    }
}
