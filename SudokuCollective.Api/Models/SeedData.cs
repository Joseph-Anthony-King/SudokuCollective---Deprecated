using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Core.Enums;

namespace SudokuCollective.Api.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app, IConfiguration config)
        {
            using (var servicesScope = app.ApplicationServices.CreateScope())
            {
                var createdDate = DateTime.UtcNow;

                DatabaseContext context = servicesScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();

                if (!context.Roles.Any())
                {
                    context.Roles.Add(

                        new Role(0, "Null", RoleLevel.NULL)
                    );

                    context.SaveChanges();

                    context.Roles.Add(

                        new Role(0, "Super User", RoleLevel.SUPERUSER)
                    );

                    context.SaveChanges();

                    context.Roles.Add(

                        new Role(0, "Admin", RoleLevel.ADMIN)
                    );

                    context.SaveChanges();

                    context.Roles.Add(

                        new Role(0, "User", RoleLevel.USER)
                    );

                    context.SaveChanges();
                }

                if (!context.Difficulties.Any())
                {
                    context.Difficulties.Add(

                        new Difficulty(0, "Null", "Null", DifficultyLevel.NULL)
                    );

                    context.SaveChanges();

                    context.Difficulties.Add(

                        new Difficulty(0, "Test", "Test", DifficultyLevel.TEST)
                    );

                    context.SaveChanges();

                    context.Difficulties.Add(

                        new Difficulty(0, "Easy", "Steady Sloth", DifficultyLevel.EASY)
                    );

                    context.SaveChanges();

                    context.Difficulties.Add(

                        new Difficulty(0, "Medium", "Leaping Lemur", DifficultyLevel.MEDIUM)
                    );

                    context.SaveChanges();

                    context.Difficulties.Add(

                        new Difficulty(0, "Hard", "Mighty Mountain Lion", DifficultyLevel.HARD)
                    );

                    context.SaveChanges();

                    context.Difficulties.Add(

                        new Difficulty(0, "Evil", "Sneaky Shark", DifficultyLevel.EVIL)
                    );

                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    var salt = BCrypt.Net.BCrypt.GenerateSalt();

                    context.Users.Add(

                        new User(
                            0,
                            config.GetValue<string>("DefaultUserAccounts:SuperUser:UserName"),
                            config.GetValue<string>("DefaultUserAccounts:SuperUser:FirstName"),
                            config.GetValue<string>("DefaultUserAccounts:SuperUser:LastName"),
                            config.GetValue<string>("DefaultUserAccounts:SuperUser:NickName"),
                            config.GetValue<string>("DefaultUserAccounts:SuperUser:Email"),
                            true,
                            false,
                            BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("DefaultUserAccounts:SuperUser:Password", salt)),
                            false,
                            true,
                            createdDate,
                            DateTime.MinValue)
                    );

                    context.SaveChanges();

                    context.Users.AddRange(

                        new User(
                            0,
                            config.GetValue<string>("DefaultUserAccounts:AdminUser:UserName"),
                            config.GetValue<string>("DefaultUserAccounts:AdminUser:FirstName"),
                            config.GetValue<string>("DefaultUserAccounts:AdminUser:LastName"),
                            config.GetValue<string>("DefaultUserAccounts:AdminUser:NickName"),
                            config.GetValue<string>("DefaultUserAccounts:AdminUser:Email"),
                            true,
                            false,
                            BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("DefaultUserAccounts:AdminUser:Password", salt)),
                            false,
                            true,
                            createdDate,
                            DateTime.MinValue)
                    );

                    context.SaveChanges();

                }

                if (!context.Apps.Any())
                {
                    context.Apps.Add(

                        new App(
                            0,
                            config.GetValue<string>("DefaultAdminApp:Name"),
                            config.GetValue<string>("DefaultAdminApp:License"),
                            1,
                            config.GetValue<string>("DefaultAdminApp:DevUrl"),
                            config.GetValue<string>("DefaultAdminApp:LiveUrl"),
                            true,
                            true,
                            true,
                            true,
                            true,
                            string.Empty,
                            string.Empty,
                            TimeFrame.DAYS,
                            1,
                            createdDate,
                            DateTime.MinValue)
                    );

                    context.SaveChanges();

                    context.Apps.Add(

                        new App(
                            0,
                            config.GetValue<string>("DefaultClientApp:Name"),
                            config.GetValue<string>("DefaultClientApp:License"),
                            2,
                            config.GetValue<string>("DefaultClientApp:DevUrl"),
                            config.GetValue<string>("DefaultClientApp:LiveUrl"),
                            false,
                            true,
                            true,
                            true,
                            true,
                            string.Empty,
                            string.Empty,
                            TimeFrame.DAYS,
                            1,
                            createdDate,
                            DateTime.MinValue)
                    );

                    context.Apps.Add(

                        new App(
                            0,
                            config.GetValue<string>("DefaultPostmanApp:Name"),
                            config.GetValue<string>("DefaultPostmanApp:License"),
                            1,
                            config.GetValue<string>("DefaultPostmanApp:DevUrl"),
                            config.GetValue<string>("DefaultPostmanApp:LiveUrl"),
                            true,
                            true,
                            true,
                            true,
                            true,
                            string.Empty,
                            string.Empty,
                            TimeFrame.DAYS,
                            1,
                            createdDate,
                            DateTime.MinValue)
                    );

                    context.SaveChanges();
                }

                if (!context.UsersApps.Any())
                {
                    context.UsersApps.Add(

                        new UserApp(0, 1, 1)
                    );

                    context.SaveChanges();

                    context.UsersApps.Add(

                        new UserApp(0, 2, 1)
                    );

                    context.SaveChanges();

                    context.UsersApps.Add(

                        new UserApp(0, 1, 2)
                    );

                    context.SaveChanges();

                    context.UsersApps.Add(

                        new UserApp(0, 2, 2)
                    );

                    context.SaveChanges();

                    context.UsersApps.Add(

                        new UserApp(0, 1, 3)
                    );

                    context.SaveChanges();

                    context.UsersApps.Add(

                        new UserApp(0, 2, 3)
                    );

                    context.SaveChanges();
                }

                if (!context.UsersRoles.Any())
                {
                    context.UsersRoles.Add(

                        new UserRole(0, 1, 2)
                    );

                    context.SaveChanges();

                    context.UsersRoles.Add(

                        new UserRole(0, 1, 3)
                    );

                    context.SaveChanges();

                    context.UsersRoles.Add(

                        new UserRole(0, 1, 4)
                    );

                    context.SaveChanges();

                    context.UsersRoles.Add(

                        new UserRole(0, 2, 3)
                    );

                    context.SaveChanges();

                    context.UsersRoles.Add(

                        new UserRole(0, 2, 4)
                    );

                    context.SaveChanges();
                }

                if (!context.AppAdmins.Any())
                {
                    context.AppAdmins.Add(

                        new AppAdmin(0, 1, 1, true)
                    );

                    context.SaveChanges();

                    context.AppAdmins.Add(

                        new AppAdmin(0, 1, 2, true)
                    );

                    context.SaveChanges();

                    context.AppAdmins.Add(

                        new AppAdmin(0, 2, 2, true)
                    );

                    context.SaveChanges();

                    context.AppAdmins.Add(

                        new AppAdmin(0, 3, 1, true)
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
