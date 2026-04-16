Feature: Medical Device Asset Inventory Management

  As a hospital biomedical engineer
  I want to track medical device assets
  So that inventory is accurate and patient safety is maintained

  Background:
    Given the inventory system is initialized with no assets

  # REQ-001: System shall allow adding valid medical device assets
  @Requirement:REQ-001 @SmokeTest @HappyPath
  Scenario: Add a new valid medical asset
    When I add an asset with serial "PM-12345", model "Philips IntelliVue Monitor", status "Available", location "ICU-3"
    Then the asset should be successfully added to the inventory
    And the total number of assets should be 1

  # REQ-002: System shall reject assets with empty serial numbers
  @Requirement:REQ-002 @NegativeTest @Validation
  Scenario: Prevent adding asset with empty serial number
    When I attempt to add an asset with serial "", model "Philips Pump", status "Available", location "OR-1"
    Then I should receive a validation error "Serial number cannot be empty"

  # REQ-003: System shall allow querying assets by serial number
  @Requirement:REQ-003 @HappyPath
  Scenario: Query an existing asset by serial number
    Given an asset with serial "PM-12345" exists
    When I query the asset by serial "PM-12345"
    Then the returned asset should have model "Philips IntelliVue Monitor" and status "Available"

  # REQ-004: System shall prevent duplicate serial numbers
  @Requirement:REQ-004 @NegativeTest @Validation
  Scenario: Prevent duplicate serial numbers
    Given an asset with serial "PM-12345" exists
    When I attempt to add an asset with serial "PM-12345", model "Philips Pump", status "Available", location "OR-1"
    Then I should receive a validation error "Asset with this serial number already exists"

  # REQ-005: System shall allow updating asset status
  @Requirement:REQ-005 @HappyPath
  Scenario: Update asset status
    Given an asset with serial "PM-12345" exists
    When I update the status of asset "PM-12345" to "In Maintenance"
    Then the asset with serial "PM-12345" should have status "In Maintenance"

  # REQ-006: System shall allow removing assets from inventory
  @Requirement:REQ-006 @HappyPath
  Scenario: Remove an asset from inventory
    Given an asset with serial "PM-12345" exists
    When I remove the asset with serial "PM-12345"
    Then the total number of assets should be 0

  # REQ-007: System shall reject assets with invalid status values
  @Requirement:REQ-007 @NegativeTest @Validation @BoundaryTest
  Scenario Outline: Reject assets with invalid status
    When I attempt to add an asset with serial "PM-99999", model "Philips Monitor", status "<Status>", location "ICU-1"
    Then I should receive a validation error "Invalid status"

    Examples:
      | Status     |
      | Broken     |
      | Retired    |
      | Unknown    |

  # REQ-008: System shall reject assets with empty model name
  @Requirement:REQ-008 @NegativeTest @Validation
  Scenario: Prevent adding asset with empty model name
    When I attempt to add an asset with serial "PM-99998", model "", status "Available", location "ICU-1"
    Then I should receive a validation error "Model cannot be empty"

  # REQ-009: System shall accept all valid status values
  @Requirement:REQ-009 @HappyPath @BoundaryTest
  Scenario Outline: Accept assets with valid status values
    When I add an asset with serial "<Serial>", model "Philips Monitor", status "<Status>", location "ICU-1"
    Then the asset should be successfully added to the inventory

    Examples:
      | Serial    | Status         |
      | PM-00001  | Available      |
      | PM-00002  | In Maintenance |
      | PM-00003  | Unavailable    |

  # REQ-010: System shall default location to Unknown when not provided
  @Requirement:REQ-010 @EdgeCase
  Scenario: Asset location defaults to Unknown when empty
    When I add an asset with serial "PM-88888", model "Philips Monitor", status "Available", location ""
    Then the asset should be successfully added to the inventory
