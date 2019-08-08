using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineService.Library
{
    /// <summary>
    /// Business Information holds static variables that will be used by multiple classes
    /// </summary>
    public static class BusinessInformation
    {
        /// <summary>
        /// This static object will hold all items by Id.
        /// I think it's dumb that I can't declare it outside a class object though
        /// </summary>
        public static Dictionary<int, Item> intItemList = new Dictionary<int, Item>();

        public static Dictionary<string, int> stringItemIdList = new Dictionary<string, int>();
        /// <summary>
        /// Keeps track of how many orders there are
        /// </summary>
        public static int OrderCount = 0;
    }
}
