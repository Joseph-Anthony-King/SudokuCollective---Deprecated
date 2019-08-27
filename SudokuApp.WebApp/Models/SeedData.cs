using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.Models;
using SudokuApp.Models.Enums;

namespace SudokuApp.WebApp.Models {

    public class SeedData {

        public static void EnsurePopulated(IApplicationBuilder app, IConfiguration config) {

            using (var servicesScope = app.ApplicationServices.CreateScope()) {

                ApplicationDbContext context = servicesScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                if (!context.Permissions.Any()) {

                    context.Permissions.AddRange(
                        
                        new Permission {

                            Name = "Admin",
                            PermissionLevel = PermissionLevel.ADMIN
                        },
                        
                        new Permission {

                            Name = "User",
                            PermissionLevel = PermissionLevel.USER
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.Difficulties.Any()) {

                    context.Difficulties.AddRange(

                        new Difficulty {

                            Name = "Test",
                            DifficultyLevel = DifficultyLevel.TEST
                        },

                        new Difficulty {

                            Name = "Easy",
                            DifficultyLevel = DifficultyLevel.EASY
                        },

                        new Difficulty {

                            Name = "Medium",
                            DifficultyLevel = DifficultyLevel.MEDIUM
                        },

                        new Difficulty {

                            Name = "Hard",
                            DifficultyLevel = DifficultyLevel.HARD
                        },

                        new Difficulty {

                            Name = "Evil",
                            DifficultyLevel = DifficultyLevel.EVIL
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.Users.Any()) {

                    context.Users.AddRange(

                        new User {

                            FirstName = config.GetValue<string>("AdminUser:FirstName"),
                            LastName = config.GetValue<string>("AdminUser:LastName"),
                            NickName = config.GetValue<string>("AdminUser:NickName"),
                            UserName = config.GetValue<string>("AdminUser:UserName"),
                            DateCreated = DateTime.Now,
                            Email = config.GetValue<string>("AdminUser:Email"),
                            Password = config.GetValue<string>("AdminUser:Password"),
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.UsersPermissions.Any()) {

                    context.UsersPermissions.AddRange(

                        new UserPermission {

                            UserId = 1,
                            PermissionId = 1
                        },

                        new UserPermission {

                            UserId = 1,
                            PermissionId = 2
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
