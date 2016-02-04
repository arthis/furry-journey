using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            var newUser = obj as Account;

            if (newUser != null) return Equals(newUser);

            return false;
        }

        public bool Equals(User u)
        {
            return Name == u.Name && Password == u.Password;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Id.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + Password.GetHashCode();
            return hash;
        }
    }
}
