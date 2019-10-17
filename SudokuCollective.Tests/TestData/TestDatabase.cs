using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
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

            if (await databaseContext.Roles.CountAsync() <= 0) {

                databaseContext.Roles.AddRange(

                    new Role()
                    {

                        Id = 1,
                        Name = "Super User",
                        RoleLevel = RoleLevel.SUPERUSER
                    },
                    new Role()
                    {

                        Id = 2,
                        Name = "Admin",
                        RoleLevel = RoleLevel.ADMIN
                    },
                    new Role()
                    {

                        Id = 3,
                        Name = "User",
                        RoleLevel = RoleLevel.USER
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Users.CountAsync() <= 0) {

                databaseContext.Users.AddRange(

                    new User()
                    {

                        Id = 1,
                        UserName = "TestSuperUser",
                        FirstName = "Test Super",
                        LastName = "User",
                        NickName = "Test Super User",
                        Email = "TestSuperUser@example.com",
                        Password = "password1",
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    },
                    new User()
                    {

                        Id = 2,
                        UserName = "Test User",
                        FirstName = "Test",
                        LastName = "User",
                        NickName = "Test User",
                        Email = "TestUser@example.com",
                        Password = "password1",
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    }
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.UsersRoles.CountAsync() <= 0) {

                databaseContext.UsersRoles.AddRange(

                    new UserRole()
                    {

                        Id = 1,
                        UserId = 1,
                        RoleId = 1
                    },
                    new UserRole()
                    {

                        Id = 2,
                        UserId = 1,
                        RoleId = 2
                    },
                    new UserRole()
                    {

                        Id = 3,
                        UserId = 2,
                        RoleId = 2
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
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
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
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
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

            return databaseContext;
        }
    }
}
