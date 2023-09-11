using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface ICodeMasterService{
        /// <summary>
        /// Gets a CodeMaster by identifier
        /// </summary>
        /// <param name="codemasterID">CodeMaster identifier</param>
        /// <returns>Setting</returns>
        CodeMaster GetCodeMasterById(int codemasterID);

        /// <summary>
        /// Deletes a codemaster
        /// </summary>
        /// <param name="codemaster">CodeMaster</param>
        void DeleteCodeMaster(CodeMaster codemaster);


        void InsertCodeMaster(CodeMaster codemaster);


        void UpdateCodeMaster(CodeMaster codemaster);


        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<CodeMaster> GetAllCodeMasters(string yn_use = "Y", int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IPagedList<CodeMaster> GetAllCodeMasters(string typecode, string yn_use = "Y", int pageIndex = 0, int pageSize = 2147483647);

        IPagedList<Campus> GetAllCampus(int pageIndex = 0, int pageSize = 2147483647);




    }

}
