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

namespace Nop.Services.Omni_Backoffice
{
    /// <summary>
    /// PDF service
    /// </summary>
    public partial class PdfService_Class : IPdfService_Class
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


        private readonly ICodeMasterService _codemasterService;
        private readonly ICourseMasterService _coursemasterService;
        private readonly ICampusService _campusService;
        private readonly IClassService _classService;




        private readonly CatalogSettings _catalogSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly MeasureSettings _measureSettings;
        private readonly PdfSettings _pdfSettings;
        private readonly TaxSettings _taxSettings;
        private readonly AddressSettings _addressSettings;

        #endregion

        #region Ctor

        public PdfService_Class(ILocalizationService localizationService,
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

            ICodeMasterService codemasterService,
            ICourseMasterService coursemasterService,
            ICampusService campusService,
            IClassService classService,


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

            this._codemasterService = codemasterService;
            this._coursemasterService = coursemasterService;
            this._campusService = campusService;
            this._classService = classService;
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

        private void MakeCompanyHeader(ref Document doc, Campus branch, PdfSettings pdfSettingsByStore
            , Font titleFont, Font titleFont_B, Font font, string copy)
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
            var cell_l1 = new PdfPCell(logo);
            cell_l1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_l1.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cell_l1);

            var cell_r1 = new PdfPCell();
            cell_r1.Border = Rectangle.NO_BORDER;
            cell_r1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_r1.AddElement(new Phrase(copy, font));
            titleTable.AddCell(cell_r1);


            var cell_l2 = new PdfPCell();
            cell_l2.Border = Rectangle.NO_BORDER;
            cell_l2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l2.AddElement(new Phrase("Office : CS Education," + branch.Campus_Title.Replace("CS", ""), titleFont));
            cell_l2.AddElement(new Phrase("Phone  : " + branch.PhoneNo, titleFont));
            cell_l2.AddElement(new Phrase("ABN    : " + branch.ABN, titleFont));
            titleTable.AddCell(cell_l2);

