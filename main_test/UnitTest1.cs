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

        static List<List<int>> rings_from_matrix_second(List<List<int>> matrix, List<List<int>> rings)
        {
            // disassamble it row by row second half
            for (int i = (matrix.Count + 1) / 2; i < matrix.Count; i++)
            {
                var from_end = matrix.Count - i - 1;
                int indent = from_end < rings.Count - 1 ? from_end : rings.Count - 1;
                //? add first numbers
                for (int j = 0; j < indent; j++)
                {
                    rings[j].Insert(0, matrix[i][j]);
                }
                //? add last numbers
                for (int j = 0; j < indent; j++)
                {
                    rings[j].Add(matrix[i][matrix[0].Count - j - 1]);
                }
                //! add middle section
                for (int j = matrix[0].Count - indent - 1; j >= indent; j--)
                {
                    rings[indent].Add(matrix[i][j]);
                }
            }
            return rings;
        }

        static List<List<int>> rotate_rings(List<List<int>> rings, int r) {
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
            return rings;
        }
        static List<List<int>> matrix_from_rings(List<List<int>> matrix, List<List<int>> rings)
        {
            var x = matrix[0].Count;
            var y = matrix.Count;
            var popFirst = (List<int> list) => {
                var first = list[0];
                list.RemoveAt(0);
                return first;
            };
            // reassemble matrix circularly
            for (int i = 0; i < rings.Count; i++)
            {
                // if the remaining matrix is a single row or column add it and break
                if (x - 2 * i == 1){
                    println("single column");
                    for (int j = i; j < y - i; j++){
                        matrix[j][i] = popFirst(rings[i]);
                    }
                    break;
                }
                if (y - 2 * i == 1){
                    println("single row");
                    for (int j = i; j < x - i; j++){
                        matrix[i][j] = popFirst(rings[i]);
                    }
                    break;
                }

                // add first column y - 2 - i -> i - 1
                for (int j = y - 2 - i; j > i; j--) {
                    matrix[j][i] = popFirst(rings[i]);
                }

                // add first row i -> x - i - 1
                for (int j = i; j <= x - i - 1; j++) {
                    matrix[i][j] = popFirst(rings[i]);
                }

                // add last column i + 1 -> y - i - 1
                for (int j = i + 1; j <= y - i - 1; j++) {
                    matrix[j][x - i - 1] = popFirst(rings[i]);
                }

                // add last row in reverse x - i - 2 -> i
                for (int j = x - i - 2; j >= i; j--) {
                    matrix[y - i - 1][j] = popFirst(rings[i]);
                }
                
            }
            return matrix;
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
            
            rings = rotate_rings(rings, r);
            // reassemble matrix circularly
            matrix = matrix_from_rings(matrix, rings);
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
                new List<int> { 1, 2, 3, 4 },
                new List<int> { 5, 6, 7, 8 },
                new List<int> { 9, 10, 11, 12 },
            };
            
            printMatrix(matrix);
            matrixRotation(matrix, 2);
            Console.WriteLine("rotated matrix:");
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
            matrix.Add(new List<int>{1});
            matrix.Add(new List<int>{2});
            return matrix;
        }
        static List<List<int>> empty_matrix() {
            return new List<List<int>>();
        }
        
        static void AssertEq(object a , object b) {
            Assert.AreEqual(a, b, $"\nExpected: {a ?? "null" }but got: {b ?? "null" }\n");
        }
        static void AssertEq(List<List<int>> a , List<List<int>> b) {
            var expected = a;
            var actual = b;
            for (int i = 0; i < expected.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], actual[i], $"\nList at index {i} did not match. Expected:\n{Program1.matrixToString(expected)}. Actual:\n{Program1.matrixToString(actual)}\n");
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
            AssertEq(new List<List<int>>() { new List<int>() { 1 } }, rings);

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
                rings[0].Add(100);
                rings = (List<List<int>>)ring_from_matrix_second.Invoke(null, new object[] { matrix, rings })!;
                return rings;
            };

            var matrix = wide_matrix();
            var rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 100, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21 },
            new List<int>() {  } }, rings);

            matrix = long_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 25, 22, 19, 16, 100, 18, 21, 24, 27, 30, 29, 28 },
            new List<int>() { 17, 20, 23, 26 } }, rings);

            matrix = small_matrix();
            rings = matrix_to_rings_first(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 100, 2 } }, rings);

            matrix = empty_matrix();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => matrix_to_rings_first(matrix));

        }
    
        [TestMethod]
        public void Test_rotate_rings() {
            MethodInfo create_rings = typeof(Program1).GetMethod("create_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo depth_of_matrix = typeof(Program1).GetMethod("depth_of_matrix", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo ring_from_matrix_first = typeof(Program1).GetMethod("rings_from_matrix_first", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo ring_from_matrix_second = typeof(Program1).GetMethod("rings_from_matrix_second", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo rotate_rings = typeof(Program1).GetMethod("rotate_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            var matrix_rotate = (List<List<int>> matrix) => {
                var depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
                var rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
                rings = (List<List<int>>)ring_from_matrix_first.Invoke(null, new object[] { matrix, rings })!;
                rings = (List<List<int>>)ring_from_matrix_second.Invoke(null, new object[] { matrix, rings })!;
                rings = (List<List<int>>)rotate_rings.Invoke(null, new object[] { rings, 3 })!;
                return rings;
            };
            
            var matrix = wide_matrix();
            var rings = matrix_rotate(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 11, 1, 2 },
            new List<int>() { 15, 16, 17, 18, 19, 12, 13, 14 } }, rings);

            matrix = long_matrix();
            rings = matrix_rotate(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 16, 13, 10, 7, 4, 1, 2, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 29, 28, 25, 22, 19 },
            new List<int>() { 14, 17, 20, 23, 26, 5, 8, 11 } }, rings);

            matrix = small_matrix();
            rings = matrix_rotate(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 2, 1 }}, rings);


        }
        [TestMethod]
        public void Test_reasamble_rings() {
            MethodInfo create_rings = typeof(Program1).GetMethod("create_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo depth_of_matrix = typeof(Program1).GetMethod("depth_of_matrix", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo ring_from_matrix_first = typeof(Program1).GetMethod("rings_from_matrix_first", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo ring_from_matrix_second = typeof(Program1).GetMethod("rings_from_matrix_second", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo rotate_rings = typeof(Program1).GetMethod("rotate_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo matrix_from_rings = typeof(Program1).GetMethod("matrix_from_rings", BindingFlags.NonPublic | BindingFlags.Static)!;
            var matrix_rotate = (List<List<int>> matrix) => {
                var depth = (int)depth_of_matrix.Invoke(null, new object[] { matrix })!;
                var rings = (List<List<int>>)create_rings.Invoke(null, new object[] { depth })!;
                rings = (List<List<int>>)ring_from_matrix_first.Invoke(null, new object[] { matrix, rings })!;
                rings = (List<List<int>>)ring_from_matrix_second.Invoke(null, new object[] { matrix, rings })!;
                rings = (List<List<int>>)rotate_rings.Invoke(null, new object[] { rings, 3 })!;
                var result = (List<List<int>>)matrix_from_rings.Invoke(null, new object[] { matrix, rings })!;
                return result;
            };

            var matrix = wide_matrix();
            var result = matrix_rotate(matrix);
            AssertEq(new List<List<int>>() { new List<int>() {  4, 5, 6, 7, 8, 9, 10, 20, 30, 29 },
            new List<int>() { 3, 15, 16, 17, 18, 19, 12, 13, 14, 28 },
            new List<int>() { 2, 1, 11, 21, 22, 23, 24, 25, 26, 27 } }, result);

            matrix = long_matrix();
            result = matrix_rotate(matrix);
            AssertEq(new List<List<int>>() {
            new List<int>() { 6, 9, 12 },
            new List<int>() { 3, 14, 15 },
            new List<int>() { 2, 17, 18 },
            new List<int>() { 1, 20, 21 },
            new List<int>() { 4, 23, 24 },
            new List<int>() { 7, 26, 27 },
            new List<int>() { 10, 5, 30 },
            new List<int>() { 13, 8, 29 },
            new List<int>() { 16, 11, 28 },
            new List<int>() { 19, 22, 25 }}, result);

            matrix = small_matrix();
            result = matrix_rotate(matrix);
            AssertEq(new List<List<int>>() { new List<int>() { 2 },
            new List<int>() { 1 } }, result);
        }
    }

}
