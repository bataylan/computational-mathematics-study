using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using System;
using System.Collections.Generic;
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

        public static IR.RealNumber Determinant(this RealMatrix matrix, bool isMinor= false)
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
                var tempResult = matrix.M[0][j] * matrix.Cofactor(0, j, isMinor);
                if (!isMinor)
                    Console.WriteLine("Determinant is: " + tempResult);
                result += tempResult;
            }
            
            

            return result;
        }

        public static IR.RealNumber Cofactor(this RealMatrix matrix, int i, int j, bool isMinor)
        {
            if ((i + j) % 2 == 0)
            {
                Console.WriteLine("M" + i.ToString() + j.ToString() + ":");
                return Minor(matrix, i, j);
            }
            else
            {
                Console.WriteLine("(-1)M" + i.ToString() + j.ToString() + ":");
                return isMinor ? Minor(matrix, i, j) : -Minor(matrix, i, j);
            }
        }

        public static IR.RealNumber Minor(this RealMatrix matrix, int i, int j)
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
            return minorMatrix.Determinant(isMinor: true);
        }

        #endregion
    }
}
