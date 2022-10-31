using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrestWalletApplication.Models;

namespace ZedCrestWalletApplication.DAL
{
    public class ZedCrestWalletContext : DbContext
    {
        public ZedCrestWalletContext(DbContextOptions<ZedCrestWalletContext> options) : base(options)
        {

        }

        public DbSet<CustomerAccountInformation> CustomerAccountInformation { get; set; }
        public DbSet<CounterpartyTransaction> CounterpartyTransactions { get; set; }
    }
}