            var cell_r2 = new PdfPCell();
            cell_r2.Border = Rectangle.NO_BORDER;
            cell_l2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l2.AddElement(new Phrase("TAX INVOICE", titleFont));
            cell_l2.AddElement(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), titleFont));
            titleTable.AddCell(cell_r2);

            doc.Add(titleTable);

        }


        private void MakeStudentInfo(ref Document doc, string termstring, string termstring_short, Member student, ClassEnrol_Pay payment, Font titleFont, Font font)
        {

            var lineTable = new PdfPTable(2);
            lineTable.SetWidths(new[] { 50, 50 });
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            var phrase1 = new Paragraph("Year : " + student.grade.ToString() + "   Name : " + student.stud_first_name + "  " + student.stud_last_name, titleFont);
            phrase1.Alignment = Element.ALIGN_LEFT;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            var phrase2 = new Paragraph("Payment Date : " + payment.PayDate.ToString("dd/MM/yyyy"), titleFont);
            phrase2.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(phrase2);
            lineTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(" ", font));
            cell.Border = Rectangle.NO_BORDER;
            lineTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(" ", font));
            cell.Border = Rectangle.NO_BORDER;
            lineTable.AddCell(cell);

            doc.Add(lineTable);

        }

        private void MakeEnrolInfo(ref Document doc, string termstring, string termstring_short, ClassEnrol enrol, Font titleFont, Font font,  Core.Domain.Localization.Language lang)
        {

            var enrolTable = new PdfPTable(6);
            enrolTable.WidthPercentage = 100f;
            enrolTable.SetWidths(new[] { 20, 60, 50, 20, 35, 15 });

            var cell = new PdfPCell(new Phrase("Term", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Course", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Class", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("No of Week/Day", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Amount", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("By", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            //term
            cell = new PdfPCell();
            cell.AddElement(new Paragraph(termstring, font));
            cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            var attributesParagraph1 = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), font);
            cell.AddElement(attributesParagraph1);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            enrolTable.AddCell(cell);

            //course
            var course = _coursemasterService.GetAllCourseMasters(enrol.Year, enrol.Term, "", "").Where(x => x.Id == enrol.Course_Id).ToList()[0];
            string name = course.CourseName.Replace("OLY ", "").Replace("ADV ", "").Replace("ELITE", "");
            cell = new PdfPCell();
            cell.AddElement(new Paragraph(name, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            var attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), font);
            cell.AddElement(attributesParagraph);
            enrolTable.AddCell(cell);

            //class
            var classinfo = _classService.GetClassInfoById(enrol.Class_Id);
            cell = new PdfPCell();
            cell.AddElement(new Paragraph(classinfo.Name, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), font);
            cell.AddElement(attributesParagraph);
            enrolTable.AddCell(cell);

            //no of week
            cell = new PdfPCell();
            cell.AddElement(new Paragraph(enrol.AttendWeek.ToString(), font));
            attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), font);
            cell.AddElement(attributesParagraph);
            enrolTable.AddCell(cell);

            //subtotal
            string subTotal = string.Empty;
            subTotal = _priceFormatter.FormatPrice(enrol.Unit_Price * enrol.AttendWeek, true, _workContext.WorkingCurrency, lang, false);

            cell = new PdfPCell(new Phrase(subTotal, font));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            enrolTable.AddCell(cell);

            //By
            string paymethod = _codemasterService.GetAllCodeMasters("CL10", "Y").Where(x => x.DetailCode == enrol.PayMethod).ToList()[0].DetailName;

            cell = new PdfPCell();
            cell.AddElement(new Paragraph(paymethod, font));
            attributesParagraph = new Paragraph(HtmlHelper.ConvertHtmlToPlainText("", true, true), font);
            attributesParagraph.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(attributesParagraph);
            enrolTable.AddCell(cell);

            doc.Add(enrolTable);

        }

        private void MakePaymentInfo(ref Document doc, string termstring, string termstring_short, ClassEnrol enrol, ClassEnrol_Pay pay, Font titleFont, Font font, Core.Domain.Localization.Language lang)
        {

            var payTable = new PdfPTable(3);
            payTable.WidthPercentage = 100f;
            payTable.SetWidths(new[] { 130, 40, 30 });

            var cell = new PdfPCell(new Paragraph("Memo : " + enrol.Remarks + System.Environment.NewLine + pay.Remarks, font));
            cell.BackgroundColor = Color.WHITE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            payTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("DISCOUNT", font));


            cell.AddElement(new Phrase("DISCOUNT", font));
            cell.AddElement(new Phrase("GST", font));
            cell.AddElement(new Phrase("TEXT BOOK", font));

            if (enrol.PayMethod == "CRCD")
            {
                cell.AddElement(new Phrase("CARD", font));
            }
            else
            {
                cell.AddElement(new Phrase(" ", font));
            }


            
            cell.AddElement(new Phrase("TOTAL", font));
            cell.AddElement(new Phrase("BALANCE BEFORE", font));
            cell.AddElement(new Phrase("PAID", font));
            cell.AddElement(new Phrase("CURRENT BALANCE", font));
            cell.AddElement(new Phrase(" ", font));


            payTable.AddCell(cell);




            cell = new PdfPCell(new Phrase(enrol.Discount_Rate.ToString( "##.00") + "%", font));
            cell.BackgroundColor = Color.WHITE;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;

            var discountphrase = new Paragraph(enrol.Discount_Rate.ToString("##.00") + "%", font);
            discountphrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(discountphrase);

            var gstphrase = new Paragraph("(10%)", font);
            gstphrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(gstphrase);

            string subTotal = string.Empty;
            subTotal = _priceFormatter.FormatPrice(enrol.MaterialFee, true, _workContext.WorkingCurrency, lang, false);
            var materialphrase = new Paragraph(subTotal, font);
            materialphrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(materialphrase);


            if (enrol.PayMethod == "CRCD")
            {
                var cardphrase = new Paragraph("(1.3%)", font);
                cardphrase.Alignment = Element.ALIGN_RIGHT;
                cell.AddElement(cardphrase);
            }
            else
            {
                cell.AddElement(new Phrase(" ", font));
            }


            string Total = string.Empty;
            Total = _priceFormatter.FormatPrice(enrol.TotalAmount, true, _workContext.WorkingCurrency, lang, false);
            var totalphrase = new Paragraph(Total, font);
            totalphrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(totalphrase);


            var paid = _classService.GetAllClassEnrol_Pays(enrol.Id).Where(x => x.Seq <= pay.Seq).Sum(x => x.PayAmount);

            string before = string.Empty;
            before = _priceFormatter.FormatPrice( (enrol.TotalAmount - paid + pay.PayAmount), true, _workContext.WorkingCurrency, lang, false);
            var beforephrase = new Paragraph(before, font);
            beforephrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(beforephrase);

            string curpay = string.Empty;
            curpay = _priceFormatter.FormatPrice(pay.PayAmount, true, _workContext.WorkingCurrency, lang, false);
            var curpayphrase = new Paragraph(curpay, font);
            curpayphrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(curpayphrase);

            string curbal = string.Empty;
            curbal = _priceFormatter.FormatPrice(enrol.TotalAmount - paid, true, _workContext.WorkingCurrency, lang, false);
            var curbalphrase = new Paragraph(curbal, font);
            curbalphrase.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(curbalphrase);

            cell.AddElement(new Phrase(" ", font));

            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            payTable.AddCell(cell);



            doc.Add(payTable);

        }

        private void MakeFooter(ref Document doc, Font titleFont, Font font)
        {

            var lineTable = new PdfPTable(2);
            lineTable.SetWidths(new[] { 50, 50 });
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            var phrase1 = new Paragraph("CS Education", font);
            phrase1.Alignment = Element.ALIGN_LEFT;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            var phrase2 = new Paragraph("Any time, Any where, Always", font);
            phrase2.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(phrase2);
            lineTable.AddCell(cell);

            doc.Add(lineTable);

        }

        public virtual void PrintReceiptToPdf(Stream stream, int Id_Payment)
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

            var payment = _classService.GetClassEnrol_PayById(Id_Payment);
            var enrol = _classService.GetClassEnrolById(payment.Id_Enrol);
            var classinfo = _classService.GetClassInfoById(enrol.Class_Id);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == enrol.Term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == enrol.Term).ToList()[0].String_01;

            var branch = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == classinfo.Branch).ToList()[0];
            var student = _classService.GetAllMembers("", "", "", "", enrol.Stud_Id, "").ToList()[0];

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;


            // office
            MakeCompanyHeader(ref doc, branch, pdfSettingsByStore, titleFont, titleFont_B, font, "(office copy)");
            MakeStudentInfo(ref doc, termstring, termstring_short, student, payment, titleFont, font);
            MakeEnrolInfo(ref doc, termstring, termstring_short, enrol, titleFont, font, lang);
            MakePaymentInfo(ref doc, termstring, termstring_short, enrol, payment, titleFont, font, lang);
            MakeFooter(ref doc, titleFont, font);




            // line
            var lineTable = new PdfPTable(1);
            lineTable.WidthPercentage = 100f;
            lineTable.HorizontalAlignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            var phrase1 = new Paragraph("  ", titleFont_MB);
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            phrase1 = new Paragraph("  ", titleFont_MB);
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.BOTTOM_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            phrase1 = new Paragraph("  ", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.TOP_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            phrase1 = new Paragraph("  ", titleFont_MB);
            phrase1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            phrase1 = new Paragraph("  ", titleFont_MB);
            cell.AddElement(phrase1);
            lineTable.AddCell(cell);



            doc.Add(lineTable);


            // customer
            MakeCompanyHeader(ref doc, branch, pdfSettingsByStore, titleFont, titleFont_B, font, "(customer copy)");
            MakeStudentInfo(ref doc, termstring, termstring_short, student, payment, titleFont, font);
            MakeEnrolInfo(ref doc, termstring, termstring_short, enrol, titleFont, font, lang);
            MakePaymentInfo(ref doc, termstring, termstring_short, enrol, payment, titleFont, font, lang);
            MakeFooter(ref doc, titleFont, font);



            doc.Close();



        }



        public virtual void PrintRollcallSheetToPdf(Stream stream, int id_class)
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
            font.Size = 10f;
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var classinfo = _classService.GetClassInfoById(id_class);
            var enrollist = _classService.GetAllClassEnrols(id_class);

            var students = _classService.GetAllMembers(classinfo.Branch, "", "", "", "", "");

            var studentlist = _classService.GetAllClassScheduleRollcalls(id_class, 0).GroupBy(group => new { group.Stud_Id })
                .Select(group => new { group.Key.Stud_Id })
                .OrderBy(x => x.Stud_Id.Replace("[", "ZZZZZ")).ToList();

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].String_01;
            var branch = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == classinfo.Branch).ToList()[0];


            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            var titleTable = new PdfPTable(2);
            titleTable.WidthPercentage = 100f;
            titleTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //logo
            var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);

            titleTable.SetWidths(new[] { 30, 70 });
            var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
            Image logo = Image.GetInstance(logoFilePath);
            logo.ScalePercent(12f);
            var cell_l1 = new PdfPCell(logo);
            cell_l1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_l1.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cell_l1);

            var cell_r1 = new PdfPCell();
            cell_r1.Border = Rectangle.NO_BORDER;
            cell_r1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_r1.AddElement(new Phrase(classinfo.Year + " - " + termstring, titleFont_MB));
            cell_r1.AddElement(new Phrase("Roll call sheet - Class : " + classinfo.Name, titleFont_MB));
            titleTable.AddCell(cell_r1);



            doc.Add(titleTable);


            // student list

            var courseinfo = _coursemasterService.GetCourseMasterById(classinfo.Course_Id);

            var rollcallTable = new PdfPTable(courseinfo.TotalWeek + 5);
            rollcallTable.WidthPercentage = 100f;
            int[] setwitdh = new int[courseinfo.TotalWeek + 5];
            for (int i = 0; i < courseinfo.TotalWeek + 5; i++ )
            {
                switch ( i )
                {
                    case 0: setwitdh[i] = 25; break;
                    case 1: setwitdh[i] = 100; break;
                    case 2: setwitdh[i] = 70; break;
                    default: setwitdh[i] = 25; break;
                }
                
            }

            rollcallTable.SetWidths(setwitdh);

            //#
            var cell = new PdfPCell(new Phrase("#", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Name
            cell = new PdfPCell(new Phrase("Name", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);


            //Student #
            cell = new PdfPCell(new Phrase("Student #", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            // unit or week
            for (int i = 1; i <= courseinfo.TotalWeek; i++)
            {
                cell = new PdfPCell(new Phrase(i.ToString(), font));
                cell.BackgroundColor = Color.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);
            }

            //H.D
            cell = new PdfPCell(new Phrase("H.D", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //H.D
            cell = new PdfPCell(new Phrase("F.M", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            int[] attendance = new int[courseinfo.TotalWeek];
            for (int i = 0; i < courseinfo.TotalWeek; i++ )
            {
                attendance[i] = 0;
            }

            if (studentlist.Count > 25)
                font.Size = 9.5f;

            for (int i = 0; i < 30; i++)
            {
                if (studentlist.Count <= i)
                {
                    for (int j = 0; j < courseinfo.TotalWeek + 6; j++)
                    {
                        cell = new PdfPCell(new Phrase(" " , font));
                        rollcallTable.AddCell(cell);
                    }
                    continue;
                }

                

                string studentname = "";
                string studentno = "";
                string studentid = "";
                string level = "";
                string tmp_hd = "";
                string tmp_fm = "";




                var student = students.Where(x => x.stud_id == studentlist[i].Stud_Id).ToList();
                var enrol = enrollist.Where(x => x.Stud_Id == studentlist[i].Stud_Id && x.Class_Id == classinfo.Id).ToList();


                if ( student.Count > 0)
                {
                    studentname = student[0].stud_first_name + System.Environment.NewLine + student[0].stud_last_name;
                    studentno = student[0].id_number;
                    studentid = student[0].stud_id;

                    
                    if ( enrol.Count > 0)
                    {
                        level = enrol[0].Remarks;
                        var addinfo = _classService.GetAllAdditionInfos(studentid).ToList();

                        if (addinfo.Count > 0 && !String.IsNullOrEmpty(addinfo[0].Actual_Grade))
                        {
                            level = addinfo[0].Actual_Grade + level;
                        }

                        var course = _coursemasterService.GetCourseMasterById(enrol[0].Course_Id);

                        if ( course != null )
                        {
                            var coursetype = _codemasterService.GetAllCodeMasters("CM04", "").Where(x => x.DetailCode == course.CourseCategory).ToList();
                            if ( coursetype[0].String_01 == "NORMAL" && coursetype[0].DetailCode != "HIGH" && coursetype[0].DetailCode != "HOLI" )
                            {
                                if ( coursetype[0].DetailCode != "WEMG")
                                {
                                    level = coursetype[0].DetailCode.Replace("_", "") + level;
                                }
                            }
                        }


                        tmp_hd = String.IsNullOrEmpty(enrol[0].Chk_HD) ? " " : enrol[0].Chk_HD == "Y" ? "√" : "ㆍ";
                        tmp_fm = String.IsNullOrEmpty(enrol[0].Chk_FM) ? " " : enrol[0].Chk_FM == "Y" ? "√" : "ㆍ";
                    }
                    else  // 학생이 이동된것으로 간주
                    {
                        level = "MOVED";
                    }
                }
                else
                {
                    studentname = studentlist[i].Stud_Id;
                    studentno = "MAKEUP";
                    studentid = studentlist[i].Stud_Id;
                    level = "";
                }


                //Name
                cell = new PdfPCell(new Phrase((i + 1).ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Name
                cell = new PdfPCell(new Phrase(studentname, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);


                //student no 
                cell = new PdfPCell(new Phrase(studentno, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                var rollcall = _classService.GetAllClassScheduleRollcalls(classinfo.Id, 0).Where(x => x.Stud_Id == studentid);
                var class_d = _classService.GetAllClassSchedules(classinfo.Id).OrderBy(x => x.Id).ToList();

                //weeks
                for (int j = 0; j < courseinfo.TotalWeek; j++)
                {
                    string temp_rollcall = " ";

                    var stud_rollcall = rollcall.Where(x => x.Class_D_Id == (class_d.Count <= j ? -1 : class_d[j].Id)).ToList();

                    if (stud_rollcall.Count > 0)
                    {
                        temp_rollcall = (String.IsNullOrEmpty(stud_rollcall[0].Attend) || class_d[j].YN_Close != "Y") ? " " : (stud_rollcall[0].Attend == "Y" )  ? "√" : "ㆍ";
                    }

                    if (temp_rollcall == "√")
                    {
                        attendance[j]++;
                    }

                    cell = new PdfPCell(new Phrase(temp_rollcall, font));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    rollcallTable.AddCell(cell);
                }



                //H.D
                cell = new PdfPCell(new Phrase(tmp_hd, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                //F.M
                cell = new PdfPCell(new Phrase(tmp_fm, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

            }


            cell = new PdfPCell(new Paragraph(" ", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            for (int i = 0; i < courseinfo.TotalWeek; i++ )
            {
                cell = new PdfPCell(new Paragraph( attendance[i] == 0 ? "" : attendance[i].ToString() , font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rollcallTable.AddCell(cell);
            }

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);




            doc.Add(rollcallTable);

            doc.Close();

        }



        public virtual void PrintEnrolmentSheetToPdf(Stream stream, int id_class)
        {

            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4.Rotate();

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
            font.Size = 12f;
            var sfont = GetFont();
            sfont.Size = 8f;
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var classinfo = _classService.GetClassInfoById(id_class);
            var enrollist = _classService.GetAllClassEnrols(id_class).OrderBy( x => x.Id ).ToList();

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].String_01;
            var branch = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == classinfo.Branch).ToList()[0];


            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            var titleTable = new PdfPTable(2);
            titleTable.WidthPercentage = 100f;
            titleTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //logo
            var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);

            titleTable.SetWidths(new[] { 30, 70 });
            var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
            Image logo = Image.GetInstance(logoFilePath);
            logo.ScalePercent(12f);
            var cell_l1 = new PdfPCell(logo);
            cell_l1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_l1.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cell_l1);

            var cell_r1 = new PdfPCell();
            cell_r1.Border = Rectangle.NO_BORDER;
            cell_r1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_r1.AddElement(new Phrase(classinfo.Year + " - " + termstring, titleFont_MB));
            cell_r1.AddElement(new Phrase("Enrolment List - Class : " + classinfo.Name, titleFont_MB));
            titleTable.AddCell(cell_r1);



            doc.Add(titleTable);


            // student list

            var courseinfo = _coursemasterService.GetCourseMasterById(classinfo.Course_Id);

            var rollcallTable = new PdfPTable(11);
            rollcallTable.WidthPercentage = 100f;
            rollcallTable.SetWidths(new[] {25, 100, 25, 40, 40, 50, 40, 70, 70, 70, 150 });
           

            //#
            var cell = new PdfPCell(new Phrase("#", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Name
            cell = new PdfPCell(new Phrase("Name", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Unit
            cell = new PdfPCell(new Phrase("Unit", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Course
            cell = new PdfPCell(new Phrase("Course", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Level
            cell = new PdfPCell(new Phrase("Level", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Student #
            cell = new PdfPCell(new Phrase("Student #", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Status
            cell = new PdfPCell(new Phrase("Status", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);


            //Home Phone
            cell = new PdfPCell(new Phrase("Home Phone", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Mobile
            cell = new PdfPCell(new Phrase("Mobile", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Mobile2
            cell = new PdfPCell(new Phrase("Mobile2", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Remarks
            cell = new PdfPCell(new Phrase("Remarks", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);


           

            for (int i = 0; i < 25; i++)
            {
                if (enrollist.Count <= i )
                {
                    for ( int j = 0 ; j < 9 ; j++)
                    {
                        cell = new PdfPCell(new Phrase(" ", font));
                        rollcallTable.AddCell(cell);
                    }
                    continue;
                    
                }
                var student = _classService.GetAllMembers("", "", "", "", enrollist[i].Stud_Id, "").ToList()[0];

                //#
                cell = new PdfPCell(new Phrase((i + 1).ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Name
                cell = new PdfPCell(new Phrase(student.stud_first_name + " " + student.stud_last_name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Week
                cell = new PdfPCell(new Phrase(enrollist[i].AttendWeek.ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rollcallTable.AddCell(cell);

                //CourseCategory 
                var realcourse = _coursemasterService.GetCourseMasterById(enrollist[i].Course_Id);
                var realcoursecategory = _codemasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.DetailCode == realcourse.CourseCategory).ToList()[0];
                cell = new PdfPCell(new Phrase(realcoursecategory.DetailName, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                //level 
                cell = new PdfPCell(new Phrase(enrollist[i].Level.Replace("_", ""), font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                //student no 
                cell = new PdfPCell(new Phrase(student.id_number, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                //status 
                cell = new PdfPCell(new Phrase(enrollist[i].PayStatus == "P020" ? "PAID" : "", font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                //Homephone 
                cell = new PdfPCell(new Phrase(student.Telephone, sfont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Mobile 
                cell = new PdfPCell(new Phrase(student.MobilePhone, sfont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Mobile2 
                cell = new PdfPCell(new Phrase(student.ParentMobilePhone, sfont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Remarks 
                cell = new PdfPCell(new Phrase(enrollist[i].Remarks, sfont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);


            }
            doc.Add(rollcallTable);



            doc.Close();





        }


        public virtual void PrintNameSheetToPdf(Stream stream, int id_class)
        {

            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4.Rotate();

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
            font.Size = 12f;
            var sfont = GetFont();
            sfont.Size = 8f;
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var classinfo = _classService.GetClassInfoById(id_class);
            var enrollist = _classService.GetAllClassEnrols(id_class).OrderBy(x => x.Id).ToList();

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].String_01;
            var branch = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == classinfo.Branch).ToList()[0];


            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            var titleTable = new PdfPTable(2);
            titleTable.WidthPercentage = 100f;
            titleTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //logo
            var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);

            titleTable.SetWidths(new[] { 30, 70 });
            var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
            Image logo = Image.GetInstance(logoFilePath);
            logo.ScalePercent(12f);
            var cell_l1 = new PdfPCell(logo);
            cell_l1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_l1.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cell_l1);

            var cell_r1 = new PdfPCell();
            cell_r1.Border = Rectangle.NO_BORDER;
            cell_r1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_r1.AddElement(new Phrase(classinfo.Year + " - " + termstring, titleFont_MB));
            cell_r1.AddElement(new Phrase("Name List - Class : " + classinfo.Name, titleFont_MB));
            titleTable.AddCell(cell_r1);



            doc.Add(titleTable);


            // student list

            var courseinfo = _coursemasterService.GetCourseMasterById(classinfo.Course_Id);

            var rollcallTable = new PdfPTable(11);
            rollcallTable.WidthPercentage = 100f;
            rollcallTable.SetWidths(new[] { 25,25,25,25,25, 100, 25, 40, 50, 150, 40 });

            //blank1
            var cell = new PdfPCell(new Phrase(" ", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //blank2
            cell = new PdfPCell(new Phrase(" ", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //blank3
            cell = new PdfPCell(new Phrase(" ", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //online
            cell = new PdfPCell(new Phrase("online", sfont));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //#
            cell = new PdfPCell(new Phrase("#", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Name
            cell = new PdfPCell(new Phrase("Name", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //WEEK
            cell = new PdfPCell(new Phrase("Wks", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Level
            cell = new PdfPCell(new Phrase("Level", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //SIB
            cell = new PdfPCell(new Phrase("SIB", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Note
            cell = new PdfPCell(new Phrase("Note", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);


            //Invoice
            cell = new PdfPCell(new Phrase("Invoice", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);




            for (int i = 0; i < 28; i++)
            {
                if (enrollist.Count <= i)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        cell = new PdfPCell(new Phrase(" ", font));
                        rollcallTable.AddCell(cell);
                    }
                    continue;

                }
                var student = _classService.GetAllMembers("", "", "", "", enrollist[i].Stud_Id, "").ToList()[0];

                //blank
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //blank
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //blank
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //online
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);




                //#
                cell = new PdfPCell(new Phrase((i + 1).ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Name
                cell = new PdfPCell(new Phrase(student.stud_first_name + " " + student.stud_last_name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Week
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rollcallTable.AddCell(cell);

                //level 
                cell = new PdfPCell(new Phrase(enrollist[i].Level.Replace("_", "").Replace("XXXX", "    "), font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                //SIB
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);


                //Remarks 
                cell = new PdfPCell(new Phrase(enrollist[i].Remarks, sfont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Invoice
                cell = new PdfPCell(new Phrase(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);


            }
            doc.Add(rollcallTable);



            doc.Close();

            
        }



        public virtual void PrintClassTestSheetToPdf(Stream stream, int id_class)
        {

            if (stream == null)
                throw new ArgumentNullException("stream");

            var pageSize = PageSize.A4.Rotate();

            var doc = new Document(pageSize);
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            //fonts
            var titleFont_B = GetFont();
            titleFont_B.SetStyle(Font.BOLD);
            titleFont_B.Color = Color.BLACK;
            titleFont_B.Size = 14f;
            var titleFont_MB = GetFont();
            titleFont_MB.SetStyle(Font.BOLD);
            titleFont_MB.Color = Color.BLACK;
            titleFont_MB.Size = 18f;
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = Color.BLACK;
            var font = GetFont();
            font.Size = 12f;
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            var classinfo = _classService.GetClassInfoById(id_class);
            var enrollist = _classService.GetAllClassEnrols(id_class);

            var termstring = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].DetailName;
            var termstring_short = _codemasterService.GetAllCodeMasters("CM02", "Y").Where(x => x.DetailCode == classinfo.Term).ToList()[0].String_01;
            var branch = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == classinfo.Branch).ToList()[0];


            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = _settingContext.LoadSetting<PdfSettings>(0);


            var lang = _languageService.GetLanguageById(0);
            if (lang == null || !lang.Published)
                lang = _workContext.WorkingLanguage;

            var titleTable = new PdfPTable(2);
            titleTable.WidthPercentage = 100f;
            titleTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //logo
            var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);

            titleTable.SetWidths(new[] { 30, 70 });
            var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
            Image logo = Image.GetInstance(logoFilePath);
            logo.ScalePercent(12f);
            var cell_l1 = new PdfPCell(logo);
            cell_l1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_l1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_l1.Border = Rectangle.NO_BORDER;
            titleTable.AddCell(cell_l1);

            var cell_r1 = new PdfPCell();
            cell_r1.Border = Rectangle.NO_BORDER;
            cell_r1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_r1.AddElement(new Phrase(classinfo.Year + " - " + termstring + "  Class Test - Class : " + classinfo.Name, titleFont_MB));
            titleTable.AddCell(cell_r1);

            doc.Add(titleTable);


            // check table
            var checkTable = new PdfPTable(9);
            checkTable.WidthPercentage = 100f;
            checkTable.SetWidths(new[] { 120, 70, 40, 70, 40, 70, 40, 70, 40});

            var cell_check1 = new PdfPCell();
            cell_check1.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check1.BackgroundColor = Color.LIGHT_GRAY;
            cell_check1.AddElement(new Phrase(" OFFICE USE ONLY ", titleFont_B));
            checkTable.AddCell(cell_check1);

            var cell_check2 = new PdfPCell();
            cell_check2.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check2.BackgroundColor = Color.LIGHT_GRAY;
            cell_check2.AddElement(new Phrase("W-Moderation", titleFont_B));
            checkTable.AddCell(cell_check2);

            var cell_check3 = new PdfPCell();
            cell_check3.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check3.AddElement(new Phrase(" ", titleFont_B));
            checkTable.AddCell(cell_check3);

            cell_check2 = new PdfPCell();
            cell_check2.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check2.BackgroundColor = Color.LIGHT_GRAY;
            cell_check2.AddElement(new Phrase("ID - ENG", titleFont_B));
            checkTable.AddCell(cell_check2);

            cell_check3 = new PdfPCell();
            cell_check3.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check3.AddElement(new Phrase(" ", titleFont_B));
            checkTable.AddCell(cell_check3);

            cell_check2 = new PdfPCell();
            cell_check2.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check2.BackgroundColor = Color.LIGHT_GRAY;
            cell_check2.AddElement(new Phrase("ID - Writing", titleFont_B));
            checkTable.AddCell(cell_check2);

            cell_check3 = new PdfPCell();
            cell_check3.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check3.AddElement(new Phrase(" ", titleFont_B));
            checkTable.AddCell(cell_check3);

            cell_check2 = new PdfPCell();
            cell_check2.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check2.BackgroundColor = Color.LIGHT_GRAY;
            cell_check2.AddElement(new Phrase("ID - MA", titleFont_B));
            checkTable.AddCell(cell_check2);

            cell_check3 = new PdfPCell();
            cell_check3.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            cell_check3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_check3.AddElement(new Phrase(" ", titleFont_B));
            checkTable.AddCell(cell_check3);

            doc.Add(checkTable);



            // student list

            var courseinfo = _coursemasterService.GetCourseMasterById(classinfo.Course_Id);

            var rollcallTable = new PdfPTable(11);
            rollcallTable.WidthPercentage = 100f;
            rollcallTable.SetWidths(new[] { 30, 30, 25, 130, 70, 30, 30, 30, 30, 50, 300 });
           


            //#
            var cell = new PdfPCell(new Paragraph(" ", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("#", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Name
            cell = new PdfPCell(new Phrase("Name", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);


            //Student No
            cell = new PdfPCell(new Phrase("Student No", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //W
            cell = new PdfPCell(new Phrase("W", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //E
            cell = new PdfPCell(new Phrase("E", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //M
            cell = new PdfPCell(new Phrase("M", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //G
            cell = new PdfPCell(new Phrase("G", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            //Report
            cell = new PdfPCell(new Phrase("Report", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", font));
            cell.BackgroundColor = Color.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            rollcallTable.AddCell(cell);


            for (int i = 0; i < 25; i++)
            {
                if (enrollist.Count <= i)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        cell = new PdfPCell(new Paragraph(" ", font));
                        rollcallTable.AddCell(cell);
                    }
                    continue;

                }

                cell = new PdfPCell(new Paragraph(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                cell = new PdfPCell(new Paragraph(" ", font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);


                var student = _classService.GetAllMembers("", "", "", "", enrollist[i].Stud_Id, "").ToList()[0];

                //#
                cell = new PdfPCell(new Phrase((i + 1).ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //Name
                cell = new PdfPCell(new Phrase(student.stud_first_name + " " + student.stud_last_name, font));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                rollcallTable.AddCell(cell);

                //student no 
                cell = new PdfPCell(new Phrase(student.id_number, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                rollcallTable.AddCell(cell);

                cell = new PdfPCell(new Paragraph(" ", font));
                rollcallTable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(" ", font));
                rollcallTable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(" ", font));
                rollcallTable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(" ", font));
                rollcallTable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(" ", font));
                rollcallTable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(" ", font));
                rollcallTable.AddCell(cell);


            }
            doc.Add(rollcallTable);

            doc.Close();

        }



        #endregion
    }
}