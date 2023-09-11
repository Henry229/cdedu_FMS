using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface IItemSetService
    {
        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="itemsetID">Itemset identifier</param>
        /// <returns>Setting</returns>
        ItemSet GetItemSetById(int itemsetID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="itemset">Itemset</param>
        void InsertItemSet(ItemSet itemset);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="itemset">Itemset</param>
        void DeleteItemSet(ItemSet itemset);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="itemset">Itemset</param>
        void UpdateItemSet(ItemSet itemset);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="itemset">Itemset</param>
        void InsertItemSet_D(ItemSet_D itemset_d);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="itemset">Itemset</param>
        void DeleteItemSet_D(ItemSet_D itemset_d);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="itemset">Itemset</param>
        void UpdateItemSet_D(ItemSet_D itemset_d);


        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="itemsetID">Itemset identifier</param>
        /// <returns>Setting</returns>
        ItemSet_D GetItemSet_DById(int itemsetdID);


        /// <summary>
        /// Gets all itemset
        /// </summary>
        /// <returns>itemsets</returns>
        IPagedList<ItemSet> GetAllItemSets(string term, string grade, string setcategory, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all itemset
        /// </summary>
        /// <returns>itemsets</returns>
        IPagedList<ItemSet_D> GetAllItemSet_Ds(ItemSet itemset, int pageIndex = 0, int pageSize = 2147483647);

    }
}
