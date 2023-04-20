using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
					
namespace main{
    public static class Program1
    {
        public static void println(String s) => Console.WriteLine(s);
        
        static List<List<int>> create_rings(int depth) {
            // create a list of rings
            var rings = new List<List<int>>();
            // create depth amount of rings
            for (int i = 0; i < depth; i++)
            {
                rings.Add(new List<int>());
            }
            return rings;
        }

        static int depth_of_matrix(List<List<int>> matrix) {
            // make shure matrix is not empty
            if (matrix.Count == 0) return 0;
            return matrix.Count< matrix[0].Count ? (matrix.Count+1) / 2 : (matrix[0].Count + 1) / 2;
        }

        static List<List<int>> rings_from_matrix_first(List<List<int>> matrix , List<List<int>> rings) {
            // disassamble it row by row !first half
            for (int i = 0; i < (matrix.Count+1) / 2; i++)
            {
                int indent = i < rings.Count-1 ? i : rings.Count - 1;
                // add first numbers
                for (int j = 0; j < indent; j++)
                {
                    rings[j].Insert(0, matrix[i][j]);
                }
                // add middle section
                for (int j = indent; j < matrix[0].Count - indent ; j++)
                {
                    rings[indent].Add(matrix[i][j]);
                }
                // add last numbers
                for (int j = 0; j < indent; j++)
                {
                    rings[j].Add(matrix[i][matrix[0].Count - j - 1]);
                }
            }
            return rings;
        }

        private static List<List<int>> rings_from_matrix_second(List<List<int>> matrix, List<List<int>> rings)
        {
            throw new NotImplementedException();
        }
        public static void matrixRotation(List<List<int>> matrix, int r)
        {
            if (matrix.Count == 0) return;
            // create rings depth from matrix
            var depth = depth_of_matrix(matrix);
            // create a list of rings
            var rings = create_rings(depth);

            rings = rings_from_matrix_first(matrix, rings);

            rings = rings_from_matrix_second(matrix, rings);
            for (int i = matrix.Count / 2; i < matrix.Count; i++)
            {
                int indent = i < rings.Count ? i : rings.Count - 1;
                // add first numbers
                for (int j = 0; j < indent; j++)
                {
                    rings[j].Insert(0, matrix[i][j]);
                }
                // add middle section
                for (int j = indent; j < matrix.Count - indent - 1; j++)
                {
                    rings[indent].Add(matrix[i][j]);
                }
                // add last numbers
                for (int j = 0; j < indent; j++)
                {
                    rings[rings.Count - j -1].Add(matrix[i][matrix.Count - j - 1]);
                }
            }

            Console.WriteLine("Rings");
            printMatrix(rings);
            Console.WriteLine();
            // rotate each ring by r % ring.Count
            for (int i = 0; i < rings.Count; i++)
            {
                // rotate ring by r % ring.Count
                // get first r elements and move them to the end
                var first = rings[i].GetRange(0, r % rings[i].Count);
                // remove first r elements
                rings[i].RemoveRange(0, r % rings[i].Count);
                // add first r elements to the end
                rings[i].AddRange(first);
            }
            // reassemble matrix row by row
            for (int i = 0; i < matrix.Count / 2; i++)
            {
                // add first row
                for (int j = i; j < matrix[i].Count - i; j++)
                {
                    matrix[i][j] = rings[i][j];
                    rings[i].RemoveAt(j);
                }
                // add last column
                for (int j = i + 1; j < matrix.Count - i; j++)
                {
                    matrix[j][matrix.Count - i - 1] = rings[i][j];
                }
                // add last row in reverse
                for (int j = matrix.Count - i - 2; j >= i; j--)
                {
                    matrix[matrix.Count - i - 1][j] = rings[i][0];
                }
                // add first column in reverse
                for (int j = matrix.Count - i - 2; j > i; j--)
                {
                    matrix[j][i] = rings[i][0];
                }
            }
        }

        public static void printMatrix(List<List<int>> matrix) =>
            matrix.ForEach(row => Console.WriteLine($"[{string.Join(", ", row)}]"));
        
        public static String matrixToString(List<List<int>> matrix) {
            String s = "";
            matrix.ForEach(row => s += $"[{string.Join(", ", row)}]\n");
            return s;
        }
        public static void Main()
        {
            var matrix = new List<List<int>> {
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
                new List<int> { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 },
            };
            printMatrix(matrix);
            matrixRotation(matrix, 0);
            Console.WriteLine("Rotated matrix");
            printMatrix(matrix);
        }
    }



