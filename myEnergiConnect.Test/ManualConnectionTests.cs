using FluentAssertions;

namespace myEnergiConnect.Test;

using Microsoft.Extensions.Configuration;

public class ManualConnectionTests
{ 
    private string? _apiKey = "";
    private string? _serialNumber = "";

    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ManualConnectionTests>()
            .Build();

        _serialNumber = configuration["serialNo"];
        _apiKey = configuration["apiKey"];
        
        if (string.IsNullOrEmpty(_serialNumber)
            || string.IsNullOrEmpty(_apiKey))
        {
            Assert.Fail("dotnet user-secrets needs to be setup with serialNo of hub and apiKey");
        }
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanConnectToZappi()
    {
        var client = new MyEnergiMyEnergiClient(_serialNumber, _apiKey);

        var zappiSummary = await client.GetZappiSummary();

        zappiSummary.Zappis.Should().HaveCountGreaterThan(0);
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanConnectToEddi()
    {
        var client = new MyEnergiMyEnergiClient(_serialNumber, _apiKey);

        var eddiSummary = await client.GetEddiSummary();

        eddiSummary.Eiddis.Should().HaveCountGreaterThan(0);
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanGetZappiHistory()
    {
        var client = new MyEnergiMyEnergiClient(_serialNumber, _apiKey);
        var zappiSummary = await client.GetZappiSummary();
        var serialNumber = zappiSummary.Zappis.First().SerialNumber;

        var now = DateTime.Now;

        var history = await client.GetZappiHistory(serialNumber, now.Year,now.Month, now.Day);

        history.Should().HaveCountGreaterThan(0);
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanGetEddiHistory()
    {
        var client = new MyEnergiMyEnergiClient(_serialNumber, _apiKey);
        var eddiHistory = await client.GetEddiSummary();
        var serialNumber = eddiHistory.Eiddis.First().SerialNumber;

        var now = DateTime.Now;

        var history = await client.GetEddiHistory(serialNumber, now.Year,now.Month, now.Day);

        history.Should().HaveCountGreaterThan(0);
    }
}