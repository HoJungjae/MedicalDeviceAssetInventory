Feature: Medical Device Asset Inventory Management

  As a hospital biomedical engineer
  I want to track medical device assets
  So that inventory is accurate and patient safety is maintained

  Background:
    Given the inventory system is initialized with no assets

  Scenario: Add a new valid medical asset
    When I add an asset with serial "PM-12345", model "Philips IntelliVue Monitor", status "Available", location "ICU-3"
    Then the asset should be successfully added to the inventory
    And the total number of assets should be 1

  Scenario: Prevent adding asset with empty serial number
    When I attempt to add an asset with serial "", model "Philips Pump", status "Available", location "OR-1"
    Then I should receive a validation error "Serial number cannot be empty"

  Scenario: Query an existing asset by serial number
    Given an asset with serial "PM-12345" exists
    When I query the asset by serial "PM-12345"
    Then the returned asset should have model "Philips IntelliVue Monitor" and status "Available"

  Scenario: Prevent duplicate serial numbers
    Given an asset with serial "PM-12345" exists
    When I attempt to add an asset with serial "PM-12345", model "Philips Pump", status "Available", location "OR-1"
    Then I should receive a validation error "Asset with this serial number already exists"

  Scenario: Update asset status
    Given an asset with serial "PM-12345" exists
    When I update the status of asset "PM-12345" to "In Maintenance"
    Then the asset with serial "PM-12345" should have status "In Maintenance"

  Scenario: Remove an asset from inventory
    Given an asset with serial "PM-12345" exists
    When I remove the asset with serial "PM-12345"
    Then the total number of assets should be 0

  Scenario Outline: Reject assets with invalid status
    When I attempt to add an asset with serial "PM-99999", model "Philips Monitor", status "<Status>", location "ICU-1"
    Then I should receive a validation error "Invalid status"

    Examples:
      | Status     |
      | Broken     |
      | Retired    |
      | Unknown    |