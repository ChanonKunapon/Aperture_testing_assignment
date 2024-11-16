using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace Aperture_testing_assignment.Tests
{
    public class ProductApiTests
    {
        private readonly string baseUrl = "https://fakestoreapi.com";
        private readonly ITestOutputHelper _output;
        private readonly string logFolderPath = "api_test_Log"; // Folder to store logs
        private readonly string logFilePath;

        public ProductApiTests(ITestOutputHelper output)
        {
            _output = output;

            // Define log folder path relative to the output directory
            logFolderPath = Path.Combine(AppContext.BaseDirectory, "api_test_Log");

            // Ensure the log folder exists
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            // Generate a unique log file name for each test run
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            logFilePath = Path.Combine(logFolderPath, $"API_tests_{timestamp}.log");
        }


        private void LogToFile(string message)
        {
            using (var writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }


        private async Task<RestResponse> ExecuteRequestAsync(RestRequest request)
        {
            var client = new RestClient(baseUrl);
            string logMessage = $"Executing Request: {request.Method} {client.BuildUri(request)}";
            _output.WriteLine(logMessage);
            LogToFile(logMessage);  // Log to file

            var response = await client.ExecuteAsync(request);

            logMessage = $"Response: {response.StatusCode} - {response.Content}";
            _output.WriteLine(logMessage);
            LogToFile(logMessage);  // Log to file

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
            LogToFile("START case GetProducts_ShouldReturnListOfProducts");
            var products = await GetProductsAsync();
            Assert.NotEmpty(products);
            string logMessage = $"Returned {products.Count} products.";
            _output.WriteLine(logMessage);
            LogToFile(logMessage);  // Log to file
            LogToFile("END case GetProducts_ShouldReturnListOfProducts");
        }

        // GET  https://fakestoreapi.com/products/{product_id}
        [Fact]
        public async Task GetSpecificProduct_ShouldReturnCorrectProduct()
        {
            LogToFile("START case GetSpecificProduct_ShouldReturnCorrectProduct");
            int searchProductId = 2;
            var request = new RestRequest($"/products/{searchProductId}", Method.Get);
            LogToFile($"Request searchProductId is {searchProductId}");
            var response = await ExecuteRequestAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            LogToFile($"Response Status code is {(int)response.StatusCode}");
            Assert.NotNull(response.Content);

            var product = JObject.Parse(response.Content);
            LogToFile($"Search product code is {(int)product["id"]}");
            Assert.Equal(searchProductId, (int)product["id"]);
            LogToFile("END case GetSpecificProduct_ShouldReturnCorrectProduct");
        }

        // POST https://fakestoreapi.com/products
        [Fact]
        public async Task PostProduct_ShouldCreateNewProduct()
        {
            LogToFile("START case PostProduct_ShouldCreateNewProduct");
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
            LogToFile($"Sent newproduct title is {newProduct.title}");
            LogToFile($"Sent newproduct price is {newProduct.price}");
            LogToFile($"Sent newproduct description is {newProduct.description}");
            LogToFile($"Sent newproduct category  is {newProduct.category}");
            LogToFile($"Sent newproduct image  is {newProduct.image}");


            var response = await ExecuteRequestAsync(request);
            Assert.Equal(200, (int)response.StatusCode);

            var createdProduct = JObject.Parse(response.Content);

            Assert.Equal(newProduct.title, (string)createdProduct["title"]);
            LogToFile($"Response new product title is {(string)createdProduct["title"]}");

            Assert.Equal((decimal)newProduct.price, (decimal)createdProduct["price"]);
            LogToFile($"Response new product price is {(decimal)createdProduct["price"]}");

            Assert.Equal((string)newProduct.description, (string)createdProduct["description"]);
            LogToFile($"Response new product description is {(string)createdProduct["description"]}");

            Assert.Equal((string)newProduct.category, (string)createdProduct["category"]);
            LogToFile($"Response new product category is {(string)createdProduct["category"]}");

            Assert.Equal((string)newProduct.image, (string)createdProduct["image"]);
            LogToFile($"Response new product image is {(string)createdProduct["image"]}");

            LogToFile("END case PostProduct_ShouldCreateNewProduct");
        }

        // PUT https://fakestoreapi.com/products/{productId}
        [Fact]
        public async Task PutProduct_ShouldUpdateExistingProduct()
        {
            LogToFile("START case PutProduct_ShouldUpdateExistingProduct");
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

            LogToFile($"Send put request productId is {productId}");
            LogToFile($"Send put request title is {updatedProduct.title}");
            LogToFile($"Send put request price is {updatedProduct.price}");
            LogToFile($"Send put request description is {updatedProduct.description}");
            LogToFile($"Send put request category is {updatedProduct.category}");
            LogToFile($"Send put request image is {updatedProduct.image}");
            request.AddJsonBody(updatedProduct);

            var response = await ExecuteRequestAsync(request);

            Assert.Equal(200, (int)response.StatusCode);
            LogToFile($"Response Status Code is {(int)response.StatusCode}");
            var updatedProductResponse = JObject.Parse(response.Content);

            Assert.Equal(updatedProduct.title, (string)updatedProductResponse["title"]);
            LogToFile($"Response new product title is {(string)updatedProductResponse["title"]}");

            Assert.Equal((decimal)updatedProduct.price, (decimal)updatedProductResponse["price"]);
            LogToFile($"Response new product price is {(decimal)updatedProductResponse["price"]}");

            Assert.Equal(updatedProduct.description, (string)updatedProductResponse["description"]);
            LogToFile($"Response new product description is {(string)updatedProductResponse["description"]}");

            Assert.Equal(updatedProduct.category, (string)updatedProductResponse["category"]);
            LogToFile($"Response new product category is {(string)updatedProductResponse["category"]}");

            Assert.Equal(updatedProduct.image, (string)updatedProductResponse["image"]);
            LogToFile($"Response new product tiimagetle is {(string)updatedProductResponse["image"]}");

            LogToFile("END case PutProduct_ShouldUpdateExistingProduct");
        }

        // DELETE https://fakestoreapi.com/products/{productId}   ***  the delete will be fail cause this site cannot delete mockup prodduct
        [Fact]
        public async Task DeleteProduct_ShouldCheckIfExistsAndRemoveProduct()
        {
            LogToFile("START case DeleteProduct_ShouldCheckIfExistsAndRemoveProduct");
            int productId = 5;
            LogToFile($"request delete productID is {productId}");
            // Ensure the product exists first
            var getRequest = new RestRequest($"/products/{productId}", Method.Get);
            var getResponse = await ExecuteRequestAsync(getRequest);

            if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                LogToFile($"the product doesn't exist");
                Assert.Equal(404, (int)getResponse.StatusCode);
                return; // Exit the test early if the product doesn't exist
            }

            // Proceed to delete the product
            var deleteRequest = new RestRequest($"/products/{productId}", Method.Delete);
            var deleteResponse = await ExecuteRequestAsync(deleteRequest);
            Assert.True(deleteResponse.StatusCode == System.Net.HttpStatusCode.OK || deleteResponse.StatusCode == System.Net.HttpStatusCode.NoContent);
            LogToFile($"Response status code is {deleteResponse.StatusCode}");

            // Recheck if product was deleted
            var getAfterDeleteRequest = new RestRequest($"/products/{productId}", Method.Get);

            LogToFile($"Recheck request delete productID is {productId}");
            var getAfterDeleteResponse = await ExecuteRequestAsync(getAfterDeleteRequest);

            if ((int)getAfterDeleteResponse.StatusCode == 404)
            {
                LogToFile($"deleted productID has been successfully");
            }
            else LogToFile($"deleted productID still occure");

            Assert.Equal(404, (int)getAfterDeleteResponse.StatusCode);

            LogToFile("END case DeleteProduct_ShouldCheckIfExistsAndRemoveProduct");
        }

        // GET https://fakestoreapi.com/products/categories
        [Fact]
        public async Task GetCategories_ShouldReturnListOfCategories()
        {
            LogToFile("START case GetCategories_ShouldReturnListOfCategories");
            var request = new RestRequest("/products/categories", Method.Get);
            LogToFile("Request to send /products/categories");

            var response = await ExecuteRequestAsync(request);
            LogToFile($"Response status code is {(int)response.StatusCode}");
            Assert.Equal(200, (int)response.StatusCode);

            if (response.Content != null)
            {
                LogToFile($"Response catagories is not null");
            }
            else LogToFile($"Response catagories is null");

            Assert.NotNull(response.Content);

            var categories = JArray.Parse(response.Content);
            Assert.NotEmpty(categories);
            LogToFile("END case GetCategories_ShouldReturnListOfCategories");
        }

        // GET https://fakestoreapi.com/products?sort=asc (or other sort variations)
        [Theory]
        [InlineData("asc")]
        [InlineData("desc")]
        public async Task GetProductsSorted_ShouldReturnSortedProducts(string sortType)
        {
            LogToFile($"START case GetProductsSorted_ShouldReturnSortedProducts with sortType = {sortType}");
            var request = new RestRequest($"/products?sort={sortType}", Method.Get);
            LogToFile($"Request to sortType is {sortType}");

            var response = await ExecuteRequestAsync(request);

            LogToFile($"Response status code is {(int)response.StatusCode}");
            Assert.Equal(200, (int)response.StatusCode);

            if (response.Content != null)
            {
                LogToFile($"Response Content is not null");
            }
            else LogToFile($"Response Content is null");

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
                    LogToFile($"Products are not sorted in ascending order.");
                }
                else
                {
                    Assert.True(prevProduct_ID >= curProduct_ID, "Products are not sorted in descending order.");
                    LogToFile($"Products are not sorted in descending order.");
                }
            }
            LogToFile($"END case GetProductsSorted_ShouldReturnSortedProducts with sortType = {sortType}");
        }

        [Fact]
        public async Task GetNonExistentProduct_ShouldReturnNotFound()
        {
            LogToFile($"START case GetNonExistentProduct_ShouldReturnNotFound");
            // Arrange
            var client = new RestClient(baseUrl);
            int invalidProductId = 9999; // using none exiting in system
            var request = new RestRequest($"/products/{invalidProductId}", Method.Get);
            LogToFile($"Request invalidProductId is {invalidProductId}");
            // Act
            var response = await client.ExecuteAsync(request);
            LogToFile($"Response from server.");


            // Assert
            Assert.Equal(404, (int)response.StatusCode); // Expected status 404 
            LogToFile($"Response StatusCode is {(int)response.StatusCode}");
            Assert.Null(response.Content); // check response not contain content (204 No Content)
            if (response.Content != null)
            {
                LogToFile($"Invalid product can found");
            }
            else LogToFile($"Invalid product can not found");
            LogToFile($"START case GetNonExistentProduct_ShouldReturnNotFound");
        }

        // PUT https://fakestoreapi.com/products/{productId} - Bad Request tests
        [Theory]
        [InlineData(null, 12.99, "95%Cotton,5%Spandex, Features: Casual...", "women's clothing", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // title is null
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", -1, "95%Cotton,5%Spandex, Features: Casual...", "women's clothing", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // negative price
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "", "women's clothing", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // empty description
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "95%Cotton,5%Spandex, Features: Casual...", "", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // empty category
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "95%Cotton,5%Spandex, Features: Casual...", "women's clothing", "")] // empty image URL
        [InlineData("DANVOUY Womens T Shirt Casual Cotton Short", 12.99, "95%Cotton,5%Spandex, Features: Casual...", "invalid_category", "https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_.jpg")] // invalid category
        public async Task PutProduct_ShouldReturnBadRequest(string title, decimal price, string description, string category, string image)
        {
            if (title == null) 
            { 
                LogToFile($"START case PutProduct_ShouldReturnBadRequest 'title is NULL'");
            }
            else if (price <0)
            {
                LogToFile($"START case PutProduct_ShouldReturnBadRequest 'negative price'");
            }
            else if (description ==null)
            {
                LogToFile($"START case PutProduct_ShouldReturnBadRequest 'empty description'");
            }
            else if (category == null)
            {
                LogToFile($"START case PutProduct_ShouldReturnBadRequest 'empty category'");
            }
            else if (image == null)
            {
                LogToFile($"START case PutProduct_ShouldReturnBadRequest 'empty image URL'");
            }
            else if (category == "invalid_category")
            {
                LogToFile($"START case PutProduct_ShouldReturnBadRequest 'invalid_category'");
            }

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
            LogToFile($"Request title is {title}");
            LogToFile($"Request price is {price}");
            LogToFile($"Request description is {description}");
            LogToFile($"Request category is {category}");
            LogToFile($"Request image is {image}");


            // Act
            var response = await client.ExecuteAsync(updateRequest);

            // Assert
            Assert.Equal(400, (int)response.StatusCode); // Expecting 400 Bad Request
            if ((int)response.StatusCode != 400)
            {
                LogToFile($"Response StatusCode is {(int)response.StatusCode} not 400 Badrequest");
            }
            else LogToFile($"Response 400 Bad Request");

            if (title == null)
            {
                LogToFile($"END case PutProduct_ShouldReturnBadRequest 'title is NULL'");
            }
            else if (price < 0)
            {
                LogToFile($"END case PutProduct_ShouldReturnBadRequest 'negative price'");
            }
            else if (description == null)
            {
                LogToFile($"END case PutProduct_ShouldReturnBadRequest 'empty description'");
            }
            else if (category == null)
            {
                LogToFile($"END case PutProduct_ShouldReturnBadRequest 'empty category'");
            }
            else if (image == null)
            {
                LogToFile($"END case PutProduct_ShouldReturnBadRequest 'empty image URL'");
            }
            else if (category == "invalid_category")
            {
                LogToFile($"END case PutProduct_ShouldReturnBadRequest 'invalid_category'");
            }
        }

        [Fact]
        public async Task DeleteNonExistentProduct_ShouldReturnNotFound()
        {
            LogToFile($"START case DeleteNonExistentProduct_ShouldReturnNotFound");
            // Arrange
            var client = new RestClient(baseUrl);
            int invalidProductId = 9999; // using none exiting in system
            var request = new RestRequest($"/products/{invalidProductId}", Method.Delete);
            LogToFile($"Request invalidProductId is {invalidProductId}");
            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            Assert.Equal(404, (int)response.StatusCode); // expexted to get 404 by Non Existent product
            if ((int)response.StatusCode != 400)
            {
                LogToFile($"Response StatusCode is {(int)response.StatusCode} not 400 Badrequest");
            }
            else LogToFile($"Response {(int)response.StatusCode} Bad Request Non Existent product");
            LogToFile($"END case DeleteNonExistentProduct_ShouldReturnNotFound");
        }

        [Fact]
        public async Task PutNonExistentProductProduct_ShouldReturnNotFound()
        {
            LogToFile($"START case PutNonExistentProductProduct_ShouldReturnNotFound");

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
            LogToFile($"Request update invalidProductId is {invalidProductId}");
            LogToFile($"Request update title is {updatedProduct.title}");
            LogToFile($"Request update price is {updatedProduct.price}");
            LogToFile($"Request update description is {updatedProduct.description}");
            LogToFile($"Request update category is {updatedProduct.category}");
            LogToFile($"Request update image is {updatedProduct.image}");

            // Act
            var updateResponse = await client.ExecuteAsync(updateRequest);

            // Assert
            Assert.Equal(404, (int)updateResponse.StatusCode); // expexted to get 404 by Non Existent product
            if ((int)updateResponse.StatusCode != 400)
            {
                LogToFile($"Response StatusCode is {(int)updateResponse.StatusCode} not 400 Badrequest");
            }
            else LogToFile($"Response {(int)updateResponse.StatusCode} Bad Request Non Existent product");

            LogToFile($"END case PutNonExistentProductProduct_ShouldReturnNotFound");
        }

    }
}
