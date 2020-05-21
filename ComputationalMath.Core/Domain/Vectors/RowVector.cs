using ComputationalMath.Core.Domain.Numbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Vectors
{
    public partial class RowVector : Vector
    { 
        public RowVector(List<IR.RealNumber> v)
        : base(v)
        {
            IsRowVector = true;
            IsColumnVector = false;
        }

        public RowVector(int length)
            : base(length)
        {

        }

        public RowVector(decimal[] v)
            : base(v)
        {

        }

        public ColumnVector Transpose()
        {
            return new ColumnVector(V);
        }

        public static RowVector operator +(RowVector a, RowVector b)
        {
            if (a.Length != b.Length)
            {
                Console.WriteLine("Vectors' length are not same.");
                return null;
            }
            var result = new RowVector(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                result.V[i] = a.V[i] + b.V[i];
            }
            return result;
        }
    }
}
