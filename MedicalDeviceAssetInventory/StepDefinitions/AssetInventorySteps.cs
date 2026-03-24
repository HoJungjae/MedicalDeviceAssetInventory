using Reqnroll;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MedicalDeviceAssetInventory.StepDefinitions;

[Binding]
public class AssetInventorySteps
{
    private readonly InventoryService _inventory = new();
    private string? _lastError;
    private Asset? _queriedAsset;

    [Given("the inventory system is initialized with no assets")]
    public void GivenInventoryIsInitializedWithNoAssets() => _inventory.Clear();

    [When(@"I add an asset with serial ""(.*)"", model ""(.*)"", status ""(.*)"", location ""(.*)""")]
    public void WhenIAddAnAsset(string serial, string model, string status, string location)
    {
        _lastError = null;
        try { _inventory.AddAsset(new Asset(serial, model, status, location)); }
        catch (Exception ex) { _lastError = ex.Message; }
    }

    [When(@"I attempt to add an asset with serial ""(.*)"", model ""(.*)"", status ""(.*)"", location ""(.*)""")]
    public void WhenIAttemptToAddAnAsset(string serial, string model, string status, string location)
        => WhenIAddAnAsset(serial, model, status, location);

    [Then("the asset should be successfully added to the inventory")]
    public void ThenAssetShouldBeAdded() => Assert.That(_lastError, Is.Null);

    [Then("the total number of assets should be {int}")]
    public void ThenTotalAssetCount(int expected)
        => Assert.That(_inventory.GetAllAssets().Count, Is.EqualTo(expected));

    [Then(@"I should receive a validation error ""(.*)""")]
    public void ThenValidationError(string expected)
        => Assert.That(_lastError, Is.EqualTo(expected));

    [Given("an asset with serial {string} exists")]
    public void GivenAssetExists(string serial)
        => WhenIAddAnAsset(serial, "Philips IntelliVue Monitor", "Available", "ICU-3");

    [When("I query the asset by serial {string}")]
    public void WhenQueryBySerial(string serial)
        => _queriedAsset = _inventory.GetAsset(serial);

    [Then(@"the returned asset should have model ""(.*)"" and status ""(.*)""")]
    public void ThenReturnedAsset(string model, string status)
    {
        Assert.That(_queriedAsset, Is.Not.Null);
        Assert.That(_queriedAsset!.Model, Is.EqualTo(model));
        Assert.That(_queriedAsset.Status, Is.EqualTo(status));
    }

    [When("I update the status of asset {string} to {string}")]
    public void WhenIUpdateAssetStatus(string serial, string newStatus)
    {
        _lastError = null;
        try { _inventory.UpdateStatus(serial, newStatus); }
        catch (Exception ex) { _lastError = ex.Message; }
    }

    [Then("the asset with serial {string} should have status {string}")]
    public void ThenAssetShouldHaveStatus(string serial, string expectedStatus)
    {
        var asset = _inventory.GetAsset(serial);
        Assert.That(asset, Is.Not.Null);
        Assert.That(asset!.Status, Is.EqualTo(expectedStatus));
    }

    [When("I remove the asset with serial {string}")]
    public void WhenIRemoveAsset(string serial)
    {
        _lastError = null;
        try { _inventory.RemoveAsset(serial); }
        catch (Exception ex) { _lastError = ex.Message; }
    }
}