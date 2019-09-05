using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.Models;
using SudokuApp.Models.Enums;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SudokuApp.WebApp.Models {

    public class SeedData {

        public static void EnsurePopulated(IApplicationBuilder app, IConfiguration config) {

            using (var servicesScope = app.ApplicationServices.CreateScope()) {

                ApplicationDbContext context = servicesScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                if (!context.Permissions.Any()) {

                    context.Permissions.AddRange(
                        
                        new Permission {

                            Name = "Null",
                            PermissionLevel = PermissionLevel.NULL
                        },
                        
                        new Permission {

                            Name = "Super User",
                            PermissionLevel = PermissionLevel.SUPERUSER
                        },
                        
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

                            Name = "Null",
                            DifficultyLevel = DifficultyLevel.NULL
                        },

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

                    var salt = BCrypt.Net.BCrypt.GenerateSalt();

                    context.Users.AddRange(

                        new User {

                            FirstName = config.GetValue<string>("SuperUser:FirstName"),
                            LastName = config.GetValue<string>("SuperUser:LastName"),
                            NickName = config.GetValue<string>("SuperUser:NickName"),
                            UserName = config.GetValue<string>("SuperUser:UserName"),
                            DateCreated = DateTime.UtcNow,
                            Email = config.GetValue<string>("SuperUser:Email"),
                            Password = BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("SuperUser:Password", salt))
                        },

                        new User {

                            FirstName = config.GetValue<string>("AdminUser:FirstName"),
                            LastName = config.GetValue<string>("AdminUser:LastName"),
                            NickName = config.GetValue<string>("AdminUser:NickName"),
                            UserName = config.GetValue<string>("AdminUser:UserName"),
                            DateCreated = DateTime.UtcNow,
                            Email = config.GetValue<string>("AdminUser:Email"),
                            Password = BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("AdminUser:Password", salt))
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.UsersPermissions.Any()) {

                    context.UsersPermissions.AddRange(

                        new UserPermission {

                            UserId = 1,
                            PermissionId = 2
                        },

                        new UserPermission {

                            UserId = 1,
                            PermissionId = 3
                        },

                        new UserPermission {

                            UserId = 1,
                            PermissionId = 4
                        },

                        new UserPermission {

                            UserId = 2,
                            PermissionId = 3
                        },

                        new UserPermission {

                            UserId = 2,
                            PermissionId = 4
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
