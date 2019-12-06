using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TestTask.Models
{
    public class StockContext : DbContext
    {
        public StockContext() : base("DbConnection")
        { }

        public DbSet<Stock> Stocks { get; set; }
    }
}
