using Ats.Models.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Ats.Models.Reports
{
    public class CandidateReport 
    {
        #region Declaration
        int _totalColumn;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfPTable = new PdfPTable(7);
        PdfPCell _pdfCell;
        MemoryStream memoryStream = new MemoryStream();
        List<InterPreEmpDetailViewModel> _PreEmplists = new List<InterPreEmpDetailViewModel>();
        List<InterReferenceViewModel> _Reference = new List<InterReferenceViewModel>();
        #endregion

        public byte[] PrepateReport(GridPreInterRegisterViewModel gridPreInterRegister)
        {
            _PreEmplists = gridPreInterRegister.PreviousEmploymentDetail;

            #region
            _document = new Document(PageSize.A4, 0f,0f,0f,0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfPTable.WidthPercentage = 100;
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            PdfWriter.GetInstance(_document, memoryStream);
            //PdfWriter writer = PdfWriter.GetInstance(_document, new FileStream( + "/candidate.pdf", FileMode.Create));
            _document.Open();
            //PdfPTable table = new PdfPTable(3);
            //table.TotalWidth = 144f;
            //table.LockedWidth = true;
            //PdfPCell cell = new PdfPCell(new Phrase("This is table 1"));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 1;
            //table.AddCell(cell);
            //table.AddCell("Col 1 Row 1");
            //table.AddCell("Col 2 Row 1");
            //table.AddCell("Col 3 Row 1");
            //table.AddCell("Col 1 Row 2");
            //table.AddCell("Col 2 Row 2");
            //table.AddCell("Col 3 Row 2");
            //table.WriteSelectedRows(0, -1, _document.Left, _document.Top, writer.DirectContent);


            //table = new PdfPTable(3);
            //table.TotalWidth = 144f;
            //table.LockedWidth = true;
            //cell = new PdfPCell(new Phrase("This is table 2"));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 1;
            //table.AddCell(cell);
            //table.AddCell("Col 1 Row 1");
            //table.AddCell("Col 2 Row 1");
            //table.AddCell("Col 3 Row 1");
            //table.AddCell("Col 1 Row 2");
            //table.AddCell("Col 2 Row 2");
            //table.AddCell("Col 3 Row 2");
            //table.WriteSelectedRows(0, -1, _document.Left + 200, _document.Top, writer.DirectContent);







            _pdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 20f, 20f, 20f });
            #endregion
            this.ReportHeader();
            this.ReportBody();
            _pdfPTable.HeaderRows = 2;
            _document.Add(_pdfPTable);
            _document.Close();
            return memoryStream.ToArray();


        }
        private void ReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 11f, 1);
            _pdfCell = new PdfPCell(new Phrase("DashTechInc", _fontStyle));
            _pdfCell.Colspan = _totalColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfCell);
            _pdfPTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Previous Employement Detail", _fontStyle));
            _pdfCell.Colspan = _totalColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfCell);
            _pdfPTable.CompleteRow();
        }
        private void ReportBody()
        {          


            #region Table header
            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("#", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("Company Name", _fontStyle));           
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("City", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("Designation", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("WorkFrom ", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("WorkTo", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 1);
            _pdfCell = new PdfPCell(new Phrase("CtcMonth", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPTable.AddCell(_pdfCell);

            _pdfPTable.CompleteRow();
            #endregion
            #region TableBody
            _fontStyle = FontFactory.GetFont("Arial Rounded MT", 8f, 0);
            int serialNumber = 1;
            foreach (var pre in _PreEmplists)
            {
                DateTime frmDate = DateTime.ParseExact(pre.WorkFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(pre.WorkTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _pdfCell = new PdfPCell(new Phrase(serialNumber++.ToString(), _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(pre.CompanyName, _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(pre.City, _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(pre.City, _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(frmDate.ToString("Y"), _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(toDate.ToString("Y"), _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(pre.CtcMonth, _fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfPTable.AddCell(_pdfCell);
                _pdfPTable.CompleteRow();
            }
            #endregion

        }
    }
}