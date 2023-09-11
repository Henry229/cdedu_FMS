using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface IItemService
    {
        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="itemID">Item identifier</param>
        /// <returns>Setting</returns>
        Item GetItemById(int itemID);

        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="itemID">Item identifier</param>
        /// <returns>Setting</returns>
        Item GetItemByCode(string itemcode);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">Item</param>
        void InsertItem(Item item);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">Item</param>
        void DeleteItem(Item item);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">Item</param>
        void UpdateItem(Item item);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">Item</param>
        bool CheckDup(Item item);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">Item</param>
        string GenerateItemCode(string category);


        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<Item> GetAllItems(string itemCategory, string itemcode, string itemname, string term, string grade, int pageIndex = 0, int pageSize = 2147483647);


        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<Item> GetTermItems(string term, string grade, int pageIndex = 0, int pageSize = 2147483647);



      
    }
}
