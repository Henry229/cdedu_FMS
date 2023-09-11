using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface ICampusService
    {
        /// <summary>
        /// Gets a Campus by identifier
        /// </summary>
        /// <param name="campusID">Campus identifier</param>
        /// <returns>Setting</returns>
        Campus GetCampusById(int campusID);

        /// <summary>
        /// Gets a Campus by identifier
        /// </summary>
        /// <param name="campusID">Campus identifier</param>
        /// <returns>Setting</returns>
        //Campus GetCampusByCode(string campuscode);

        /// <summary>
        /// Deletes a campus
        /// </summary>
        /// <param name="campus">Campus</param>
        void InsertCampus(Campus campus);

        /// <summary>
        /// Deletes a campus
        /// </summary>
        /// <param name="campus">Campus</param>
        void DeleteCampus(Campus campus);

        /// <summary>
        /// Deletes a campus
        /// </summary>
        /// <param name="campus">Campus</param>
        void UpdateCampus(Campus campus);

        /*
        /// <summary>
        /// Deletes a campus
        /// </summary>
        /// <param name="campus">Campus</param>
        bool CheckDup(Campus campus);

        /// <summary>
        /// Deletes a campus
        /// </summary>
        /// <param name="campus">Campus</param>
        string GenerateCampusCode(string category);
        */

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        //IPagedList<Campus> GetAllCampus(string campusCategory, string campuscode, string campusname, string term, string grade, int pageIndex = 0, int pageSize = 2147483647);
        IPagedList<Campus> GetAllCampus(string title, int pageIndex = 0, int pageSize = 2147483647);


        IPagedList<Campus> GetAllCampus(int pageIndex = 0, int pageSize = 2147483647);

    }
}
