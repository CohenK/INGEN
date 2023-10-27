using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using QuestPDF.Previewer;



namespace InGen.Services
{
    public class PDFService : IDocument
    {
        private Invoice invoice { get; set; }

        public PDFService(Invoice invoice) 
        {
            this.invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }
        public void ComposeHeader(QuestPDF.Infrastructure.IContainer container)
        {
            var invoiceStyle = TextStyle.Default.FontSize(15).SemiBold().FontColor("#4b438e");
            var metaDataHeadingStyle = TextStyle.Default.FontSize(8);
            var metaDataStyle = TextStyle.Default.FontSize(8).FontColor("#949494");

            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text($"Invoice #{invoice.InvoiceNo}").Style(invoiceStyle);
                    col.Item().Text(text =>
                    {
                        text.Span("Issue date: ").Style(metaDataHeadingStyle);
                        text.Span($"{invoice.IssueDate}").Style(metaDataStyle);
                    });
                    col.Item().Text(text =>
                    {
                        text.Span("Due date: ").Style(metaDataHeadingStyle);
                        text.Span($"{invoice.DueDate}").Style(metaDataStyle);
                    });
                    col.Item().Text($"Total: {invoice.Total:C}").Style(metaDataHeadingStyle);
                });
            });
        }
        public void ComposeContent(QuestPDF.Infrastructure.IContainer container)
        {
            container.PaddingTop(40).Column(col =>
            {
                col.Item().Element(ComposeCompanyInfo);
                col.Item().Element(ComposeTable);
                col.Item().Element(ComposeSummary);
                col.Item().AlignBottom().Element(ComposeComments);

            });
        }
        public void ComposeCompanyInfo(QuestPDF.Infrastructure.IContainer container)
        {
            var CompanyStyle = TextStyle.Default.FontSize(12).SemiBold().FontColor("#4b438e");
            var detailsStyle = TextStyle.Default.FontColor(QuestPDF.Helpers.Colors.Black);

            container.Row(row =>
            {
                row.RelativeItem().AlignLeft().Column(col =>
                {
                    col.Item().Text("Billed To:").Style(CompanyStyle);
                    col.Item().Text($"{invoice.Customer.Name}").Style(detailsStyle);
                    col.Item().Text($"{invoice.Customer.Address}").Style(detailsStyle);
                    col.Item().Text($"{invoice.Customer.City}, {invoice.Customer.State} {invoice.Customer.ZipCode}").Style(detailsStyle);
                });
                row.RelativeItem().PaddingLeft(20).Column(col =>
                {
                    col.Item().AlignLeft().Text("Billed From:").Style(CompanyStyle);
                    col.Item().AlignLeft().Text($"{invoice.Company.Name}").Style(detailsStyle);
                    col.Item().AlignLeft().Text($"{invoice.Company.Address}").Style(detailsStyle);
                    col.Item().AlignLeft().Text($"{invoice.Company.City}, {invoice.Company.State} {invoice.Company.ZipCode}").Style(detailsStyle);
                });
            });
        }
        public void ComposeTable(QuestPDF.Infrastructure.IContainer container)
        {
            var headerStyle = TextStyle.Default.FontSize(12).SemiBold().FontColor("#4b438e");
            var infoStyle = TextStyle.Default.FontSize(10).FontColor(QuestPDF.Helpers.Colors.Black);
            var descStyle = TextStyle.Default.FontSize(8).FontColor("#949494");

            static QuestPDF.Infrastructure.IContainer HeaderStyle(QuestPDF.Infrastructure.IContainer container)
            {
                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor("#4b438e");
            }

            static QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
            {
                return container.PaddingVertical(5).BorderBottom(1).BorderColor("#949494").PaddingVertical(5);
            }


            container.PaddingTop(50).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(4);
                    cols.RelativeColumn(2);
                    cols.RelativeColumn(1);
                    cols.RelativeColumn(1);
                });
                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).AlignLeft().Text("Item").Style(headerStyle);
                    header.Cell().Element(HeaderStyle).AlignLeft().Text("Unit Price").Style(headerStyle);
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Quantity").Style(headerStyle);
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Total").Style(headerStyle);
                });
                foreach (ItemSold item in invoice.ItemsSold)
                {
                    table.Cell().Element(CellStyle).Column(col => 
                    {
                        col.Item().AlignLeft().Text(text => {
                            text.Span($"{item.Name}");
                        });
                        col.Item().AlignLeft().Text(text =>
                        {
                            text.Span($"{item.Description}").Style(descStyle);
                        });
                    });
                    table.Cell().Element(CellStyle).AlignLeft().Text($"{item.UnitPrice:C}/{item.UnitName}").Style(infoStyle);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{(int)item.Amount} {item.UnitName}(s)").Style(infoStyle);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.SubTotal:C}").Style(infoStyle);
                }
            });
        }
        public void ComposeSummary(QuestPDF.Infrastructure.IContainer container)
        {
            container.Column(col =>
            {
                static QuestPDF.Infrastructure.IContainer HeaderStyle(QuestPDF.Infrastructure.IContainer container)
                {
                    return container.DefaultTextStyle(x => x).PaddingVertical(5).BorderBottom(1).BorderColor("#4b438e");
                }


                col.Item().Row(row =>
                {
                    row.RelativeItem(4);
                    row.RelativeItem(2);
                    row.RelativeItem(1).AlignRight().Text("Sub Total: ");
                    row.RelativeItem(1).AlignRight().Text($"{invoice.Subtotal:C}");
                });
                if (invoice.Discount > 0)
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem(4);
                        row.RelativeItem(2);
                        row.RelativeItem(1).AlignRight().Text("Discount: ");
                        row.RelativeItem(1).AlignRight().Text($"-{invoice.Discount:C}");
                    });
                }
                col.Item().Row(row =>
                {
                    row.RelativeItem(4);
                    row.RelativeItem(2);
                    row.RelativeItem(1).Element(HeaderStyle).AlignRight().Text("Tax: ");
                    row.RelativeItem(1).Element(HeaderStyle).AlignRight().Text($"{invoice.Tax:C}");
                });
                col.Item().Row(row =>
                {
                    row.RelativeItem(4);
                    row.RelativeItem(2);
                    row.RelativeItem(1).AlignRight().Text("Total: ").SemiBold();
                    row.RelativeItem(1).AlignRight().Text($"{(invoice.Total):C}").SemiBold();
                });

            });
        }
        public void ComposeComments(QuestPDF.Infrastructure.IContainer container)
        {
            container.AlignBottom().Text("Thank you for your purchase!").FontSize(8).SemiBold().FontColor("#4b438e");
        }
        public void ComposeFooter(QuestPDF.Infrastructure.IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        }
    }
}
