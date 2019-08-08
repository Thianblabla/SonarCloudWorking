using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineService.Library
{
    public class Customer
    {
        private string first;
        private string last;
        private SecurityLevel accessLevel = SecurityLevel.NONE;
        [Display(Name = "First Name")]
        public string FirstName { get { return first; } set { if (value != null) first = value; } }
        [Display(Name = "Last Name")]
        public string LastName { get { return last; } set { if (value != null) last = value; } }

        public SecurityLevel Security { get { return accessLevel; } set { accessLevel = value; } }

        public List<Order> PastOrders { get; set; }

        public int Id { get; set; }
        [Display(Name = "Customer Name")]
        public string FullName { get { return FirstName + " " + LastName; } }


        public Customer() : this("John", "Doe")
        {

        }

        public Customer(string _first, string _last) : this(_first, _last, new List<Order>())
        {
            
        }
        public Customer(string _first, string _last, List<Order> order)
        {
            FirstName = _first;
            LastName = _last;
            PastOrders = order;
        }

        public Customer(string _first, string _last, List<Order> order, SecurityLevel _accessLevel) : this (_first, _last, order)
        {
            Security = _accessLevel;
        }

    }
}
