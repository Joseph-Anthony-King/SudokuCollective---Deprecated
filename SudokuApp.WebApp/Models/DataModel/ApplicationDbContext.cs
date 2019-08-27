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

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<SudokuCell> SudokuCells { get; set; }
        public DbSet<SudokuMatrix> SudokuMatrices { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UsersPermissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql(configuration.GetValue<string>("ConnectionStrings:DatabaseConnection"));

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.ForNpgsqlUseIdentityColumns();

            modelBuilder.Entity<Permission>()
                .HasKey(permission => permission.Id);

            modelBuilder.Entity<Difficulty>()
                .HasKey(difficulty => difficulty.Id);

            modelBuilder.Entity<Difficulty>()
                .HasMany(difficulty => difficulty.Matrices)
                .WithOne(matrix => matrix.Difficulty);

            modelBuilder.Entity<SudokuCell>()
                .HasKey(cell => cell.Id);

            modelBuilder.Entity<SudokuMatrix>()
                .HasKey(matrix => matrix.Id);
                
            modelBuilder.Entity<SudokuMatrix>()
                .HasMany(matrix => matrix.SudokuCells)
                .WithOne(cell => cell.SudokuMatrix);
                
            modelBuilder.Entity<SudokuMatrix>()
                .Ignore(matrix => matrix.Columns)
                .Ignore(matrix => matrix.Regions)
                .Ignore(matrix => matrix.Rows)
                .Ignore(matrix => matrix.FirstColumn)
                .Ignore(matrix => matrix.SecondColumn)
                .Ignore(matrix => matrix.ThirdColumn)
                .Ignore(matrix => matrix.FourthColumn)
                .Ignore(matrix => matrix.FifthColumn)
                .Ignore(matrix => matrix.SixthColumn)
                .Ignore(matrix => matrix.SeventhColumn)
                .Ignore(matrix => matrix.EighthColumn)
                .Ignore(matrix => matrix.NinthColumn)
                .Ignore(matrix => matrix.FirstRegion)
                .Ignore(matrix => matrix.SecondRegion)
                .Ignore(matrix => matrix.ThirdRegion)
                .Ignore(matrix => matrix.FourthRegion)
                .Ignore(matrix => matrix.FifthRegion)
                .Ignore(matrix => matrix.SixthRegion)
                .Ignore(matrix => matrix.SeventhRegion)
                .Ignore(matrix => matrix.EighthRegion)
                .Ignore(matrix => matrix.NinthRegion)
                .Ignore(matrix => matrix.FirstRow)
                .Ignore(matrix => matrix.SecondRow)
                .Ignore(matrix => matrix.ThirdRow)
                .Ignore(matrix => matrix.FourthRow)
                .Ignore(matrix => matrix.FifthRow)
                .Ignore(matrix => matrix.SixthRow)
                .Ignore(matrix => matrix.SeventhRow)
                .Ignore(matrix => matrix.EighthRow)
                .Ignore(matrix => matrix.NinthRow)
                .Ignore(matrix => matrix.FirstColumnValues)
                .Ignore(matrix => matrix.SecondColumnValues)
                .Ignore(matrix => matrix.ThirdColumnValues)
                .Ignore(matrix => matrix.FourthColumnValues)
                .Ignore(matrix => matrix.FifthColumnValues)
                .Ignore(matrix => matrix.SixthColumnValues)
                .Ignore(matrix => matrix.SeventhColumnValues)
                .Ignore(matrix => matrix.EighthColumnValues)
                .Ignore(matrix => matrix.NinthColumnValues)
                .Ignore(matrix => matrix.FirstRegionValues)
                .Ignore(matrix => matrix.SecondRegionValues)
                .Ignore(matrix => matrix.ThirdRegionValues)
                .Ignore(matrix => matrix.FourthRegionValues)
                .Ignore(matrix => matrix.FifthRegionValues)
                .Ignore(matrix => matrix.SixthRegionValues)
                .Ignore(matrix => matrix.SeventhRegionValues)
                .Ignore(matrix => matrix.EighthRegionValues)
                .Ignore(matrix => matrix.NinthRegionValues)
                .Ignore(matrix => matrix.FirstRowValues)
                .Ignore(matrix => matrix.SecondRowValues)
                .Ignore(matrix => matrix.ThirdRowValues)
                .Ignore(matrix => matrix.FourthRowValues)
                .Ignore(matrix => matrix.FifthRowValues)
                .Ignore(matrix => matrix.SixthRowValues)
                .Ignore(matrix => matrix.SeventhRowValues)
                .Ignore(matrix => matrix.EighthRowValues)
                .Ignore(matrix => matrix.NinthRowValues);

            modelBuilder.Entity<Game>()
                .HasKey(game => game.Id);
                
            modelBuilder.Entity<Game>()
                .HasOne(game => game.SudokuMatrix)
                .WithOne(matrix => matrix.Game);
            
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);
            
            modelBuilder.Entity<User>()
                .HasMany(user => user.Games)
                .WithOne(game => game.User);

            modelBuilder.Entity<User>()
                .Property(user => user.UserName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(user => user.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(user => user.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId});

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(user => user.Permissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(permission => permission.Users)
                .HasForeignKey(up => up.PermissionId);
        }
    }
}