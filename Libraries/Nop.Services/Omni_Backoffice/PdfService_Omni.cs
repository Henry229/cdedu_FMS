using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core.Domain.Omni_Printing;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Core.Html;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Services.Omni_Backoffice;
using Nop.Services.Omni_Printing;
using Nop.Services.Customers;

namespace Nop.Services.Omni_Backoffice
{
    /// <summary>
    /// PDF service
    /// </summary>
    public partial class PdfService_Omni : IPdfService_Omni
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IPaymentService _paymentService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ICurrencyService _currencyService;
        private readonly IMeasureService _measureService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingContext;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerService _customerService;


        private readonly ICodeMasterService _codemasterService;
        private readonly IOrderService_Omni _orderservice;
        private readonly IItemService _itemservice;
        private readonly ICourseMasterService _coursemasterService;
        private readonly IItemSetService _itemsetService;
        private readonly ICampusService _campusService;
        private readonly IRoyaltyService _royaltyService;
        private readonly IPrintService _printService;




        private readonly CatalogSettings _catalogSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly MeasureSettings _measureSettings;
        private readonly PdfSettings _pdfSettings;
        private readonly TaxSettings _taxSettings;
        private readonly AddressSettings _addressSettings;

        #endregion

        #region Ctor

        public PdfService_Omni(ILocalizationService localizationService,
            ILanguageService languageService,
            IWorkContext workContext,
            IOrderService orderService,
            IPaymentService paymentService,
            IDateTimeHelper dateTimeHelper,
            IPriceFormatter priceFormatter,
            ICurrencyService currencyService,
            IMeasureService measureService,
            IPictureService pictureService,
            IProductService productService,
            IProductAttributeParser productAttributeParser,
            IStoreService storeService,
            IStoreContext storeContext,
            ISettingService settingContext,
            IWebHelper webHelper,
            ICustomerService customerservice,

            ICodeMasterService codemasterService,
            IItemService itemService,
            IOrderService_Omni orderservice,
            ICourseMasterService coursemasterService,
            IItemSetService itemsetService,
            ICampusService campusService,
            IRoyaltyService royaltyService,
            IPrintService printService,

            CatalogSettings catalogSettings,
            CurrencySettings currencySettings,
            MeasureSettings measureSettings,
            PdfSettings pdfSettings,
            TaxSettings taxSettings,
            AddressSettings addressSettings)
        {
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._workContext = workContext;
            this._paymentService = paymentService;
            this._dateTimeHelper = dateTimeHelper;
            this._priceFormatter = priceFormatter;
            this._currencyService = currencyService;
            this._measureService = measureService;
            this._pictureService = pictureService;
            this._productService = productService;
            this._productAttributeParser = productAttributeParser;
            this._storeService = storeService;
            this._storeContext = storeContext;
            this._settingContext = settingContext;
            this._webHelper = webHelper;
            this._currencySettings = currencySettings;
            this._catalogSettings = catalogSettings;
            this._measureSettings = measureSettings;
            this._pdfSettings = pdfSettings;
            this._taxSettings = taxSettings;
            this._addressSettings = addressSettings;
            this._customerService = customerservice;

            this._codemasterService = codemasterService;
            this._orderservice = orderservice;
            this._itemservice = itemService;
            this._coursemasterService = coursemasterService;
            this._itemsetService = itemsetService;
            this._campusService = campusService;
            this._royaltyService = royaltyService;
            this._printService = printService;
        }

        #endregion

        #region Utilities

        protected virtual Font GetFont()
        {
            //nopCommerce supports unicode characters
            //nopCommerce uses Free Serif font by default (~/App_Data/Pdf/FreeSerif.ttf file)
            //It was downloaded from http://savannah.gnu.org/projects/freefont
            string fontPath = Path.Combine(_webHelper.MapPath("~/App_Data/Pdf/"), _pdfSettings.FontFileName);
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(baseFont, 10, Font.NORMAL);
            return font;
        }

        #endregion

        #region Methods

        private void MakeCompanyHeader(ref Document doc, IPagedList<CodeMaster> companyinfo, PdfSettings pdfSettingsByStore
            , Font titleFont, Font titleFont_B, Font font)
        {
            // 향후 회사정보 넣는 테이블 만들어서 가져오도록 하자.
            var titleTable = new PdfPTable(2);
            titleTable.WidthPercentage = 100f;
            titleTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //logo
            var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);

