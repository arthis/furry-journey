using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Core
{
    public interface ISpecification<TEntity,TResult>
    {
        bool IsSatisfiedBy(TEntity entity, TResult visitorResult);
    }

    public interface IConditionnalSpecification<TEntity, TResult>
    {
        ISpecification<TEntity, TResult> Then(ISpecification<TEntity,TResult> thenSpecification);
    }

    internal class AndSpecification<TEntity, TResult> : ISpecification<TEntity, TResult>
    {
        private readonly ISpecification<TEntity, TResult> _spec1;
        private readonly ISpecification<TEntity, TResult> _spec2;

        protected ISpecification<TEntity, TResult> Spec1
        {
            get
            {
                return _spec1;
            }
        }

        protected ISpecification<TEntity, TResult> Spec2
        {
            get
            {
                return _spec2;
            }
        }

        internal AndSpecification(ISpecification<TEntity, TResult> spec1, ISpecification<TEntity, TResult> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            _spec1 = spec1;
            _spec2 = spec2;
        }

        public bool IsSatisfiedBy(TEntity candidate, TResult visitorResult)
        {
            return Spec1.IsSatisfiedBy(candidate, visitorResult) && Spec2.IsSatisfiedBy(candidate, visitorResult);
        }

    }

    internal class OrSpecification<TEntity,TResult> : ISpecification<TEntity, TResult>
    {
        private readonly ISpecification<TEntity, TResult> _spec1;
        private readonly ISpecification<TEntity, TResult> _spec2;

        protected ISpecification<TEntity, TResult> Spec1
        {
            get
            {
                return _spec1;
            }
        }

        protected ISpecification<TEntity, TResult> Spec2
        {
            get
            {
                return _spec2;
            }
        }

        internal OrSpecification(ISpecification<TEntity, TResult> spec1, ISpecification<TEntity, TResult> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            _spec1 = spec1;
            _spec2 = spec2;
        }

        public bool IsSatisfiedBy(TEntity candidate, TResult visitorResult)
        {
            return Spec1.IsSatisfiedBy(candidate,visitorResult) || Spec2.IsSatisfiedBy(candidate,visitorResult);
        }
    }

    internal class NotSpecification<TEntity, TResult> : ISpecification<TEntity, TResult>
    {
        private readonly ISpecification<TEntity, TResult> _wrapped;

        protected ISpecification<TEntity, TResult> Wrapped
        {
            get
            {
                return _wrapped;
            }
        }

        internal NotSpecification(ISpecification<TEntity, TResult> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException("spec");
            }

            _wrapped = spec;
        }

        public bool IsSatisfiedBy(TEntity candidate, TResult visitorResult)
        {
            return !Wrapped.IsSatisfiedBy(candidate,visitorResult);
        }
    }
    
    internal class AndIfSpecification<TEntity, TResult> : IConditionnalSpecification<TEntity, TResult>
   { 
        internal class ThenSpecification : ISpecification<TEntity, TResult>
        {
            
            private readonly ISpecification<TEntity, TResult> _andSpec;
            private readonly ISpecification<TEntity, TResult> _ifSpec;
            private readonly ISpecification<TEntity, TResult> _thenSpec;

            protected ISpecification<TEntity, TResult> Spec1
            {
                get
                {
                    return _andSpec;
                }
            }

            protected ISpecification<TEntity, TResult> Spec2
            {
                get
                {
                    return _ifSpec;
                }
            }

            protected ISpecification<TEntity, TResult> Spec3
            {
                get
                {
                    return _thenSpec;
                }
            }

            internal ThenSpecification(ISpecification<TEntity, TResult> spec1, ISpecification<TEntity, TResult> spec2, ISpecification<TEntity, TResult> spec3)
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

            public bool IsSatisfiedBy(TEntity candidate, TResult visitorResult)
            {
                if  (Spec2.IsSatisfiedBy(candidate, visitorResult))
                    return Spec1.IsSatisfiedBy(candidate, visitorResult) && Spec3.IsSatisfiedBy(candidate, visitorResult);

                return Spec1.IsSatisfiedBy(candidate, visitorResult);
            }
        }

        private readonly ISpecification<TEntity, TResult> _andSpec;
        private readonly ISpecification<TEntity, TResult> _ifWrapped;

        protected ISpecification<TEntity, TResult> AndSpec
        {
            get
            {
                return _andSpec;
            }
        }

        protected ISpecification<TEntity, TResult> IfWrapped
        {
            get
            {
                return _ifWrapped;
            }
        }

        internal AndIfSpecification(ISpecification<TEntity, TResult> andSpec, ISpecification<TEntity, TResult> ifSpec)
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


        public ISpecification<TEntity, TResult> Then(ISpecification<TEntity, TResult> thenSpecification)
        {
            return new ThenSpecification( AndSpec, IfWrapped, thenSpecification);
        }

    }
    
    public static class ExtensionMethods
    {
        public static ISpecification<TEntity,TResult> And<TEntity,TResult>(this ISpecification<TEntity,TResult> spec1, ISpecification<TEntity, TResult> spec2)
        {
            return new AndSpecification<TEntity, TResult>(spec1, spec2);
        }

        public static ISpecification<TEntity,TResult> Or<TEntity,TResult>(this ISpecification<TEntity, TResult> spec1, ISpecification<TEntity, TResult> spec2)
        {
            return new OrSpecification<TEntity, TResult>(spec1, spec2);
        }

        public static ISpecification<TEntity,TResult> Not<TEntity,TResult>(this ISpecification<TEntity, TResult> spec)
        {
            return new NotSpecification<TEntity, TResult>(spec);
        }

        public static IConditionnalSpecification<TEntity, TResult> AndIf<TEntity,TResult>(this ISpecification<TEntity, TResult> andSpec, ISpecification<TEntity, TResult> conditionnalSpec)
        {
            return new AndIfSpecification<TEntity, TResult>(andSpec, conditionnalSpec);
        }
    }
}
