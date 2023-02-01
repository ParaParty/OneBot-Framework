using System.Collections;
using System.Linq;
using OneBot.Core.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OneBot.UnitTest.Util;

[TestClass]
public class StringUtilsTest
{
    /// <summary>
    /// 摆烂，干脆先把常用的单词列出来吧
    /// </summary>
    public readonly static string[] lockedWords = { "QQ", "ID" };
    [TestMethod]
    public void ToSnakeCaseTest()
    {
        Assert.AreEqual("qq", StringUtils.ToSnakeCase("QQ", StringUtils.CaseType.Lower, lockedWords));
        Assert.AreEqual("id", StringUtils.ToSnakeCase("ID", StringUtils.CaseType.Lower, lockedWords));
        Assert.AreEqual("qq id", StringUtils.ToSnakeCase("QQ ID", StringUtils.CaseType.Lower, lockedWords));
        Assert.AreEqual("qq_qq_id qq_id_id_qq", StringUtils.ToSnakeCase("QQQQID QQIDIDQQ", StringUtils.CaseType.Lower, lockedWords));
        Assert.AreEqual("user_qq_token", StringUtils.ToSnakeCase("UserQQToken", StringUtils.CaseType.Lower, lockedWords));
        Assert.AreEqual("sweet_icelolly_id", StringUtils.ToSnakeCase("SweetIcelollyID", StringUtils.CaseType.Lower, lockedWords));
        Assert.AreEqual("wey_sun+ice_eric_vancheng_dresses", StringUtils.ToSnakeCase("WeySun+IceEricVanChengDresses", StringUtils.CaseType.Lower, new string[] { "SUN+ICE", "VANCHENG" }));

        Assert.AreEqual("QQ", StringUtils.ToSnakeCase("QQ", StringUtils.CaseType.Upper, lockedWords));
        Assert.AreEqual("ID", StringUtils.ToSnakeCase("ID", StringUtils.CaseType.Upper, lockedWords));
        Assert.AreEqual("QQ ID", StringUtils.ToSnakeCase("QQ ID", StringUtils.CaseType.Upper, lockedWords));
        Assert.AreEqual("QQ_QQ_ID QQ_ID_ID_QQ", StringUtils.ToSnakeCase("QQQQID QQIDIDQQ", StringUtils.CaseType.Upper, lockedWords));
        Assert.AreEqual("USER_QQ_TOKEN", StringUtils.ToSnakeCase("UserQQToken", StringUtils.CaseType.Upper, lockedWords));
        Assert.AreEqual("SWEET_ICELOLLY_ID", StringUtils.ToSnakeCase("SweetIcelollyID", StringUtils.CaseType.Upper, lockedWords));
        Assert.AreEqual("WEY_SUN+ICE_ERIC_VANCHENG_DRESSES", StringUtils.ToSnakeCase("WeySun+IceEricVanChengDresses", StringUtils.CaseType.Upper, new string[] { "SUN+ICE", "VANCHENG" }));
    }
    [TestMethod]
    public void ToLowerCamelTest()
    {
        Assert.AreEqual("userQQ", StringUtils.SnakeToCamelCase("user_qq", StringUtils.CamelCaseType.Lower, lockedWords));
        Assert.AreEqual("userID", StringUtils.SnakeToCamelCase("user_id", StringUtils.CamelCaseType.Lower, lockedWords));
        Assert.AreEqual("qqID", StringUtils.SnakeToCamelCase("qq_id", StringUtils.CamelCaseType.Lower, lockedWords));
        Assert.AreEqual("icelolly", StringUtils.SnakeToCamelCase("icelolly", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelolly", StringUtils.SnakeToCamelCase("Icelolly", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelolly", StringUtils.SnakeToCamelCase("IceLOlly", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress", StringUtils.SnakeToCamelCase("icelolly_dress", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress2", StringUtils.SnakeToCamelCase("icelolly_dress2", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress", StringUtils.SnakeToCamelCase("ICelolly_DRess", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress daiSuki", StringUtils.SnakeToCamelCase("iCelolly_dRess dai_suki", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress daiSuki404", StringUtils.SnakeToCamelCase("iCelolly_dRess dai_suki_404", StringUtils.CamelCaseType.Lower));
    }
    [TestMethod]
    public void ToUpperCamelTest()
    {
        Assert.AreEqual("UserQQ", StringUtils.SnakeToCamelCase("user_qq", StringUtils.CamelCaseType.Upper, lockedWords));
        Assert.AreEqual("UserID", StringUtils.SnakeToCamelCase("user_id", StringUtils.CamelCaseType.Upper, lockedWords));
        Assert.AreEqual("QQID", StringUtils.SnakeToCamelCase("qq_id", StringUtils.CamelCaseType.Upper, lockedWords));
        Assert.AreEqual("Icelolly", StringUtils.SnakeToCamelCase("icelolly", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("Icelolly", StringUtils.SnakeToCamelCase("Icelolly", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("Icelolly", StringUtils.SnakeToCamelCase("IceLOlly", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress", StringUtils.SnakeToCamelCase("icelolly_dress", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress2", StringUtils.SnakeToCamelCase("icelolly_dress2", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress", StringUtils.SnakeToCamelCase("ICelolly_DRess", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress DaiSuki", StringUtils.SnakeToCamelCase("iCelolly_dRess dai_suki", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress DaiSuki404", StringUtils.SnakeToCamelCase("iCelolly_dRess dai_suki_404", StringUtils.CamelCaseType.Upper));
    }
}

