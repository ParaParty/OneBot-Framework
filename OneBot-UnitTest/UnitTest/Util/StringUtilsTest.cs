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
        Assert.AreEqual("qq", StringUtils.ToSnakeCase("QQ", StringUtils.SnakeCaseType.Lower, lockedWords));
        Assert.AreEqual("id", StringUtils.ToSnakeCase("ID", StringUtils.SnakeCaseType.Lower, lockedWords));
        Assert.AreEqual("qq id", StringUtils.ToSnakeCase("QQ ID", StringUtils.SnakeCaseType.Lower, lockedWords));
        Assert.AreEqual("qq_qq_id qq_id_id_qq", StringUtils.ToSnakeCase("QQQQID QQIDIDQQ", StringUtils.SnakeCaseType.Lower, lockedWords));
        Assert.AreEqual("user_qq_token", StringUtils.ToSnakeCase("UserQQToken", StringUtils.SnakeCaseType.Lower, lockedWords));
        Assert.AreEqual("sweet_icelolly_id", StringUtils.ToSnakeCase("SweetIcelollyID", StringUtils.SnakeCaseType.Lower, lockedWords));
        Assert.AreEqual("wey_sun+ice_eric_vancheng_dresses", StringUtils.ToSnakeCase("WeySun+IceEricVanChengDresses", StringUtils.SnakeCaseType.Lower, new string[] { "SUN+ICE", "VANCHENG" }));

        Assert.AreEqual("QQ", StringUtils.ToSnakeCase("QQ", StringUtils.SnakeCaseType.Upper, lockedWords));
        Assert.AreEqual("ID", StringUtils.ToSnakeCase("ID", StringUtils.SnakeCaseType.Upper, lockedWords));
        Assert.AreEqual("QQ ID", StringUtils.ToSnakeCase("QQ ID", StringUtils.SnakeCaseType.Upper, lockedWords));
        Assert.AreEqual("QQ_QQ_ID QQ_ID_ID_QQ", StringUtils.ToSnakeCase("QQQQID QQIDIDQQ", StringUtils.SnakeCaseType.Upper, lockedWords));
        Assert.AreEqual("USER_QQ_TOKEN", StringUtils.ToSnakeCase("UserQQToken", StringUtils.SnakeCaseType.Upper, lockedWords));
        Assert.AreEqual("SWEET_ICELOLLY_ID", StringUtils.ToSnakeCase("SweetIcelollyID", StringUtils.SnakeCaseType.Upper, lockedWords));
        Assert.AreEqual("WEY_SUN+ICE_ERIC_VANCHENG_DRESSES", StringUtils.ToSnakeCase("WeySun+IceEricVanChengDresses", StringUtils.SnakeCaseType.Upper, new string[] { "SUN+ICE", "VANCHENG" }));
    }
    [TestMethod]
    public void ToLowerCamelTest()
    {
        Assert.AreEqual("userQQ", StringUtils.ToCamelCase("user_qq", StringUtils.CamelCaseType.Lower, lockedWords));
        Assert.AreEqual("userID", StringUtils.ToCamelCase("user_id", StringUtils.CamelCaseType.Lower, lockedWords));
        Assert.AreEqual("qqID", StringUtils.ToCamelCase("qq_id", StringUtils.CamelCaseType.Lower, lockedWords));
        Assert.AreEqual("icelolly", StringUtils.ToCamelCase("icelolly", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelolly", StringUtils.ToCamelCase("Icelolly", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelolly", StringUtils.ToCamelCase("IceLOlly", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress", StringUtils.ToCamelCase("icelolly_dress", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress2", StringUtils.ToCamelCase("icelolly_dress2", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress", StringUtils.ToCamelCase("ICelolly_DRess", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress daiSuki", StringUtils.ToCamelCase("iCelolly_dRess dai_suki", StringUtils.CamelCaseType.Lower));
        Assert.AreEqual("icelollyDress daiSuki404", StringUtils.ToCamelCase("iCelolly_dRess dai_suki_404", StringUtils.CamelCaseType.Lower));
    }
    [TestMethod]
    public void ToUpperCamelTest()
    {
        Assert.AreEqual("UserQQ", StringUtils.ToCamelCase("user_qq", StringUtils.CamelCaseType.Upper, lockedWords));
        Assert.AreEqual("UserID", StringUtils.ToCamelCase("user_id", StringUtils.CamelCaseType.Upper, lockedWords));
        Assert.AreEqual("QQID", StringUtils.ToCamelCase("qq_id", StringUtils.CamelCaseType.Upper, lockedWords));
        Assert.AreEqual("Icelolly", StringUtils.ToCamelCase("icelolly", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("Icelolly", StringUtils.ToCamelCase("Icelolly", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("Icelolly", StringUtils.ToCamelCase("IceLOlly", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress", StringUtils.ToCamelCase("icelolly_dress", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress2", StringUtils.ToCamelCase("icelolly_dress2", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress", StringUtils.ToCamelCase("ICelolly_DRess", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress DaiSuki", StringUtils.ToCamelCase("iCelolly_dRess dai_suki", StringUtils.CamelCaseType.Upper));
        Assert.AreEqual("IcelollyDress DaiSuki404", StringUtils.ToCamelCase("iCelolly_dRess dai_suki_404", StringUtils.CamelCaseType.Upper));
    }
}

