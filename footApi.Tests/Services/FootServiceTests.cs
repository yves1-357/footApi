using footApi.Services;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace footApi.Tests.Services;

[TestClass]
[TestSubject(typeof(FootService))]
public class FootServiceTests
{

    [TestMethod]
    public void ConvertToBelgiumTime_WithNSStatus_ReturnsHourInBelgium()
    {
        var footService = new FootService(null!);

        var status = new Status { Short = "NS" };

        string utcTime = "2025-02-25T12:00:00+00:00";

        var result = footService.ConvertToBelgiumTime(utcTime, status);
        Assert.AreEqual("13:00", result);
    }

}
