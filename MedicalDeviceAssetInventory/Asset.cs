namespace MedicalDeviceAssetInventory;

public class Asset
{
    public string SerialNumber { get; }
    public string Model { get; }
    public string Status { get; }
    public string Location { get; }

    public Asset(string serialNumber, string model, string status, string location)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException("Serial number cannot be empty");

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model cannot be empty");

        if (status != "Available" && status != "In Maintenance" && status != "Unavailable")
            throw new ArgumentException("Invalid status");

        SerialNumber = serialNumber.Trim();
        Model = model.Trim();
        Status = status;
        Location = string.IsNullOrWhiteSpace(location) ? "Unknown" : location.Trim();
    }
}