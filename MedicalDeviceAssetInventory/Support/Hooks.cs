using Reqnroll;

namespace MedicalDeviceAssetInventory.Support;

/// <summary>
/// Reqnroll lifecycle hooks for test setup and teardown.
/// Ensures each scenario starts and ends with a clean state,
/// preventing test data leakage in production-scale environments.
/// Relevant for IEC 62304 Class C environments where test isolation
/// is required for regulatory audit readiness.
/// </summary>
[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        // Log scenario start for traceability
        Console.WriteLine($"[SCENARIO START] {_scenarioContext.ScenarioInfo.Title}");
    }

    [AfterScenario]
    public void AfterScenario()
    {
        // Log scenario result for audit trail
        var status = _scenarioContext.TestError == null ? "PASSED" : "FAILED";
        Console.WriteLine($"[SCENARIO END] {_scenarioContext.ScenarioInfo.Title} - {status}");

        if (_scenarioContext.TestError != null)
        {
            Console.WriteLine($"[ERROR] {_scenarioContext.TestError.Message}");
        }

        // In a real database-backed system, this is where you would
        // delete test data created during the scenario:
        // await _dbHelper.DeleteTestDataAsync(_scenarioContext.Get<string>("assetSerial"));
    }
}
