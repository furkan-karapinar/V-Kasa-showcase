using System.Globalization;
using System.Text.RegularExpressions;
using BarcodeMatching;

namespace BarcodeMatching.Tests;

public sealed class BarcodeNormalizerTests
{
    /// <summary>Optimizasyondan önceki (Regex + çoklu Replace) referans uygulama.</summary>
    private static string Legacy(string? val)
    {
        if (string.IsNullOrWhiteSpace(val)) return "";
        var s = val.Trim().ToLower(new CultureInfo("tr-TR"))
            .Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");
        s = Regex.Replace(s, @"\s+", "");
        return s.Replace("-", "").Replace("/", "");
    }

    [Theory]
    [InlineData("8690000000001", "8690000000001")]
    [InlineData("  8690000000001  ", "8690000000001")]
    [InlineData("KG-KARGO/123", "kgkargo123")]
    [InlineData("a - b / c", "abc")]
    [InlineData("ISPARTA ışık İĞNE", "ispartaisikigne")]
    [InlineData("Kadın Bisiklet Yaka Body", "kadinbisikletyakabody")]
    [InlineData("--//--", "")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void Normalize_KnownCases(string? input, string expected) =>
        Assert.Equal(expected, BarcodeNormalizer.Normalize(input));

    [Fact]
    public void Normalize_MatchesLegacy_OnRandomStrings()
    {
        var rnd = new Random(20261006);
        const string alphabet = "abcçdefgğhıijklmnoöprsştuüvyzABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ0123456789-/ _.#\t\n ";
        for (var n = 0; n < 5000; n++)
        {
            var chars = new char[rnd.Next(0, 24)];
            for (var i = 0; i < chars.Length; i++) chars[i] = alphabet[rnd.Next(alphabet.Length)];
            var s = new string(chars);
            Assert.Equal(Legacy(s), BarcodeNormalizer.Normalize(s));
        }
    }
}
