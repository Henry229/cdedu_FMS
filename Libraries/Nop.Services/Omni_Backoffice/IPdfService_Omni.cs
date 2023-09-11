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
    public partial interface IPdfService_Omni
    {

        /// <summary>
        /// Print product collection to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="products">Products</param>
        void PrintMaterialInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId);

        void PrintRoyaltyInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId);
        void PrintTestfeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId);

        void PrintSupplefeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId);

        void PrintPAfeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId);

        void PrintItemfeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId);

        void PrintMarketingfeeInvoiceToPdf(Stream stream, string branchcode, int languageId);


        void PrintingInvoice(Stream stream,  int reqid);
    }
}