using FluentAssertions;
using myEnergiConnect.Model.Enums;
using MyEnergiConnect.Model.External.Shared;

namespace MyEnergiConnect.Test;

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
        IMyEnergiClient client = new MyEnergiClient(_serialNumber, _apiKey);

        var zappiSummary = await client.GetZappiSummaries();

        zappiSummary.Should().HaveCountGreaterThan(0);
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanConnectToEddi()
    {
        IMyEnergiClient client = new MyEnergiClient(_serialNumber, _apiKey);

        var eddiSummaries = await client.GetEddiSummaries();

        eddiSummaries.Should().HaveCountGreaterThan(0);
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanGetZappiHistory()
    {
        IMyEnergiClient client = new MyEnergiClient(_serialNumber, _apiKey);
        var zappiSummary = await client.GetZappiSummaries();
        var serialNumber = zappiSummary.First().SerialNumber;

        var now = DateTime.Now;

        var history = await client.GetZappiHistory(serialNumber, now.Year,now.Month, now.Day, PowerUnits.KiloWatt);

        history.MinuteValue.Should().HaveCountGreaterThan(0);
    }

    [Test]
    [Explicit("Manual Connection test")]
    public async Task CanGetEddiHistory()
    {
        IMyEnergiClient client = new MyEnergiClient(_serialNumber, _apiKey);
        var eddiHistory = await client.GetEddiSummaries();
        var serialNumber = eddiHistory.First().SerialNumber;

        var now = DateTime.Now;

        var history = await client.GetEddiHistory(serialNumber, now.Year,now.Month, now.Day, PowerUnits.KiloWatt);

        history.MinuteValue.Should().HaveCountGreaterThan(0);
    }
}