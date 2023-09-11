using System.Collections.Generic;
using System.IO;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Services.Omni_Backoffice
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface IPdfService_Class
    {

        /// <summary>
        /// Print product collection to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="products">Products</param>
        void PrintReceiptToPdf(Stream stream, int Id_Payment);

        void PrintRollcallSheetToPdf(Stream stream, int id_class);

        void PrintEnrolmentSheetToPdf(Stream stream, int id_class);

        void PrintNameSheetToPdf(Stream stream, int id_class);

        void PrintClassTestSheetToPdf(Stream stream, int id_class);

    }
}