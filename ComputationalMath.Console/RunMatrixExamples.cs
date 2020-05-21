using ComputationalMath.Core.Domain.Matrices;
using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using ComputationalMath.Methods.Matrices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Console
{
    public class RunMatrixExamples
    {
        #region Fields
        private readonly IMatrixMethods _matrixMethods;
        #endregion

        #region Ctor
        public RunMatrixExamples(IMatrixMethods matrixMethods)
        {
            _matrixMethods = matrixMethods;
        }
        #endregion

        #region Methods
        public void Run()
        {
            //while (true)
            //{
            //    decimal t1 = 5;
            //    decimal t2 = 3;

            //    decimal argument = t1 + t2;
            //    int count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];
            //    System.Console.WriteLine(count);

            //    argument = t1 - t2;
            //    count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];
            //    System.Console.WriteLine(count + "--" + argument);

            //    argument = t1 / t2;
            //    count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];
            //    System.Console.WriteLine(count + "--" + argument);

            //    argument = t1 * t2;
            //    count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];
            //    System.Console.WriteLine(count + "--" + argument);

            //    System.Console.ReadKey();
            //}

            decimal[] row1 = { 2, 1, -1 };
            decimal[] row2 = { 3, 1, 4 };
            decimal[] row3 = { 5, -3, 3 };
            //decimal[] row4 = { 1, 1, -1, 2 };

            //var rational1 = IQ.New(0, 1);
            //var rational2 = IQ.New(3, 4);
            //var result = rational1 * rational2;

            //decimal[] row1 = { 3, 2, 3 };
            //decimal[] row2 = { 1, 2, -1 };
            //decimal[] row3 = {1,1,1};

            var list = new List<RowVector>();
            list.Add(new RowVector(row1));
            list.Add(new RowVector(row2));
            list.Add(new RowVector(row3));
            //list.Add(new RowVector(row4));
            //list.Add(new RowVector<IR.RealNumber>(row3));
            //list.Add(new RowVector<IR.RealNumber>(row4));
            //decimal[] column = { -3, -2 };
            //var b = new ColumnVector(column);
            //list.Add(new RowVector<IR.RealNumber>(row4));
            var newMatrix = new RealMatrix(list);

            System.Console.WriteLine("Your matrix is: ");
            newMatrix.PrintToConsole();

            var determinant = newMatrix.Determinant();
            System.Console.WriteLine("Your determinant is: " + determinant);

            System.Console.ReadKey();
        }

        #endregion
    }
}
