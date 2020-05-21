using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Numbers
{
    public interface IRealNumberExtensions<T> where T : IR.RealNumber
    {
        public IR.RealNumber ConvertToRealNumber(T number);
    }
}
