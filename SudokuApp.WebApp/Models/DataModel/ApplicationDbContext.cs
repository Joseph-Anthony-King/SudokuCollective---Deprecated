using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SudokuApp.Models;

namespace SudokuApp.WebApp.Models.DataModel {

    public class ApplicationDbContext : DbContext {
        
        private IConfiguration configuration;

        public ApplicationDbContext(
                DbContextOptions<ApplicationDbContext> options,
                IConfiguration iConfigService
            ) : base(options) {

            configuration = iConfigService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql(configuration.GetValue<string>("ConnectionStrings:DatabaseConnection"));
    }
}