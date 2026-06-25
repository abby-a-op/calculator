namespace Calculator.UnitTesting;

[TestClass]
public class IsEncryptionHandledCorrectly
{
    [TestMethod]
    public void IsCaesarEncodedCorrectly()
    {
        string expected = "Khoor, brx!";
        Text actual = Encryption.CaesarEn(new Text("Hello, you!"));
    
        Assert.AreEqual(expected, actual.Value);
    }

    [TestMethod]
    public void IsCaesarDecodedCorrectly()
    {
        string expected = "Hello, you!";
        Text actual = Encryption.CaesarDe(new Text("Khoor, brx!"));
    
        Assert.AreEqual(expected, actual.Value);
    }

    [TestMethod]
    public void IsAffineEncodedCorrectly()
    {
        Text actual = Encryption.AffineEn(5, 6, new Text("Hi there"));

        string expected = "Pu xpana";

        Assert.AreEqual(expected, actual.Value);
    }

    [TestMethod]
    public void IsAffineDecodedCorrectly()
    {
        Text actual = Encryption.AffineDe(5, 6, new Text("Pu xpana"));

        string expected = "Hi there";

        Assert.AreEqual(expected, actual.Value);
    }

    [TestMethod]
    public void IsAffineValidKeyCheckHandledCorrectly()
    {
        Assert.Throws<ArgumentException>(() => Encryption.AffineEn(4, 2, new Text("unimportant")));
    }
}