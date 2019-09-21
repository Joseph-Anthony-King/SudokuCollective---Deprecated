using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestObjects.RegisterRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class UsersService : IUsersService {

        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<User>> GetUser(int id, bool fullRecord = true) {

            var user = new User();

            if (fullRecord) {

                user = await _context.Users
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null) {
                    
                    var createdDate = DateTime.UtcNow;

                    return new User {

                        UserName = String.Empty,
                        FirstName = String.Empty,
                        LastName = String.Empty,
                        NickName = string.Empty,
                        DateCreated = createdDate,
                        DateUpdated = createdDate,
                        Email = string.Empty,
                        Password = string.Empty
                    };
                }

                foreach (var game in user.Games) {
                    
                    game.SudokuMatrix = await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                }

                foreach (var userRole in user.Roles) {

                    userRole.Role = await _context.Roles
                        .Where(r => r.Id == userRole.RoleId)
                        .FirstOrDefaultAsync();

                    userRole.Role.Users = null;
                }

            } else {

                user = await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == id);

                user.Games = null;
            }

            return user;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers(bool fullRecord = true) {

            var users = new List<User>();

            if (fullRecord){

                users = await _context.Users
                    .OrderBy(u => u.Id)
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .ToListAsync();

                foreach (var user in users) {

                    foreach (var game in user.Games) {
                        
                        // Attach the matrices for each game.
                        game.SudokuMatrix = await _context.SudokuMatrices
                            .Where(m => m.Id == game.SudokuMatrixId)
                            .Include(m => m.Difficulty)
                            .FirstOrDefaultAsync();

                        // Remove the navigation reference to each difficulties matrices
                        // to prevent circular references to all matrices.
                        game.SudokuMatrix.Difficulty.Matrices = null;
                    }

                    foreach (var userRole in user.Roles) {

                        userRole.Role = await _context.Roles
                            .Where(r => r.Id == userRole.RoleId)
                            .FirstOrDefaultAsync();

                        userRole.Role.Users = null;
                    }
                }

            } else {               

                users = await _context.Users
                    .OrderBy(u => u.Id)
                    .Include(u => u.Roles)
                    .ToListAsync();

                foreach (var user in users) {

                    foreach (var userRole in user.Roles) {

                        userRole.Role = await _context.Roles
                            .Where(r => r.Id == userRole.RoleId)
                            .FirstOrDefaultAsync();

                        userRole.Role.Users = null;
                    }
                }
            }

            return users;
        }

        public async Task<User> CreateUser(RegisterRO registerRO) {

            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            var createdDate = DateTime.UtcNow;

            User user = new User {

                UserName = registerRO.UserName,
                FirstName = registerRO.FirstName, 
                LastName = registerRO.LastName,
                NickName = registerRO.NickName,
                DateCreated = createdDate,
                DateUpdated = createdDate,
                Email = registerRO.Email,
                Password = BCrypt.Net.BCrypt
                    .HashPassword(registerRO.Password, salt)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Add default user permission to the new user
            var savedUser = await _context.Users
                .Where(u => u.UserName.Equals(user.UserName))
                .FirstOrDefaultAsync();

            var defaultRole = await _context.Roles
                .Where(p => p.Id == 4)
                .FirstOrDefaultAsync();

            UserRole newUserRole = new UserRole {

                UserId = savedUser.Id,
                User = savedUser,
                RoleId = defaultRole.Id,
                Role = defaultRole
            };

            _context.UsersRoles.Add(newUserRole);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task UpdateUser(int id, User user) {

            if (id == user.Id) {

                user.DateUpdated = DateTime.UtcNow;

                // Encrypt the users password
                user.Password = BCrypt.Net.BCrypt
                    .HashPassword(user.Password);

                _context.Entry(user).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> DeleteUser(int id) {

            var user = await _context.Users.FindAsync(id);

            if (user == null) {
                
                var createdDate = DateTime.UtcNow;

                return new User {

                    UserName = String.Empty,
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    NickName = string.Empty,
                    DateCreated = createdDate,
                    DateUpdated = createdDate,
                    Email = string.Empty,
                    Password = string.Empty
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task AddUserRoles(int userId, List<int> roleIds) {

            var user = await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            foreach (var userRole in user.Roles) {

                userRole.Role = await _context.Roles
                    .Where(r => r.Id == userRole.RoleId)
                    .FirstOrDefaultAsync();
            }

            var roles = new List<Role>();
            var userRoles = new List<UserRole>();

            foreach (var roleId in roleIds) {

                roles.Add(await _context.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync());
            }

            foreach (var role in roles) {

                if (!user.Roles.Any(ur => ur.RoleId == role.Id)) {
                    
                    userRoles.Add(new UserRole(){ 
                        UserId = user.Id, 
                        User = user, 
                        RoleId = role.Id, 
                        Role = role });
                }
            }

            if (userRoles.Count > 0) {
                
                _context.UsersRoles.AddRange(userRoles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserRoles(int userId, List<int> roleIds) {

            var usersRoles = _context.UsersRoles.Where(ur => ur.UserId == userId && roleIds.Contains(ur.RoleId));
                
            _context.UsersRoles.RemoveRange(usersRoles);
            await _context.SaveChangesAsync();
        }
    }
}