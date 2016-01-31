using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Core
{
    public interface ISpecification<TEntity>
    {
        bool IsSatisfiedBy(TEntity entity);
    }

    public interface IConditionnalSpecification<TEntity>
    {
        ISpecification<TEntity> Then(ISpecification<TEntity> thenSpecification);
    }

    internal class AndSpecification<TEntity> : ISpecification<TEntity>
    {
        private readonly ISpecification<TEntity> _spec1;
        private readonly ISpecification<TEntity> _spec2;

        protected ISpecification<TEntity> Spec1
        {
            get
            {
                return _spec1;
            }
        }

        protected ISpecification<TEntity> Spec2
        {
            get
            {
                return _spec2;
            }
        }

        internal AndSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            _spec1 = spec1;
            _spec2 = spec2;
        }

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
        }

    }

    internal class OrSpecification<TEntity> : ISpecification<TEntity>
    {
        private readonly ISpecification<TEntity> _spec1;
        private readonly ISpecification<TEntity> _spec2;

        protected ISpecification<TEntity> Spec1
        {
            get
            {
                return _spec1;
            }
        }

        protected ISpecification<TEntity> Spec2
        {
            get
            {
                return _spec2;
            }
        }

        internal OrSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            _spec1 = spec1;
            _spec2 = spec2;
        }

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
        }
    }

    internal class NotSpecification<TEntity> : ISpecification<TEntity>
    {
        private readonly ISpecification<TEntity> _wrapped;

        protected ISpecification<TEntity> Wrapped
        {
            get
            {
                return _wrapped;
            }
        }

        internal NotSpecification(ISpecification<TEntity> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException("spec");
            }

            _wrapped = spec;
        }

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return !Wrapped.IsSatisfiedBy(candidate);
        }
    }
    
    internal class AndIfSpecification<TEntity> : IConditionnalSpecification<TEntity>
   { 
        internal class ThenSpecification : ISpecification<TEntity>
        {
            
            private readonly ISpecification<TEntity> _andSpec;
            private readonly ISpecification<TEntity> _ifSpec;
            private readonly ISpecification<TEntity> _thenSpec;

            protected ISpecification<TEntity> Spec1
            {
                get
                {
                    return _andSpec;
                }
            }

            protected ISpecification<TEntity> Spec2
            {
                get
                {
                    return _ifSpec;
                }
            }

            protected ISpecification<TEntity> Spec3
            {
                get
                {
                    return _thenSpec;
                }
            }

            internal ThenSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2, ISpecification<TEntity> spec3)
            {
                if (spec1 == null)
                    throw new ArgumentNullException("spec1");

                if (spec2 == null)
                    throw new ArgumentNullException("spec2");

                if (spec3 == null)
                    throw new ArgumentNullException("spec3");

                _andSpec = spec1;
                _ifSpec = spec2;
                _thenSpec = spec3;
            }

            public bool IsSatisfiedBy(TEntity candidate)
            {
                if  (Spec2.IsSatisfiedBy(candidate))
                    return Spec1.IsSatisfiedBy(candidate) && Spec3.IsSatisfiedBy(candidate);

                return Spec1.IsSatisfiedBy(candidate);
            }
        }

        private readonly ISpecification<TEntity> _andSpec;
        private readonly ISpecification<TEntity> _ifWrapped;

        protected ISpecification<TEntity> AndSpec
        {
            get
            {
                return _andSpec;
            }
        }

        protected ISpecification<TEntity> IfWrapped
        {
            get
            {
                return _ifWrapped;
            }
        }

        internal AndIfSpecification(ISpecification<TEntity> andSpec, ISpecification<TEntity> ifSpec)
        {
            if (andSpec == null)
            {
                throw new ArgumentNullException("andspec");
            }
            if (ifSpec == null)
            {
                throw new ArgumentNullException("ifspec");
            }

            _andSpec = andSpec;
            _ifWrapped = ifSpec;
        }


        public ISpecification<TEntity> Then(ISpecification<TEntity> thenSpecification)
        {
            return new ThenSpecification( AndSpec, IfWrapped, thenSpecification);
        }

    }
    
    public static class ExtensionMethods
    {
        public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            return new AndSpecification<TEntity>(spec1, spec2);
        }

        public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            return new OrSpecification<TEntity>(spec1, spec2);
        }

        public static ISpecification<TEntity> Not<TEntity>(this ISpecification<TEntity> spec)
        {
            return new NotSpecification<TEntity>(spec);
        }

        public static IConditionnalSpecification<TEntity> AndIf<TEntity>(this ISpecification<TEntity> andSpec, ISpecification<TEntity> conditionnalSpec)
        {
            return new AndIfSpecification<TEntity>(andSpec, conditionnalSpec);
        }
    }
}
