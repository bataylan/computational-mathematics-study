using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputationalMath.Core.Domain.Matrices
{

    public partial class RealMatrix
    {
        #region Real Matrix

        #region Ctor
        public RealMatrix(int m, int n)
        {
            var matrix = new List<List<IR.RealNumber>>();
            for (int i = 0; i < m; i++)
            {
                var row = new List<IR.RealNumber>();
                for (int j = 0; j < n; j++)
                {
                    row.Add(IR.New(0));
                }
                matrix.Add(row);
            }

            M = matrix;
            RowCount = M.Count;
            ColumnCount = M[0].Count;
        }
        public RealMatrix(List<List<IR.RealNumber>> m)
        {
            M = m;
            RowCount = M.Count;
            ColumnCount = M[0].Count;
        }
        public RealMatrix(List<RowVector> m)
        {
            M = new List<List<IR.RealNumber>>(m.Select(x => x.V).ToList());
            RowCount = M.Count;
            ColumnCount = M[0].Count;
        }
        public RealMatrix(List<ColumnVector> m)
        {
            M = new List<List<IR.RealNumber>>();
            for (int i = 0; i < m.Count; i++)
            {
                for (int j = 0; j < m[0].Length; j++)
                {
                    M[i][j] = m[j].V[i];
                }
            }
            RowCount = M.Count;
            ColumnCount = M[0].Count;
        }

        #region Augmented Matrix Constructors
        internal RealMatrix(List<List<IR.RealNumber>> m, List<List<IR.RealNumber>> b)
        {
            M = m;
            RowCount = M.Count;
            ColumnCount = M[0].Count;

            B = b;
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = B[0].Count;
            IsAugmentedMatrix = true;
        }
        internal RealMatrix(List<List<IR.RealNumber>> m, ColumnVector b)
        {
            M = m;
            RowCount = M.Count;
            ColumnCount = M[0].Count;

            B = new List<List<IR.RealNumber>>(b.V.Select(x => { var list = new List<IR.RealNumber>(); list.Add(x); return list; }));
            //B.Add(b.V);
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = 1;
            IsAugmentedMatrix = true;
        }
        internal RealMatrix(List<RowVector> m, List<RowVector> b)
        {
            M = new List<List<IR.RealNumber>>(m.Select(x => x.V).ToList());
            RowCount = M.Count;
            ColumnCount = M[0].Count;

            B = new List<List<IR.RealNumber>>(b.Select(x => x.V).ToList());
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = B[0].Count;
            IsAugmentedMatrix = true;
        }
        internal RealMatrix(List<RowVector> m, ColumnVector b)
        {
            M = new List<List<IR.RealNumber>>(m.Select(x => x.V).ToList());
            RowCount = M.Count;
            ColumnCount = M[0].Count;

            B = new List<List<IR.RealNumber>>(b.V.Select(x => { var list = new List<IR.RealNumber>(); list.Add(x); return list; }));
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = 1;
            IsAugmentedMatrix = true;
        }
        #endregion

        #endregion

        //Matrix
        public List<List<IR.RealNumber>> M { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public bool IsSquareMatrix => RowCount == ColumnCount;

        public bool IsSymmetricMatrix => M == Transpose().M;

        public bool IsInverseExist => this.Determinant() != 0;


        //Augmented part
        public List<List<IR.RealNumber>> B { get; set; }
        public int AugmentedRowCount { get; set; }
        public int AugmentedColumnCount { get; set; }
        public bool IsAugmentedMatrix { get; set; }

        public bool IsInconsistent { get; set; }



        public List<RowVector> RowVectors()
        {
            var result = new List<RowVector>();
            for (int i = 0; i < M.Count; i++)
            {
                result.Add(new RowVector(M[i]));
            }
            return result;
        }
        public List<ColumnVector> ColumnVectors()
        {
            var result = new List<ColumnVector>();
            for (int j = 0; j < M[0].Count; j++)
            {
                var tempColumnList = new List<IR.RealNumber>();
                for (int i = 0; i < M.Count; i++)
                {
                    tempColumnList.Add(M[i][j]);
                }
                result.Add(new ColumnVector(tempColumnList));
            }

            return result;
        }

        #region Addition and Scalar Multiplication

        public static RealMatrix operator +(RealMatrix a, RealMatrix b)
        {
            if (!(a.RowCount == b.RowCount && a.ColumnCount == b.ColumnCount))
                Console.WriteLine("Two matrix must have same size (m,n) for addition.");

            var result = new RealMatrix(a.RowCount, a.ColumnCount);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result.M[i][j] = a.M[i][j] + b.M[i][j];
                }
                if (a.IsAugmentedMatrix && b.IsAugmentedMatrix)
                {
                    result.B[0][i] = a.B[0][i] + b.B[0][i];
                }
            }
            return result;
        }

        public static RealMatrix operator *(RealMatrix a, IR.RealNumber c)
        {
            var result = new RealMatrix(a.RowCount, a.ColumnCount);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result.M[i][j] = a.M[i][j] * c;
                }
                if (a.IsAugmentedMatrix)
                {
                    result.B[0][i] = a.B[0][i] * c;
                }
            }
            return result;
        }

        public static RealMatrix operator *(IR.RealNumber c, RealMatrix a)
        {
            return a * c;
        }

        public static bool operator ==(RealMatrix a, RealMatrix b)
        {
            if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                return false;

            bool result = true;

            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    if (a.M[i][j] != b.M[i][j])
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public static bool operator !=(RealMatrix a, RealMatrix b)
        {
            return !(a == b);
        }

        #endregion

        #region Methods
        public void PrintToConsole()
        {
            Console.WriteLine();
            if (RowCount == 0 || ColumnCount == 0)
            {
                Console.WriteLine("Your matrix is empty.");
                return;
            }

            for (int i = 0; i < RowCount; i++)
            {
                IR.RealNumber num;
                for (int j = 0; j < ColumnCount; j++)
                {

                    //If this is the last column for augmented matrix
                    if (j < ColumnCount)
                    {
                        Console.Write((j > 0 ? (M[i][j] >= 0 ? "  " : " ") : "") + M[i][j].ToString());
                    }
                }
                if (IsAugmentedMatrix)
                {
                    for (int j = 0; j < AugmentedColumnCount; j++)
                    {
                        if (j == 0)
                        {
                            Console.Write(" | " + (j > 0 ? (B[i][j] >= 0 ? "  " : " ") : "") + B[i][j].ToString());
                        }
                        else
                        {
                            Console.Write((j > 0 ? (B[i][j] >= 0 ? "  " : " ") : "") + B[i][j].ToString());
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public RealMatrix Transpose()
        {
            var columnVectors = new List<ColumnVector>();
            for (int i = 0; i < RowCount; i++)
            {
                columnVectors.Add(this.GetRow(i).Transpose());
            }
            return new RealMatrix(columnVectors);
        }

        /// <summary>
        /// Return row of a matrix
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>Vector</returns>
        public RowVector GetRow(int rowIndex)
        {
            var row = new List<IR.RealNumber>();
            row.AddRange(M[rowIndex].GetRange(0, this.ColumnCount).ToList());
            var vector = new RowVector(v: row);
            return vector;
        }

        /// <summary>
        /// Return column of a matrix
        /// </summary>
        /// <param name="columnNumber">Column index</param>
        /// <returns>Vector</returns>
        public ColumnVector GetColumn(int columnIndex)
        {
            var vector = new ColumnVector(RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                vector.V.Add(M[i][columnIndex]);
            }
            return vector;
        }

        /// <summary>
        /// Return row of a matrix
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>Vector</returns>
        public RowVector GetAugmentedRow(int rowIndex)
        {
            var row = B[rowIndex].GetRange(0, this.AugmentedColumnCount).ToList();
            var vector = new RowVector(v: row);
            return vector;
        }

        /// <summary>
        /// Return column of a matrix
        /// </summary>
        /// <param name="columnNumber">Column index</param>
        /// <returns>Vector</returns>
        public ColumnVector GetAugmentedColumn(int columnIndex)
        {
            var vector = new ColumnVector(AugmentedRowCount);
            for (int i = 0; i < AugmentedRowCount; i++)
            {
                vector.V.Add(B[i][columnIndex]);
            }
            return vector;
        }

        //ERO One, interchange two rows
        /// <summary>
        /// Elementary row operation one, interchange two rows
        /// </summary>
        /// <param name="rowOneIndex">Row one index</param>
        /// <param name="rowTwoIndex">Row two index</param>
        public void EROOne(int rowOneIndex, int rowTwoIndex)
        {
            var rowOne = GetRow(rowOneIndex);
            var rowTwo = GetRow(rowTwoIndex);

            for (int i = 0; i < ColumnCount; i++)
            {
                this.M[rowOneIndex][i] = rowTwo.V[i];
                this.M[rowTwoIndex][i] = rowOne.V[i];
            }

            if (IsAugmentedMatrix)
            {
                var augmentedRowOne = GetAugmentedRow(rowOneIndex);
                var augmentedRowTwo = GetAugmentedRow(rowTwoIndex);

                for (int i = 0; i < AugmentedColumnCount; i++)
                {
                    this.B[rowOneIndex][i] = augmentedRowOne.V[i];
                    this.B[rowTwoIndex][i] = augmentedRowTwo.V[i];
                }
            }

            //TODO
            //If two rows of A are interchanged to produce a matrix B, then det(B) =
            //− det(A).
        }

        /// <summary>
        /// Elemental row operation two, multiply one row with a constant
        /// </summary>
        /// <param name="rowIndex">Row to multiplt</param>
        /// <param name="constant">Multiply row with this constant</param>
        public void EROTwo(int rowIndex, IR.RealNumber constant)
        {
            if (constant == 0)
                return;

            var row = GetRow(rowIndex);
            row.ElementMultiplication(constant);
            WriteOnRow(row, rowIndex);

            if (IsAugmentedMatrix)
            {
                var augmentedRow = GetAugmentedRow(rowIndex);
                augmentedRow.ElementMultiplication(constant);
                WriteOnRow(augmentedRow, rowIndex, isAugmentedRow: true);
            }

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
        public void EROThree(int rowToMultiply, IR.RealNumber constant, int rowToAddIndex)
        {
            if (constant == 0)
                return;

            for (int i = 0; i < ColumnCount; i++)
            {
                M[rowToAddIndex][i] += this.M[rowToMultiply][i] * -constant;
            }
            if (IsAugmentedMatrix)
            {
                for (int i = 0; i < AugmentedColumnCount; i++)
                {
                    B[rowToAddIndex][i] += this.B[rowToMultiply][i] * -constant;
                }
            }
        }

        /// <summary>
        /// Replace row with vector that same size
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="rowIndex">Row index</param>
        public void WriteOnRow(RowVector rowVector, int rowIndex, bool isAugmentedRow = false)
        {
            //check that matrix row length (column count) is equal to vector length
            //and row index is valid
            if (!isAugmentedRow)
            {
                if (rowVector.V.Count != this.ColumnCount && rowIndex >= this.RowCount)
                    return;

                for (int i = 0; i < rowVector.V.Count; i++)
                {
                    this.M[rowIndex][i] = rowVector.V[i];
                }
            }
            else
            {
                if (rowVector.V.Count != this.AugmentedColumnCount && rowIndex >= this.AugmentedRowCount)
                    return;

                for (int i = 0; i < rowVector.V.Count; i++)
                {
                    this.B[rowIndex][i] = rowVector.V[i];
                }
            }
        }

        /// <summary>
        /// Replace column with vector that same size
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="rowIndex">Row index</param>
        public void WriteOnColumn(ColumnVector vector, int columnIndex, bool isAugmentedRow = false)
        {
            //check that matrix column length (row count) is equal to vector length
            //and column index is valid
            if (!isAugmentedRow)
            {
                if (vector.V.Count != this.RowCount && columnIndex >= this.ColumnCount)
                    return;

                for (int i = 0; i < vector.V.Count; i++)
                {

                    this.M[i][columnIndex] = vector.V[i];
                }
            }
            else
            {
                if (vector.V.Count != this.AugmentedRowCount && columnIndex >= this.AugmentedColumnCount)
                    return;

                for (int i = 0; i < vector.V.Count; i++)
                {

                    this.B[i][columnIndex] = vector.V[i];
                }
            }
        }

        public void GetMatrixFromInput()
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    Console.WriteLine("Please write " + i + " row, " + j + "th element: ");
                    do
                    {
                        var input = Console.ReadLine();
                        if (decimal.TryParse(input, out decimal result))
                        {
                            this.M[i][j] = result;
                            break;
                        }

                        Console.WriteLine("Your input cannot be parsed to decimal. " +
                            "Please enter a valid input for matrix: ");

                    } while (true);
                }
            }
        }

        public virtual RealMatrix RowEchelonForm()
        {
            var matrix = this;
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
                            EROOne(nonZeroRowIndex.Value, startFromRow);
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
                            EROTwo(rowIndex: nonZeroRowIndex.Value, multiplier);
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
                for (int i = nonZeroRowIndex.Value + 1; i < RowCount; i++)
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
            if (IsAugmentedMatrix)
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

        public virtual RealMatrix ReducedRowEchelonForm()
        {
            var matrix = this;
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
                            EROOne(nonZeroRowIndex.Value, startFromRow);
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
                            EROTwo(rowIndex: nonZeroRowIndex.Value, multiplier);

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
                    if (i == RowCount)
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
            if (IsAugmentedMatrix)
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

        public RealMatrix Inverse()
        {
            if (!IsSquareMatrix)
            {
                Console.WriteLine("Non square matrices cannot be inversed.");
                return null;
            }


            var identityMatrix = new RealMatrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    if (i == j)
                        identityMatrix.M[i][j] = IR.New(IZ.New(1));
                }
            }

            var newAugmentedMatrix = new RealMatrix(M, identityMatrix.M);
            newAugmentedMatrix.PrintToConsole();
            newAugmentedMatrix.ReducedRowEchelonForm();

            return new RealMatrix(newAugmentedMatrix.B);
        }

        public RealMatrix InverseByDeterminant()
        {
            if (!IsSquareMatrix)
            {
                Console.WriteLine("Non square matrices cannot be inversed.");
                return null;
            }

            return this.Determinant() * this;
        }

        
        #endregion

    }

    #endregion

}
