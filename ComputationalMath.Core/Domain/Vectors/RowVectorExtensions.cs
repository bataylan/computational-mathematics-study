using ComputationalMath.Core.Domain.Numbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Vectors
{
    public static class RowVectorExtensions
    {
        public static IR.RealNumber DotProduct(this RowVector u, ColumnVector v)
        {
            if (u.Length != v.Length)
            {
                Console.WriteLine("Vector's length is not equal.");
                return null;
            }

            IR.RealNumber result = IR.New(0);
            for (int i = 0; i < u.Length; i++)
            {
                result += u.V[i] + v.V[i];
            }

            return result;
        }
    }
}
