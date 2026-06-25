namespace Calculator.UnitTesting;

[TestClass]
public class IsEncryptionHandledCorrectly
{
    // "Hello, you!" was chosen as the test plaintext as it contains a letter that loops back around (y->b) and punctuation, two possible pain spots
    [TestMethod]
    public void IsCaesarEncodedCorrectly()
    {
        string expected = "Khoor, brx!";
        Text actual = Encryption.CaesarEn(new Text("Hello, you!"));
    
        Assert.AreEqual(expected, actual.Value);
    }

    // Same reasoning for encoding, but to test decoding instead
    [TestMethod]
    public void IsCaesarDecodedCorrectly()
    {
        string expected = "Hello, you!";
        Text actual = Encryption.CaesarDe(new Text("Khoor, brx!"));
    
        Assert.AreEqual(expected, actual.Value);
    }

    // As effectively all affine strings have some looping over, this case was chosen at random just to double check that the encoding works
    [TestMethod]
    public void IsAffineEncodedCorrectly()
    {
        Text actual = Encryption.AffineEn(5, 6, new Text("Hi, there"));

        string expected = "Pu, xpana";

        Assert.AreEqual(expected, actual.Value);
    }

    // Same reasoning as encoding
    [TestMethod]
    public void IsAffineDecodedCorrectly()
    {
        Text actual = Encryption.AffineDe(5, 6, new Text("Pu xpana"));

        string expected = "Hi there";

        Assert.AreEqual(expected, actual.Value);
    }

    // Checks to see if an error is thrown when the key has no inverse
    // Important as you do not want to allow strings that cannot be decoded
    [TestMethod]
    public void IsAffineValidKeyCheckHandledCorrectly()
    {
        Assert.Throws<ArgumentException>(() => Encryption.AffineEn(4, 2, new Text("unimportant")));
    }
}