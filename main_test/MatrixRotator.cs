namespace MatrixRotation;
public static class MatrixRotator
{
    static List<List<int>> CreateRings(int depth)
    {
        // create a list of rings
        var rings = new List<List<int>>();
        // create depth amount of rings
        for (int i = 0; i < depth; i++)
        {
            rings.Add([]);
        }
        return rings;
    }

    static int DepthOfMatrix(List<List<int>> matrix)
    {
        // make sure matrix is not empty
        if (matrix.Count == 0) return 0;
        return matrix.Count< matrix[0].Count ? (matrix.Count+1) / 2 : (matrix[0].Count + 1) / 2;
    }

    static List<List<int>> RingsFromMatrixTopHalf(List<List<int>> matrix , List<List<int>> rings)
    {
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

    static List<List<int>> RingsFromMatrixLowerHalf(List<List<int>> matrix, List<List<int>> rings)
    {
        // disassamble it row by row second half
        for (int i = (matrix.Count + 1) / 2; i < matrix.Count; i++){
            var from_end = matrix.Count - i - 1;
            int indent = from_end < rings.Count - 1 ? from_end : rings.Count - 1;
            // add first numbers
            for (int j = 0; j < indent; j++)
            {
                rings[j].Insert(0, matrix[i][j]);
            }
            // add last numbers
            for (int j = 0; j < indent; j++)
            {
                rings[j].Add(matrix[i][matrix[0].Count - j - 1]);
            }
            // add middle section
            for (int j = matrix[0].Count - indent - 1; j >= indent; j--)
            {
                rings[indent].Add(matrix[i][j]);
            }
        }
        return rings;
    }

    static List<List<int>> RotateRings(List<List<int>> rings, int r)
    {
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
    
    static void MatrixFromRings(List<List<int>> matrix, List<List<int>> rings)
    {
        var x = matrix[0].Count;
        var y = matrix.Count;
        static int PopFirst(List<int> list)
        {
            var first = list[0];
            list.RemoveAt(0);
            return first;
        }
        // reassemble matrix circularly
        for (int i = 0; i < rings.Count; i++)
        {
            // if the remaining matrix is a single row or column add it and break
            if (x - 2 * i == 1){
                Console.WriteLine("single column");
                for (int j = i; j < y - i; j++){
                    matrix[j][i] = PopFirst(rings[i]);
                }
                break;
            }
            if (y - 2 * i == 1){
                Console.WriteLine("single row");
                for (int j = i; j < x - i; j++){
                    matrix[i][j] = PopFirst(rings[i]);
                }
                break;
            }

            // add first column y - 2 - i -> i - 1
            for (int j = y - 2 - i; j > i; j--) {
                matrix[j][i] = PopFirst(rings[i]);
            }

            // add first row i -> x - i - 1
            for (int j = i; j <= x - i - 1; j++) {
                matrix[i][j] = PopFirst(rings[i]);
            }

            // add last column i + 1 -> y - i - 1
            for (int j = i + 1; j <= y - i - 1; j++) {
                matrix[j][x - i - 1] = PopFirst(rings[i]);
            }

            // add last row in reverse x - i - 2 -> i
            for (int j = x - i - 2; j >= i; j--) {
                matrix[y - i - 1][j] = PopFirst(rings[i]);
            }
        }
    }
    
    public static void MatrixRotation(List<List<int>> matrix, int r)
    {
        if (matrix.Count == 0)
            return;
        // create rings depth from matrix
        var depth = DepthOfMatrix(matrix);
        // create a list of rings
        var rings = CreateRings(depth);

        rings = RingsFromMatrixTopHalf(matrix, rings);

        rings = RingsFromMatrixLowerHalf(matrix, rings);
        
        rings = RotateRings(rings, r);
        // reassemble matrix circularly
        MatrixFromRings(matrix, rings);
    }

    public static void PrintMatrix(List<List<int>> matrix) =>
        matrix.ForEach(row => Console.WriteLine($"[{string.Join(", ", row)}]"));
    
    public static string MatrixToString(List<List<int>> matrix)
    {
        string s = "";
        matrix.ForEach(row => s += $"[{string.Join(", ", row)}]\n");
        return s;
    }

    public static void Main()
    {
        List<List<int>> matrix =
        [
            [1, 2, 3, 4],
            [5, 6, 7, 8],
            [9, 10, 11, 12],
        ];
        
        PrintMatrix(matrix);
        MatrixRotation(matrix, 3);
        Console.WriteLine("rotated matrix:");
        PrintMatrix(matrix);
    }
}
