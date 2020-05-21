using ComputationalMath.Core.Domain.Numbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Vectors
{
    public partial class ColumnVector : Vector
    {
        #region Ctors
        public ColumnVector(List<IR.RealNumber> v)
        : base(v)
        {
            IsRowVector = false;
            IsColumnVector = true;
        }

        public ColumnVector(int length)
            : base(length)
        {

        }

        public ColumnVector(decimal[] v)
            : base(v)
        {

        }

        public RowVector Transpose()
        {
            return new RowVector(V);
        }
        #endregion


        public static ColumnVector operator +(ColumnVector a, ColumnVector b)
        {
            if(a.Length != b.Length)
            {
                Console.WriteLine("Vectors' length are not same.");
                return null;
            }
            var result = new ColumnVector(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                result.V[i] = a.V[i] + b.V[i];
            }
            return result;
        }
    }
}
