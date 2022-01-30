using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.FrameworkDemo.Models;

namespace OneBot_UnitTest;

[TestClass]
public class DurationTest
{
    [TestMethod]
    public void ParseFromString()
    {
        Assert.IsTrue(Duration.TryParse("11d4h51m4s",out var duration));
        Assert.AreEqual(11 * 86400 + 4 * 3600 + 51 * 60 + 4, duration.Seconds);
    }

    [TestMethod]
    public void DurationToString()
    {
        Duration.TryParse("11d4h51m4s", out var duration);
        Assert.AreEqual("11d4h51m4s", duration.ToString());
        Duration.TryParse("1919d8m10s", out duration);
        Assert.AreEqual("1919d8m10s", duration.ToString());
    }
}