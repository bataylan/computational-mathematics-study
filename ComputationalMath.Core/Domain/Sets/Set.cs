using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputationalMath.Core.Domain.Sets
{
    /// Summary
    /// Correspond to mathematical set
    /// Summary
    public abstract class Set
    {
        public Set()
        {
            //Elements = objects.Distinct().ToArray();
            //Count = Elements.Count();
        }

        /// Summary
        /// Correspond to mathematical distinct elements in a set
        /// Summary
        public object[] Elements { get; set; }

        /// Summary
        /// Element count in set
        /// Summary
        public int Count { get; set; }

    }
}
