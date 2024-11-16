using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using System.Data;
using System.Web;

namespace WebComponent
{
    public class RPlusPdfGenerator_Finrex
    {
        public Document document = null;
        public static string _defaultFont = "Arial";
        public static float _defaultFontSize = 8;
        public void Generate(string fileName)
        {
            //document = new Document(iTextSharp.text.PageSize.A4, 20f, 20f, 10f, 10f);
            document = new Document(iTextSharp.text.PageSize.LETTER, 20f, 20f, 10f, 10f);
            // Create a PDF writer to write the document to a file
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
            //pdfWriter.PageEvent = new ITextEvents();
            // Open the document
            document.Open();
            //AddHeaderLine(document, "#FF0000");
        }
        public void AddHTML(string html)
        {
            html = "<font style='font-size:" + _defaultFontSize + "px;font-family:" + _defaultFont + ";'>" + html + "</font>";
            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(html), null);
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                document.Add((IElement)htmlarraylist[k]);
            }
        }
        public void AddHTML(PdfPCell cell, string html)
        {
            html = "<font style='font-size:" + _defaultFontSize + "px;font-family:" + _defaultFont + ";'>" + html + "</font>";
            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(html), null);
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                cell.AddElement((IElement)htmlarraylist[k]);
            }
        }
        public PdfPTable AddTable(int cols)
        {
            PdfPTable table = new PdfPTable(cols);
            table.WidthPercentage = 100;
            return table;
        }
        public void AddTable(PdfPTable table)
        {
            document.Add(table);
        }
        public PdfPCell AddTableCellImg(PdfPTable table, string imgPath)
        {
            iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(imgPath, false);
            PdfPCell pcell = new PdfPCell(pic);
            pcell.Border = 0;
            table.AddCell(pcell);
            return pcell;
        }
        public PdfPCell AddTableCell(PdfPTable table, string data)
        {
            Phrase ph = new Phrase(data);
            PdfPCell pcell = new PdfPCell(ph);
            pcell.Border = 0;
            table.AddCell(pcell);
            return pcell;
        }
        public PdfPCell AddTableCellHtml(PdfPTable table, string html)
        {
            PdfPCell pcell = new PdfPCell();
            pcell.Border = 0;
            AddHTML(pcell, html);
            table.AddCell(pcell);
            return pcell;
        }
        public void Close()
        {
            document.Close();
        }
        public void AddLine(string hexaColor)
        {
            BaseColor color = GetColor(hexaColor);
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            cell.BorderWidth = 0.5f;
            cell.Padding = 5;
            cell.PaddingTop = 0;
            cell.BackgroundColor = color;
            cell.BorderColor = color;
            table.AddCell(cell);
            table.WidthPercentage = 100;
            document.Add(table);
        }
        public void AddTitle(string title, string borderColor, string textColor)
        {
            BaseColor bcolor = GetColor(borderColor);
            BaseColor tcolor = GetColor(textColor);
            PdfPTable table = new PdfPTable(1);
            table.SpacingBefore = 10;
            PdfPCell cell = new PdfPCell();
            cell.Padding = 3;
            cell.PaddingTop = 0;
            //cell.BackgroundColor = bcolor;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            cell.BorderColor = bcolor;
            cell.PaddingBottom = 10;
            Paragraph ph = new Paragraph(title);
            //Font f = FontFactory.GetFont(_defaultFont, _defaultFontSize, iTextSharp.text.Font.BOLD, tcolor);
            Font f = FontFactory.GetFont(_defaultFont, _defaultFontSize, tcolor);
            ph.Font = f;
            cell.AddElement(ph);
            Chunk c;
            table.WidthPercentage = 100;
            table.AddCell(cell);
            document.Add(table);
        }
        public void AddSpace()
        {
            AddSpace(10, 10);
        }
        public void AddSpace(float before, float after)
        {
            PdfPTable table = new PdfPTable(1);
            table.SpacingBefore = before;
            table.SpacingAfter = before;
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            Paragraph ph = new Paragraph("");
            cell.AddElement(ph);
            table.AddCell(cell);
            document.Add(table);
        }
        public void AddTable(DataTable dttbl, string header, string columns)
        {
            AddTable(dttbl, header, columns, "", "");
        }
        public void AddTable(DataTable dttbl, string header, string columns, string borderColor, string headerBgColor)
        {
            if (borderColor == "") borderColor = "#888888";
            if (headerBgColor == "") headerBgColor = "#eeeeee";
            Font defaultFont = GetDefaultFont();
            BaseColor bcolor = GetColor(borderColor);
            BaseColor hcolor = GetColor(headerBgColor);
            Array arrheader = header.Split(',');
            Array arrcols = columns.Split(',');
            PdfPTable table = new PdfPTable(arrheader.Length);
            table.SpacingBefore = 10;
            table.SpacingAfter = 10;
            //table.SetTotalWidth(new float[] { 200, 50, 50, 50 });
            table.WidthPercentage = 100;
            ArrayList arrheaders = new ArrayList();
            table.HeaderRows = 1;
            for (int i = 0; i < arrheader.Length; i++)
            {
                PdfPHeaderCell cell = new PdfPHeaderCell();
                cell.Name = "header" + i;
                cell.BorderColor = bcolor;
                cell.BackgroundColor = hcolor;
                cell.BorderWidthBottom = 1f;
                cell.BorderWidthLeft = 1f;
                cell.BorderWidthTop = 1f;
                if (i == arrheader.Length - 1)
                {
                    cell.BorderWidthRight = 1f;
                }
                else
                {
                    cell.BorderWidthRight = 0;
                }
                cell.Padding = 5;
                Paragraph ph = new Paragraph(arrheader.GetValue(i).ToString());
                Font f = FontFactory.GetFont(_defaultFont, _defaultFontSize, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                ph.Font = f;
                cell.AddElement(ph);
                //cell.AddElement(GetHeader("header1"));

                cell.Scope = PdfPHeaderCell.ROW;
                //PdfPHeaderCell hcell = new PdfPHeaderCell();
                //Phrase p1 = new Phrase("test");
                //hcell.AddElement(p1);
                //cell.AddHeader(hcell);
                
                //table.KeepTogether = true;
                arrheaders.Add(cell);
                table.AddCell(cell);
            }

            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                for (int j = 0; j < arrcols.Length; j++)
                {
                    string colName = arrcols.GetValue(j).ToString();
                    string data = GlobalUtilities.ConvertToString(dttbl.Rows[i][colName]);
                    PdfPCell cell = new PdfPCell();
                    cell.Padding = 5;
                    cell.BorderColor = bcolor;
                    cell.BorderWidthBottom = 1f;
                    cell.BorderWidthLeft = 1f;
                    cell.BorderWidthTop = 0;
                    if (j == arrheader.Length - 1)
                    {
                        cell.BorderWidthRight = 1f;
                    }
                    else
                    {
                        cell.BorderWidthRight = 0;
                    }
                    Paragraph ph = new Paragraph(data);
                    ph.Font = defaultFont;
                    cell.AddElement(ph);
                    //cell.AddHeader((PdfPHeaderCell)arrheaders[j]);

                    table.AddCell(cell);
                }
            }
            //table.SplitLate = false;  
            document.Add(table);
            //TableHeader eventHandler = new TableHeader();
            //eventHandler.HeaderTable = table;
        }
        public void AddTableByHtml(string html)
        {
            AddTableByHtml(html, null);
        }
        public void AddTableByHtml(string html, float[] widths)
        {
            if (html == "") return;
            int trcounter = 0;
            int startIndex = 0;
            int endIndex = 0;
            html = html.Replace("<tbody>", "").Replace("</tbody>", "");
            //html = "<font style='font-size:" + _defaultFontSize + "px;font-family:" + _defaultFont + ";'>" + html + "</font>";
            int thstartindex = html.IndexOf("<tr");
            int thendindex = html.IndexOf("</tr>");
            string headertds = html.Substring(thstartindex, thendindex - thstartindex);
            int headerCount = 0;
            int hendindex = 0;
            bool isheader = false;
            int tblEndIndex1 = html.IndexOf(">");
            string tableattributes = html.Substring(6, tblEndIndex1 - 6);
            RPlusPdfTableProperties tblproperties = new RPlusPdfTableProperties();
            if (tableattributes != "")
            {
                GetTableProperties(tableattributes, tblproperties);
            }
            int tdstartIndex1 = 0;
            int tdstartIndex2 = 0;
            int tdendIndex = 0;

            int startIndexheadertr = html.IndexOf("<tr");
            int endIndexheadertr = html.IndexOf("</tr>");
            string headertrhtml = html.Substring(startIndexheadertr + 4, endIndexheadertr - startIndexheadertr - 4);

            while (true)
            {
                tdstartIndex1 = headertrhtml.IndexOf("<td", tdstartIndex1);
                if (tdstartIndex1 == -1) break;
                tdstartIndex2 = headertrhtml.IndexOf(">", tdstartIndex1);
                if (tdstartIndex2 == -1) break;
                int attrcnt = tdstartIndex2 - tdstartIndex1;
                RPlusPdfTableTdProperties tdproperties = new RPlusPdfTableTdProperties();
                string tdattributes = headertrhtml.Substring(tdstartIndex1 + 3, tdstartIndex2 - tdstartIndex1 - 3).Trim();
                GetTableTdProperties(tdattributes, tdproperties);
                int colspan = 1;
                if (tdproperties.ColSpan > 0) colspan = tdproperties.ColSpan;
                headerCount += colspan;
                tdstartIndex1 = tdstartIndex2;
            }
            PdfPTable table = new PdfPTable(headerCount);
            table.SpacingBefore = 10;
            table.SpacingAfter = 10;
            //table.SetTotalWidth(new float[] { 200, 50, 50, 50 });
            table.WidthPercentage = 100;
            if (widths != null)
            {
                table.SetTotalWidth(widths);
            }
            //table.SetTotalWidth(new float[] { 5, 10,10,25,5,5,5,10,15,10,15 });
            table.HeaderRows = 1;
            ArrayList arrheaders = new ArrayList();
            while (true)
            {
                startIndex = html.IndexOf("<tr", startIndex);
                if (startIndex == -1) break;
                endIndex = html.IndexOf("</tr>", startIndex);
                if (endIndex == -1) break;
                string trhtml = html.Substring(startIndex + 4, endIndex - startIndex - 4);

                int tdcounter = 0;
                tdstartIndex1 = 0;
                tdstartIndex2 = 0;
                tdendIndex = 0;
                while (true)
                {
                    tdstartIndex1 = trhtml.IndexOf("<td", tdstartIndex1);
                    if (tdstartIndex1 == -1) break;
                    tdstartIndex2 = trhtml.IndexOf(">", tdstartIndex1);
                    if (tdstartIndex2 == -1) break;
                    int attrcnt = tdstartIndex2 - tdstartIndex1;
                    RPlusPdfTableTdProperties tdproperties = new RPlusPdfTableTdProperties();
                    string tdattributes = trhtml.Substring(tdstartIndex1 + 3, tdstartIndex2 - tdstartIndex1 - 3).Trim();
                    tdendIndex = trhtml.IndexOf("</td>", tdstartIndex2);
                    if (tdendIndex == -1) break;
                    string data = trhtml.Substring(tdstartIndex2 + 1, tdendIndex - tdstartIndex2 - 1);
                    if (tdattributes != "")
                    {
                        GetTableTdProperties(tdattributes, tdproperties);
                    }
                    if (data == "&nbsp;") data = "";
                    bool isLastColumn = false;
                    if (tdcounter == headerCount - 1)
                    {
                        isLastColumn = true;
                    }
                    if (trcounter == 0)
                    {
                        AddTableHeaderCell(table, tblproperties, tdproperties, data, isLastColumn, tdcounter);
                    }
                    else
                    {
                        AddTableCell(table, tblproperties, tdproperties, data, isLastColumn, tdcounter);
                    }
                    int colspan = 1;
                    if (tdproperties.ColSpan > 0) colspan = tdproperties.ColSpan;
                    tdcounter += colspan;
                    tdstartIndex1 = tdendIndex;
                    if (tdcounter > 100) break;
                }
                startIndex = endIndex;
                trcounter++;
                if (trcounter > 10000) break;
            }
            document.Add(table);
        }
       
        private void AddTableHeaderCell(PdfPTable table, RPlusPdfTableProperties tblproperties, RPlusPdfTableTdProperties tdproperties, string data,
           bool isLastColumn, int tdIndex)
        {
            PdfPHeaderCell cell = new PdfPHeaderCell();
            cell.Name = "header" + tdIndex;
            SetTableCell(table, cell, tblproperties, tdproperties, data, isLastColumn, tdIndex, true);
            cell.Scope = PdfPHeaderCell.ROW;
            
            table.AddCell(cell);
        }
        private void AddTableCell(PdfPTable table, RPlusPdfTableProperties tblproperties, RPlusPdfTableTdProperties tdproperties, string data,
            bool isLastColumn, int tdIndex)
        {
            PdfPCell cell = new PdfPCell();
            SetTableCell(table, cell, tblproperties, tdproperties, data, isLastColumn, tdIndex, false);
            table.AddCell(cell);
        }
        private void SetTableCell(PdfPTable table, PdfPCell cell, RPlusPdfTableProperties tblproperties, RPlusPdfTableTdProperties tdproperties, string data,
           bool isLastColumn, int tdIndex, bool isHeader)
        {
            if (tblproperties.Border > 0)
            {
                if (tblproperties.BorderColor == null) tblproperties.BorderColor = "#888888";
                BaseColor bordercolor = GetColor(tblproperties.BorderColor);
                cell.BorderColor = bordercolor;
            }
            if (isHeader)
            {
                if (tdproperties.BgColor == null)
                {
                    tdproperties.BgColor = "#eeeeee";
                }
            }
            if (tdproperties.BgColor != null)
            {
                BaseColor bgcolor = GetColor(tdproperties.BgColor);
                cell.BackgroundColor = bgcolor;
            }
            cell.BorderWidthBottom = tblproperties.Border;
            cell.BorderWidthLeft = tblproperties.Border;
            if (!isHeader)
            {
                cell.BorderWidthTop = 0;
            }
            if (isLastColumn)
            {
                cell.BorderWidthRight = tblproperties.Border;
            }
            else
            {
                cell.BorderWidthRight = 0;
            }
            if (tdproperties.ColSpan > 0) cell.Colspan = tdproperties.ColSpan;
            cell.PaddingTop = 0f;
            cell.PaddingBottom = 5f;
            cell.PaddingLeft = 5f;
            cell.Padding = 5f;
            Paragraph ph = new Paragraph(data);
            //ph.PaddingTop = 5f;
            //ph.MultipliedLeading = 0.5f;
            //ph.Alignment = Element.ALIGN_CENTER;

            int fontWeight = iTextSharp.text.Font.NORMAL;
            if (tdproperties.IsBold) fontWeight = iTextSharp.text.Font.BOLD;
            Font f = FontFactory.GetFont(_defaultFont, _defaultFontSize, fontWeight, BaseColor.BLACK);
            ph.Font = f;
            cell.AddElement(ph);
        }
        private void GetTableProperties(string attributes, RPlusPdfTableProperties properties)
        {
            Array arrattr = attributes.Split(' ');
            for (int i = 0; i < arrattr.Length; i++)
            {
                string attrvals = arrattr.GetValue(i).ToString();
                if (attrvals == "") continue;
                Array arr = attrvals.Split('=');
                string attribute = arr.GetValue(0).ToString();
                string attrval = arr.GetValue(1).ToString().Replace("'", "");
                if (attribute == "border")
                {
                    properties.Border = (float)Convert.ToDouble(attrval);
                }
                else if (attribute == "bordercolor")
                {
                    properties.BorderColor = attrval;
                }
                else if (attribute == "cellpadding")
                {
                    properties.CellPadding = Convert.ToInt32(attrval);
                }
                else if (attribute == "cellspacing")
                {
                    properties.CellSpacing = Convert.ToInt32(attrval);
                }
            }
        }
        private void GetTableTdProperties(string tdattributes, RPlusPdfTableTdProperties properties)
        {
            Array arrattr = tdattributes.Split(' ');
            for (int i = 0; i < arrattr.Length; i++)
            {
                string attrvals = arrattr.GetValue(i).ToString();
                if (attrvals == "") continue;
                Array arr = attrvals.Split('=');
                string attribute = arr.GetValue(0).ToString();
                string attrval = arr.GetValue(1).ToString().Replace("'", "");
                if (attribute == "colspan")
                {
                    properties.ColSpan = Convert.ToInt32(attrval);
                }
                else if (attribute == "bgcolor")
                {
                    properties.BgColor = attrval;
                }
                else if (attribute == "font-size")
                {
                    properties.FontSize = Convert.ToInt32(attrval.Replace("px", ""));
                }
                else if (attribute == "font-weight")
                {
                    if (attrval == "bold") properties.IsBold = true;
                }
            }
        }
        private PdfPTable GetHeader(string headerText) {  
            PdfPCell cell = null;  
            Phrase phrs = null;  
            //Header design table  
            PdfPTable table = new PdfPTable(1);  
            table.WidthPercentage = 100;  
            table.DefaultCell.Padding = 0;  
            table.DefaultCell.Border = 0;  
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            phrs = new Phrase(headerText);  
            cell = new PdfPCell(phrs);  
            cell.Border = 0;  
            cell.HorizontalAlignment = Element.ALIGN_CENTER;  
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;  
            table.AddCell(cell);  
            return table;  
        }  
        private void AddHeaderLine(Document document, string hexaColor)
        {
            BaseColor color = GetColor(hexaColor);
            PdfPTable table;
            PdfPCell cell;

            // single element w/ border
            table = new PdfPTable(1);
            cell = new PdfPCell();
            cell.BorderWidth = 2;
            cell.Padding = 5;
            cell.PaddingTop = 0;
            cell.BackgroundColor = color;
            cell.BorderColor = color;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            table.SetWidthPercentage(new float[1] { 1000f }, PageSize.LETTER);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            document.Add(table);
        }
        public BaseColor GetColor(string color)
        {
            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(color);
            BaseColor bc = new BaseColor(c.R, c.G, c.B);
            return bc;
        }
        private Font GetDefaultFont()
        {
            Font f = FontFactory.GetFont(_defaultFont, _defaultFontSize);
            return f;
        }
    }

    public class ITextEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Phrase p1Header = new Phrase("Sample Header Here", baseFontNormal);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell1 = new PdfPCell();
            PdfPCell pdfCell2 = new PdfPCell(p1Header);
            PdfPCell pdfCell3 = new PdfPCell();
            String text = "Page " + writer.PageNumber + " of ";

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
            }

            //Row 2
            PdfPCell pdfCell4 = new PdfPCell(new Phrase("Sub Header Description", baseFontNormal));

            //Row 3 
            PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
            PdfPCell pdfCell6 = new PdfPCell();
            PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));

            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

            pdfCell4.Colspan = 3;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            pdfTab.AddCell(pdfCell4);
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 100);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }

    /*
    public class TableHeader : PdfPageEventHelper
    {
        public PdfPTable HeaderTable { get; set; }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if (HeaderTable != null)
            {
                HeaderTable.WriteSelectedRows(0, -1, document.LeftMargin, document.Top, writer.DirectContent);
            }
        }
    }
    */
    public class RPlusPdfTableProperties
    {
        public int CellPadding { get; set; }
        public int CellSpacing { get; set; }
        public float Border { get; set; }
        public string BorderColor { get; set; }
    }
    public class RPlusPdfTableTdProperties
    {
        public int ColSpan { get; set; }
        public string BgColor{ get; set; }
        public string Color { get; set; }
        public int FontSize { get; set; }
        public bool IsBold { get; set; }
    }
}
