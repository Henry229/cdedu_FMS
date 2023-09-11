using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface IItemCategoryService
    {
        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="itemcategoryID">ItemCategory identifier</param>
        /// <returns>Setting</returns>
        ItemCategory GetItemCategoryById(int itemcategoryID);

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="itemcategory">ItemCategory</param>
        void DeleteItemCategory(ItemCategory itemcategory);


        void InsertItemCategory(ItemCategory itemcategory);

        void UpdateItemCategory(ItemCategory itemcategory);

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<ItemCategory> GetAllItemCategorys(int pageIndex = 0, int pageSize = 2147483647);


    }


}
