![BDD Tests](https://github.com/HoJungjae/MedicalDeviceAssetInventory/actions/workflows/dotnet-tests.yml/badge.svg)
# Medical Device Asset Inventory Management System
### BDD Test Automation with C# + Reqnroll (Gherkin)

A behavior-driven development (BDD) project simulating hospital medical device 
asset tracking, built to demonstrate test automation practices used in medical 
device software development (Philips-style Gherkin → C# automation flow).

## Tech Stack
- **C# / .NET 8** — Core domain logic
- **Reqnroll** — Gherkin BDD framework (SpecFlow successor)
- **NUnit** — Assertion framework
- **Visual Studio 2026**

## Features Tested
- Add / Remove / Query medical device assets
- Serial number validation (empty, duplicate prevention)
- Status validation (Available, In Maintenance, Unavailable)
- Data-driven testing via Scenario Outline + Examples tables

## Test Results
9 scenarios, 9 passed

## How It Mirrors Medical Device Automation Practices
- Requirements translated directly into human-readable Gherkin scenarios
- Step definitions in C# bind business language to testable code
- Validates reliability for safety-critical medical device workflows