using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface ICalendarMasterService
    {
        /// <summary>
        /// Gets a CalendarMaster by identifier
        /// </summary>
        /// <param name="calendarmasterID">CalendarMaster identifier</param>
        /// <returns>Setting</returns>
        CalendarMaster GetCalendarMasterById(int calendarmasterID);

        /// <summary>
        /// Deletes a calendarmaster
        /// </summary>
        /// <param name="calendarmaster">CalendarMaster</param>
        void DeleteCalendarMaster(CalendarMaster calendarmaster);


        void InsertCalendarMaster(CalendarMaster calendarmaster);


        void UpdateCalendarMaster(CalendarMaster calendarmaster);


        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<CalendarMaster> GetAllCalendarMasters(int pageIndex = 0, int pageSize = 2147483647);

        string GetCurrentWeek();

    }

}
