using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Aperture_testing_assignment.Tests   
{
    public class ProductApiTests
    {
        private readonly string baseUrl = "https://fakestoreapi.com";
        private readonly ITestOutputHelper _output;

        public ProductApiTests(ITestOutputHelper output)
        {
            _output = output;
        }


        private async Task<RestResponse> ExecuteRequestAsync(RestRequest request)
        {
            var client = new RestClient(baseUrl);
            _output.WriteLine($"Executing Request: {request.Method} {client.BuildUri(request)}");

            var response = await client.ExecuteAsync(request);

            _output.WriteLine($"Response: {response.StatusCode} - {response.Content}");

            return response;
        }

        // Helper method to handle Get and ensure successful responses
        private async Task<JArray> GetProductsAsync()
        {
            var request = new RestRequest("/products", Method.Get);
            var response = await ExecuteRequestAsync(request);
            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotNull(response.Content);
            return JArray.Parse(response.Content);
        }

        // GET  https://fakestoreapi.com/products
        [Fact]
        public async Task GetProducts_ShouldReturnListOfProducts()
        {
            var products = await GetProductsAsync();
            Assert.NotEmpty(products);
            _output.WriteLine($"Returned {products.Count} products.");
        }

        // GET  https://fakestoreapi.com/products/{product_id}
        [Fact]
        public async Task GetSpecificProduct_ShouldReturnCorrectProduct()
        {
            int searchProductId = 2;
            var request = new RestRequest($"/products/{searchProductId}", Method.Get);
            var response = await ExecuteRequestAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotNull(response.Content);

            var product = JObject.Parse(response.Content);
            Assert.Equal(searchProductId, (int)product["id"]);
        }

        // POST https://fakestoreapi.com/products
        [Fact]
        public async Task PostProduct_ShouldCreateNewProduct()
        {
            var newProduct = new
            {
                title = "New Product",
                price = 29.99,
                description = "A new product description",
                category = "electronics",
                image = "https://fakestoreapi.com/img/newproduct.png"
            };

            var request = new RestRequest("/products", Method.Post);
            request.AddJsonBody(newProduct);

            var response = await ExecuteRequestAsync(request);
            Assert.Equal(200, (int)response.StatusCode);

            var createdProduct = JObject.Parse(response.Content);
            Assert.Equal(newProduct.title, (string)createdProduct["title"]);
            Assert.Equal((decimal)newProduct.price, (decimal)createdProduct["price"]);
        }

        // PUT https://fakestoreapi.com/products/{productId}
        [Fact]
        public async Task PutProduct_ShouldUpdateExistingProduct()
        {
            int productId = 7;
            var updatedProduct = new
            {
                title = "Updated Product Title",
                price = 59.99,
                description = "This product has been updated",
                category = "electronics",
                image = "https://fakestoreapi.com/img/updatedproduct.png"
            };

            var request = new RestRequest($"/products/{productId}", Method.Put);
            request.AddJsonBody(updatedProduct);

            var response = await ExecuteRequestAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            var updatedProductResponse = JObject.Parse(response.Content);
            Assert.Equal(updatedProduct.title, (string)updatedProductResponse["title"]);
        }

        // DELETE https://fakestoreapi.com/products/{productId}   ***  the delete will be fail cause this site cannot delete mockup prodduct
        [Fact]
        public async Task DeleteProduct_ShouldCheckIfExistsAndRemoveProduct()
        {
            int productId = 5;

            // Ensure the product exists first
            var getRequest = new RestRequest($"/products/{productId}", Method.Get);
            var getResponse = await ExecuteRequestAsync(getRequest);

            if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Assert.Equal(404, (int)getResponse.StatusCode);
                return; // Exit the test early if the product doesn't exist
            }

            // Proceed to delete the product
            var deleteRequest = new RestRequest($"/products/{productId}", Method.Delete);
            var deleteResponse = await ExecuteRequestAsync(deleteRequest);
            Assert.True(deleteResponse.StatusCode == System.Net.HttpStatusCode.OK || deleteResponse.StatusCode == System.Net.HttpStatusCode.NoContent);

            // Recheck if product was deleted
            var getAfterDeleteRequest = new RestRequest($"/products/{productId}", Method.Get);
            var getAfterDeleteResponse = await ExecuteRequestAsync(getAfterDeleteRequest);
            Assert.Equal(404, (int)getAfterDeleteResponse.StatusCode);
        }

        // GET https://fakestoreapi.com/products/categories
        [Fact]
        public async Task GetCategories_ShouldReturnListOfCategories()
        {
            var request = new RestRequest("/products/categories", Method.Get);
            var response = await ExecuteRequestAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotNull(response.Content);

            var categories = JArray.Parse(response.Content);
            Assert.NotEmpty(categories);
        }

        // GET https://fakestoreapi.com/products?sort=asc (or other sort variations)
        [Theory]
        [InlineData("asc")]
        [InlineData("desc")]
        public async Task GetProductsSorted_ShouldReturnSortedProducts(string sortType)
        {
            var request = new RestRequest($"/products?sort={sortType}", Method.Get);
            var response = await ExecuteRequestAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotNull(response.Content);

            var products = JArray.Parse(response.Content);
            Assert.NotEmpty(products);

            // Assuming sorting should be done by price instead of ID
            for (int i = 1; i < products.Count; i++)
            {
                var prevProduct_ID = (decimal)products[i - 1]["id"];
                var curProduct_ID = (decimal)products[i]["id"];

                if (sortType == "asc")
                {
                    Assert.True(prevProduct_ID <= curProduct_ID, "Products are not sorted in ascending order.");
                }
                else
                {
                    Assert.True(prevProduct_ID >= curProduct_ID, "Products are not sorted in descending order.");
                }
            }
        }

        [Fact]
        public async Task GetNonExistentProduct_ShouldReturnNotFound()
        {
            // Arrange
            var client = new RestClient(baseUrl);
            int invalidProductId = 9999; // using none exiting in system
            var request = new RestRequest($"/products/{invalidProductId}", Method.Get);

            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            Assert.Equal(404, (int)response.StatusCode); // Expected status 404 
            Assert.Null(response.Content); // check response not contain content (204 No Content)
        }

        // PUT https://fakestoreapi.com/products/{productId} - Bad Request tests
        [Theory]
        [InlineData(null, 12.99, "95%Cotton,5%Spandex, Features: Casual...", "women's clothing", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // title is null
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", -1, "95%Cotton,5%Spandex, Features: Casual...", "women's clothing", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // negative price
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "", "women's clothing", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // empty description
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "95%Cotton,5%Spandex, Features: Casual...", "", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // empty category
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "95%Cotton,5%Spandex, Features: Casual...", "women's clothing", "")] // empty image URL
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "95%Cotton,5%Spandex, Features: Casual...", "invalid_category", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // invalid category
        public async Task PutProduct_ShouldReturnBadRequest(
            string title, decimal price, string description, string category, string image)
        {
            // Arrange
            var client = new RestClient(baseUrl);
            int productId = 21; // Product ID to update (can be any existing ID)

            var updateRequest = new RestRequest($"/products/{productId}", Method.Put);
            var updatedProduct = new
            {
                title = title,
                price = price,
                description = description,
                category = category,
                image = image
            };
            updateRequest.AddJsonBody(updatedProduct);

            // Act
            var response = await client.ExecuteAsync(updateRequest);

            // Assert
            Assert.Equal(400, (int)response.StatusCode); // Expecting 400 Bad Request
        }

        [Fact]
        public async Task DeleteNonExistentProduct_ShouldReturnNotFound()
        {
            // Arrange
            var client = new RestClient(baseUrl);
            int invalidProductId = 9999; // using none exiting in system
            var request = new RestRequest($"/products/{invalidProductId}", Method.Delete);

            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            Assert.Equal(404, (int)response.StatusCode); // expexted to get 404 by Non Existent product
        }

        [Fact]
        public async Task PutProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var client = new RestClient(baseUrl);
            int invalidProductId = 9999; // using none exiting in system
            var updateRequest = new RestRequest($"/products/{invalidProductId}", Method.Put);

            var updatedProduct = new
            {
                title = "Updated Product Title",
                price = 59.99,
                description = "This product has been updated",
                category = "electronics",
                image = "https://fakestoreapi.com/img/updatedproduct.png"
            };

            updateRequest.AddJsonBody(updatedProduct);

            // Act
            var updateResponse = await client.ExecuteAsync(updateRequest);

            // Assert
            Assert.Equal(404, (int)updateResponse.StatusCode); // expexted to get 404 by Non Existent product
        }

    }
}
