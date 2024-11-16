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
  


PART 2 (UI TEST)

Overview

This project automates UI tests for the "https://www.saucedemo.com/" website using Selenium WebDriver, NUnit, and C#. The tests verify various user flows such as login, product addition to the cart, checkout, and logout. The tests are structured using Page Object Model (POM) for better maintainability.

Features
  Login Tests: Valid and invalid login scenarios with different user types.
  Checkout Flow: Tests for adding products to the cart and completing the checkout process.
  Negative Test Cases: Invalid credentials and empty field validations.
  Logging: All test activities are logged into a dynamically created log file for each test run.

Project Structure

  resources/ Directory
      page_object/: Contains page objects for various parts of the UI such as login, product page, cart, checkout, etc.
        common.cs: Contains reusable methods and helpers used across the test pages.
        Logger.cs: Custom logger for logging test activities and errors to a file with a dynamic name for each run.

  tests/ Directory
  Tests.cs: Contains the actual test cases and test suites using NUnit attributes.

Prerequisites
  To run this project, ensure you have the following installed:

 - .NET SDK (version 5.0 or later)
 - Selenium WebDriver
 - NUnit for test framework
 - ChromeDriver for Chrome browser automation

Running the Tests
  1) Run Tests Locally: Run all tests using the following command: dotnet test
  2) Run Tests Execution: dotnet test
  3) Log location store in ..\ui_test\bin\Debug\net6.0\result_ui_test_log
