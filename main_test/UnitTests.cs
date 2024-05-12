using System.Reflection;
using MatrixRotation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatrixRotationUnitTests;

[TestClass]
public class UnitTests
{
    static List<List<int>> WideTestMatrix()
    {
        return [
            [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
            [11, 12, 13, 14, 15, 16, 17, 18, 19, 20],
            [21, 22, 23, 24, 25, 26, 27, 28, 29, 30]
        ];
    }

    static List<List<int>> LongTestMatrix()
    {
        return [
            [1,2,3],
            [4,5,6],
            [7,8,9],
            [10,11,12],
            [13,14,15],
            [16,17,18],
            [19,20,21],
            [22,23,24],
            [25,26,27],
            [28,29,30]
        ];
    }
    
    static List<List<int>> BigTestMatrix()
    {
        List<List<int>> matrix = [];
        for (int i = 0; i <6; i++) {
            matrix.Add([]);
            for (int j = 1; j < 7 ; j++){
                matrix[i].Add(i*6 + j);
            }
        }
        return matrix;
    }
    
    static List<List<int>> SmallTestMatrix()
    {
        List<List<int>> matrix =
        [
            [1],
            [2]
        ];
        return matrix;
    }
    
    static List<List<int>> EmptyTestMatrix()
    {
        return [];
    }
    
    static void AssertEq(List<List<int>> a , List<List<int>> b)
    {
        var expected = a;
        var actual = b;
        for (int i = 0; i < expected.Count; i++)
        {
            CollectionAssert.AreEqual(expected[i], actual[i], $"\nList at index {i} did not match. Expected:\n{MatrixRotator.MatrixToString(expected)}. Actual:\n{MatrixRotator.MatrixToString(actual)}\n");
        }
    }

    static MethodInfo GetMethodInfo(string name) =>
        typeof(MatrixRotator).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!;

    [TestMethod]
    public void TestCreateRings()
    {
        MethodInfo createRings = GetMethodInfo("CreateRings");
        var depth = 2;
        var rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
        Assert.AreEqual(2, rings.Count);
        depth = 5;
        rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
        Assert.AreEqual(5, rings.Count);
        depth = 1;
        rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
        Assert.AreEqual(1, rings.Count);
        depth = 0;
        rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
        Assert.AreEqual(0, rings.Count);
    }

    [TestMethod]
    public void TestDepthOfMatrix()
    {
        MethodInfo depthOfMatrix = GetMethodInfo("DepthOfMatrix");
        var matrix = WideTestMatrix();
        var depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
        Assert.AreEqual(2, depth);
        matrix = LongTestMatrix();
        depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
        Assert.AreEqual(2, depth);
        matrix = BigTestMatrix();
        depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
        Assert.AreEqual(3, depth);
        matrix = SmallTestMatrix();
        depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
        Assert.AreEqual(1, depth);
        matrix = EmptyTestMatrix();
        depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
        Assert.AreEqual(0, depth);
    }

    [TestMethod]
    public void TestCreateRingsUpperFalf()
    {
        MethodInfo createRings = GetMethodInfo("CreateRings");
        MethodInfo depthOfMatrix = GetMethodInfo("DepthOfMatrix");
        MethodInfo ringsFromMatrixTopHalf = GetMethodInfo("RingsFromMatrixTopHalf");
        List<List<int>> matrixToRingsTopHalf(List<List<int>> matrix)
        {
            var depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
            var rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
            rings = (List<List<int>>)ringsFromMatrixTopHalf.Invoke(null, [matrix, rings])!;
            return rings;
        }
        var matrix = WideTestMatrix();
        var rings = matrixToRingsTopHalf(matrix);
        AssertEq(
            [
                [11, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20],
                [12, 13, 14, 15, 16, 17, 18, 19]
            ], rings);

        matrix = LongTestMatrix();
        rings = matrixToRingsTopHalf(matrix);
        AssertEq(
            [
                [13, 10, 7, 4, 1, 2, 3, 6, 9, 12, 15],
                [5, 8, 11, 14]
            ], rings);

        matrix = SmallTestMatrix();
        rings = matrixToRingsTopHalf(matrix);
        AssertEq([[1]], rings);

        matrix = EmptyTestMatrix();
        rings = matrixToRingsTopHalf(matrix);
        AssertEq([], rings);

    }
    
    [TestMethod]
    public void TestCreateRingsLowerHalf()
    {
        MethodInfo createRings = GetMethodInfo("CreateRings");
        MethodInfo depthOfMatrix = GetMethodInfo("DepthOfMatrix");
        MethodInfo ringsFromMatrixLowerHalf = GetMethodInfo("RingsFromMatrixLowerHalf");
        List<List<int>> matrixToRingsLowerHalf(List<List<int>> matrix)
        {
            var depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
            var rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
            rings[0].Add(100);
            rings = (List<List<int>>)ringsFromMatrixLowerHalf.Invoke(null, [matrix, rings])!;
            return rings;
        }

        var matrix = WideTestMatrix();
        var rings = matrixToRingsLowerHalf(matrix);
        AssertEq(
            [
                [100, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21],
                []
            ], rings);

        matrix = LongTestMatrix();
        rings = matrixToRingsLowerHalf(matrix);
        AssertEq(
            [
                [25, 22, 19, 16, 100, 18, 21, 24, 27, 30, 29, 28],
                [17, 20, 23, 26]
            ], rings);

        matrix = SmallTestMatrix();
        rings = matrixToRingsLowerHalf(matrix);
        AssertEq([[100, 2]], rings);

        matrix = EmptyTestMatrix();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => matrixToRingsLowerHalf(matrix));
    }

