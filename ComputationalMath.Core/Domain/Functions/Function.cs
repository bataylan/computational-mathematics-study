using ComputationalMath.Core.Domain.Sets;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ComputationalMath.Core.Domain.Functions
{
    public partial class Function
    {
        public Function(Expression function, Set domain, Set range = null)
        {
            Expression = function;
            Domain = domain;

        }

        public Expression Expression { get; set; }

        public Set Domain { get; set; }

        public Set Range { get; set; }

        public void GenerateRange()
        {

        }
    }
}
