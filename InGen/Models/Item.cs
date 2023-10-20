using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InGen.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string unitName { get; set; }
        public double? unitPrice { get; set; }
    }
}
