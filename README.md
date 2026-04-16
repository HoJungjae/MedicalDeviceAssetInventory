![BDD Tests](https://github.com/HoJungjae/MedicalDeviceAssetInventory/actions/workflows/dotnet-tests.yml/badge.svg)

# Medical Device Asset Inventory — BDD Test Automation Framework

A Behavior Driven Development (BDD) test automation framework built with **Reqnroll**, **C#**, and **NUnit**, designed to simulate enterprise-grade testing practices in a **Class C medical device environment** (IEC 62304).

---

## Project Overview

This framework demonstrates key SDET practices used in safety-critical medical device software:

- Converting functional requirements into Gherkin scenarios
- Linking scenarios to C# step definitions via Reqnroll
- Data-driven testing using Scenario Outlines with Examples tables
- IEC 62304-aligned traceability via requirement tags
- Automated CI/CD with tiered test execution via GitHub Actions

---

## Tech Stack

| Tool | Purpose |
|------|---------|
| Reqnroll | BDD framework (open-source SpecFlow successor) |
| C# / .NET 8 | Step definition implementation |
| NUnit | Assertions and test runner |
| GitHub Actions | CI/CD pipeline |

---

## Project Structure

```
MedicalDeviceAssetInventory/
├── Features/
│   └── AssetInventory.feature      # Gherkin scenarios with requirement tags
├── StepDefinitions/
│   └── AssetInventorySteps.cs      # Step implementations with descriptive errors
├── Support/
│   └── Hooks.cs                    # BeforeScenario / AfterScenario lifecycle hooks
├── Asset.cs                        # Asset model with validation logic
├── Inventory.cs                    # InventoryService with CRUD operations
└── .github/workflows/
    └── dotnet-tests.yml            # Tiered CI/CD pipeline
```

---

## Test Coverage

| Requirement | Scenario | Type | Tag |
|-------------|----------|------|-----|
| REQ-001 | Add valid medical asset | Happy Path | @Requirement:REQ-001 |
| REQ-002 | Reject empty serial number | Negative | @Requirement:REQ-002 |
| REQ-003 | Query asset by serial | Happy Path | @Requirement:REQ-003 |
| REQ-004 | Prevent duplicate serials | Negative | @Requirement:REQ-004 |
| REQ-005 | Update asset status | Happy Path | @Requirement:REQ-005 |
| REQ-006 | Remove asset | Happy Path | @Requirement:REQ-006 |
| REQ-007 | Reject invalid status (3 cases) | Negative/Boundary | @Requirement:REQ-007 |
| REQ-008 | Reject empty model name | Negative | @Requirement:REQ-008 |
| REQ-009 | Accept all valid statuses (3 cases) | Happy Path/Boundary | @Requirement:REQ-009 |
| REQ-010 | Default location to Unknown | Edge Case | @Requirement:REQ-010 |

---

## Key Design Decisions

### 1. AfterScenario Hooks (Test Isolation)
`Support/Hooks.cs` implements `BeforeScenario` and `AfterScenario` hooks for lifecycle logging and cleanup. In a production database-backed system, `AfterScenario` would delete test data created during each scenario — ensuring tests are fully isolated and don't interfere when run in parallel.

### 2. Descriptive Error Messages
All assertions include failure messages explaining what was expected vs. what was found. This reduces debugging time and supports root cause analysis in CI/CD.

### 3. IEC 62304 Traceability Tags
Every scenario is tagged with `@Requirement:REQ-XXX`. In an Azure DevOps environment, these map to Work Item IDs, enabling automated traceability between requirements and test results for Class C regulatory audits.

### 4. Tiered CI/CD Pipeline
The GitHub Actions pipeline runs in two tiers:
- **Tier 1 (Smoke Tests):** Critical path only — fastest feedback on every commit
- **Tier 2 (Full Suite):** All scenarios — complete coverage with TRX report output

### 5. ScenarioContext for Step Data Sharing
Steps share data (like serial numbers) via `ScenarioContext` instead of class fields, ensuring data is scoped per scenario and doesn't leak between tests.

---

## Running Tests

```bash
# Run all tests
dotnet test

# Run smoke tests only (Tier 1)
dotnet test --filter "Category=SmokeTest"

# Run negative tests only
dotnet test --filter "Category=NegativeTest"

# Run with TRX output for traceability
dotnet test --logger "trx;LogFileName=test-results.trx"
```

---

## CI/CD

Tests run automatically on every push and pull request to `main`/`master` via GitHub Actions. The pipeline:
1. Builds the project
2. Runs Tier 1 smoke tests (fast feedback)
3. Runs full test suite (complete coverage)
4. Publishes TRX test results for audit trail

---

## IEC 62304 Alignment

This project simulates Class C medical device software testing practices:

- **Traceability:** Every test maps to a requirement via `@Requirement` tags
- **Test isolation:** Hooks ensure clean state before/after each scenario
- **Audit trail:** Console logging + TRX reports provide evidence of test execution
- **Risk coverage:** Negative, boundary, and edge case scenarios cover failure modes