            titleTable.SetWidths(new[] { 30, 70 });
            var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
            Image logo = Image.GetInstance(logoFilePath);
            logo.ScalePercent(12f);
            var cellLogo = new PdfPCell(logo);
            cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
            cellLogo.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellLogo.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cellLogo);


            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "0000").ToList()[0].String_01, titleFont_B));
            cell.AddElement(new Phrase("ABN : " + companyinfo.Where(x => x.DetailCode == "0000").ToList()[0].String_02, titleFont));
            titleTable.AddCell(cell);

            var cell2 = new PdfPCell();
            cell2.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cell2);

            var cell3 = new PdfPCell();
            cell3.Border = Rectangle.NO_BORDER;
            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            var phrase1 = new Paragraph(companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_01, font);
            phrase1.Alignment = Element.ALIGN_RIGHT;
            cell3.AddElement(phrase1);
            var phrase2 = new Paragraph(companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_02, font);
            phrase2.Alignment = Element.ALIGN_RIGHT;
            cell3.AddElement(phrase2);
            var phrase3 = new Paragraph(companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_03, font);
            phrase3.Alignment = Element.ALIGN_RIGHT;
            cell3.AddElement(phrase3);
            cell3.AddElement(new Phrase(" ", font));

            titleTable.AddCell(cell3);

            doc.Add(titleTable);

        }


        private void MakeHeaderTo(ref Document doc, string termstring, string termstring_short, string year, string term, string branchcode
            , Font titleFont, Font font, Core.Domain.Localization.Language lang, Closing closings)
        {

            //header
            var headerTable = new PdfPTable(4);
            headerTable.WidthPercentage = 100f;
            headerTable.SetWidths(new[] { 18, 35, 15, 32 });
            headerTable.HorizontalAlignment = Element.ALIGN_LEFT;

            var store = _storeContext.CurrentStore;
            var campuslist = _codemasterService.GetAllCampus().Where(c => c.Campus_Prefix == branchcode);
            Campus campus = campuslist.ToList()[0];
            var InvoiceNo = campus.Campus_EN
                + year.Substring(2, 2) + termstring_short
                + closings.Type.Substring(0, 1) + closings.SEQ.ToString("00");

            //store info
            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddElement(new Phrase("Period : ", titleFont));
            cell.AddElement(new Phrase("To : ", font));
            cell.AddElement(new Phrase("ABN : ", font));
            cell.AddElement(new Phrase("Invoice No. : ", font));
            headerTable.AddCell(cell);


            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddElement(new Phrase(termstring + "-" + year, titleFont));
            cell.AddElement(new Phrase("CS Education," + campus.Campus_Title.Replace("CS", ""), font));
            cell.AddElement(new Phrase(campus.ABN, font));
            cell.AddElement(new Phrase(InvoiceNo, font));
            headerTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddElement(new Phrase( " ", titleFont));
            cell.AddElement(new Phrase("Invoice Date : ", font));
            cell.AddElement(new Phrase("Due Date : ", font));
            cell.AddElement(new Phrase("", font));
            headerTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.AddElement(new Phrase(" ", titleFont));
            cell.AddElement(new Phrase(DateTime.Now.ToString("dd/MM/yyyy"), font));

            string duedate = "Immediate";

            if ( !String.IsNullOrEmpty(closings.DueDate))
            {
                duedate = closings.DueDate;
            }


            cell.AddElement(new Phrase(duedate, font));
            cell.AddElement(new Phrase("", font));
            headerTable.AddCell(cell);


            cell.AddElement(new Phrase("Period : " + termstring + "-" + year, titleFont));

          



            cell.AddElement(new Phrase("To : CS Education," + campus.Campus_Title.Replace("CS", ""), font));
            cell.AddElement(new Phrase("ABN : " + campus.ABN, font));
            cell.AddElement(new Phrase("Invoice No. : " + InvoiceNo, font));
            cell.AddElement(new Phrase(String.Format(_localizationService.GetResource("PDFInvoice.OrderDate", lang.Id), _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Local).ToString("D", new CultureInfo(lang.LanguageCulture))), font));
            cell.AddElement(new Phrase("DUE : immediate", font));

            headerTable.AddCell(cell);
            doc.Add(headerTable);

        }

        
        private void MakeRemmittance(ref Document doc, IPagedList<CodeMaster> companyinfo, Font titleFont, Font font)
        {

            var premmittance = new Paragraph("Remittance To ", titleFont);
            premmittance.Alignment = Element.ALIGN_LEFT;
            doc.Add(premmittance);


            var remmitTable = new PdfPTable(4);
            remmitTable.WidthPercentage = 100f;
            remmitTable.SetWidths(new[] { 18, 35, 15, 32 });
            remmitTable.HorizontalAlignment = Element.ALIGN_LEFT;


            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase("Cheque Payable :", font));
            cell.AddElement(new Phrase("Bank Deposit :", font));
            cell.AddElement(new Phrase(" ", font));

            //cell.AddElement(new Phrase("Contact :", font));
            remmitTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "2000").ToList()[0].String_01, font));
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "3000").ToList()[0].String_01 + ", "
                + companyinfo.Where(x => x.DetailCode == "3000").ToList()[0].String_02, font));
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "3000").ToList()[0].String_03, font));
            //cell.AddElement(new Phrase("Amy 0414 322688", font));
            remmitTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase("Post to :", font));
            cell.AddElement(new Phrase("Email to :", font));
            //cell.AddElement(new Phrase("Contact :", font));
            remmitTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_01 + " "
                + companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_02, font));
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "4000").ToList()[0].String_01, font));
            //cell.AddElement(new Phrase("Amy 0414 322688", font));
            remmitTable.AddCell(cell);


            doc.Add(remmitTable);

        }

        //계좌번호 생성 - 3100
        private void MakeRemmittance_3100(ref Document doc, IPagedList<CodeMaster> companyinfo, Font titleFont, Font font)
        {

            var premmittance = new Paragraph("Remittance To ", titleFont);
            premmittance.Alignment = Element.ALIGN_LEFT;
            doc.Add(premmittance);


            var remmitTable = new PdfPTable(4);
            remmitTable.WidthPercentage = 100f;
            remmitTable.SetWidths(new[] { 18, 35, 15, 32 });
            remmitTable.HorizontalAlignment = Element.ALIGN_LEFT;


            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase("Cheque Payable :", font));
            cell.AddElement(new Phrase("Bank Deposit :", font));
            cell.AddElement(new Phrase(" ", font));

            //cell.AddElement(new Phrase("Contact :", font));
            remmitTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "2000").ToList()[0].String_01, font));
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "3100").ToList()[0].String_01 + ", "
                + companyinfo.Where(x => x.DetailCode == "3100").ToList()[0].String_02, font));
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "3100").ToList()[0].String_03, font));
            //cell.AddElement(new Phrase("Amy 0414 322688", font));
            remmitTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase("Post to :", font));
            cell.AddElement(new Phrase("Email to :", font));
            //cell.AddElement(new Phrase("Contact :", font));
            remmitTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_01 + " "
                + companyinfo.Where(x => x.DetailCode == "1000").ToList()[0].String_02, font));
            cell.AddElement(new Phrase(companyinfo.Where(x => x.DetailCode == "4000").ToList()[0].String_01, font));
            //cell.AddElement(new Phrase("Amy 0414 322688", font));
            remmitTable.AddCell(cell);


            doc.Add(remmitTable);

        }


        private void MakeWatermark(ref Document doc, ref PdfWriter writer )
        {
            float fontSize = 80;
            float xPosition = iTextSharp.text.PageSize.A4.Width / 2;
            float yPosition = (iTextSharp.text.PageSize.A4.Height - 140f) / 2;
            float angle = 45;
            try
            {
                PdfContentByte under = writer.DirectContentUnder;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                under.BeginText();
                under.SetColorFill(Color.LIGHT_GRAY);
                under.SetFontAndSize(baseFont, fontSize);
                under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "NOT APPROVED", xPosition, yPosition, angle);
                under.EndText();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }


            
        }

        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintMaterialInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 20f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].String_01;
            var tempGrade = "";
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;


            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);

            var closings = _royaltyService.GetClosing(year, term, "BOOK", branchcode);
            MakeHeaderTo(ref doc, termstring, termstring_short, year, term, branchcode
            , titleFont, font,  lang, closings);


            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }


            // 데이터 로드

            var orders = _orderservice.GetAllOrders(branchcode, year, term, "GROP", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var courses = _coursemasterService.GetAllCourseMasters();
            var ordersets = _orderservice.GetAllOrderbyItemSets();
            var itemsets = _itemsetService.GetAllItemSets(term, "", "");

            var discount = _codemasterService.GetAllCodeMasters("OR03", "Y");
            var coursetype = _codemasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "NORMAL");  // 일반코스인것만 카운트

            

            var teacherdiscount = discount.Where( x => x.DetailCode == "TCHR").ToList()[0].Val_01;
            var incentivediscount = discount.Where( x => x.DetailCode == "STUD").ToList()[0].Val_01;
            var onlinefacility = discount.Where(x => x.DetailCode == "STUD").ToList()[0].Val_02;  // 처음엔 기준정보에서 가져왔으나 코스마다 따로 설정으로 변경

            var bookorders = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Qty_T = y.Qty_Teacher, Qty_N =y.Qty_NewBook, isHalf = y.is_Half, CourseId = y.Course_Id })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.CourseId })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Qty = group.Sum(a => a.isHalf == "N" ? a.Qty : 0),
                    Qty_H = group.Sum(a => a.isHalf == "Y" ? a.Qty : 0),
                    Qty_T = group.Sum(a => a.Qty_T),
                    Qty_N = group.Sum(a => a.Qty_N)
                })
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    BranchCode = x.Branch,
                    CourseType = y.CourseCategory,
                    Year = x.Year,
                    Term = x.Term,
                    Course_Id = x.Course_Id,
                    Course_Name = y.CourseName,
                    MaterialFee = y.BookFee,
                    NewBookFee = y.NewBookFee,
                    Qty_Student = x.Qty,
                    Qty_Half = x.Qty_H,
                    Qty_Teacher = x.Qty_T,
                    Qty_NewBook = x.Qty_N,
                    Grade = y.Grade,
                    Subject = (y.CourseCategory == "HIGH") ? y.Level : "" ,    // 하이스쿨은 레벨이 과목역할을 하니까 따로 분리한다 과목필드를 만들껄 그랬나.. 싶기도하다
                    Facility = (y.CourseCategory == "HIGH") ? 0 : y.Facility // 하이스쿨은 온라인 없음
                })
                .Join(coursetype, x => x.CourseType, y => y.DetailCode, (x, y)
                    => new { x.BranchCode, x.CourseType, x.Year, x.Term, x.Course_Id, x.Course_Name, x.MaterialFee, x.Qty_Student, x.Qty_Half, x.Qty_Teacher, x.Qty_NewBook, x.Grade, x.Subject, x.Facility, x.NewBookFee })
                .GroupBy(group => new { group.Year, group.Term, group.Grade, group.CourseType, group.BranchCode, group.MaterialFee, group.Subject, group.Facility, group.NewBookFee })
                .Select(group => new
                {
                    BranchCode = group.Key.BranchCode,
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    MaterialFee = group.Key.MaterialFee,
                    Qty_Student = group.Sum(a => a.Qty_Student),
                    Qty_Half = group.Sum(a => a.Qty_Half),
                    Qty_Teacher = group.Sum(a => a.Qty_Teacher),
                    Qty_NewBook = group.Sum(a => a.Qty_NewBook),
                    Course_Name = group.Max(a => a.Course_Name).Split(' ')[0] + " " + group.Max(a => a.Course_Name).Split(' ')[1],
                    Facility = group.Key.Facility,
                    NewBookFee = group.Key.NewBookFee,
                    CourseType = group.Key.CourseType
                })
                .OrderBy(x => x.BranchCode)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.Course_Name).ToList();




            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : Material fee", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(9);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 20, 20, 50, 20, 20, 20, 30, 20, 30 });

            //Date
            cell = new PdfPCell( new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Grade
            cell = new PdfPCell(new Phrase("Grade", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Course Description
            cell = new PdfPCell(new Phrase("Course Description", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //qty
            cell = new PdfPCell(new Phrase("Unit Qty", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Unit Fee", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Online"+System.Environment.NewLine+"Facilities", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Subtotal
            cell = new PdfPCell(new Phrase("Sub Total", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Discount
            cell = new PdfPCell(new Phrase("Discount", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //total
            cell = new PdfPCell(new Phrase("Total" + System.Environment.NewLine + "(Inc. GST)", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;
            decimal student = 0;
            decimal teacher = 0;

            for (int i = 0; i < bookorders.Count; i++)
            {
                var orderItem = bookorders[i];

                //Date 
                string date = termstring + System.Environment.NewLine + year;
                if (i != 0) date = "";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);

                //grade
                string grade = (tempGrade == orderItem.Grade ? "" : orderItem.Course_Name.Split(' ')[0]);
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(grade, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph2 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph2);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);
                tempGrade = orderItem.Grade;

                //product name
                string name = orderItem.Course_Name.Replace(orderItem.Course_Name.Split(' ')[0], "") + " Material Fee";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);

                //qty
                string qty = null;
                if (orderItem.Qty_Teacher > 0)
                {
                    qty = (orderItem.Qty_Student + orderItem.Qty_Half).ToString() + "(" + orderItem.Qty_Teacher + ")" + "(" + orderItem.Qty_NewBook + ")";
                }
                else
                {
                    qty = (orderItem.Qty_Student + orderItem.Qty_Half).ToString() + "(" + orderItem.Qty_Teacher + ")";
                }

                cell = new PdfPCell(new Phrase(qty, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(orderItem.MaterialFee - orderItem.Facility, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //price
                string unitPrice2 = string.Empty;
                unitPrice2 = _priceFormatter.FormatPrice(orderItem.Facility, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice2, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //subtotal
                string subTotal = string.Empty;
                decimal subtotal_val = Math.Round((((orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Student) + (orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Half * Convert.ToDecimal(0.5)
                                        + (orderItem.NewBookFee * orderItem.Qty_NewBook)
                                        + (orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Teacher * teacherdiscount / 100), 2);
                subTotal = _priceFormatter.FormatPrice(subtotal_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(subTotal, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                // incentive
                string incentive = string.Empty;
                decimal incentive_val = Math.Round((((orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Student) + (orderItem.NewBookFee * orderItem.Qty_NewBook) + (orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Half * Convert.ToDecimal(0.5)
                                    ) * incentivediscount / 100, 2);
                if (orderItem.CourseType == "HIGH" || orderItem.Term.Substring(0, 1) == "V")  // 하이스쿨은 인센티브 없음 방학도 없음
                {
                    incentive_val = 0;
                }
                //회사 운영방침 변경에 따라, NSW FMS 업데이트가 필요 -> 브라이언실장
                //2022년 term 3 부터 교재 로열티에 제공되던 5 % 브랜치 서포트 비용이 없어짐.
                //VIC는 0으로 되어 있고 NSW만 반영처리함.
                //(H)Discount 값은 0으로 셋팅 요청에 따라 처리
                //2022.09.16 slack 으로 요청받음.
                //omni_royaltycontroller.cs에서 화면먼저 수정후.. 0으로.. pdf도 0으로 해야한다고 요청받아서 처리됨.
                incentive_val = 0;

                incentive = _priceFormatter.FormatPrice(incentive_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(incentive, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                //total
                string Total = string.Empty;
                decimal total_val = subtotal_val - incentive_val + orderItem.Facility * (orderItem.Qty_Student+orderItem.Qty_Half);
                Total = _priceFormatter.FormatPrice(total_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(Total, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                totalsum += total_val;
                student += orderItem.Qty_Student + orderItem.Qty_Half;
                teacher += orderItem.Qty_Teacher;
            }
            doc.Add(productsTable);

            #endregion


            #region Totals


            // Amnt_Freight 
            if (closings.Amnt_Freight != 0)
            {
                string adjustStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Material Total:", adjustStr), titleFont);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                string freightStr = _priceFormatter.FormatPrice(closings.Amnt_Freight, true, _workContext.WorkingCurrency, lang, false);

                var pfreight = new Paragraph(String.Format("{0} {1}", "Freight & Delivery:", freightStr), titleFont);
                pfreight.Alignment = Element.ALIGN_RIGHT;
                doc.Add(pfreight);

                totalsum = totalsum + closings.Amnt_Freight;
            }
            else // Item total only
            {
                string adjustStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Material Total:", adjustStr), titleFont);
                padjust.Alignment = Element.ALIGN_RIGHT;
                //doc.Add(padjust);
            }

            // adjust amnt 
            if (closings.Amnt_adjust != 0)
            {
                string adjustStr = _priceFormatter.FormatPrice(closings.Amnt_adjust, true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Adjust Amount:", adjustStr), titleFont);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                totalsum = totalsum + closings.Amnt_adjust;
            }


            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);

            //GST
            decimal IGST = totalsum - (totalsum / Convert.ToDecimal(1.1));
            string GSTStr = _priceFormatter.FormatPrice(IGST, true, _workContext.WorkingCurrency, lang, false);

            var pTotal = new Paragraph(String.Format("{0} {1}    {2} {3}", "GST:", GSTStr, "Invoice Total:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            pTotal = new Paragraph("(Including GST)", font);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            var pStudent = new Paragraph("Student : " + student.ToString() + "     Tutor : " + teacher.ToString(), font);
            pStudent.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pStudent);

            #endregion

            MakeRemmittance(ref doc, companyinfo, titleFont, font);



            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();





        }





        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintRoyaltyInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;
            var doc = new Document(pageSize);
            //doc.SetMargins(4f, 4f, 3f, 3f);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 18f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 16f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            font.Size = 9f;
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].DetailName;
            
            var tempGrade = "";
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);


            var closings = _royaltyService.GetClosing(year, term, "ROYL", branchcode);
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].String_01;
            MakeHeaderTo(ref doc, termstring, termstring_short, year, term, branchcode
            , titleFont, font, lang, closings);


            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }


            // 데이터 로드
            var orders = _orderservice.GetAllOrders(branchcode, year, term, "GROP", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var coursetype = _codemasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "NORMAL");   // 일반코스인것만 카운트
            var courses = _coursemasterService.GetAllCourseMasters().Where(x => x.Term == term).Join( coursetype, x => x.CourseCategory, y => y.DetailCode, (x,y)
                => new { Year = x.Year, Term = x.Term, Grade = x.Grade, Level = x.Level, CourseCategory = x.CourseCategory
                    , CourseName = x.CourseName, CourseFee = x.CourseFee, TotalWeek = x.TotalWeek, Id = x.Id }); 
            var ordersets = _orderservice.GetAllOrderbyItemSets();
            var itemsets = _itemsetService.GetAllItemSets(term, "", "");

            var discount = _codemasterService.GetAllCodeMasters("RY01", "Y");
            var earlybird = _codemasterService.GetAllCodeMasters("OR03", "Y").Where(x => x.DetailCode == "EARL").ToList();
            
            var campus1 = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType, y => y.DetailCode, (x, y)
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, RoyaltyType = y.DetailName });
            var campus2 = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType_HI, y => y.DetailCode, (x, y)  // 하이스쿨용 로열티
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, RoyaltyType = y.DetailName });


            // Enrol 내역 집계
            var enrolls = _royaltyService.GetAllEnrollments(branchcode, 0)
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    Year = y.Year,
                    Term = y.Term,
                    Grade = y.Grade,
                    Level = y.Level,
                    CourseType = y.CourseCategory,
                    CourseName = y.CourseName,
                    CourseFee = y.CourseFee,
                    Branch = x.BranchCode,
                    CourseId = x.Course_Id,
                    Week = x.Week,
                    CourseWeek = y.TotalWeek,
                    Qty = x.Qty_Modified
                }).Where(x => x.Year == year && x.Term == term)
                .GroupBy(group => new
                {
                    group.Branch,
                    group.CourseId,
                    group.CourseWeek,
                    group.Week,
                    group.Year,
                    group.CourseType,
                    group.Term,
                    group.Grade,
                    group.Level,
                    group.CourseFee
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    CourseType = group.Key.CourseType,
                    CourseName = group.Max(a => a.CourseName).Split(' ')[0] + " " + group.Max(a => a.CourseName).Split(' ')[1],
                    Branch = group.Key.Branch,
                    CourseId = group.Key.CourseId,
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek,
                    Week = group.Key.Week,
                    Qty = group.Sum(a => a.Qty),
                    Qty_Cum = group.Sum(a => a.Week >= 98 ? 0 : (a.CourseWeek - a.Week + 1) * a.Qty)
                });

            // order 집계
            var courseorder = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Week = y.Week, CourseId = y.Course_Id })
                .Join(courses, x => x.CourseId, y => y.Id, (x, y) => new
                {
                    Branch = x.Branch,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = y.Grade,
                    Level = y.Level,
                    Order_Id = x.Order_Id,
                    Qty = x.Qty,
                    Week = x.Week,
                    CourseId = x.CourseId,
                    CourseName = y.CourseName,
                    CourseFee = y.CourseFee,
                    CourseWeek = y.TotalWeek,
                    CourseType = y.CourseCategory
                })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.Grade, group.CourseFee, group.CourseWeek, group.CourseId, group.Level, group.CourseType, group.Week })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    CourseType = group.Key.CourseType,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Week = group.Key.Week,
                    Qty = group.Sum(a => a.Qty),
                    Qty_Cum = group.Sum(a => (a.CourseWeek - a.Week + 1) * a.Qty),
                    CourseName = group.Max(a => a.CourseName).Split(' ')[0] + " " + group.Max(a => a.CourseName).Split(' ')[1],
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek
                }).ToList();

            // order와 enrol 을 집계하여 최종결과 산출

            // enroll이 있으면 지운다
            for (int i = courseorder.Count - 1; i >= 0; i--)
            {
                var enroll = enrolls.Where(x => x.Week == courseorder[i].Week && x.CourseId == courseorder[i].Course_Id).ToList();
                if (enroll.Count > 0)
                    courseorder.Remove(courseorder[i]);
            }
            // enroll을 넣는다

            foreach (var enroll in enrolls)
            {
                courseorder.Add(new
                {
                    Year = enroll.Year,
                    Term = enroll.Term,
                    Grade = enroll.Grade,
                    Level = enroll.Level,
                    CourseType = enroll.CourseType,
                    Branch = enroll.Branch,
                    Course_Id = enroll.CourseId,
                    Week = enroll.Week,
                    Qty = enroll.Qty,
                    Qty_Cum = enroll.Qty_Cum,
                    CourseName = enroll.CourseName,
                    CourseFee = enroll.CourseFee,
                    CourseWeek = enroll.CourseWeek
                });
            }

            var final = courseorder.Select(x => new
            {
                Year = x.Year,
                Term = x.Term,
                Grade = x.Grade,
                Level = x.Level,
                CourseType = x.CourseType,
                Branch = x.Branch,
                Course_Id = x.Course_Id,
                Qty = x.Week >= 98 ? 0 : x.Qty,
                Qty_Cum = x.Qty_Cum,
                CourseName = x.CourseName,
                CourseFee = x.CourseFee,
                CourseWeek = x.CourseWeek,
                Early_W1 = x.Week == 98 ? x.Qty : 0,
                Early_W2 = x.Week == 99 ? x.Qty : 0,
                Subject = (x.CourseType == "HIGH") ? x.Level : ""

            });

            var final2 = final.GroupBy(group => new { group.Branch, group.Year, group.Term, group.Grade, group.CourseFee, group.CourseWeek, group.CourseName, group.Subject, group.CourseType })
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Term,
                    group.Key.Grade,
                    group.Key.Branch,
                    group.Key.CourseType,
                    Qty_Cum = group.Sum(a => a.Qty_Cum),
                    Qty = group.Sum(a => a.Qty),
                    CourseName = group.Key.CourseName,
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek,
                    Early_W1 = group.Sum(a => a.Early_W1),
                    Early_W2 = group.Sum(a => a.Early_W2),
                }).OrderBy(x => x.Branch)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.CourseName).ToList();
                
                



            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : Course Royalty", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(8);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 20, 20, 50, 20, 20, 20, 30, 30 });

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Grade
            cell = new PdfPCell(new Phrase("Grade", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Course Description
            cell = new PdfPCell(new Phrase("Course Description", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //unit price
            cell = new PdfPCell(new Phrase("Unit Price", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Attendence
            cell = new PdfPCell(new Phrase("No. Attendance", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Early Bird
            cell = new PdfPCell(new Phrase("Early bird" + System.Environment.NewLine + "discount", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Net Tuition
            cell = new PdfPCell(new Phrase("Net Tuition" + System.Environment.NewLine + "(exc.GST)", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Royalty
            cell = new PdfPCell(new Phrase("Royalty", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;
            decimal totalsumgst = 0;
            decimal students_pri = 0;
            decimal students_high = 0;

            for (int i = 0; i < final2.Count; i++)
            {
                var orderItem = final2[i];

                //Date 
                string date = termstring + System.Environment.NewLine + year;
                if (i != 0) date = "";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);

                //grade
                string grade = (tempGrade == orderItem.Grade ? "" : orderItem.CourseName.Split(' ')[0]);
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(grade, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph2 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph2);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);
                tempGrade = orderItem.Grade;

                //product name
                string name = orderItem.CourseName.Replace(orderItem.CourseName.Split(' ')[0], "") + " Course";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);



                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(orderItem.CourseFee, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //attendence
                string qty = (orderItem.Qty_Cum).ToString();
                cell = new PdfPCell(new Phrase(qty, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //early bird
                string earlybird_str = string.Empty;
                decimal earlybird_val = orderItem.Early_W1 * orderItem.CourseFee * orderItem.CourseWeek * earlybird[0].Val_02 / 100
                           + orderItem.Early_W2 * orderItem.CourseFee * (orderItem.CourseWeek - 1) * earlybird[0].Val_02 / 100; 
                earlybird_str = _priceFormatter.FormatPrice(earlybird_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(earlybird_str, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //subtotal   (courseroyaltyModel.TotalTuition - courseroyaltyModel.Earlybird) / Convert.ToDecimal(1.1);
                string subTotal = string.Empty;
                decimal subtotal_val = (orderItem.CourseFee * orderItem.Qty_Cum - earlybird_val) / Convert.ToDecimal(1.1);
                subTotal = _priceFormatter.FormatPrice(subtotal_val, true, _workContext.WorkingCurrency, lang, false);


                cell = new PdfPCell(new Phrase(subTotal, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                



                //total
                var royaltytype = campus1.Where(c => c.Branch == branchcode).ToList();
                if ( orderItem.CourseType == "HIGH" )
                {
                    royaltytype = campus2.Where(c => c.Branch == branchcode).ToList();
                }

                string Total = string.Empty;
                decimal total_val = subtotal_val * royaltytype[0].Discount / 100;
                Total = _priceFormatter.FormatPrice(total_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(Total, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                totalsum += total_val;
                totalsumgst += (total_val * Convert.ToDecimal( 1.1 ));
                if ( orderItem.CourseType == "HIGH")
                {
                    students_high += orderItem.Qty;
                }
                else
                {
                    students_pri += orderItem.Qty;
                }



            }
            doc.Add(productsTable);

            #endregion


            #region Totals




            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);

            var pTotal = new Paragraph(String.Format("{0} {1}", "Amount:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);


            string GSTStr = _priceFormatter.FormatPrice(totalsumgst-totalsum, true, _workContext.WorkingCurrency, lang, false);
            string orderTotalGSTStr = _priceFormatter.FormatPrice(totalsumgst, true, _workContext.WorkingCurrency, lang, false);

            var pGST = new Paragraph(String.Format("{0} {1}    {2} {3}", "GST:", GSTStr, "Total(inc.GST):", orderTotalGSTStr), titleFont);
            pGST.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pGST);


            // adjust amnt 
            if (closings.Amnt_adjust != 0)
            {
                totalsumgst = totalsumgst + closings.Amnt_adjust;

                string adjustStr = _priceFormatter.FormatPrice(closings.Amnt_adjust, true, _workContext.WorkingCurrency, lang, false);
                string adjustGSTStr = _priceFormatter.FormatPrice(totalsumgst - (totalsumgst / Convert.ToDecimal(1.1)), true, _workContext.WorkingCurrency, lang, false);
                string adjustTotalStr = _priceFormatter.FormatPrice((totalsumgst / Convert.ToDecimal(1.1)), true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Adjust Amount:", adjustStr), font);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                var padjust2 = new Paragraph(String.Format("{0} {1}   {2} {3}"
                    , "GST:", adjustGSTStr, "Total(exc.GST):", adjustTotalStr), font);
                padjust2.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust2);

                orderTotalGSTStr = _priceFormatter.FormatPrice(totalsumgst, true, _workContext.WorkingCurrency, lang, false);

                var pTotalGST = new Paragraph(String.Format("{0} {1}", "Total(inc.GST):", orderTotalGSTStr), titleFont);
                pTotalGST.Alignment = Element.ALIGN_RIGHT;
                doc.Add(pTotalGST);

            }


            var pStudent = new Paragraph("Primary : " + students_pri.ToString() + "     High : " + students_high.ToString(), font);
            pStudent.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pStudent);

            #endregion

            MakeRemmittance(ref doc, companyinfo, titleFont, font);



            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }



        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintTestfeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();



            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 20f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].DetailName;
            var tempGrade = "";
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;


            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);




            
            var closings = _royaltyService.GetClosing(year, term, "TEST", branchcode);
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].String_01;
            MakeHeaderTo(ref doc, termstring, termstring_short, year, term, branchcode
            , titleFont, font, lang, closings);


            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }


            // 데이터 로드
            var orders = _orderservice.GetAllOrders(branchcode, year, term, "TEST", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var coursetype = _codemasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "TEST");   // 테스트인것만 카운트
            var courses = _coursemasterService.GetAllCourseMasters().Where(x => x.Term == term).Join(coursetype, x => x.CourseCategory, y => y.DetailCode, (x, y)
                            => new
                            {
                                Id = x.Id,
                                Year = x.Year,
                                Term = x.Term,
                                Grade = x.Grade,
                                Level = x.Level,
                                CourseCategory = x.CourseCategory,
                                CourseName = x.CourseName,
                                CourseFee = x.CourseFee,
                                TotalWeek = x.TotalWeek,
                                StartWeek = x.StartWeek
                            }); 
            var ordersets = _orderservice.GetAllOrderbyItemSets();
            var itemsets = _itemsetService.GetAllItemSets(term, "", "");
            var enrollments = _royaltyService.GetAllEnrollments(branchcode, 0);

            var codes = _codemasterService.GetAllCodeMasters("OR04", "Y");

            var discount = _codemasterService.GetAllCodeMasters("RY03", "Y");
            var campus1 = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType, y => y.DetailCode, (x, y)
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, Discount2 = y.Val_02, RoyaltyType = y.DetailName });


            // Enrol 내역 집계
            var enrolls = _royaltyService.GetAllEnrollments(branchcode, 0)
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    Year = y.Year,
                    Term = y.Term,
                    Grade = y.Grade,
                    Level = y.Level,
                    CourseType = y.CourseCategory,
                    CourseName = y.CourseName,
                    CourseFee = y.CourseFee,
                    Branch = x.BranchCode,
                    CourseId = x.Course_Id,
                    Week = x.Week,
                    CourseWeek = y.TotalWeek,
                    StartWeek = y.StartWeek,
                    Qty = x.Qty_Modified
                }).Where(x => x.Year == year && x.Term == term)
                .GroupBy(group => new
                {
                    group.Branch,
                    group.CourseId,
                    group.CourseWeek,
                    group.Week,
                    group.Year,
                    group.CourseType,
                    group.Term,
                    group.Grade,
                    group.Level,
                    group.CourseFee,
                    group.StartWeek
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    CourseType = group.Key.CourseType,
                    CourseName = group.Max(a => a.CourseName),
                    Branch = group.Key.Branch,
                    CourseId = group.Key.CourseId,
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek,
                    Week = group.Key.Week,
                    Qty = group.Sum(a => a.Qty),
                    Qty_Cum = group.Sum(a => a.Week >= 98 ? 0 : (a.CourseWeek - a.Week + 1) * a.Qty)
                });


            // order 집계
            var courseorder = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Week = y.Week, CourseId = y.Course_Id })
                .Join(courses, x => x.CourseId, y => y.Id, (x, y) => new
                {
                    Branch = x.Branch,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = y.Grade,
                    Level = y.Level,
                    Order_Id = x.Order_Id,
                    Qty = x.Qty,
                    Week = x.Week,
                    CourseId = x.CourseId,
                    CourseName = y.CourseName,
                    CourseFee = y.CourseFee,
                    CourseWeek = y.TotalWeek,
                    StartWeek = y.StartWeek,
                    CourseType = y.CourseCategory
                })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.Grade, group.CourseFee, group.CourseWeek, group.CourseId, group.Level, group.CourseType, group.Week, group.StartWeek })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    CourseType = group.Key.CourseType,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Week = group.Key.Week,
                    Qty = group.Sum(a => a.Qty),
                    Qty_Cum = group.Sum(a => (a.CourseWeek - a.Week + 1) * a.Qty),
                    CourseName = group.Max(a => a.CourseName),
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek
                }).ToList();


            // order와 enrol 을 집계하여 최종결과 산출

            // enroll이 있으면 지운다
            for (int i = courseorder.Count - 1; i >= 0; i--)
            {
                var enroll = enrolls.Where(x => x.Week == courseorder[i].Week && x.CourseId == courseorder[i].Course_Id).ToList();
                if (enroll.Count > 0)
                    courseorder.Remove(courseorder[i]);
            }
            // enroll을 넣는다

            foreach (var enroll in enrolls)
            {
                courseorder.Add(new
                {
                    Year = enroll.Year,
                    Term = enroll.Term,
                    Grade = enroll.Grade,
                    Level = enroll.Level,
                    CourseType = enroll.CourseType,
                    Branch = enroll.Branch,
                    Course_Id = enroll.CourseId,
                    Week = enroll.Week,
                    Qty = enroll.Qty,
                    Qty_Cum = enroll.Qty_Cum,
                    CourseName = enroll.CourseName,
                    CourseFee = enroll.CourseFee,
                    CourseWeek = enroll.CourseWeek,
                    StartWeek = enroll.StartWeek
                });
            }



            var ordersetgroup = courseorder
                .GroupBy(x => new { x.Branch, x.Course_Id, x.Year, x.Term, x.CourseType, x.Grade, x.Level, x.CourseName, x.CourseWeek, x.StartWeek, x.CourseFee })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    BranchCode = group.Key.Branch,
                    Course_Id = group.Key.Course_Id,
                    CourseName = group.Key.CourseName,
                    SetCategory = group.Key.CourseType,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek,
                    CourseFee = group.Key.CourseFee,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    Qty1 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 1 ? a.Qty : 0),
                    Qty2 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 2 ? a.Qty : 0),
                    Qty3 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 3 ? a.Qty : 0),
                    Qty4 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 4 ? a.Qty : 0),
                    Qty5 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 5 ? a.Qty : 0),
                    Qty6 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 6 ? a.Qty : 0),
                    Qty7 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 7 ? a.Qty : 0),
                    Qty8 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 8 ? a.Qty : 0),
                    Qty9 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 9 ? a.Qty : 0),
                    Qty10 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 10 ? a.Qty : 0),
                    Qty11 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 11 ? a.Qty : 0),
                    Qty12 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 12 ? a.Qty : 0),
                    Qty13 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 13 ? a.Qty : 0),
                    Qty14 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 14 ? a.Qty : 0),
                    Qty15 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 15 ? a.Qty : 0),
                    Qty16 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 16 ? a.Qty : 0),
                    QtyTotal = group.Sum(a => a.Qty),
                    Code = "OR04"
                })
                .Join(codes, x => x.Code, y => y.CodeType, (x, y) => new
                {
                    BranchCode = x.BranchCode,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = x.Grade,
                    SetCategory = x.SetCategory,
                    Level = x.Level,
                    Course_Id = x.Course_Id,
                    CourseName = x.CourseName,
                    TotalWeek = x.CourseWeek,
                    StartWeek = x.StartWeek,
                    CourseFee = x.CourseFee,
                    QtyType = y.DetailCode,
                    Qty1 = (y.DetailCode == "BOOK") ? x.StartWeek : x.Qty1,
                    Qty2 = (x.CourseWeek < 2) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 1 : x.Qty2 + x.Qty1,
                    Qty3 = (x.CourseWeek < 3) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 2 : x.Qty3 + x.Qty2 + x.Qty1,
                    Qty4 = (x.CourseWeek < 4) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 3 : x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty5 = (x.CourseWeek < 5) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 4 : x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty6 = (x.CourseWeek < 6) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 5 : x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty7 = (x.CourseWeek < 7) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 6 : x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty8 = (x.CourseWeek < 8) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 7 : x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty9 = (x.CourseWeek < 9) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 8 : x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty10 = (x.CourseWeek < 10) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 9 : x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty11 = (x.CourseWeek < 11) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 10 : x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty12 = (x.CourseWeek < 12) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 11 : x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty13 = (x.CourseWeek < 13) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 12 : x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty14 = (x.CourseWeek < 14) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 13 : x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty15 = (x.CourseWeek < 15) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 14 : x.Qty15 + x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty16 = (x.CourseWeek < 16) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 15 : x.Qty16 + x.Qty15 + x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    QtyTotal = (y.DetailCode == "BOOK") ? 0 : x.QtyTotal
                })
                .OrderBy(y => y.BranchCode)
                .ThenBy(y => y.Grade)
                .ThenBy(y => y.SetCategory)
                .ThenBy(y => y.Level).ToList();




            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : Test Fee", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(6);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 30, 50, 20, 20, 20, 40});

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);


            //Course Description
            cell = new PdfPCell(new Phrase("Test Description", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //unit price
            cell = new PdfPCell(new Phrase("Unit Price", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Qty
            cell = new PdfPCell(new Phrase("Unit Qty", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //OQty
            cell = new PdfPCell(new Phrase("Prime Qty", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Net Tuition
            cell = new PdfPCell(new Phrase("Test Fee" + System.Environment.NewLine + "(exc.GST)", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;
            decimal totalsumgst = 0;
            int totalqty = 0;

            for (int i = 0; i < ordersetgroup.Count; i=i+2)
            {
                var orderItem = ordersetgroup[i];
                var orderItem2 = ordersetgroup[i+1];

                

                for (int j = 1; j <= 16; j++)
                {
                    var qty1 = 0;// orderItem.Qty1;
                    var qty2 = 0;// orderItem2.Qty1;

                    switch(j)
                    {
                        case 1 :
                            qty1 = orderItem.Qty1;
                            qty2 = orderItem2.Qty1;
                            break;
                        case 2:
                            qty1 = orderItem.Qty2;
                            qty2 = orderItem2.Qty2;
                            break;
                        case 3:
                            qty1 = orderItem.Qty3;
                            qty2 = orderItem2.Qty3;
                            break;
                        case 4:
                            qty1 = orderItem.Qty4;
                            qty2 = orderItem2.Qty4;
                            break;
                        case 5:
                            qty1 = orderItem.Qty5;
                            qty2 = orderItem2.Qty5;
                            break;
                        case 6:
                            qty1 = orderItem.Qty6;
                            qty2 = orderItem2.Qty6;
                            break;
                        case 7:
                            qty1 = orderItem.Qty7;
                            qty2 = orderItem2.Qty7;
                            break;
                        case 8:
                            qty1 = orderItem.Qty8;
                            qty2 = orderItem2.Qty8;
                            break;
                        case 9:
                            qty1 = orderItem.Qty9;
                            qty2 = orderItem2.Qty9;
                            break;
                        case 10:
                            qty1 = orderItem.Qty10;
                            qty2 = orderItem2.Qty10;
                            break;
                        case 11:
                            qty1 = orderItem.Qty11;
                            qty2 = orderItem2.Qty11;
                            break;
                        case 12:
                            qty1 = orderItem.Qty12;
                            qty2 = orderItem2.Qty12;
                            break;
                        case 13:
                            qty1 = orderItem.Qty13;
                            qty2 = orderItem2.Qty13;
                            break;
                        case 14:
                            qty1 = orderItem.Qty14;
                            qty2 = orderItem2.Qty14;
                            break;
                        case 15:
                            qty1 = orderItem.Qty15;
                            qty2 = orderItem2.Qty15;
                            break;
                        case 16:
                            qty1 = orderItem.Qty16;
                            qty2 = orderItem2.Qty16;
                            break;
                    }

                    if (qty2 == 0)
                        continue;


                    //Date 
                    string date = termstring + " / " + year;
                    if (totalsum != 0 ) date = "";
                    cell = new PdfPCell();
                    cell.AddElement(new Paragraph(date, font));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                    cell.AddElement(attributesParagraph1);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    productsTable.AddCell(cell);


                    //test name
                    string name = orderItem.CourseName.Replace(orderItem.CourseName.Split(' ')[0], "") + " No." + qty1;
                    cell = new PdfPCell();
                    cell.AddElement(new Paragraph(name, font));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                    cell.AddElement(attributesParagraph);
                    productsTable.AddCell(cell);



                    //price
                    string unitPrice = string.Empty;
                    unitPrice = _priceFormatter.FormatPrice(orderItem.CourseFee, true, _workContext.WorkingCurrency, lang, false);

                    cell = new PdfPCell(new Phrase(unitPrice, font));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productsTable.AddCell(cell);

                    //attendence
                    string qty = (qty2).ToString();
                    cell = new PdfPCell(new Phrase(qty, font));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productsTable.AddCell(cell);

                    //Online
                    string qtyo = (0).ToString();
                    cell = new PdfPCell(new Phrase(qtyo, font));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productsTable.AddCell(cell);


                    //total

                    string Total = string.Empty;
                    decimal total_val = orderItem.CourseFee * qty2 + 0 *5;  // 온라인 추가
                    Total = _priceFormatter.FormatPrice(total_val, true, _workContext.WorkingCurrency, lang, false);

                    cell = new PdfPCell(new Phrase(Total, font));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productsTable.AddCell(cell);


                    totalsum += total_val;
                    totalsumgst += (total_val * Convert.ToDecimal(1.1));
                    totalqty += qty2;
                }

            }

            //Date
            cell = new PdfPCell(new Phrase("", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);


            //Course Description
            cell = new PdfPCell(new Phrase("", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //unit price
            cell = new PdfPCell(new Phrase("Total", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Qty
            cell = new PdfPCell(new Phrase(totalqty.ToString(), font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            productsTable.AddCell(cell);

            //OQty
            cell = new PdfPCell(new Phrase(totalqty.ToString(), font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            productsTable.AddCell(cell);

            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);



            cell = new PdfPCell(new Phrase(orderTotalStr, font));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BackgroundColor = Color.LIGHT_GRAY;
            productsTable.AddCell(cell);

            doc.Add(productsTable);

            #endregion


            #region Totals




            string GSTStr = _priceFormatter.FormatPrice(totalsumgst - totalsum, true, _workContext.WorkingCurrency, lang, false);
            string orderTotalGSTStr = _priceFormatter.FormatPrice(totalsumgst, true, _workContext.WorkingCurrency, lang, false);

            var pGST = new Paragraph(String.Format("{0} {1}    {2} {3}", "GST:", GSTStr, "Total(inc.GST):", orderTotalGSTStr), titleFont);
            pGST.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pGST);




            // adjust amnt 
            if (closings.Amnt_adjust != 0)
            {
                totalsumgst = totalsumgst + closings.Amnt_adjust;

                string adjustStr = _priceFormatter.FormatPrice(closings.Amnt_adjust, true, _workContext.WorkingCurrency, lang, false);
                string adjustGSTStr = _priceFormatter.FormatPrice(totalsumgst - (totalsumgst / Convert.ToDecimal(1.1)), true, _workContext.WorkingCurrency, lang, false);
                string adjustTotalStr = _priceFormatter.FormatPrice(totalsumgst / Convert.ToDecimal(1.1), true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Adjust Amount:", adjustStr), font);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                var padjust2 = new Paragraph(String.Format("{0} {1}   {2} {3}"
                    , "GST:", adjustGSTStr, "Total(exc.GST):", adjustTotalStr), font);
                padjust2.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust2);

                orderTotalGSTStr = _priceFormatter.FormatPrice(totalsumgst, true, _workContext.WorkingCurrency, lang, false);

                var pTotalGST = new Paragraph(String.Format("{0} {1}", "Total(inc.GST):", orderTotalGSTStr), titleFont);
                pTotalGST.Alignment = Element.ALIGN_RIGHT;
                doc.Add(pTotalGST);

            }





            #endregion


            MakeRemmittance(ref doc, companyinfo, titleFont, font);


            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }



        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintSupplefeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();


            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 20f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].String_01;
            var tempGrade = "";
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);



            var closings = _royaltyService.GetClosing(year, term, "SUPP", branchcode);
            MakeHeaderTo(ref doc, termstring, termstring_short, year, term, branchcode
            , titleFont, font, lang, closings);


            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }


            // 데이터 로드

            var orders = _orderservice.GetAllOrders(branchcode, year, term, "SUPP", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var courses = _coursemasterService.GetAllCourseMasters();
            var ordersets = _orderservice.GetAllOrderbyItemSets();
            var itemsets = _itemsetService.GetAllItemSets(term, "", "");

            var discount = _codemasterService.GetAllCodeMasters("OR03", "Y");
            var coursetype = _codemasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "SUPPL");  // supple코스인것만 카운트



            var teacherdiscount = discount.Where(x => x.DetailCode == "TCHR").ToList()[0].Val_01;
            var incentivediscount = discount.Where(x => x.DetailCode == "STUD").ToList()[0].Val_01;
            var onlinefacility = discount.Where(x => x.DetailCode == "STUD").ToList()[0].Val_02;  // 처음엔 기준정보에서 가져왔으나 코스마다 따로 설정으로 변경

            var bookorders = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Qty_T = y.Qty_Teacher, isHalf = y.is_Half, CourseId = y.Course_Id })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.CourseId })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Qty = group.Sum(a => a.isHalf == "N" ? a.Qty : 0),
                    Qty_H = group.Sum(a => a.isHalf == "Y" ? a.Qty : 0),
                    Qty_T = group.Sum(a => a.Qty_T)
                })
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    BranchCode = x.Branch,
                    CourseType = y.CourseCategory,
                    Year = x.Year,
                    Term = x.Term,
                    Course_Id = x.Course_Id,
                    Course_Name = y.CourseName,
                    MaterialFee = y.BookFee,
                    Qty_Student = x.Qty,
                    Qty_Half = x.Qty_H,
                    Qty_Teacher = x.Qty_T,
                    Grade = y.Grade,
                    Subject = y.Level,    // 하이스쿨은 레벨이 과목역할을 하니까 따로 분리한다 과목필드를 만들껄 그랬나.. 싶기도하다
                    Facility = (y.CourseCategory == "HIGH") ? 0 : y.Facility // 하이스쿨은 온라인 없음
                })
                .Join(coursetype, x => x.CourseType, y => y.DetailCode, (x, y)
                    => new { x.BranchCode, x.CourseType, x.Year, x.Term, x.Course_Id, x.Course_Name, x.MaterialFee, x.Qty_Student, x.Qty_Half, x.Qty_Teacher, x.Grade, x.Subject, x.Facility })
                .GroupBy(group => new { group.Year, group.Term, group.Grade, group.CourseType, group.BranchCode, group.MaterialFee, group.Subject, group.Facility })
                .Select(group => new
                {
                    BranchCode = group.Key.BranchCode,
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    MaterialFee = group.Key.MaterialFee,
                    Qty_Student = group.Sum(a => a.Qty_Student),
                    Qty_Half = group.Sum(a => a.Qty_Half),
                    Qty_Teacher = group.Sum(a => a.Qty_Teacher),
                    Course_Name = group.Max(a => a.Course_Name),
                    Facility = group.Key.Facility,
                    CourseType = group.Key.CourseType
                })
                .OrderBy(x => x.BranchCode)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.Course_Name).ToList();




            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : Material fee", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(6);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 20, 20, 50, 20, 20, 30 });

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Grade
            cell = new PdfPCell(new Phrase("Grade", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Course Description
            cell = new PdfPCell(new Phrase("Course Description", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //qty
            cell = new PdfPCell(new Phrase("Unit Qty", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Unit Fee", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //total
            cell = new PdfPCell(new Phrase("Total", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;
            decimal student = 0;
            decimal teacher = 0;

            for (int i = 0; i < bookorders.Count; i++)
            {
                var orderItem = bookorders[i];

                //Date 
                string date = termstring + System.Environment.NewLine + year;
                if (i != 0) date = "";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);

                //grade
                string grade = (tempGrade == orderItem.Grade ? "" : orderItem.Course_Name.Split(' ')[0]);
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(grade, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph2 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph2);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);
                tempGrade = orderItem.Grade;

                //product name
                string name = orderItem.Course_Name.Replace(orderItem.Course_Name.Split(' ')[0], "") + " Material Fee";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);

                //qty
                string qty = (orderItem.Qty_Student + orderItem.Qty_Half).ToString() + "(" + orderItem.Qty_Teacher + ")";
                cell = new PdfPCell(new Phrase(qty, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(orderItem.MaterialFee - orderItem.Facility, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                //total
                string Total = string.Empty;
                decimal total_val = Math.Round((((orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Student) + (orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Half * Convert.ToDecimal(0.5)
                                    + (orderItem.MaterialFee - orderItem.Facility) * orderItem.Qty_Teacher * teacherdiscount / 100), 2); ;
                Total = _priceFormatter.FormatPrice(total_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(Total, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                totalsum += total_val;
                student += orderItem.Qty_Student + orderItem.Qty_Half;
                teacher += orderItem.Qty_Teacher;
            }
            doc.Add(productsTable);

            #endregion


            #region Totals

            // adjust amnt 
            if (closings.Amnt_adjust != 0)
            {
                string adjustStr = _priceFormatter.FormatPrice(closings.Amnt_adjust, true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Adjust Amount:", adjustStr), titleFont);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                totalsum = totalsum + closings.Amnt_adjust;
            }


            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);


            var pTotal = new Paragraph(String.Format("{0} {1}", "Invoice Total:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            pTotal = new Paragraph("(Including GST)", font);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            var pStudent = new Paragraph("Student : " + student.ToString() + "     Tutor : " + teacher.ToString(), font);
            pStudent.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pStudent);

            #endregion


            MakeRemmittance(ref doc, companyinfo, titleFont, font);


            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }



        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintPAfeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();




            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 20f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].String_01;
            var tempGrade = "";
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;


            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);


            
            var closings = _royaltyService.GetClosing(year, term, "PA__", branchcode);
            MakeHeaderTo(ref doc, termstring, termstring_short, year, term, branchcode
            , titleFont, font, lang, closings);


            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }
            // 데이터 로드

         
            var paorders = _royaltyService.GetAllOrder_PASales(branchcode, year, term);



            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : PA & CS Online Product fee", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(5);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 50, 50, 20, 20, 30 });

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Course Description
            cell = new PdfPCell(new Phrase("Course Description", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Price", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Payment", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //total
            cell = new PdfPCell(new Phrase("Amount", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;
            decimal student = 0;
            decimal teacher = 0;


            var paroyalty = _codemasterService.GetAllCodeMasters("RY02", "Y").ToList()[0];

            var paratio = paroyalty.Val_01;
            var otherratio = paroyalty.Val_02; 

            for (int i = 0; i < paorders.Count; i++)
            {
                var orderItem = paorders[i];

                //Date 
                string date = termstring + " " + year;
                if (i != 0) date = "";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);

                //product name
                string name = orderItem.Title;
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);

                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(orderItem.Price, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //payment

                cell = new PdfPCell(new Phrase(orderItem.Payment, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                productsTable.AddCell(cell);


                //total
                string Total = string.Empty;
                decimal total_val = 0; 

                if (orderItem.Title.Contains("PA+"))
                {
                    total_val = orderItem.Payment.ToUpper() == "BANK" ? orderItem.Price * Convert.ToDecimal(paratio)/100 : orderItem.Price * Convert.ToDecimal(paratio)/100 * -1;
                }
                else
                {
                    total_val = orderItem.Payment.ToUpper() == "BANK" ? orderItem.Price * Convert.ToDecimal(100 - otherratio)/100 : orderItem.Price * Convert.ToDecimal(otherratio)/100 * -1;
                }

                Total = _priceFormatter.FormatPrice(total_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(Total, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                totalsum += total_val;
            }
            doc.Add(productsTable);

            #endregion


            #region Totals

            // adjust amnt 
            if (closings.Amnt_adjust != 0)
            {
                string adjustStr = _priceFormatter.FormatPrice(closings.Amnt_adjust, true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Adjust Amount:", adjustStr), titleFont);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                totalsum = totalsum + closings.Amnt_adjust;
            }


            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);


            var pTotal = new Paragraph(String.Format("{0} {1}", "Invoice Total:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            pTotal = new Paragraph("(Including GST)", font);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            #endregion

            MakeRemmittance(ref doc, companyinfo, titleFont, font);



            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }



        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintItemfeeInvoiceToPdf(Stream stream, string year, string term, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();




            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 20f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == term).ToList()[0].String_01;
            var tempGrade = "";
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;


            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);



            var closings = _royaltyService.GetClosing(year, term, "ITEM", branchcode);
            MakeHeaderTo(ref doc, termstring, termstring_short, year, term, branchcode
            , titleFont, font, lang, closings);

            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }

            // 데이터 로드


            var orders = _orderservice.GetAllOrders(branchcode, year, term, "ITEM", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var order_ds = _orderservice.GetAllOrder_Ds(branchcode, year, term, "ITEM", "");


            var bookorders = orders
                .Join(order_ds, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Price = y.UnitPrice, ItemCode = y.ItemCode, y.Remarks })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.ItemCode, group.Price })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Branch = group.Key.Branch,
                    ItemCode = group.Key.ItemCode,
                    Price = group.Key.Price,
                    Qty = group.Sum(a => a.Qty),
                    Remarks = group.Max(a => a.Remarks)

                })
                .OrderBy(x => x.Branch)
                .ThenBy(x => x.ItemCode).ToList();
            int id = 1;

            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : Item fee", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(5);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 25, 50,  20, 20, 30 });

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Item Description
            cell = new PdfPCell(new Phrase("Item Name", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Item Description
            //cell = new PdfPCell(new Phrase("Remarks", font));
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Price", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Qty
            cell = new PdfPCell(new Phrase("Qty", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //total
            cell = new PdfPCell(new Phrase("Amount", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;
            decimal student = 0;
            decimal teacher = 0;

            for (int i = 0; i < bookorders.Count; i++)
            {
                var orderItem = bookorders[i];

                //Date 
                string date = "";// termstring + " " + year;
                //if (i != 0) date = "";
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);

                //product name
                string name = _itemservice.GetItemByCode(orderItem.ItemCode).ItemName;
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);

                //remarks
                //cell = new PdfPCell();
                //cell.AddElement(new Paragraph(orderItem.Remarks, font));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                //cell.AddElement(attributesParagraph);
                //productsTable.AddCell(cell);


                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(orderItem.Price, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

                //payment

                cell = new PdfPCell(new Phrase(orderItem.Qty.ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                //total
                string Total = string.Empty;
                decimal total_val = orderItem.Price * orderItem.Qty;
                Total = _priceFormatter.FormatPrice(total_val, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(Total, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                totalsum += total_val;
            }
            doc.Add(productsTable);

            #endregion


            #region Totals

            // adjust amnt 
            if (closings.Amnt_adjust != 0)
            {
                string adjustStr = _priceFormatter.FormatPrice(closings.Amnt_adjust, true, _workContext.WorkingCurrency, lang, false);

                var padjust = new Paragraph(String.Format("{0} {1}", "Adjust Amount:", adjustStr), titleFont);
                padjust.Alignment = Element.ALIGN_RIGHT;
                doc.Add(padjust);

                totalsum = totalsum + closings.Amnt_adjust;
            }


            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);


            var pTotal = new Paragraph(String.Format("{0} {1}", "Invoice Total:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            pTotal = new Paragraph("(Including GST)", font);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);


            #endregion

            MakeRemmittance(ref doc, companyinfo, titleFont, font);


            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }



        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        public virtual void PrintMarketingfeeInvoiceToPdf(Stream stream, string branchcode, int languageId = 0)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();




            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 20f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

         
            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;


            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("TAX INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);



            var closings = new Closing(DateTime.Now.Year.ToString(), "", "NMKT", branchcode, 1, "Y", "Y", "Y", 0, 0, DateTime.Now, "", "", ""); 
            MakeHeaderTo(ref doc, "YEAR", "", DateTime.Now.Year.ToString(), "", branchcode
            , titleFont, font, lang, closings);

            if (closings.YN_Approval != "Y")
            {
                MakeWatermark(ref doc, ref pdfWriter);
            }

            // 데이터 로드

            var marketingfee = _royaltyService.GetAllMarketingFees(branchcode).Where(x => x.YN_Paid == "N").OrderBy(x => x.IssueDate).ThenBy(x => x.Item).ToList();

            int id = 1;

            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : National Marketing fee", titleFont));
            doc.Add(new Paragraph(" "));



            var productsTable = new PdfPTable(5);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 25, 50, 20, 20, 30 });

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Item Description
            cell = new PdfPCell(new Phrase("Item Name", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Item Description
            //cell = new PdfPCell(new Phrase("Remarks", font));
            //cell.BackgroundColor = Color.LIGHT_GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Price", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Paid
            cell = new PdfPCell(new Phrase("Paid", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //total
            cell = new PdfPCell(new Phrase("Balance", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            decimal totalsum = 0;

            for (int i = 0; i < marketingfee.Count; i++)
            {
                var fee = marketingfee[i];

                //Date 
                string date = fee.IssueDate.ToString("dd/MM/yyyy");

                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);

                //product name
                string name = fee.Item;
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);

                //remarks
                //cell = new PdfPCell();
                //cell.AddElement(new Paragraph(orderItem.Remarks, font));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                //cell.AddElement(attributesParagraph);
                //productsTable.AddCell(cell);


                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(fee.Amount, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                var paid = _royaltyService.GetAllMarketingFeePayments(fee.Id).Sum(a => a.Amount);
                if (paid == null) paid = 0;


                //payment
                string paidamount = string.Empty;
                paidamount = _priceFormatter.FormatPrice(paid, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(paidamount, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                //balance
                string balanceamount = string.Empty;
                balanceamount = _priceFormatter.FormatPrice(fee.Amount - paid, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(balanceamount, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);


                totalsum += (fee.Amount - paid);
            }
            doc.Add(productsTable);

            #endregion


            #region Totals

            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(totalsum, true, _workContext.WorkingCurrency, lang, false);


            var pTotal = new Paragraph(String.Format("{0} {1}", "Invoice Total:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            pTotal = new Paragraph("(Including GST)", font);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);


            #endregion

            MakeRemmittance_3100(ref doc, companyinfo, titleFont, font);


            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }



        public virtual void PrintingInvoice(Stream stream, int reqid)
        {


            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4;
            var doc = new Document(pageSize);
            //doc.SetMargins(4f, 4f, 3f, 3f);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 18f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 16f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            font.Size = 9f;
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var companyinfo = _codemasterService.GetAllCodeMasters("CM06", "Y");

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            MakeCompanyHeader(ref doc, companyinfo, pdfSettingsByStore, titleFont, titleFont_B, font);

            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            var phrase1 = new Paragraph("PRINTING INVOICE", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            doc.Add(lineTable);




            //header
            var request = _printService.GetRequestById(reqid);
            var quotation = _printService.GetAllQuotations(reqid).ToList();
            var requestitem = _printService.GetAllRequestItems(reqid);

            var itemtype = _printService.GetAllItems();     


            var user = _customerService.GetCustomerByUsername(request.User_Id);

            var branchcode = user.AdminComment;
            if (!String.IsNullOrEmpty(branchcode))
            {

                var headerTable = new PdfPTable(4);
                headerTable.WidthPercentage = 100f;
                headerTable.SetWidths(new[] { 18, 35, 15, 32 });
                headerTable.HorizontalAlignment = Element.ALIGN_LEFT;

                var store = _storeContext.CurrentStore;
                var campuslist = _codemasterService.GetAllCampus().Where(c => c.Campus_Prefix == branchcode);
                Campus campus = campuslist.ToList()[0];

                //store info
                cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                cell.AddElement(new Phrase("To : ", font));
                cell.AddElement(new Phrase("ABN : ", font));
                cell.AddElement(new Phrase("Invoice No. : ", font));
                headerTable.AddCell(cell);


                cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                cell.AddElement(new Phrase("CS Education," + campus.Campus_Title.Replace("CS", ""), font));
                cell.AddElement(new Phrase(campus.ABN, font));
                cell.AddElement(new Phrase("PR" + reqid.ToString("########"), font));
                headerTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                cell.AddElement(new Phrase(" ", titleFont));
                cell.AddElement(new Phrase("Invoice Date : ", font));
                cell.AddElement(new Phrase("Due Date : ", font));
                cell.AddElement(new Phrase("", font));
                headerTable.AddCell(cell);

                cell = new PdfPCell();
                cell.Border = Rectangle.NO_BORDER;
                cell.AddElement(new Phrase(" ", titleFont));
                cell.AddElement(new Phrase(DateTime.Now.ToString("dd/MM/yyyy"), font));

                string duedate = "Immediate";


                cell.AddElement(new Phrase(duedate, font));
                cell.AddElement(new Phrase("", font));
                headerTable.AddCell(cell);





                cell.AddElement(new Phrase("To : CS Education," + campus.Campus_Title.Replace("CS", ""), font));
                cell.AddElement(new Phrase("ABN : " + campus.ABN, font));
                cell.AddElement(new Phrase("Invoice No. : " + "PR" + reqid.ToString("########"), font));
                cell.AddElement(new Phrase(String.Format(_localizationService.GetResource("PDFInvoice.OrderDate", lang.Id), _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Local).ToString("D", new CultureInfo(lang.LanguageCulture))), font));
                cell.AddElement(new Phrase("DUE : immediate", font));

                headerTable.AddCell(cell);
                doc.Add(headerTable);
            }


            #region Products
            //products
            doc.Add(new Paragraph(_localizationService.GetResource("PDFInvoice.Product(s)", lang.Id) + " : Printing Service", titleFont));
            doc.Add(new Paragraph(" "));


            var productsTable = new PdfPTable(4);
            productsTable.WidthPercentage = 100f;
            productsTable.SetWidths(new[] { 20, 30, 50, 20});

            //Date
            cell = new PdfPCell(new Phrase("DATE", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Item
            cell = new PdfPCell(new Phrase("Item", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //Title
            cell = new PdfPCell(new Phrase("Title", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

            //price
            cell = new PdfPCell(new Phrase("Price", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cell);

      

            for (int i = 0; i < 1; i++)  // 일단 한줄만
            {

                //Date 
                string date = quotation[0].Due_date.ToString("dd/MM/yyyy");
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(date, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cell);


                //Item
                string Item = itemtype.Where(x => x.Id == requestitem[0].Item_Type).ToList()[0].ItemName;
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(Item, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph2 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph2);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                productsTable.AddCell(cell);

                //Title
                string title = request.Title;
                cell = new PdfPCell();
                cell.AddElement(new Paragraph(title, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), attributesFont);
                cell.AddElement(attributesParagraph);
                productsTable.AddCell(cell);

                //price
                string unitPrice = string.Empty;
                unitPrice = _priceFormatter.FormatPrice(quotation[0].Quot_Amount, true, _workContext.WorkingCurrency, lang, false);

                cell = new PdfPCell(new Phrase(unitPrice, font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cell);

            }
            doc.Add(productsTable);

            #endregion


            #region Totals




            //order total
            string orderTotalStr = _priceFormatter.FormatPrice(quotation[0].Quot_Amount, true, _workContext.WorkingCurrency, lang, false);

            var pTotal = new Paragraph(String.Format("{0} {1}", "Amount:", orderTotalStr), titleFont);
            pTotal.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pTotal);

            var GST = quotation[0].Quot_Amount * ((decimal)0.1);

            string GSTStr = _priceFormatter.FormatPrice(GST, true, _workContext.WorkingCurrency, lang, false);
            string orderTotalGSTStr = _priceFormatter.FormatPrice(GST + quotation[0].Quot_Amount, true, _workContext.WorkingCurrency, lang, false);

            var pGST = new Paragraph(String.Format("{0} {1}    {2} {3}", "GST:", GSTStr, "Total(inc.GST):", orderTotalGSTStr), titleFont);
            pGST.Alignment = Element.ALIGN_RIGHT;
            doc.Add(pGST);

            #endregion

            MakeRemmittance(ref doc, companyinfo, titleFont, font);

            #region Footer

            if (!String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) || !String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
            {
                var column1Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var column2Lines = String.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                    new List<string>() :
                    pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (column1Lines.Count > 0 || column2Lines.Count > 0)
                {
                    var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
                    const float margin = 43;

                    //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
                    int footerHeight = totalLines * 9;
                    var directContent = pdfWriter.DirectContent;
                    directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
                    directContent.Stroke();


                    var footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100f;
                    footerTable.SetTotalWidth(new float[] { 250, 250 });

                    //column 1
                    if (column1Lines.Count > 0)
                    {
                        var column1 = new PdfPCell();
                        column1.Border = Rectangle.NO_BORDER;
                        column1.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column1Lines)
                        {
                            column1.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column1);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    //column 2
                    if (column2Lines.Count > 0)
                    {
                        var column2 = new PdfPCell();
                        column2.Border = Rectangle.NO_BORDER;
                        column2.HorizontalAlignment = Element.ALIGN_LEFT;
                        foreach (var footerLine in column2Lines)
                        {
                            column2.AddElement(new Phrase(footerLine, font));
                        }
                        footerTable.AddCell(column2);
                    }
                    else
                    {
                        var column = new PdfPCell(new Phrase(" "));
                        column.Border = Rectangle.NO_BORDER;
                        footerTable.AddCell(column);
                    }

                    footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
                }
            }

            #endregion



            doc.Close();

        }

        #endregion
    }
}