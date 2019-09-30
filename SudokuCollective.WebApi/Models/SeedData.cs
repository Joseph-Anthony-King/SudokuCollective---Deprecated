using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;

namespace SudokuCollective.WebApi.Models {

    public class SeedData {

        public static void EnsurePopulated(IApplicationBuilder app, IConfiguration config) {

            using (var servicesScope = app.ApplicationServices.CreateScope()) {
                
                var createdDate = DateTime.UtcNow;

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

                    context.Users.Add(

                        new User {

                            FirstName = config.GetValue<string>("DefaultUserAccounts:SuperUser:FirstName"),
                            LastName = config.GetValue<string>("DefaultUserAccounts:SuperUser:LastName"),
                            NickName = config.GetValue<string>("DefaultUserAccounts:SuperUser:NickName"),
                            UserName = config.GetValue<string>("DefaultUserAccounts:SuperUser:UserName"),
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            Email = config.GetValue<string>("DefaultUserAccounts:SuperUser:Email"),
                            Password = BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("DefaultUserAccounts:SuperUser:Password", salt)),
                            IsActive = true
                        }
                    );

                    context.SaveChanges();

                    context.Users.Add(

                        new User {

                            FirstName = config.GetValue<string>("DefaultUserAccounts:AdminUser:FirstName"),
                            LastName = config.GetValue<string>("DefaultUserAccounts:AdminUser:LastName"),
                            NickName = config.GetValue<string>("DefaultUserAccounts:AdminUser:NickName"),
                            UserName = config.GetValue<string>("DefaultUserAccounts:AdminUser:UserName"),
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            Email = config.GetValue<string>("DefaultUserAccounts:AdminUser:Email"),
                            Password = BCrypt.Net.BCrypt.HashPassword(config.GetValue<string>("DefaultUserAccounts:AdminUser:Password", salt)),
                            IsActive = true
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.Apps.Any()) {

                    context.Apps.Add(

                        new App {
                            
                            Name = config.GetValue<string>("DefaultAdminApp:Name"),
                            License = config.GetValue<string>("DefaultAdminApp:License"),
                            OwnerId = 1,
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            DevUrl = config.GetValue<string>("DefaultAdminApp:DevUrl"),
                            LiveUrl = config.GetValue<string>("DefaultAdminApp:LiveUrl"),
                            IsActive = true
                        }
                    );

                    context.SaveChanges();

                    context.Apps.Add(

                        new App {
                            
                            Name = config.GetValue<string>("DefaultClientApp:Name"),
                            License = config.GetValue<string>("DefaultClientApp:License"),
                            OwnerId = 1,
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            DevUrl = config.GetValue<string>("DefaultClientApp:DevUrl"),
                            LiveUrl = config.GetValue<string>("DefaultClientApp:LiveUrl"),
                            IsActive = true
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.UsersApps.Any()) {
                    
                    context.UsersApps.AddRange(

                        new UserApp {

                            UserId = 1,
                            AppId = 1
                        },

                        new UserApp {

                            UserId = 2,
                            AppId = 1
                        },

                        new UserApp {

                            UserId = 1,
                            AppId = 2
                        },

                        new UserApp {

                            UserId = 2,
                            AppId = 2
                        }
                    );
                }

                if (!context.UsersRoles.Any()) {

                    context.UsersRoles.AddRange(

                        new UserRole {

                            UserId = 1,
                            RoleId = 2
                        },

                        new UserRole {

                            UserId = 1,
                            RoleId = 3
                        },

                        new UserRole {

                            UserId = 1,
                            RoleId = 4
                        },

                        new UserRole {

                            UserId = 2,
                            RoleId = 3
                        },

                        new UserRole {

                            UserId = 2,
                            RoleId = 4
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
