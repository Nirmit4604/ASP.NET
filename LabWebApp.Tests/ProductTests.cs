
using Xunit;
using LabWebApp.Models;
namespace LabWebApp.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_PropertiesSetCorrectly()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 9.99m,
                Description = "Test product description"
            };

            // Act
            // No action needed for this test.

            // Assert
            Assert.Equal(1, product.Id);
            Assert.Equal("Test Product", product.Name);
            Assert.Equal(9.99m, product.Price);
            Assert.Equal("Test product description", product.Description);
        }

        [Fact]
        public void Product_PriceFormattedCorrectlyToString()
        {
            // Arrange
            var product = new Product
            {
                Id = 2,
                Name = "Another Test Product",
                Price = 15.5m,
                Description = "Another test product description"
            };

            // Act
            var formattedPrice = FormatPriceToString(product.Price); // Format price as currency

            // Assert
            Assert.Equal("$15.50", formattedPrice); // Assuming default currency formatting
        }

        private string FormatPriceToString(decimal price)
        {
            // Local function to format the price as currency
            return price.ToString("C");
        }
    }
}
