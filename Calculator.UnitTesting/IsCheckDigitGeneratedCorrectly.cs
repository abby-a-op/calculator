namespace Calculator.UnitTesting;

[TestClass]
public class IsCheckDigitGeneratedCorrectly
{
    [TestMethod]
    // Sample UPC barcodes
    [DataRow("71234567890", 4)]
    [DataRow("01234567890", 5)]
    // Sample ISBNs
    [DataRow("043902348", 3)]
    [DataRow("030640615", 2)]
    // Sample EAN-13
    [DataRow("501234567890", 0)]
    public void IsDigitGeneratedCorrectly(string digits, int expectedDigit)
    {
        int actualDigit = NumberTheory.NumCheckDigit(digits);

        Assert.AreEqual(actualDigit, expectedDigit);
    }
}