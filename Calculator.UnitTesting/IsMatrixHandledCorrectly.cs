using System.ComponentModel.DataAnnotations;

namespace Calculator.UnitTesting;

[TestClass]
public class IsMatrixHandledCorrectly
{

    private static readonly Matrix Identity = new Matrix(1, 0, 0, 1);

    // Asserts if two matrices are true to a small 
    bool MatrixEqual(Matrix a, Matrix b)
    {
        double delta = 0.0001;
        bool equals;

        equals = Math.Abs(a.A - b.A) < delta;
        equals = equals && Math.Abs(a.B - b.B) < delta;
        equals = equals && Math.Abs(a.C - b.C) < delta;
        equals = equals && Math.Abs(a.D - b.D) < delta;

        return equals;
    }
    
    [TestMethod]
    public void IsMatrixInverseAndMultiplicationHandledCorrectly()
    {
        Matrix m = new Matrix(3, 7, 6, 2);

        Matrix inv = m.Inv();

        Assert.IsTrue(MatrixEqual(Identity, m.Dot(inv)));
    }

    [TestMethod]
    public void IsMultiplicationByIdentityCorrect()
    {
        Matrix m = new Matrix(3, 7, 6, 2);

        Assert.IsTrue(MatrixEqual(m, m.Dot(Identity)));
    }
}