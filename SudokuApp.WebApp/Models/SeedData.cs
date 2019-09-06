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

                if (!context.Roles.Any()) {

                    context.Roles.AddRange(
                        
                        new Role {

                            Name = "Null",
                            RoleLevel = RoleLevel.NULL
                        },
                        
                        new Role {

                            Name = "Super User",
                            RoleLevel = RoleLevel.SUPERUSER
                        },
                        
                        new Role {

                            Name = "Admin",
                            RoleLevel = RoleLevel.ADMIN
                        },
                        
                        new Role {

                            Name = "User",
                            RoleLevel = RoleLevel.USER
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
                    var createdDate = DateTime.UtcNow;

                    context.Users.AddRange(

                        new User {

                            FirstName = config.GetValue<string>("DefaultUserAccounts:SuperUser:FirstName"),
                            LastName = config.GetValue<string>("DefaultUserAccounts:SuperUser:LastName"),
                            NickName = config.GetValue<string>("DefaultUserAccounts:SuperUser:NickName"),
                            UserName = config.GetValue<string>("DefaultUserAccounts:SuperUser:UserName"),
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            Email = config.GetValue<string>("DefaultUserAccounts:SuperUser:Email"),
                            Password = BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("DefaultUserAccounts:SuperUser:Password", salt))
                        },

                        new User {

                            FirstName = config.GetValue<string>("DefaultUserAccounts:AdminUser:FirstName"),
                            LastName = config.GetValue<string>("DefaultUserAccounts:AdminUser:LastName"),
                            NickName = config.GetValue<string>("DefaultUserAccounts:AdminUser:NickName"),
                            UserName = config.GetValue<string>("DefaultUserAccounts:AdminUser:UserName"),
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            Email = config.GetValue<string>("DefaultUserAccounts:AdminUser:Email"),
                            Password = BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("DefaultUserAccounts:AdminUser:Password", salt))
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.UsersRoles.Any()) {

                    context.UsersRoles.AddRange(

                        new UserRole {

                            UserId = 1,
                            RoleId = 2,
                            RoleName = "Super User"
                        },

                        new UserRole {

                            UserId = 1,
                            RoleId = 3,
                            RoleName = "Admin"
                        },

                        new UserRole {

                            UserId = 1,
                            RoleId = 4,
                            RoleName = "User"
                        },

                        new UserRole {

                            UserId = 2,
                            RoleId = 3,
                            RoleName = "Admin"
                        },

                        new UserRole {

                            UserId = 2,
                            RoleId = 4,
                            RoleName = "User"
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
