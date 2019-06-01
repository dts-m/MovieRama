using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.pocos;

namespace AppDAL
{
    public class AppDataDbContext : ApplicationDataDbContext
    {
        public AppDataDbContext() : base("AuthDataConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDataDbContext>());
        }

        public static AppDataDbContext Create()
        {
            return new AppDataDbContext();
        }
    }
}
