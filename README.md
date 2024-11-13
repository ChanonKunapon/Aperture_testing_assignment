Aperture Testing Assignment

This repository contains API testing automation scripts for the Aperture Testing Assignment. 
The goal of this project is to validate the functionality and reliability of the Fake Store API using automated tests written in C# and XUnit.

Project Overview

PART 1 (API TEST)
  This project automates testing for the Fake Store API(https://fakestoreapi.com). 
  It tests various API endpoints for fetching products, posting new products, updating existing products, and deleting products. 
  Both positive and negative test cases are included to ensure thorough validation.
  
  
  
  Features:
   - Test for valid and invalid product retrieval.
   - Post, update, and delete product operations.
   - Negative scenarios to cover bad requests, missing products, and failed updates.
   - Comprehensive test logging and reporting.
  
  Technologies Used
   - C#
   - RestSharp: For making HTTP requests.
   - XUnit: For unit testing.
   - Newtonsoft.Json (Json.NET): For JSON parsing and handling.
   - .NET SDK: To build and run the project
  
  Setup Instructions
  To get started, follow these steps:
  
  Prerequisites
   - Install .NET SDK.
   - Clone this repository.
     
  Installation
  1) Clone the repository to your local machine: git clone https://github.com/your-username/Aperture_testing_assignment.git
  2) Navigate into the project directory:  cd Aperture_testing_assignment
  3) Restore the dependencies:  dotnet restore
  
  Running Tests
  To execute the tests:
     1) Navigate to the root of the project directory.
     2) Run the following command to execute all tests: dotnet test
  
  Running with Detailed Test Results
  To generate a detailed test report in .trx format, use the following command:  dotnet test --logger "trx;LogFileName=test_results.trx"
  The report will be saved as test_results.trx in the root folder.
  
  Logging & Reporting
  Tests are set up to include comprehensive logs for easier debugging and reporting:
  
   - Detailed Error Messages: The tests include clear assertions that will provide feedback in case of test failures, including expected vs. actual results.
   - Test Reports: The TRX format allows you to view a detailed report on passed/failed tests, which can be integrated into CI/CD systems.
  
  Negative Path Coverage
    - This project includes several negative path test cases to ensure robustness in handling failures and edge cases:
    - GetNonExistentProduct_ShouldReturnNotFound(): Tests that trying to retrieve a non-existent product returns a 404 error.
    - PutProduct_ShouldReturnNotFound_WhenProductDoesNotExist(): Tests that updating a non-existent product returns a 404 error.
    - PutProduct_ShouldReturnBadRequest(): Tests that sending invalid input (e.g., null or incorrect values) to the update product endpoint returns a 400 Bad Request.
    - DeleteNonExistentProduct_ShouldReturnNotFound(): Tests that deleting a non-existent product returns a 404 error.
  
  Contributing
  We welcome contributions! If you'd like to improve the tests or add more coverage, follow these steps:
  
  1) Fork the repository.
  2) Create a new branch with your feature/bug fix.
  3) Push to your fork and submit a pull request.
     
  Please make sure your changes include appropriate test coverage and are aligned with the existing testing framework.

PART 2 (UI TEST)
