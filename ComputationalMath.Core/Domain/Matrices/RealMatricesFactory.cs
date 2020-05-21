using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Matrices
{
    public static class RealMatricesFactory
    {
        #region Augmented Matrix Factories
        public static RealMatrix CreateAugmentedMatrix(List<List<IR.RealNumber>> m, List<List<IR.RealNumber>> b)
        {
            if (m.Count != b.Count)
                throw new Exception("Row counts should be equal");
            else
            {
                return new RealMatrix(m, b);
            }
        }

        public static RealMatrix CreateAugmentedMatrix(List<List<IR.RealNumber>> m, ColumnVector b)
        {
            if (m.Count != b.V.Count)
                throw new Exception("Row counts should be equal");
            else
            {
                return new RealMatrix(m, b);
            }
        }

        public static RealMatrix CreateAugmentedMatrix(List<RowVector> m, List<RowVector> b)
        {
            if (m.Count != b.Count)
                throw new Exception("Row counts should be equal");
            else
            {
                return new RealMatrix(m, b);
            }
        }

        public static RealMatrix CreateAugmentedMatrix(List<RowVector> m, ColumnVector b)
        {
            if (m.Count != b.V.Count)
                throw new Exception("Row counts should be equal");
            else
            {
                return new RealMatrix(m, b);
            }
        }
        #endregion
    }
}
