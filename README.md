# Calculator

## Aim of the project

CalculatorApp is a calculator that evaluates arithmetic expressions entered as text, for example:

`(3+4)*-2/7`

The expression parser is implemented manually and does not use third-party evaluators or `DataTable.Compute`.

It supports:

* Basic arithmetic operators: `+`, `-`, `*`, `/`
* Standard operator precedence (`*` and `/` before `+` and `-`)
* Nested parentheses
* Negative numbers and multiple signs, for example `3--2 = 5` and `9/-3 = -3`
* Decimal numbers using `.` as the decimal separator
* Error handling for:

  * invalid characters
  * unbalanced parentheses
  * invalid operator combinations such as `**` or `+/`
  * division by zero

The calculation engine is shared by two applications:

* a **console application**
* an **ASP.NET Core MVC web application** with an on-screen keypad

The project also contains **MSTest unit tests** for the calculation engine.

## Technologies used

| Area      | Technology                                  |
| --------- | ------------------------------------------- |
| Language  | C#                                          |
| Runtime   | .NET 7 (`CalculatorService` targets .NET 6) |
| Web       | ASP.NET Core MVC, Razor Views               |
| Front end | Bootstrap, jQuery                           |
| Testing   | MSTest, Microsoft.NET.Test.Sdk, Coverlet    |
| IDE       | Visual Studio 2022                          |
| Solution  | `CalculatorApp.sln`                         |

> The `Math-Expression-Evaluator` NuGet package is referenced by the project, but it is not used by the calculation logic.

## Solution structure

```text
Calculator/
├── CalculatorApp.sln
│
├── CalculatorService/
│   ├── CalculationsService.cs
│   └── Exceptions/
│       ├── ExecuteException.cs
│       └── WithoutParanthesisException.cs
│
├── CalculatorApp/
│   └── Program.cs
│
├── WebApplication/
│   ├── Program.cs
│   ├── Controllers/
│   │   └── HomeController.cs
│   ├── Services/
│   │   └── CalculatorServicesMVC.cs
│   ├── Models/
│   │   └── ErrorViewModel.cs
│   ├── Views/
│   │   ├── Home/
│   │   │   └── Index.cshtml
│   │   └── Shared/
│   │       └── _Layout.cshtml
│   ├── wwwroot/
│   ├── appsettings.json
│   └── Properties/
│       └── launchSettings.json
│
└── TestProject/
    └── CalculationServiceTest.cs
```

## Projects

### CalculatorService

`CalculatorService` contains the main calculation logic.

The `CalculationsService` class is responsible for parsing and evaluating arithmetic expressions, including:

* validating characters
* handling operators
* calculating expressions
* handling parentheses
* processing negative numbers
* detecting invalid expressions
* handling division by zero

### CalculatorApp

`CalculatorApp` is the console version of the calculator.

The application asks the user to enter an arithmetic expression, evaluates it using `CalculationsService.Execute`, and prints either the result or an error message.

`InvariantCulture` is used so that `.` is treated as the decimal separator.

### CalculatorWebApp

`WebApplication1` contains the ASP.NET Core MVC version of the calculator.

The web interface provides an on-screen keypad with:

* digits `0-9`
* arithmetic operators `+`, `-`, `*`, `/`
* parentheses `(` and `)`
* decimal separator `.`
* `C` to remove the last character
* `AC` to clear the expression
* `=` to calculate the result

`HomeController` handles the calculator actions and sends expressions to the shared `CalculationsService`.

`CalculatorServicesMVC` performs additional input validation while the user is entering an expression.

For example, it prevents:

* invalid leading operators
* invalid consecutive operators
* multiple decimal separators in the same number
* invalid closing parentheses

### CalculatorTest

`TestProject` contains MSTest unit tests for `CalculationsService`.

The tests cover:

* character validation
* number detection
* invalid operator combinations
* addition, subtraction, multiplication and division
* negative numbers and sign handling
* division by zero
* parentheses
* invalid or missing parentheses
* complete expression calculation

## Running the project

### Visual Studio

1. Open `CalculatorApp.sln` in Visual Studio 2022.
2. Choose the project you want to run:

   * `CalculatorApp` for the console version
   * `CalculatorWebApp` for the web version
3. Right-click the project and select **Set as Startup Project**.
4. Run the application.

## Running tests

1. Open **Test Explorer** in Visual Studio.
2. Select **Run All**.

The tests from `CalculatorTest` will execute automatically.
