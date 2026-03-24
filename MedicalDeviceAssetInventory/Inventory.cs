namespace MedicalDeviceAssetInventory;

public class InventoryService
{
    private readonly Dictionary<string, Asset> _assets = new();

    public void Clear() => _assets.Clear();

    public void AddAsset(Asset asset)
    {
        if (_assets.ContainsKey(asset.SerialNumber))
            throw new InvalidOperationException("Asset with this serial number already exists");

        _assets[asset.SerialNumber] = asset;
    }

    public Asset? GetAsset(string serialNumber)
    {
        _assets.TryGetValue(serialNumber, out var asset);
        return asset;
    }

    public List<Asset> GetAllAssets() => _assets.Values.ToList();

    public void UpdateStatus(string serialNumber, string newStatus)
    {
        if (!_assets.ContainsKey(serialNumber))
            throw new InvalidOperationException("Asset not found");

        var existing = _assets[serialNumber];
        _assets[serialNumber] = new Asset(serialNumber, existing.Model, newStatus, existing.Location);
    }

    public void RemoveAsset(string serialNumber)
    {
        if (!_assets.ContainsKey(serialNumber))
            throw new InvalidOperationException("Asset not found");

        _assets.Remove(serialNumber);
    }
}