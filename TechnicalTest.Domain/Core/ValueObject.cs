using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Core
{
    
    public class ValueObject<T> 
    {
        private T _value;

        
        public ValueObject(T value)
        {
            if (value== null) throw new ArgumentNullException("value cannot be null");


            _value = value;
        }

        public override bool Equals(object obj)
        {
            var s = obj as string;
            if (s != null) return Equals(s);

            var other = obj as ValueObject<T>;
            if (other  != null) return Equals(other);

            return false;
        }

        public bool Equals(ValueObject<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public static bool operator == (ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (((object)a == null) || ((object)b == null)) return false;
            return EqualityComparer<T>.Default.Equals(a._value,b._value);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + _value.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
