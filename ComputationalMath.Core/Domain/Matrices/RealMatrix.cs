using ComputationalMath.Core.Domain.Numbers;
using ComputationalMath.Core.Domain.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputationalMath.Core.Domain.Matrices
{

    public partial class RealMatrix
    {
        #region Real Matrix

        #region Ctor
        public RealMatrix(int m, int n)
        {
            var matrix = new List<List<IR.RealNumber>>();
            for (int i = 0; i < m; i++)
            {
                var row = new List<IR.RealNumber>();
                for (int j = 0; j < n; j++)
                {
                    row.Add(IR.New(0));
                }
                matrix.Add(row);
            }

            _matrix = matrix;
            _rowCount = M.Count;
            _columnCount = M[0].Count;
        }
        public RealMatrix(List<List<IR.RealNumber>> m)
        {
            _matrix = m;
            _rowCount = M.Count;
            _columnCount = M[0].Count;
        }
        public RealMatrix(List<RowVector> m)
        {
            _matrix = new List<List<IR.RealNumber>>(m.Select(x => x.V).ToList());
            _rowCount = M.Count;
            _columnCount = M[0].Count;
        }
        public RealMatrix(List<ColumnVector> m)
        {
            _matrix = new List<List<IR.RealNumber>>();
            for (int i = 0; i < m.Count; i++)
            {
                for (int j = 0; j < m[0].Length; j++)
                {
                    M[i][j] = m[j].V[i];
                }
            }
            _rowCount = M.Count;
            _columnCount = M[0].Count;
        }

        #region Augmented Matrix Constructors
        internal RealMatrix(List<List<IR.RealNumber>> m, List<List<IR.RealNumber>> b)
        {
            _matrix = m;
            _rowCount = M.Count;
            _columnCount = M[0].Count;

            B = b;
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = B[0].Count;
            IsAugmentedMatrix = true;
        }
        internal RealMatrix(List<List<IR.RealNumber>> m, ColumnVector b)
        {
            _matrix = m;
            _rowCount = M.Count;
            _columnCount = M[0].Count;

            B = new List<List<IR.RealNumber>>(b.V.Select(x => { var list = new List<IR.RealNumber>(); list.Add(x); return list; }));
            //B.Add(b.V);
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = 1;
            IsAugmentedMatrix = true;
        }
        internal RealMatrix(List<RowVector> m, List<RowVector> b)
        {
            _matrix = new List<List<IR.RealNumber>>(m.Select(x => x.V).ToList());
            _rowCount = M.Count;
            _columnCount = M[0].Count;

            B = new List<List<IR.RealNumber>>(b.Select(x => x.V).ToList());
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = B[0].Count;
            IsAugmentedMatrix = true;
        }
        internal RealMatrix(List<RowVector> m, ColumnVector b)
        {
            _matrix = new List<List<IR.RealNumber>>(m.Select(x => x.V).ToList());
            _rowCount = M.Count;
            _columnCount = M[0].Count;

            B = new List<List<IR.RealNumber>>(b.V.Select(x => { var list = new List<IR.RealNumber>(); list.Add(x); return list; }));
            AugmentedRowCount = B.Count;
            AugmentedColumnCount = 1;
            IsAugmentedMatrix = true;
        }
        #endregion

        #endregion

        #region Fields
        #region Fields that are based on size
        private int _rowCount;
        private int _columnCount;
        public bool _isSquareMatrix;
        #endregion

        #region Fields that are based on elements
        private bool? _isSymmetricMatrix;
        private bool? _isInverseExist;
        private RealMatrix _identityMatrix;
        private RealMatrix _rowEchelonForm;
        private RealMatrix _reducedRowEchelonForm;
        private List<RowVector> _rowVectors;
        private List<ColumnVector> _columnVectors;
        private List<List<IR.RealNumber>> _transpose;
        private List<List<IR.RealNumber>> _matrix;
        #endregion
        #endregion

        #region Properties
        public List<List<IR.RealNumber>> M => _matrix;
        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;
        public bool IsSquareMatrix => RowCount == ColumnCount;
        public bool IsSymmetricMatrix
        {
            get
            {
                if (_isSymmetricMatrix.HasValue)
                    return _isSymmetricMatrix.Value;
                _isSymmetricMatrix = M == this.Transpose;
                return _isSymmetricMatrix.Value;
            }
        }
        public bool IsInverseExist
        {
            get
            {
                if (_isInverseExist.HasValue)
                    return _isInverseExist.Value;
                _isInverseExist = this.GetDeterminant() != 0;
                return _isInverseExist.Value;
            }
        }
        public List<List<IR.RealNumber>> Transpose
        {
            get
            {
                if (_transpose != null)
                    return _transpose;

                _transpose = this.GetTranspose().M;
                return _transpose;
            }
        }
        public RealMatrix IdentityMatrix
        {
            get
            {
                if (!IsSquareMatrix)
                    return null;
                if (_identityMatrix != null)
                    return _identityMatrix;

                return this.GetIdentityMatrix();
            }
        }
        public RealMatrix RowEchelonForm
        {
            get
            {
                if (_rowEchelonForm != null)
                    return _rowEchelonForm;

                _rowEchelonForm = this.GetRowEchelonForm();
                return _rowEchelonForm;
            }
        }
        public RealMatrix ReducedRowEchelonForm
        {
            get
            {
                if (_reducedRowEchelonForm != null)
                    return _reducedRowEchelonForm;

                _reducedRowEchelonForm = this.GetReducedRowEchelonForm();
                return _reducedRowEchelonForm;
            }
        }
        public IList<RealMatrix> ElementaryMatrices { get; set; }
        public List<RowVector> RowVectors
        {
            get
            {
                if (_rowVectors != null)
                    return _rowVectors;

                _rowVectors = this.GetRowVectors();
                return _rowVectors;
            }
        }
        public List<ColumnVector> ColumnVectors
        {
            get
            {
                if (_columnVectors != null)
                    return _columnVectors;

                _columnVectors = this.GetColumnVectors();
                return _columnVectors;
            }
        }

        //Augmented part
        public List<List<IR.RealNumber>> B { get; set; }
        public bool IsAugmentedMatrix { get; set; }
        public int AugmentedRowCount { get; set; }
        public int AugmentedColumnCount { get; set; }
        public bool IsInconsistent { get; set; }
        #endregion

        #region Addition and Scalar Multiplication

        public static RealMatrix operator +(RealMatrix a, RealMatrix b)
        {
            if (!(a.RowCount == b.RowCount && a.ColumnCount == b.ColumnCount))
                Console.WriteLine("Two matrix must have same size (m,n) for addition.");

            var result = new RealMatrix(a.RowCount, a.ColumnCount);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result.M[i][j] = a.M[i][j] + b.M[i][j];
                }
                if (a.IsAugmentedMatrix && b.IsAugmentedMatrix)
                {
                    result.B[0][i] = a.B[0][i] + b.B[0][i];
                }
            }
            return result;
        }

        public static RealMatrix operator *(RealMatrix a, IR.RealNumber c)
        {
            var result = new RealMatrix(a.RowCount, a.ColumnCount);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    result.M[i][j] = a.M[i][j] * c;
                }
                if (a.IsAugmentedMatrix)
                {
                    result.B[0][i] = a.B[0][i] * c;
                }
            }
            return result;
        }

        public static RealMatrix operator *(IR.RealNumber c, RealMatrix a)
        {
            return a * c;
        }

        public static bool operator ==(RealMatrix a, RealMatrix b)
        {
            if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                return false;

            bool result = true;

            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    if (a.M[i][j] != b.M[i][j])
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public static bool operator !=(RealMatrix a, RealMatrix b)
        {
            return !(a == b);
        }

        #endregion

        #region Methods

        public void MatrixElementsChanged()
        {
            _isSymmetricMatrix = null;
            _isInverseExist = null;
            _identityMatrix = null;
            _rowEchelonForm = null;
            _reducedRowEchelonForm = null;
            _rowVectors = null;
            _columnVectors = null;
            _transpose = null;
            _matrix = null;
        }

        public void MatrixSizeChanged()
        {
        }

        #endregion

    }

    #endregion

}
