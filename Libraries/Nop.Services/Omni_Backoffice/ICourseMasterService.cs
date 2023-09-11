using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface ICourseMasterService{
        /// <summary>
        /// Gets a CourseMaster by identifier
        /// </summary>
        /// <param name="coursemasterID">CourseMaster identifier</param>
        /// <returns>Setting</returns>
        CourseMaster GetCourseMasterById(int coursemasterID);

        /// <summary>
        /// Deletes a coursemaster
        /// </summary>
        /// <param name="coursemaster">CourseMaster</param>
        void DeleteCourseMaster(CourseMaster coursemaster);


        void InsertCourseMaster(CourseMaster coursemaster);


        void UpdateCourseMaster(CourseMaster coursemaster);


        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<CourseMaster> GetAllCourseMasters(string year, string term, string coursecategory, string grade, int pageIndex = 0, int pageSize = 2147483647);

        IPagedList<CourseMaster> GetAllCourseMasters(int pageIndex = 0, int pageSize = 2147483647);

    }

}
