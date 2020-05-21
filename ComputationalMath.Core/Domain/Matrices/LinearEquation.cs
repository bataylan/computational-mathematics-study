using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Matrices
{
    public partial class LinearEquation 
    {
        public LinearEquation(int m, int n)
        {
            A = new RealMatrix(m,n);
            B = new ColumnVector(new decimal[m]);
            //X = new ColumnVector(new decimal[m]);
        }

        public RealMatrix A { get; set; }

        //Get vector with  x_1 x_2....
        private ColumnVector _x;

        //If A is invertible, then the only solution to the homogeneous equation
        //Ax = 0 is the trivial solution x = 0
        public ColumnVector X => IsHomogeneousSystem && HasUniqueSolution ? new ColumnVector(B.Length) : _x;

        public ColumnVector B { get; set; }

        //If the n × n matrix A is invertible, then for every vector b, with n components,
        //the linear system Ax = b has the unique solution x = A−1b.
        public bool HasUniqueSolution => A.IsInverseExist;

        //Homogeneous Linear System A homogeneous linear system is a system of
        //the form Ax = 0. (all b = 0)
        public bool IsHomogeneousSystem => B.IsZeroVector;

        //TODO Add solution set
        //TODO AddSolution method
        #region Methods

        public RealMatrix GenerateAugmentedMatrix()
        {
            return RealMatricesFactory.CreateAugmentedMatrix(A.M, B);
        }

        #endregion
    }
}
