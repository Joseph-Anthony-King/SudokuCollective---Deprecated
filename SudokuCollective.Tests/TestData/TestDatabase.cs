using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;

namespace SudokuCollective.Tests.TestData {

    public static class TestDatabase {

        public static async Task<DatabaseContext> GetDatabaseContext() {

            Mock<IConfiguration> config = new Mock<IConfiguration>();

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DatabaseContext(options, config.Object);

            databaseContext.Database.EnsureCreated();

            var dateCreated = DateTime.UtcNow;

            if (await databaseContext.Roles.CountAsync() <= 0) {

                databaseContext.Roles.AddRange(

                    new Role() {

                        Id = 1,
                        Name = "NULL",
                        RoleLevel = RoleLevel.NULL
                    },
                    new Role() {

                        Id = 2,
                        Name = "Super User",
                        RoleLevel = RoleLevel.SUPERUSER
                    },
                    new Role() {

                        Id = 3,
                        Name = "Admin",
                        RoleLevel = RoleLevel.ADMIN
                    },
                    new Role() {

                        Id = 4,
                        Name = "User",
                        RoleLevel = RoleLevel.USER
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Difficulties.CountAsync() <= 0)
            {

                databaseContext.Difficulties.AddRange(

                    new Difficulty() {

                        Id = 1,
                        Name = "Null",
                        DisplayName = "Null",
                        DifficultyLevel = DifficultyLevel.NULL
                    },
                    new Difficulty() {

                        Id = 2,
                        Name = "Test",
                        DisplayName = "Test",
                        DifficultyLevel = DifficultyLevel.TEST
                    },
                    new Difficulty() {

                        Id = 3,
                        Name = "Easy",
                        DisplayName = "Steady Sloth",
                        DifficultyLevel = DifficultyLevel.EASY
                    },
                    new Difficulty() {

                        Id = 4,
                        Name = "Medium",
                        DisplayName = "Leaping Lemur",
                        DifficultyLevel = DifficultyLevel.MEDIUM
                    },
                    new Difficulty() {

                        Id = 5,
                        Name = "Hard",
                        DisplayName = "Mighty Mountain Lion",
                        DifficultyLevel = DifficultyLevel.HARD
                    },
                    new Difficulty() {

                        Id = 6,
                        Name = "Evil",
                        DisplayName = "Sneaky Shark",
                        DifficultyLevel = DifficultyLevel.EVIL
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Users.CountAsync() <= 0) {

                var salt = BCrypt.Net.BCrypt.GenerateSalt();

                databaseContext.Users.AddRange(

                    new User()
                    {

                        Id = 1,
                        UserName = "TestSuperUser",
                        FirstName = "Test Super",
                        LastName = "User",
                        NickName = "Test Super User",
                        Email = "TestSuperUser@example.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("password1", salt),
                        IsActive = true,
                        DateCreated = dateCreated,
                        DateUpdated = dateCreated
                    },
                    new User()
                    {

                        Id = 2,
                        UserName = "Test User",
                        FirstName = "Test",
                        LastName = "User",
                        NickName = "Test User",
                        Email = "TestUser@example.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("password1", salt),
                        IsActive = true,
                        DateCreated = dateCreated,
                        DateUpdated = dateCreated
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.UsersRoles.CountAsync() <= 0) {

                databaseContext.UsersRoles.AddRange(

                    new UserRole() {

                        Id = 1,
                        UserId = 1,
                        RoleId = 2
                    },
                    new UserRole() {

                        Id = 2,
                        UserId = 1,
                        RoleId = 3
                    },
                    new UserRole() {

                        Id = 3,
                        UserId = 1,
                        RoleId = 4
                    },
                    new UserRole() {

                        Id = 4,
                        UserId = 2,
                        RoleId = 4
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Apps.CountAsync() <= 0) {

                databaseContext.Apps.AddRange(

                    new App()
                    {

                        Id = 1,
                        Name = "Test App 1",
                        License = "D17F0ED3-BE9A-450A-A146-F6733DB2BBDB",
                        OwnerId = 1,
                        DevUrl = "https://localhost:4200",
                        LiveUrl = "https://testapp.com",
                        IsActive = true,
                        DateCreated = dateCreated,
                        DateUpdated = dateCreated,
                        Users = new List<UserApp>()
                    },
                    new App()
                    {

                        Id = 2,
                        Name = "Test App 2",
                        License = "03C0D43F-3AD8-490A-A131-F73C81FE02C0",
                        OwnerId = 1,
                        DevUrl = "https://localhost:8080",
                        LiveUrl = "https://testapp2.com",
                        IsActive = true,
                        DateCreated = dateCreated,
                        DateUpdated = dateCreated,
                        Users = new List<UserApp>()
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.UsersApps.CountAsync() <= 0) {

                databaseContext.UsersApps.AddRange(

                    new UserApp()
                    {

                        Id = 1,
                        UserId = 1,
                        AppId = 1
                    },
                    new UserApp()
                    {

                        Id = 2,
                        UserId = 1,
                        AppId = 2
                    },
                    new UserApp()
                    {

                        Id = 3,
                        UserId = 2,
                        AppId = 1
                    },
                    new UserApp()
                    {

                        Id = 4,
                        UserId = 2,
                        AppId = 2
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.SudokuMatrices.CountAsync() <= 0) {

                databaseContext.SudokuMatrices.AddRange(

                    new SudokuMatrix() {

                        Id = 1,
                        DifficultyId = 4
                    },
                    new SudokuMatrix() {

                        Id = 2,
                        DifficultyId = 4
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.SudokuCells.CountAsync() <= 0)
            {

                databaseContext.SudokuCells.AddRange(

                    new SudokuCell() {

                        Id = 81,
                        Index = 1,
                        Column = 1,
                        Region = 1,
                        Row = 1,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 58,
                        Index = 2,
                        Column = 2,
                        Region = 1,
                        Row = 1,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 57,
                        Index = 3,
                        Column = 3,
                        Region = 1,
                        Row = 1,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 56,
                        Index = 4,
                        Column = 4,
                        Region = 2,
                        Row = 1,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 55,
                        Index = 5,
                        Column = 5,
                        Region = 2,
                        Row = 1,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 54,
                        Index = 6,
                        Column = 6,
                        Region = 2,
                        Row = 1,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 53,
                        Index = 7,
                        Column = 7,
                        Region = 3,
                        Row = 1,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 52,
                        Index = 8,
                        Column = 8,
                        Region = 3,
                        Row = 1,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 51,
                        Index = 9,
                        Column = 9,
                        Region = 3,
                        Row = 1,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 50,
                        Index = 10,
                        Column = 1,
                        Region = 1,
                        Row = 2,
                        Value = 3,
                        DisplayValue = 3,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 49,
                        Index = 11,
                        Column = 2,
                        Region = 1,
                        Row = 2,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 48,
                        Index = 12,
                        Column = 3,
                        Region = 1,
                        Row = 2,
                        Value = 6,
                        DisplayValue = 6,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 47,
                        Index = 13,
                        Column = 4,
                        Region = 2,
                        Row = 2,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 46,
                        Index = 14,
                        Column = 5,
                        Region = 2,
                        Row = 2,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 45,
                        Index = 15,
                        Column = 6,
                        Region = 2,
                        Row = 2,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 44,
                        Index = 16,
                        Column = 7,
                        Region = 3,
                        Row = 2,
                        Value = 1,
                        DisplayValue = 1,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 43,
                        Index = 17,
                        Column = 8,
                        Region = 3,
                        Row = 2,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 42,
                        Index = 18,
                        Column = 9,
                        Region = 3,
                        Row = 2,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 59,
                        Index = 19,
                        Column = 1,
                        Region = 1,
                        Row = 3,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 61,
                        Index = 20,
                        Column = 2,
                        Region = 1,
                        Row = 3,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 80,
                        Index = 21,
                        Column = 3,
                        Region = 1,
                        Row = 3,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 62,
                        Index = 22,
                        Column = 4,
                        Region = 2,
                        Row = 3,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 79,
                        Index = 23,
                        Column = 5,
                        Region = 2,
                        Row = 3,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 78,
                        Index = 24,
                        Column = 6,
                        Region = 2,
                        Row = 3,
                        Value = 9,
                        DisplayValue = 9,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 77,
                        Index = 25,
                        Column = 7,
                        Region = 3,
                        Row = 3,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 76,
                        Index = 26,
                        Column = 8,
                        Region = 3,
                        Row = 3,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 75,
                        Index = 27,
                        Column = 9,
                        Region = 3,
                        Row = 3,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 74,
                        Index = 28,
                        Column = 1,
                        Region = 4,
                        Row = 4,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 73,
                        Index = 29,
                        Column = 2,
                        Region = 4,
                        Row = 4,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 72,
                        Index = 30,
                        Column = 3,
                        Region = 4,
                        Row = 4,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 71,
                        Index = 31,
                        Column = 4,
                        Region = 5,
                        Row = 4,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 70,
                        Index = 32,
                        Column = 5,
                        Region = 5,
                        Row = 4,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 69,
                        Index = 33,
                        Column = 6,
                        Region = 5,
                        Row = 4,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 68,
                        Index = 34,
                        Column = 7,
                        Region = 6,
                        Row = 4,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 67,
                        Index = 35,
                        Column = 8,
                        Region = 6,
                        Row = 4,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 66,
                        Index = 36,
                        Column = 9,
                        Region = 6,
                        Row = 4,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 65,
                        Index = 37,
                        Column = 1,
                        Region = 4,
                        Row = 5,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 64,
                        Index = 38,
                        Column = 2,
                        Region = 4,
                        Row = 5,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 63,
                        Index = 39,
                        Column = 3,
                        Region = 4,
                        Row = 5,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 41,
                        Index = 40,
                        Column = 4,
                        Region = 5,
                        Row = 5,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 40,
                        Index = 41,
                        Column = 5,
                        Region = 5,
                        Row = 5,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 39,
                        Index = 42,
                        Column = 6,
                        Region = 5,
                        Row = 5,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 38,
                        Index = 43,
                        Column = 7,
                        Region = 6,
                        Row = 5,
                        Value = 5,
                        DisplayValue = 5,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 16,
                        Index = 44,
                        Column = 8,
                        Region = 6,
                        Row = 5,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 15,
                        Index = 45,
                        Column = 9,
                        Region = 6,
                        Row = 5,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 14,
                        Index = 46,
                        Column = 1,
                        Region = 4,
                        Row = 6,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 13,
                        Index = 47,
                        Column = 2,
                        Region = 4,
                        Row = 6,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 12,
                        Index = 48,
                        Column = 3,
                        Region = 4,
                        Row = 6,
                        Value = 9,
                        DisplayValue = 9,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 11,
                        Index = 49,
                        Column = 4,
                        Region = 5,
                        Row = 6,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 10,
                        Index = 50,
                        Column = 5,
                        Region = 5,
                        Row = 6,
                        Value = 5,
                        DisplayValue = 5,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 17,
                        Index = 51,
                        Column = 6,
                        Region = 5,
                        Row = 6,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 9,
                        Index = 52,
                        Column = 7,
                        Region = 6,
                        Row = 6,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 7,
                        Index = 53,
                        Column = 8,
                        Region = 6,
                        Row = 6,
                        Value = 6,
                        DisplayValue = 6,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 6,
                        Index = 54,
                        Column = 9,
                        Region = 6,
                        Row = 6,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 5,
                        Index = 55,
                        Column = 1,
                        Region = 7,
                        Row = 7,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 4,
                        Index = 56,
                        Column = 2,
                        Region = 7,
                        Row = 7,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 3,
                        Index = 57,
                        Column = 3,
                        Region = 7,
                        Row = 7,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 2,
                        Index = 58,
                        Column = 4,
                        Region = 8,
                        Row = 7,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 1,
                        Index = 59,
                        Column = 5,
                        Region = 8,
                        Row = 7,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 8,
                        Index = 60,
                        Column = 6,
                        Region = 8,
                        Row = 7,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 18,
                        Index = 61,
                        Column = 7,
                        Region = 9,
                        Row = 7,
                        Value = 3,
                        DisplayValue = 3,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 19,
                        Index = 62,
                        Column = 8,
                        Region = 9,
                        Row = 7,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 20,
                        Index = 63,
                        Column = 9,
                        Region = 9,
                        Row = 7,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 37,
                        Index = 64,
                        Column = 1,
                        Region = 7,
                        Row = 8,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 36,
                        Index = 65,
                        Column = 2,
                        Region = 7,
                        Row = 8,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 35,
                        Index = 66,
                        Column = 3,
                        Region = 7,
                        Row = 8,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 34,
                        Index = 67,
                        Column = 4,
                        Region = 8,
                        Row = 8,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 33,
                        Index = 68,
                        Column = 5,
                        Region = 8,
                        Row = 8,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 32,
                        Index = 69,
                        Column = 6,
                        Region = 8,
                        Row = 8,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 31,
                        Index = 70,
                        Column = 7,
                        Region = 9,
                        Row = 8,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 30,
                        Index = 71,
                        Column = 8,
                        Region = 9,
                        Row = 8,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 29,
                        Index = 72,
                        Column = 9,
                        Region = 9,
                        Row = 8,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 28,
                        Index = 73,
                        Column = 1,
                        Region = 7,
                        Row = 9,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 27,
                        Index = 74,
                        Column = 2,
                        Region = 7,
                        Row = 9,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 26,
                        Index = 75,
                        Column = 3,
                        Region = 7,
                        Row = 9,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 25,
                        Index = 76,
                        Column = 4,
                        Region = 8,
                        Row = 9,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 24,
                        Index = 77,
                        Column = 5,
                        Region = 8,
                        Row = 9,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 23,
                        Index = 78,
                        Column = 6,
                        Region = 8,
                        Row = 9,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 22,
                        Index = 79,
                        Column = 7,
                        Region = 9,
                        Row = 9,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 21,
                        Index = 80,
                        Column = 8,
                        Region = 9,
                        Row = 9,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 60,
                        Index = 81,
                        Column = 9,
                        Region = 9,
                        Row = 9,
                        Value = 6,
                        DisplayValue = 6,
                        Obscured = false,
                        SudokuMatrixId = 1
                    },
                    new SudokuCell() {

                        Id = 162,
                        Index = 1,
                        Column = 1,
                        Region = 1,
                        Row = 1,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 139,
                        Index = 2,
                        Column = 2,
                        Region = 1,
                        Row = 1,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 138,
                        Index = 3,
                        Column = 3,
                        Region = 1,
                        Row = 1,
                        Value = 1,
                        DisplayValue = 1,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 137,
                        Index = 4,
                        Column = 4,
                        Region = 2,
                        Row = 1,
                        Value = 9,
                        DisplayValue = 9,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 136,
                        Index = 5,
                        Column = 5,
                        Region = 2,
                        Row = 1,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 135,
                        Index = 6,
                        Column = 6,
                        Region = 2,
                        Row = 1,
                        Value = 6,
                        DisplayValue = 6,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 134,
                        Index = 7,
                        Column = 7,
                        Region = 3,
                        Row = 1,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 133,
                        Index = 8,
                        Column = 8,
                        Region = 3,
                        Row = 1,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 132,
                        Index = 9,
                        Column = 9,
                        Region = 3,
                        Row = 1,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 131,
                        Index = 10,
                        Column = 1,
                        Region = 1,
                        Row = 2,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 130,
                        Index = 11,
                        Column = 2,
                        Region = 1,
                        Row = 2,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 129,
                        Index = 12,
                        Column = 3,
                        Region = 1,
                        Row = 2,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 128,
                        Index = 13,
                        Column = 4,
                        Region = 2,
                        Row = 2,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 127,
                        Index = 14,
                        Column = 5,
                        Region = 2,
                        Row = 2,
                        Value = 3,
                        DisplayValue = 3,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 126,
                        Index = 15,
                        Column = 6,
                        Region = 2,
                        Row = 2,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 125,
                        Index = 16,
                        Column = 7,
                        Region = 3,
                        Row = 2,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 124,
                        Index = 17,
                        Column = 8,
                        Region = 3,
                        Row = 2,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 123,
                        Index = 18,
                        Column = 9,
                        Region = 3,
                        Row = 2,
                        Value = 6,
                        DisplayValue = 6,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 140,
                        Index = 19,
                        Column = 1,
                        Region = 1,
                        Row = 3,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 142,
                        Index = 20,
                        Column = 2,
                        Region = 1,
                        Row = 3,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 161,
                        Index = 21,
                        Column = 3,
                        Region = 1,
                        Row = 3,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 143,
                        Index = 22,
                        Column = 4,
                        Region = 2,
                        Row = 3,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 160,
                        Index = 23,
                        Column = 5,
                        Region = 2,
                        Row = 3,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 159,
                        Index = 24,
                        Column = 6,
                        Region = 2,
                        Row = 3,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 158,
                        Index = 25,
                        Column = 7,
                        Region = 3,
                        Row = 3,
                        Value = 1,
                        DisplayValue = 1,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 157,
                        Index = 26,
                        Column = 8,
                        Region = 3,
                        Row = 3,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 156,
                        Index = 27,
                        Column = 9,
                        Region = 3,
                        Row = 3,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 155,
                        Index = 28,
                        Column = 1,
                        Region = 4,
                        Row = 4,
                        Value = 1,
                        DisplayValue = 1,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 154,
                        Index = 29,
                        Column = 2,
                        Region = 4,
                        Row = 4,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 153,
                        Index = 30,
                        Column = 3,
                        Region = 4,
                        Row = 4,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 152,
                        Index = 31,
                        Column = 4,
                        Region = 5,
                        Row = 4,
                        Value = 4,
                        DisplayValue = 4,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 151,
                        Index = 32,
                        Column = 5,
                        Region = 5,
                        Row = 4,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 150,
                        Index = 33,
                        Column = 6,
                        Region = 5,
                        Row = 4,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 149,
                        Index = 34,
                        Column = 7,
                        Region = 6,
                        Row = 4,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 148,
                        Index = 35,
                        Column = 8,
                        Region = 6,
                        Row = 4,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 147,
                        Index = 36,
                        Column = 9,
                        Region = 6,
                        Row = 4,
                        Value = 5,
                        DisplayValue = 5,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 146,
                        Index = 37,
                        Column = 1,
                        Region = 4,
                        Row = 5,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 145,
                        Index = 38,
                        Column = 2,
                        Region = 4,
                        Row = 5,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 144,
                        Index = 39,
                        Column = 3,
                        Region = 4,
                        Row = 5,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 122,
                        Index = 40,
                        Column = 4,
                        Region = 5,
                        Row = 5,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 121,
                        Index = 41,
                        Column = 5,
                        Region = 5,
                        Row = 5,
                        Value = 6,
                        DisplayValue = 6,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 120,
                        Index = 42,
                        Column = 6,
                        Region = 5,
                        Row = 5,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 119,
                        Index = 43,
                        Column = 7,
                        Region = 6,
                        Row = 5,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 97,
                        Index = 44,
                        Column = 8,
                        Region = 6,
                        Row = 5,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 96,
                        Index = 45,
                        Column = 9,
                        Region = 6,
                        Row = 5,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 95,
                        Index = 46,
                        Column = 1,
                        Region = 4,
                        Row = 6,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 94,
                        Index = 47,
                        Column = 2,
                        Region = 4,
                        Row = 6,
                        Value = 9,
                        DisplayValue = 9,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 93,
                        Index = 48,
                        Column = 3,
                        Region = 4,
                        Row = 6,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 92,
                        Index = 49,
                        Column = 4,
                        Region = 5,
                        Row = 6,
                        Value = 5,
                        DisplayValue = 5,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 91,
                        Index = 50,
                        Column = 5,
                        Region = 5,
                        Row = 6,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 98,
                        Index = 51,
                        Column = 6,
                        Region = 5,
                        Row = 6,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 90,
                        Index = 52,
                        Column = 7,
                        Region = 6,
                        Row = 6,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 88,
                        Index = 53,
                        Column = 8,
                        Region = 6,
                        Row = 6,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 87,
                        Index = 54,
                        Column = 9,
                        Region = 6,
                        Row = 6,
                        Value = 2,
                        DisplayValue = 2,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 86,
                        Index = 55,
                        Column = 1,
                        Region = 7,
                        Row = 7,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 85,
                        Index = 56,
                        Column = 2,
                        Region = 7,
                        Row = 7,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 84,
                        Index = 57,
                        Column = 3,
                        Region = 7,
                        Row = 7,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 83,
                        Index = 58,
                        Column = 4,
                        Region = 8,
                        Row = 7,
                        Value = 7,
                        DisplayValue = 7,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 82,
                        Index = 59,
                        Column = 5,
                        Region = 8,
                        Row = 7,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 89,
                        Index = 60,
                        Column = 6,
                        Region = 8,
                        Row = 7,
                        Value = 9,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 99,
                        Index = 61,
                        Column = 7,
                        Region = 9,
                        Row = 7,
                        Value = 5,
                        DisplayValue = 5,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 100,
                        Index = 62,
                        Column = 8,
                        Region = 9,
                        Row = 7,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 101,
                        Index = 63,
                        Column = 9,
                        Region = 9,
                        Row = 7,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 118,
                        Index = 64,
                        Column = 1,
                        Region = 7,
                        Row = 8,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 117,
                        Index = 65,
                        Column = 2,
                        Region = 7,
                        Row = 8,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 116,
                        Index = 66,
                        Column = 3,
                        Region = 7,
                        Row = 8,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 115,
                        Index = 67,
                        Column = 4,
                        Region = 8,
                        Row = 8,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 114,
                        Index = 68,
                        Column = 5,
                        Region = 8,
                        Row = 8,
                        Value = 8,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 113,
                        Index = 69,
                        Column = 6,
                        Region = 8,
                        Row = 8,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 112,
                        Index = 70,
                        Column = 7,
                        Region = 9,
                        Row = 8,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 111,
                        Index = 71,
                        Column = 8,
                        Region = 9,
                        Row = 8,
                        Value = 9,
                        DisplayValue = 9,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 110,
                        Index = 72,
                        Column = 9,
                        Region = 9,
                        Row = 8,
                        Value = 3,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 109,
                        Index = 73,
                        Column = 1,
                        Region = 7,
                        Row = 9,
                        Value = 8,
                        DisplayValue = 8,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 108,
                        Index = 74,
                        Column = 2,
                        Region = 7,
                        Row = 9,
                        Value = 5,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 107,
                        Index = 75,
                        Column = 3,
                        Region = 7,
                        Row = 9,
                        Value = 9,
                        DisplayValue = 9,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 106,
                        Index = 76,
                        Column = 4,
                        Region = 8,
                        Row = 9,
                        Value = 6,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 105,
                        Index = 77,
                        Column = 5,
                        Region = 8,
                        Row = 9,
                        Value = 1,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 104,
                        Index = 78,
                        Column = 6,
                        Region = 8,
                        Row = 9,
                        Value = 3,
                        DisplayValue = 3,
                        Obscured = false,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 103,
                        Index = 79,
                        Column = 7,
                        Region = 9,
                        Row = 9,
                        Value = 7,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 102,
                        Index = 80,
                        Column = 8,
                        Region = 9,
                        Row = 9,
                        Value = 2,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    },
                    new SudokuCell() {

                        Id = 141,
                        Index = 81,
                        Column = 9,
                        Region = 9,
                        Row = 9,
                        Value = 4,
                        DisplayValue = 0,
                        Obscured = true,
                        SudokuMatrixId = 2
                    }
                );

                await databaseContext.SaveChangesAsync();
            }
            
            if (await databaseContext.SudokuSolutions.CountAsync() <= 0)
            {

                databaseContext.SudokuSolutions.AddRange(

                    new SudokuSolution()
                    {

                        Id = 1,
                        SolutionList = new List<int>(),
                        DateCreated = dateCreated,
                        DateSolved = dateCreated
                    },
                    new SudokuSolution()
                    {

                        Id = 2,
                        SolutionList = new List<int>(),
                        DateCreated = dateCreated,
                        DateSolved = dateCreated
                    }
                ); ; ;

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Games.CountAsync() <= 0) {

                databaseContext.Games.AddRange(

                    new Game() {

                        Id = 1,
                        UserId = 1,
                        SudokuMatrixId = 1,
                        SudokuSolutionId = 1,
                        ContinueGame = true,
                        DateCreated = dateCreated,
                        DateCompleted = DateTime.MinValue
                    },
                    new Game() {

                        Id = 2,
                        UserId = 2,
                        SudokuMatrixId = 2,
                        SudokuSolutionId = 2,
                        ContinueGame = true,
                        DateCreated = dateCreated,
                        DateCompleted = DateTime.MinValue
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }
    }
}
