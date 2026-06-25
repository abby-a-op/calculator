namespace Calculator.UnitTesting;

[TestClass]
public class IsCheckDigitGeneratedCorrectly
{
    [TestMethod]
    // Sample UPC barcodes
    [DataRow("71234567890", "712345678904")]
    [DataRow("01234567890", "012345678905")]
    // Sample ISBNs
    [DataRow("043902348", "0439023483")]
    [DataRow("030640615", "0306406152")]
    // Sample EAN-13
    [DataRow("501234567890", "5012345678900")]
    public void IsDigitGeneratedCorrectly(string digits, string expectedString)
    {
        Text actualString = NumberTheory.NumCheckDigit(new Text(digits));

        Assert.AreEqual(expectedString, actualString.Value);
    }
}