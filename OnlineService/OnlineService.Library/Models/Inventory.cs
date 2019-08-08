using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineService.Library
{
    public class Inventory
    {
        readonly private Dictionary<int, uint> itemCounts;

        public Inventory() : this(new Dictionary<int, uint>())
        {

        }

        public Inventory(Dictionary<int, uint> items)
        {
            itemCounts = items;
        }

        /// <summary>
        /// // Remove items from stock if the order count is less than or equal to
        /// the number in stock
        /// </summary>
        /// <param name="_ordered"></param>
        /// <param name="access"></param>
        /// <returns>Number ordered upon success,ArgumentOutOfRange, KeyNotFound or AccessViolation on failure</returns>
        public uint OrderItems(int id, uint _ordered, SecurityLevel access)
        {
            if (itemCounts.ContainsKey(id))
            {
                if (access.HasFlag(BusinessInformation.intItemList[id].GetSecurityLevel()))
                {
                    uint amount = GetCount(id);
                    if (_ordered <= amount)
                    {
                        SetCount(id, amount - _ordered);
                        return _ordered;
                    }
                    throw new ArgumentOutOfRangeException("There is not enough of this item in stock!");
                }
                throw new AccessViolationException("You cannot purchase this item, since you do not have security clearance!");
            }
            throw new KeyNotFoundException("This item is not sold at this location!");
        }

        void SetCount(int id, uint count)
        {
            if (itemCounts.ContainsKey(id))
            {
                itemCounts[id] = count;
                return;
            }
            itemCounts.Add(id, count);
        }

        /// <summary>
        /// Check how many items are in stock
        /// </summary>
        /// <returns>Count of items in stock</returns>
        public uint GetCount(int id)
        {
            if (itemCounts.ContainsKey(id))
            {
                return itemCounts[id];
            }
            throw new KeyNotFoundException("There is no item with that Id in this location!");
        }

        // Add to the total count for this item in stock
        public void AddItems(int id, uint count)
        {
            if (itemCounts.TryAdd(id, count))
            {
                
            }
            else
            {
                itemCounts[id] += count;
            }
        }

        public Dictionary<int, uint> GetContents()
        {
            return itemCounts;
        }
    }
}