    [TestMethod]
    public void TestRotateRings()
    {
        MethodInfo createRings = GetMethodInfo("CreateRings");
        MethodInfo depthOfMatrix = GetMethodInfo("DepthOfMatrix");
        MethodInfo ringsFromMatrixTopHalf = GetMethodInfo("RingsFromMatrixTopHalf");
        MethodInfo ringsFromMatrixLowerHalf = GetMethodInfo("RingsFromMatrixLowerHalf");
        MethodInfo rotateRings = GetMethodInfo("RotateRings");
        List<List<int>> matrixRotate(List<List<int>> matrix)
        {
            var depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
            var rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
            rings = (List<List<int>>)ringsFromMatrixTopHalf.Invoke(null, [matrix, rings])!;
            rings = (List<List<int>>)ringsFromMatrixLowerHalf.Invoke(null, [matrix, rings])!;
            rings = (List<List<int>>)rotateRings.Invoke(null, [rings, 3])!;
            return rings;
        }

        var matrix = WideTestMatrix();
        var rings = matrixRotate(matrix);
        AssertEq(
            [
                [3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 11, 1, 2],
                [15, 16, 17, 18, 19, 12, 13, 14]
            ], rings);

        matrix = LongTestMatrix();
        rings = matrixRotate(matrix);
        AssertEq(
            [
                [16, 13, 10, 7, 4, 1, 2, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 29, 28, 25, 22, 19],
                [14, 17, 20, 23, 26, 5, 8, 11]
            ], rings);

        matrix = SmallTestMatrix();
        rings = matrixRotate(matrix);
        AssertEq([[2, 1]], rings);
    }

    [TestMethod]
    public void TestReasambleRings()
    {
        MethodInfo createRings = GetMethodInfo("CreateRings");
        MethodInfo depthOfMatrix = GetMethodInfo("DepthOfMatrix");
        MethodInfo ringsFromMatrixTopHalf = GetMethodInfo("RingsFromMatrixTopHalf");
        MethodInfo ringsFromMatrixLowerHalf = GetMethodInfo("RingsFromMatrixLowerHalf");
        MethodInfo rotateRings = GetMethodInfo("RotateRings");
        MethodInfo matrixFromRings = GetMethodInfo("MatrixFromRings");
        List<List<int>> RotateMatrix(List<List<int>> matrix)
        {
            var depth = (int)depthOfMatrix.Invoke(null, [matrix])!;
            var rings = (List<List<int>>)createRings.Invoke(null, [depth])!;
            rings = (List<List<int>>)ringsFromMatrixTopHalf.Invoke(null, [matrix, rings])!;
            rings = (List<List<int>>)ringsFromMatrixLowerHalf.Invoke(null, [matrix, rings])!;
            rings = (List<List<int>>)rotateRings.Invoke(null, [rings, 3])!;
            var result = (List<List<int>>)matrixFromRings.Invoke(null, [matrix, rings])!;
            return result;
        }

        var matrix = WideTestMatrix();
        var result = RotateMatrix(matrix);
        AssertEq(
            [
                [4, 5, 6, 7, 8, 9, 10, 20, 30, 29],
                [3, 15, 16, 17, 18, 19, 12, 13, 14, 28],
                [2, 1, 11, 21, 22, 23, 24, 25, 26, 27]
            ], result);

        matrix = LongTestMatrix();
        result = RotateMatrix(matrix);
        AssertEq(
            [
                [6, 9, 12],
                [3, 14, 15],
                [2, 17, 18],
                [1, 20, 21],
                [4, 23, 24],
                [7, 26, 27],
                [10, 5, 30],
                [13, 8, 29],
                [16, 11, 28],
                [19, 22, 25]
            ], result);

        matrix = SmallTestMatrix();
        result = RotateMatrix(matrix);
        AssertEq([ [2], [1] ], result);
    }

    [TestMethod]
    public void TestRotateMatrix()
    {
        var matrix = WideTestMatrix();
        MatrixRotator.MatrixRotation(matrix, 3);
        AssertEq(
            [
                [4, 5, 6, 7, 8, 9, 10, 20, 30, 29],
                [3, 15, 16, 17, 18, 19, 12, 13, 14, 28],
                [2, 1, 11, 21, 22, 23, 24, 25, 26, 27]
            ], matrix);

        matrix = LongTestMatrix();
        MatrixRotator.MatrixRotation(matrix, 3);
        AssertEq(
            [
                [6, 9, 12],
                [3, 14, 15],
                [2, 17, 18],
                [1, 20, 21],
                [4, 23, 24],
                [7, 26, 27],
                [10, 5, 30],
                [13, 8, 29],
                [16, 11, 28],
                [19, 22, 25]
            ], matrix);

        matrix = SmallTestMatrix();
        MatrixRotator.MatrixRotation(matrix, 3);
        AssertEq([ [2], [1] ], matrix);

        matrix = BigTestMatrix();
        MatrixRotator.MatrixRotation(matrix, 3);
        AssertEq(
            [
                [4, 5, 6, 12, 18, 24],
                [3, 11, 17, 23, 29, 30],
                [2, 10, 21, 15, 28, 36],
                [1, 9, 22, 16, 27, 35],
                [7, 8, 14, 20, 26, 34],
                [13, 19, 25, 31, 32, 33]
            ], matrix);
    }
}
