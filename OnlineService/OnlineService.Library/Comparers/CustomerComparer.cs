using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineService.Library.Comparers
{
    public class CustomerComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer x, Customer y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Customer obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
