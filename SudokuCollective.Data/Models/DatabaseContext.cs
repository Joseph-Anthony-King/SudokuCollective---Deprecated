using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SudokuCollective.Core.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SudokuCollective.Data.Models
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DatabaseContext(
                DbContextOptions<DatabaseContext> options,
                IConfiguration iConfigService
            ) : base(options)
        {
            configuration = iConfigService;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<SudokuCell> SudokuCells { get; set; }
        public DbSet<SudokuMatrix> SudokuMatrices { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<SudokuSolution> SudokuSolutions { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<UserApp> UsersApps { get; set; }
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; }
        public DbSet<PasswordUpdate> PasswordUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var valueConverter = new ValueConverter<List<int>, string>(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(val => int.Parse(val))
                    .ToList()
            );

            var valueComparer = new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            modelBuilder.UseIdentityColumns();

            modelBuilder.Entity<Role>()
                .HasKey(role => role.Id);

            modelBuilder.Entity<Difficulty>()
                .HasKey(difficulty => difficulty.Id);

            modelBuilder.Entity<SudokuCell>()
                .HasKey(cell => cell.Id);

            modelBuilder.Entity<SudokuCell>()
                .HasOne(cell => cell.SudokuMatrix)
                .WithMany(matrix => matrix.SudokuCells)
                .HasForeignKey(cell => cell.SudokuMatrixId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SudokuCell>()
                .Ignore(cell => cell.AvailableValues);

            modelBuilder.Entity<SudokuMatrix>()
                .HasKey(matrix => matrix.Id);

            modelBuilder.Entity<SudokuMatrix>()
                .HasOne(matrix => matrix.Difficulty)
                .WithMany(difficulty => difficulty.Matrices)
                .HasForeignKey(matrix => matrix.DifficultyId);

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
                .WithOne(matrix => matrix.Game)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .HasOne(game => game.SudokuSolution)
                .WithOne(solution => solution.Game);

            modelBuilder.Entity<Game>()
                .HasOne(game => game.User)
                .WithMany(user => user.Games)
                .HasForeignKey(game => game.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

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

            modelBuilder.Entity<User>()
                .Ignore(user => user.IsAdmin)
                .Ignore(user => user.IsSuperUser);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(user => user.Roles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<SudokuSolution>()
                .HasKey(solution => solution.Id);

            modelBuilder.Entity<SudokuSolution>()
                .Property(solution => solution.SolutionList)
                .HasConversion(valueConverter)
                .Metadata
                .SetValueComparer(valueComparer);

            modelBuilder.Entity<SudokuSolution>()
                .Ignore(solution => solution.FirstRow)
                .Ignore(solution => solution.SecondRow)
                .Ignore(solution => solution.ThirdRow)
                .Ignore(solution => solution.FourthRow)
                .Ignore(solution => solution.FifthRow)
                .Ignore(solution => solution.SixthRow)
                .Ignore(solution => solution.SeventhRow)
                .Ignore(solution => solution.EighthRow)
                .Ignore(solution => solution.NinthRow);

            modelBuilder.Entity<App>()
                .HasKey(app => app.Id);

            modelBuilder.Entity<App>()
                .Ignore(app => app.GameCount);

            modelBuilder.Entity<App>()
                .Ignore(app => app.UserCount);

            modelBuilder.Entity<UserApp>()
                .HasKey(ua => ua.Id);

            modelBuilder.Entity<UserApp>()
                .HasOne(ua => ua.User)
                .WithMany(user => user.Apps)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserApp>()
                .HasOne(ua => ua.App)
                .WithMany(app => app.Users)
                .HasForeignKey(ua => ua.AppId);

            modelBuilder.Entity<EmailConfirmation>()
                .HasKey(ec => ec.Id);

            modelBuilder.Entity<EmailConfirmation>()
                .Ignore(ec => ec.IsUpdate);

            modelBuilder.Entity<PasswordUpdate>()
                .HasKey(pu => pu.Id);
        }
    }
}
