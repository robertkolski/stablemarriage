using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stablemarriage.core
{
    public class PersonComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            if (string.Equals(x.FirstName, y.FirstName, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(x.LastName, y.LastName, StringComparison.InvariantCultureIgnoreCase)
                &&
                x.Gender == y.Gender)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(Person obj)
        {
            return obj.FirstName.GetHashCode() ^ obj.LastName.GetHashCode();
        }

        private static PersonComparer instance = new PersonComparer();
        public static PersonComparer Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
