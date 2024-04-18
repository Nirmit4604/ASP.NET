using DotNetDrinks.Controllers;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDrinks.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        [TestMethod]
        public async Task Edit_Get_ReturnsEditView()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Edit_Get_ReturnsEditView_Test_Database")
                .Options;
            using (var context = new ApplicationDbContext(options))
            {
                // Seed some test data into the in-memory database
                var product = new Product { Id = 1, Name = "Test Product" };
                context.Products.Add(product);
                context.SaveChanges();

                var controller = new ProductsController(context);

                // Act
                var result = await controller.Edit(1);

                // Assert
                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult);
                Assert.AreEqual("Edit", viewResult.ViewName);
            }
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesProductFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteConfirmed_RemovesProductFromDatabase_Test_Database")
                .Options;
            using (var context = new ApplicationDbContext(options))
            {
                // Seed some test data into the in-memory database
                var product = new Product { Id = 1, Name = "Test Product" };
                context.Products.Add(product);
                context.SaveChanges();

                var controller = new ProductsController(context);

                // Act
                await controller.DeleteConfirmed(1);

                // Assert
                var deletedProduct = context.Products.FirstOrDefault(p => p.Id == 1);
                Assert.IsNull(deletedProduct);
            }
        }
    }
}