    // tests strart here ___________________________________________________________________________
    [TestClass]
    public class UnitTest1
    {
        static List<List<int>> wide_matrix() {
            return new List<List<int>>{
                new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9, 10},
                new List<int>{11, 12, 13, 14, 15, 16, 17, 18, 19, 20},
                new List<int>{21, 22, 23, 24, 25, 26, 27, 28, 29, 30}
            };
        }
        static List<List<int>> long_matrix() {
            return new List<List<int>>{
                new List<int>{1,2,3},
                new List<int>{4,5,6},
                new List<int>{7,8,9},
                new List<int>{10,11,12},
                new List<int>{13,14,15},
                new List<int>{16,17,18},
                new List<int>{19,20,21},
                new List<int>{22,23,24},
                new List<int>{25,26,27},
                new List<int>{28,29,30}
            };
        }
        static List<List<int>> big_matrix() {
            List<List<int>> matrix = new List<List<int>>();
            for (int i = 0; i <10; i++) {
                matrix.Add(new List<int>());
                for (int j = 0; j < 10 ; j++){
                    matrix[i].Add(i*10 + j);
                }
            }
            return matrix;
        }
        static List<List<int>> small_matrix() {
            List<List<int>> matrix = new List<List<int>>();
            matrix.Add(new List<int>{1,2});
            return matrix;
        }
        static List<List<int>> empty_matrix() {
            return new List<List<int>>();
        }
        
        static void AssertEq(object a , object b) {
            Assert.AreEqual(a, b, $"\nExpected: {a ?? "null" }but got: {b ?? "null" }");
        }
        static void AssertEq(List<List<int>> a , List<List<int>> b) {
            var expected = a;
            var actual = b;
            for (int i = 0; i < expected.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], actual[i], $"Sub-list at index {i} did not match. Expected: {string.Join(",", expected[i])}. Actual: {string.Join(",", actual[i])}");
            }
        }
        [TestMethod]
        public void Test_depth_of_matrix()
        {
            MethodInfo depth_of_matrix = typeof(Program1).GetMethod("depth_of_matrix", BindingFlags.NonPublic | BindingFlags.Static)!;
            var matrix = wide_matrix();
            var depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
            Assert.AreEqual(2, depth);
            matrix = long_matrix();
            depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
            Assert.AreEqual(2, depth);
            matrix = big_matrix();
            depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
            Assert.AreEqual(5, depth);
            matrix = small_matrix();
            depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
            Assert.AreEqual(1, depth);
            matrix = empty_matrix();
            depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
            Assert.AreEqual(0, depth);
        }
        [TestMethod]
        public void Test_create_rings(){
            MethodInfo create_rings = typeof(Program1).GetMethod("create_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            var matrix = wide_matrix();
            var depth = 2;
            var rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
            Assert.AreEqual(2, rings.Count);
            matrix = long_matrix();
            depth = 2;
            rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
            Assert.AreEqual(2, rings.Count);
            matrix = big_matrix();
            depth = 5;
            rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
            Assert.AreEqual(5, rings.Count);
            matrix = small_matrix();
            depth = 1;
            rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
            Assert.AreEqual(1, rings.Count);
            matrix = empty_matrix();
            depth = 0;
            rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
            Assert.AreEqual(0, rings.Count);
        }

        [TestMethod]
        public void Test_create_rings_first_half()
        {
            MethodInfo create_rings = typeof(Program1).GetMethod("create_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo depth_of_matrix = typeof(Program1).GetMethod("depth_of_matrix", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo ring_from_matrix = typeof(Program1).GetMethod("rings_from_matrix_first", BindingFlags.NonPublic | BindingFlags.Static)!;
            var matrix_to_rings_first = (List<List<int>> matrix) => {
                var depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
                var rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
                rings = (List<List<int>>)ring_from_matrix.Invoke(null, new object[] { matrix, rings })!;
                return rings;
            };
            var matrix = wide_matrix();
            var rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 11, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20 },
            new List<int>() { 12, 13, 14, 15, 16, 17, 18, 19 } }, rings);

            matrix = long_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 13, 10, 7, 4, 1, 2, 3, 6, 9, 12, 15 },
            new List<int>() { 5, 8, 11, 14 } }, rings);

            matrix = small_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 1, 2 } }, rings);

            matrix = empty_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { }, rings);

        }
        
        [TestMethod]
        public void Test_create_rings_second_half()
        {
            MethodInfo create_rings = typeof(Program1).GetMethod("create_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo depth_of_matrix = typeof(Program1).GetMethod("depth_of_matrix", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo ring_from_matrix_second = typeof(Program1).GetMethod("rings_from_matrix_second", BindingFlags.NonPublic | BindingFlags.Static)!;
            var matrix_to_rings_first = (List<List<int>> matrix) => {
                var depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
                var rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
                rings = (List<List<int>>)ring_from_matrix_second.Invoke(null, new object[] { matrix, rings })!;
                return rings;
            };
            var matrix = wide_matrix();
            var rings = matrix_to_rings_first(matrix);
            rings[0].Add(100);
            AssertEq(new List<List<int>>() { new List<int>() { 21, 100, 30, 29, 28, 27, 26, 25, 24, 23, 22 },
            new List<int>() {  } }, rings);

            matrix = long_matrix();
            rings = matrix_to_rings_first(matrix);
            rings[0].Add(100);
            AssertEq(new List<List<int>>() { new List<int>() { 28, 25, 22, 19, 16, 100, 18, 21, 24, 27, 30, 29 },
            new List<int>() { 17, 20, 23, 26 } }, rings);

            matrix = small_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() {  } }, rings);

            matrix = empty_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { }, rings);

        }
    }

}
