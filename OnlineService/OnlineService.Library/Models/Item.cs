using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineService.Library
{
    // This will be a derived class for all
    // items/products in an inventory
    public class Item
    {
        private int _id;
        private SecurityLevel _securityLevel;
        private string _name;
        private decimal _price;

        public decimal Price {
            get
            {
                return _price;
            }

            set
            {
               _price = value;
            }
        }
        public int Id { get { return _id; } set { _id = value; } }
        [Display(Name = "Name")]
        public string Name { get { return _name; } set { _name = value; } }
        public SecurityLevel SecurityLevel { get { return _securityLevel; } set { _securityLevel = value; } }

        public Item() : this(-1, SecurityLevel.NONE)
        {
        }

        public Item(int id, SecurityLevel securityLevel) : this(id, securityLevel, "default")
        {
            
        }

        public Item(int id, SecurityLevel securityLevel, string name)
        {
            _id = id;
            _securityLevel = securityLevel;
            _name = name;
            if (id > -1 && !Equals(name, "default"))
            {
                BusinessInformation.intItemList.TryAdd(id, this);
                BusinessInformation.stringItemIdList.TryAdd(name, id);
            }
        }

        /// <summary>
        /// Get the item id
        /// </summary>
        /// <returns>Item Id</returns>
        public int GetId()
        {
            return _id;
        }

        /// <summary>
        /// Get the required security access of the item
        /// </summary>
        /// <returns>SecurityLevel Object</returns>
        public SecurityLevel GetSecurityLevel()
        {
            return _securityLevel;
        }

        /// <summary>
        /// Get the name of the item
        /// </summary>
        /// <returns>string object</returns>
        public string GetName()
        {
            return _name;
        }
    }
}
