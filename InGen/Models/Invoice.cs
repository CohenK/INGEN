using InGen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InGen.Models
{
    public class Invoice
    {
        #region vars
        private int invoiceNo;
        public int InvoiceNo
        {
            get { return invoiceNo; }
            set { invoiceNo = value; }
        }
        private Company company;
        public Company Company
        {
            get { return company; }
            set { company = value; }
        }
        private Company customer;
        public Company Customer
        {
            get { return customer; }
            set { customer = value; }
        }
        private string issueDate;
        public string IssueDate
        {
            get { return issueDate; }
            set { issueDate = value; }
        }
        private string dueDate;
        public string DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        private List<ItemSold> itemsSold;
        public List<ItemSold> ItemsSold
        {
            get { return itemsSold; }
            set { itemsSold = value; }
        }
        private double subtotal;
        public double Subtotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }
        private double discount;
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        private double tax;
        public double Tax
        {
            get { return tax; }
            set { tax = value; }
        }
        private double total;
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        #endregion

        public Invoice(int invNo, Company customer, DateTime dueDate, ObservableCollection<ItemSold> items, DateTime? issueDate = null, double discount = 0)
        {
            Company = CompanyService.GetCompany();
            Customer = customer;
            if (issueDate is null)
            {
                IssueDate = DateTime.Now.ToShortDateString();
            }
            else
            {
                DateTime temp = (DateTime)issueDate;
                IssueDate = temp.ToShortDateString();
            }
            DueDate = dueDate.ToShortDateString();
            IEnumerable<ItemSold> obsCol = (IEnumerable<ItemSold>)items;
            ItemsSold = new List<ItemSold>(obsCol);
            foreach (ItemSold item in ItemsSold) {
                Subtotal += item.SubTotal;
            }
            Discount = discount;
            Tax = (Subtotal - Discount) * (Company.Tax/100.00);
            Total = Subtotal - Discount + Tax;
        }

    }
}
