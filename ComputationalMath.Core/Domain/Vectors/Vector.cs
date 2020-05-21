using ComputationalMath.Core.Domain.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputationalMath.Core.Domain.Vectors
{
    public abstract class Vector
    {
        public Vector(List<IR.RealNumber> v)
        {
            V = v;
        }

        public Vector(decimal[] v)
        {
            V = v.Select(x=> IR.New(x)).ToList();
        }

        public Vector(int length)
        {
            for (int i = 0; i < length; i++)
            {
                V.Add(IR.New(0));
            }
        }

        public int Length => V != null ? V.Count : 0;

        public List<IR.RealNumber> V { get; set; }

        public bool IsColumnVector { get; set; }

        public bool IsRowVector { get; set; }

        public bool IsZeroVector => IsAllComponentsZero();

        public void ElementMultiplication(IR.RealNumber multiplier)
        {
            for (int i = 0; i < V.Count; i++)
            {
                V[i] = V[i] * multiplier;
            }
        }

        public void ElementAddition(IR.RealNumber num)
        {
            for (int i = 0; i < V.Count; i++)
            {
                V[i] = V[i] + num;
            }
        }

        public bool IsAllComponentsZero()
        {
            for (int i = 0; i < V.Count; i++)
            {
                if(V[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        #region Operations

        

        #endregion

    }
}
