using System.ComponentModel.DataAnnotations;

namespace Calculator.UnitTesting;

[TestClass]
public class IsMatrixHandledCorrectly
{

    private static readonly Matrix Identity = new Matrix(1, 0, 0, 1);

    // Asserts if two matrices are true to a small margin of error (required due to floating point inpercision)
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
    
    // This case was chosen as it tests multiple operations: the determinate, inverse, and multiplication
    [TestMethod]
    public void IsMatrixInverseAndMultiplicationHandledCorrectly()
    {
        Matrix m = new Matrix(3, 7, 6, 2);

        Matrix inv = m.Inv();

        Assert.IsTrue(MatrixEqual(Identity, m.Dot(inv)));
    }
    
    // Chosen as a basic test of multiplication
    [TestMethod]
    public void IsMultiplicationByIdentityCorrect()
    {
        Matrix m = new Matrix(3, 7, 6, 2);

        Assert.IsTrue(MatrixEqual(m, m.Dot(Identity)));
    }
}