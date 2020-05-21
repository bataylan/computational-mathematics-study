using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputationalMath.Core.Domain.Matrices
{
    public static class RealMatrixExtensions
    {
        public static RealMatrix DotProduct(this RealMatrix a, ColumnVector v)
        {
            if (v.Length != a.ColumnCount)
            {
                Console.WriteLine("Matrix column count is not equal to column vectors row count (mxn . nxp).");
                return null;
            }

            var m = a.RowCount;
            var p = v.Length;
            var result = new RealMatrix(m, p);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result.M[i][0] += a.M[i][j] + v.V[j];
                }
            }
            return result;
        }

        public static RealMatrix DotProduct(this RealMatrix a, RealMatrix b)
        {
            if (a.ColumnCount != b.RowCount)
            {
                Console.WriteLine("For matrix A column count not equal to B row count.");
                return null;
            }
            var result = new RealMatrix(a.RowCount, b.ColumnCount);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result.M[i][j] = a.GetRow(i).DotProduct(b.GetColumn(j));
                }
            }
            return result;
        }

        #region Methods
        //TODO A = LU  factorization (L - lower trianguler, U-upper triangular)
        public static IR.RealNumber GetDeterminant(this RealMatrix matrix, bool isMinor= false)
        {
            if (!matrix.IsSquareMatrix)
                return null;

            //TODO
            //Check that if matrix is triangular,then the determinant of A is the product of the
            //terms on the diagonal

            IR.RealNumber result = IR.New(0);

            if (matrix.RowCount == 2 && matrix.ColumnCount == 2)
            {
                result = (matrix.M[0][0] * matrix.M[1][1]) - (matrix.M[0][1] * matrix.M[1][0]);
                if(!isMinor)
                    Console.WriteLine("Determinant is: " + result);

                return result;
            }

            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                var tempResult = matrix.M[0][j] * matrix.GetCofactor(0, j, isMinor);
                if (!isMinor)
                    Console.WriteLine("Determinant is: " + tempResult);
                result += tempResult;
            }
            
            

            return result;
        }

        public static IR.RealNumber GetCofactor(this RealMatrix matrix, int i, int j, bool isMinor)
        {
            if ((i + j) % 2 == 0)
            {
                Console.WriteLine("M" + i.ToString() + j.ToString() + ":");
                return GetMinor(matrix, i, j);
            }
            else
            {
                Console.WriteLine("(-1)M" + i.ToString() + j.ToString() + ":");
                return isMinor ? GetMinor(matrix, i, j) : -GetMinor(matrix, i, j);
            }
        }

        public static IR.RealNumber GetMinor(this RealMatrix matrix, int i, int j)
        {
            var minorMatrix = new RealMatrix(matrix.RowCount - 1, matrix.ColumnCount - 1);
            int row = -1;
            int column = -1;

            for (int k = 0; k < matrix.RowCount; k++)
            {
                if (k == i)
                    continue;
                row++;
                for (int l = 0; l < matrix.ColumnCount; l++)
                {
                    if (l == j)
                        continue;
                    column++;
                    minorMatrix.M[row][column] = matrix.M[k][l];
                }
                column = -1;
            }
            minorMatrix.PrintToConsole();
            return minorMatrix.GetDeterminant(isMinor: true);
        }

        public static RealMatrix GetIdentityMatrix(this RealMatrix matrix)
        {
            if (!matrix.IsSquareMatrix)
                return null;
            var identityMatrix  = GetIdentityMatrix(matrix.RowCount);
            return identityMatrix;
        }

        public static RealMatrix GetIdentityMatrix(int n)
        {
            var identityMatrix = new RealMatrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        identityMatrix.M[i][j] = IR.New(IZ.New(1));
                }
            }

            return identityMatrix;
        }

        public static RealMatrix GetRowEchelonForm(this RealMatrix matrix)
        {
            int startFromRow = 0;
            int? nonZeroRowIndex = null;
            // if this is a augmented matrix, don't check last column

            //column loop to check each column
            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                nonZeroRowIndex = null;
                //first row loop for interchange and division for leading one
                for (int i = startFromRow; i < matrix.RowCount; i++)
                {
                    if (matrix.M[i][j] != 0)
                    {
                        nonZeroRowIndex = i;
                        //if non zero row is not in the right place, interchange it
                        if (nonZeroRowIndex > startFromRow)
                        {
                            //interchange rows i and start row
                            matrix.EROOne(nonZeroRowIndex.Value, startFromRow);
                            Console.WriteLine("ERO1 is applied. R_" + (i + 1) +
                                " <-> R_" + (nonZeroRowIndex.Value + 1));
                            matrix.PrintToConsole();

                            //non zero row changed since ERO1 interchanged rows
                            nonZeroRowIndex = i;
                        }

                        if (matrix.M[nonZeroRowIndex.Value][j] != 1)
                        {
                            //Multiply nonzero row to making leading entry one
                            var multiplier = 1 / matrix.M[nonZeroRowIndex.Value][j];
                            matrix.EROTwo(rowIndex: nonZeroRowIndex.Value, multiplier);
                            Console.WriteLine("ERO2 is applied. " + multiplier + "*R_" + (i + 1) +
                                " -> R_" + (nonZeroRowIndex.Value + 1));
                            matrix.PrintToConsole();
                        }

                        startFromRow++;
                        break;
                    }
                }

                if (!nonZeroRowIndex.HasValue)
                    break;

                //second row loop for eliminating other values in matrix
                for (int i = nonZeroRowIndex.Value + 1; i < matrix.RowCount; i++)
                {
                    var num = matrix.M[i][j];
                    if (num != 0)
                    {
                        // add multiple of first nonzero row in column to current row 
                        // for making leading of current row 0
                        Console.WriteLine("ERO3 is applied: " + num +
                            "*R_" + (nonZeroRowIndex + 1) + " + R_" + (i + 1) + "-> R_" + (i + 1));
                        matrix.EROThree(rowToMultiply: nonZeroRowIndex.Value,
                                constant: num, rowToAddIndex: i);
                        matrix.PrintToConsole();
                    }
                }
            }

            //if this matrix is augmented matrix, check that 
            if (matrix.IsAugmentedMatrix)
            {
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    if (matrix.GetRow(i).IsZeroVector
                        && matrix.M[i][matrix.ColumnCount - 1] != 0)
                    {
                        Console.WriteLine("Augmented matrix is inconsistent");
                    }
                }
            }

            return matrix;
        }

        public static RealMatrix GetReducedRowEchelonForm(this RealMatrix matrix)
        {
            int startFromRow = 0;
            int? nonZeroRowIndex = null;

            //column loop to check each column
            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                nonZeroRowIndex = null;
                //first row loop for interchange and division for leading one
                for (int i = startFromRow; i < matrix.RowCount; i++)
                {
                    if (matrix.M[i][j] != 0)
                    {
                        nonZeroRowIndex = i;
                        //if non zero row is not in the right place, interchange it
                        if (nonZeroRowIndex > startFromRow)
                        {
                            //interchange rows i and start row
                            Console.WriteLine("\nERO1 is applied. R" + (i + 1) +
                                " <-> R" + (nonZeroRowIndex.Value + 1));
                            matrix.EROOne(nonZeroRowIndex.Value, startFromRow);
                            matrix.PrintToConsole();

                            //non zero row changed since ERO1 interchanged rows
                            nonZeroRowIndex = i;
                        }

                        if (matrix.M[nonZeroRowIndex.Value][j] != 1)
                        {
                            //Multiply nonzero row to making leading entry one
                            var multiplier = 1 / matrix.M[nonZeroRowIndex.Value][j];
                            Console.WriteLine("\nERO2 is applied. " + multiplier +
                                "*R_" + (nonZeroRowIndex.Value + 1) +
                                " -> R_" + (nonZeroRowIndex.Value + 1));
                            matrix.EROTwo(rowIndex: nonZeroRowIndex.Value, multiplier);

                            matrix.PrintToConsole();
                        }

                        startFromRow++;
                        break;
                    }
                }

                if (!nonZeroRowIndex.HasValue)
                    break;

                //second row loop for eliminating other values in matrix
                for (int i = nonZeroRowIndex.Value + 1; true; i++)
                {
                    if (i == matrix.RowCount)
                        i = 0;

                    if (i == nonZeroRowIndex.Value)
                        break;

                    var num = matrix.M[i][j];
                    if (matrix.M[i][j] != 0)
                    {
                        // add multiple of first nonzero row in column to current row 
                        // for making leading of current row 0
                        var multiplier = matrix.M[i][j];
                        Console.WriteLine("\nERO3 is applied: " + multiplier +
                            "*R_" + (nonZeroRowIndex + 1) + " + R_" + (i + 1) + "-> R_" + (i + 1));
                        matrix.EROThree(rowToMultiply: nonZeroRowIndex.Value,
                                constant: multiplier, rowToAddIndex: i);
                        matrix.PrintToConsole();
                    }
                }
            }

            //if this matrix is augmented matrix, check that 
            if (matrix.IsAugmentedMatrix)
            {
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    if (matrix.GetRow(i).IsZeroVector
                        && matrix.M[i][matrix.ColumnCount - 1] != 0)
                    {
                        Console.WriteLine("Augmented matrix is inconsistent");
                    }
                }
            }

            return matrix;
        }

        public static RealMatrix GetInverse(this RealMatrix matrix)
        {
            if (!matrix.IsSquareMatrix)
            {
                Console.WriteLine("Non square matrices cannot be inversed.");
                return null;
            }

            var identityMatrix = matrix.GetIdentityMatrix();

            var newAugmentedMatrix = new RealMatrix(matrix.M, identityMatrix.M);
            newAugmentedMatrix.PrintToConsole();
            newAugmentedMatrix.GetReducedRowEchelonForm();

            return new RealMatrix(newAugmentedMatrix.B);
        }

        public static  RealMatrix GetInverseByDeterminant(this RealMatrix matrix)
        {
            if (!matrix.IsSquareMatrix)
            {
                Console.WriteLine("Non square matrices cannot be inversed.");
                return null;
            }

            return matrix.GetDeterminant() * matrix;
        }

        public static void PrintToConsole(this RealMatrix matrix)
        {
            Console.WriteLine();
            if (matrix.RowCount == 0 || matrix.ColumnCount == 0)
            {
                Console.WriteLine("Your matrix is empty.");
                return;
            }

            for (int i = 0; i < matrix.RowCount; i++)
            {
                IR.RealNumber num;
                for (int j = 0; j < matrix.ColumnCount; j++)
                {

                    //If this is the last column for augmented matrix
                    if (j < matrix.ColumnCount)
                    {
                        Console.Write((j > 0 ? (matrix.M[i][j] >= 0 ? "  " : " ") : "") + matrix.M[i][j].ToString());
                    }
                }
                if (matrix.IsAugmentedMatrix)
                {
                    for (int j = 0; j < matrix.AugmentedColumnCount; j++)
                    {
                        if (j == 0)
                        {
                            Console.Write(" | " + (j > 0 ? (matrix.B[i][j] >= 0 ? "  " : " ") : "") + matrix.B[i][j].ToString());
                        }
                        else
                        {
                            Console.Write((j > 0 ? (matrix.B[i][j] >= 0 ? "  " : " ") : "") + matrix.B[i][j].ToString());
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static RealMatrix GetTranspose(this RealMatrix matrix)
        {
            var columnVectors = new List<ColumnVector>();
            for (int i = 0; i < matrix.RowCount; i++)
            {
                columnVectors.Add(matrix.GetRow(i).Transpose());
            }
            return new RealMatrix(columnVectors);
        }

        /// <summary>
        /// Return row of a matrix
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>Vector</returns>
        public static RowVector GetRow(this RealMatrix matrix, int rowIndex)
        {
            var row = new List<IR.RealNumber>();
            row.AddRange(matrix.M[rowIndex].GetRange(0, matrix.ColumnCount).ToList());
            var vector = new RowVector(v: row);
            return vector;
        }

        /// <summary>
        /// Return column of a matrix
        /// </summary>
        /// <param name="columnNumber">Column index</param>
        /// <returns>Vector</returns>
        public static ColumnVector GetColumn(this RealMatrix matrix, int columnIndex)
        {
            var vector = new ColumnVector(matrix.RowCount);
            for (int i = 0; i < matrix.RowCount; i++)
            {
                vector.V.Add(matrix.M[i][columnIndex]);
            }
            return vector;
        }

        /// <summary>
        /// Return row of a matrix
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>Vector</returns>
        public static RowVector GetAugmentedRow(this RealMatrix matrix, int rowIndex)
        {
            var row = matrix.B[rowIndex].GetRange(0, matrix.AugmentedColumnCount).ToList();
            var vector = new RowVector(v: row);
            return vector;
        }

        /// <summary>
        /// Return column of a matrix
        /// </summary>
        /// <param name="columnNumber">Column index</param>
        /// <returns>Vector</returns>
        public static ColumnVector GetAugmentedColumn(this RealMatrix matrix, int columnIndex)
        {
            var vector = new ColumnVector(matrix.AugmentedRowCount);
            for (int i = 0; i < matrix.AugmentedRowCount; i++)
            {
                vector.V.Add(matrix.B[i][columnIndex]);
            }
            return vector;
        }

        public static List<RowVector> GetRowVectors(this RealMatrix matrix)
        {
            var result = new List<RowVector>();
            for (int i = 0; i < matrix.RowCount; i++)
            {
                result.Add(new RowVector(matrix.M[i]));
            }
            return result;
        }
        public static List<ColumnVector> GetColumnVectors(this RealMatrix matrix)
        {
            var result = new List<ColumnVector>();
            for (int j = 0; j < matrix.ColumnCount; j++)
            {
                var tempColumnList = new List<IR.RealNumber>();
                for (int i = 0; i < matrix.M.Count; i++)
                {
                    tempColumnList.Add(matrix.M[i][j]);
                }
                result.Add(new ColumnVector(tempColumnList));
            }

            return result;
        }

        /// <summary>
        /// Elementary row operation one, interchange two rows
        /// </summary>
        /// <param name="rowOneIndex">Row one index</param>
        /// <param name="rowTwoIndex">Row two index</param>
        public static void EROOne(this RealMatrix matrix, int rowOneIndex, int rowTwoIndex)
        {
            var rowOne = matrix.GetRow(rowOneIndex);
            var rowTwo = matrix.GetRow(rowTwoIndex);

            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                matrix.M[rowOneIndex][i] = rowTwo.V[i];
                matrix.M[rowTwoIndex][i] = rowOne.V[i];
            }

            if (matrix.IsAugmentedMatrix)
            {
                var augmentedRowOne = matrix.GetAugmentedRow(rowOneIndex);
                var augmentedRowTwo = matrix.GetAugmentedRow(rowTwoIndex);

                for (int i = 0; i < matrix.AugmentedColumnCount; i++)
                {
                    matrix.B[rowOneIndex][i] = augmentedRowOne.V[i];
                    matrix.B[rowTwoIndex][i] = augmentedRowTwo.V[i];
                }
            }

            matrix.MatrixElementsChanged();

            //TODO
            //If two rows of A are interchanged to produce a matrix B, then det(B) =
            //− det(A).
        }

        /// <summary>
        /// Elemental row operation two, multiply one row with a constant
        /// </summary>
        /// <param name="rowIndex">Row to multiplt</param>
        /// <param name="constant">Multiply row with this constant</param>
        public static void EROTwo(this RealMatrix matrix, int rowIndex, IR.RealNumber constant)
        {
            if (constant == 0)
                return;

            var row = matrix.GetRow(rowIndex);
            row.ElementMultiplication(constant);
            matrix.WriteOnRow(row, rowIndex);

            if (matrix.IsAugmentedMatrix)
            {
                var augmentedRow = matrix.GetAugmentedRow(rowIndex);
                augmentedRow.ElementMultiplication(constant);
                matrix.WriteOnRow(augmentedRow, rowIndex, isAugmentedRow: true);
            }

            matrix.MatrixElementsChanged();
            //TODO
            //If a row of A is multiplied by a real number α to produce a matrix B, then
            //det(B) = αdet(A)
        }

        /// <summary>
        /// Elemental row operation three, multiply one row to add another row
        /// </summary>
        /// <param name="rowToMultiply">Multiply this index row</param>
        /// <param name="constant">Multiply with this constant row</param>
        /// <param name="rowToAddIndex">Add multiple of a row to this indexex row</param>
        public static void EROThree(this RealMatrix matrix, int rowToMultiply, IR.RealNumber constant, int rowToAddIndex)
        {
            if (constant == 0)
                return;

            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                matrix.M[rowToAddIndex][i] += matrix.M[rowToMultiply][i] * -constant;
            }
            if (matrix.IsAugmentedMatrix)
            {
                for (int i = 0; i < matrix.AugmentedColumnCount; i++)
                {
                    matrix.B[rowToAddIndex][i] += matrix.B[rowToMultiply][i] * -constant;
                }
            }

            matrix.MatrixElementsChanged();
        }

        /// <summary>
        /// Replace row with vector that same size
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="rowIndex">Row index</param>
        public static void WriteOnRow(this RealMatrix matrix, RowVector rowVector, int rowIndex, bool isAugmentedRow = false)
        {
            //check that matrix row length (column count) is equal to vector length
            //and row index is valid
            if (!isAugmentedRow)
            {
                if (rowVector.V.Count != matrix.ColumnCount && rowIndex >= matrix.RowCount)
                    return;

                for (int i = 0; i < rowVector.V.Count; i++)
                {
                    matrix.M[rowIndex][i] = rowVector.V[i];
                }
            }
            else
            {
                if (rowVector.V.Count != matrix.AugmentedColumnCount && rowIndex >= matrix.AugmentedRowCount)
                    return;

                for (int i = 0; i < rowVector.V.Count; i++)
                {
                    matrix.B[rowIndex][i] = rowVector.V[i];
                }
            }

            matrix.MatrixElementsChanged();
        }

        /// <summary>
        /// Replace column with vector that same size
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="rowIndex">Row index</param>
        public static void WriteOnColumn(this RealMatrix matrix, ColumnVector vector, int columnIndex, bool isAugmentedRow = false)
        {
            //check that matrix column length (row count) is equal to vector length
            //and column index is valid
            if (!isAugmentedRow)
            {
                if (vector.V.Count != matrix.RowCount && columnIndex >= matrix.ColumnCount)
                    return;

                for (int i = 0; i < vector.V.Count; i++)
                {

                    matrix.M[i][columnIndex] = vector.V[i];
                }
            }
            else
            {
                if (vector.V.Count != matrix.AugmentedRowCount && columnIndex >= matrix.AugmentedColumnCount)
                    return;

                for (int i = 0; i < vector.V.Count; i++)
                {

                    matrix.B[i][columnIndex] = vector.V[i];
                }
            }

            matrix.MatrixElementsChanged();
        }

        //public void GetMatrixFromInput(this RealMatrix matrix)
        //{
        //    for (int i = 0; i < this.RowCount; i++)
        //    {
        //        for (int j = 0; j < this.ColumnCount; j++)
        //        {
        //            Console.WriteLine("Please write " + i + " row, " + j + "th element: ");
        //            do
        //            {
        //                var input = Console.ReadLine();
        //                if (decimal.TryParse(input, out decimal result))
        //                {
        //                    this.M[i][j] = result;
        //                    break;
        //                }

        //                Console.WriteLine("Your input cannot be parsed to decimal. " +
        //                    "Please enter a valid input for matrix: ");

        //            } while (true);
        //        }
        //    }
        //}
        #endregion
    }
}
