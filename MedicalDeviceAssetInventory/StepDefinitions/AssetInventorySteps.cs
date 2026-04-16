using Reqnroll;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MedicalDeviceAssetInventory.StepDefinitions;

/// <summary>
/// Step definitions for Medical Device Asset Inventory BDD scenarios.
/// Implements IEC 62304-aligned test structure for Class C medical device software.
/// Each step uses descriptive error messages to support root cause analysis.
/// </summary>
[Binding]
public class AssetInventorySteps
{
    private readonly InventoryService _inventory = new();
    private readonly ScenarioContext _scenarioContext;
    private string? _lastError;
    private Asset? _queriedAsset;

    public AssetInventorySteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    // ─── Given Steps ────────────────────────────────────────────────────────────

    [Given("the inventory system is initialized with no assets")]
    public void GivenInventoryIsInitializedWithNoAssets()
    {
        _inventory.Clear();
        Console.WriteLine("[SETUP] Inventory cleared — starting with clean state");
    }

    [Given("an asset with serial {string} exists")]
    public void GivenAssetExists(string serial)
    {
        try
        {
            _inventory.AddAsset(new Asset(serial, "Philips IntelliVue Monitor", "Available", "ICU-3"));
            _scenarioContext.Set(serial, "existingAssetSerial");
            Console.WriteLine($"[SETUP] Pre-condition asset created: serial={serial}");
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"[SETUP FAILED] Could not create pre-condition asset with serial '{serial}': {ex.Message}", ex);
        }
    }

    // ─── When Steps ─────────────────────────────────────────────────────────────

    [When(@"I add an asset with serial ""(.*)"", model ""(.*)"", status ""(.*)"", location ""(.*)""")]
    public void WhenIAddAnAsset(string serial, string model, string status, string location)
    {
        _lastError = null;
        try
        {
            _inventory.AddAsset(new Asset(serial, model, status, location));
            _scenarioContext.Set(serial, "lastAddedSerial");
            Console.WriteLine($"[ACTION] Asset added: serial={serial}, model={model}, status={status}");
        }
        catch (Exception ex)
        {
            _lastError = ex.Message;
            Console.WriteLine($"[ACTION] Asset add rejected: serial='{serial}', reason='{ex.Message}'");
        }
    }

    [When(@"I attempt to add an asset with serial ""(.*)"", model ""(.*)"", status ""(.*)"", location ""(.*)""")]
    public void WhenIAttemptToAddAnAsset(string serial, string model, string status, string location)
        => WhenIAddAnAsset(serial, model, status, location);

    [When("I query the asset by serial {string}")]
    public void WhenQueryBySerial(string serial)
    {
        try
        {
            _queriedAsset = _inventory.GetAsset(serial);
            Console.WriteLine($"[ACTION] Queried asset by serial '{serial}': {(_queriedAsset != null ? "found" : "not found")}");
        }
        catch (Exception ex)
        {
            throw new Exception($"[QUERY FAILED] Error querying asset with serial '{serial}': {ex.Message}", ex);
        }
    }

    [When("I update the status of asset {string} to {string}")]
    public void WhenIUpdateAssetStatus(string serial, string newStatus)
    {
        _lastError = null;
        try
        {
            _inventory.UpdateStatus(serial, newStatus);
            Console.WriteLine($"[ACTION] Asset status updated: serial={serial}, newStatus={newStatus}");
        }
        catch (Exception ex)
        {
            _lastError = ex.Message;
            Console.WriteLine($"[ACTION] Status update rejected: serial='{serial}', reason='{ex.Message}'");
        }
    }

    [When("I remove the asset with serial {string}")]
    public void WhenIRemoveAsset(string serial)
    {
        _lastError = null;
        try
        {
            _inventory.RemoveAsset(serial);
            Console.WriteLine($"[ACTION] Asset removed: serial={serial}");
        }
        catch (Exception ex)
        {
            _lastError = ex.Message;
            Console.WriteLine($"[ACTION] Asset removal rejected: serial='{serial}', reason='{ex.Message}'");
        }
    }

    // ─── Then Steps ─────────────────────────────────────────────────────────────

    [Then("the asset should be successfully added to the inventory")]
    public void ThenAssetShouldBeAdded()
        => Assert.That(_lastError, Is.Null,
            $"Expected asset to be added successfully, but got error: '{_lastError}'");

    [Then("the total number of assets should be {int}")]
    public void ThenTotalAssetCount(int expected)
        => Assert.That(_inventory.GetAllAssets().Count, Is.EqualTo(expected),
            $"Expected {expected} asset(s) in inventory, but found {_inventory.GetAllAssets().Count}");

    [Then(@"I should receive a validation error ""(.*)""")]
    public void ThenValidationError(string expected)
        => Assert.That(_lastError, Is.EqualTo(expected),
            $"Expected validation error '{expected}', but got: '{_lastError ?? "no error"}'");

    [Then(@"the returned asset should have model ""(.*)"" and status ""(.*)""")]
    public void ThenReturnedAsset(string model, string status)
    {
        Assert.That(_queriedAsset, Is.Not.Null,
            "Expected to find an asset but query returned null");
        Assert.That(_queriedAsset!.Model, Is.EqualTo(model),
            $"Expected model '{model}', but got '{_queriedAsset.Model}'");
        Assert.That(_queriedAsset.Status, Is.EqualTo(status),
            $"Expected status '{status}', but got '{_queriedAsset.Status}'");
    }

    [Then("the asset with serial {string} should have status {string}")]
    public void ThenAssetShouldHaveStatus(string serial, string expectedStatus)
    {
        var asset = _inventory.GetAsset(serial);
        Assert.That(asset, Is.Not.Null,
            $"Expected to find asset with serial '{serial}', but it was not found");
        Assert.That(asset!.Status, Is.EqualTo(expectedStatus),
            $"Expected status '{expectedStatus}' for asset '{serial}', but got '{asset.Status}'");
    }
}