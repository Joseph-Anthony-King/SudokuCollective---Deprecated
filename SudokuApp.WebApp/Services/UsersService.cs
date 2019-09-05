using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Helpers;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Models.DTO;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services
{

    public class UsersService : IUsersService {

        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<User>> GetUser(int id) {

            var user = await _context.Users
                .Include(u => u.Games)
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {

                return new User {

                    UserName = String.Empty,
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    NickName = string.Empty,
                    Email = string.Empty,
                    Password = string.Empty
                };
            }

            return user;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {

            var users = await _context.Users
                .OrderBy(u => u.Id)
                .Include(u => u.Games)
                .ThenInclude(g => g.SudokuMatrix)
                .Include(u => u.Permissions)
                .ToListAsync();

            foreach (var user in users) {

                foreach (var game in user.Games) {
                    
                    game.SudokuMatrix.SudokuCells = await StaticApiHelpers.ResetSudokuCells(game, _context);
                }
            }

            return users;
        }

        public async Task<User> CreateUser(UserDTO userDTO) {

            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            User user = new User {

                UserName = userDTO.UserName,
                FirstName = userDTO.FirstName, 
                LastName = userDTO.LastName,
                NickName = userDTO.NickName,
                DateCreated = DateTime.UtcNow,
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, salt)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Add default user permission to the new user
            var savedUser = await _context.Users.Where(u => u.UserName.Equals(user.UserName)).FirstOrDefaultAsync();
            var defaultPermission = await _context.Permissions.Where(p => p.Id == 2).FirstOrDefaultAsync();

            UserPermission newUserPermissions = new UserPermission {

                UserId = savedUser.Id,
                User = savedUser,
                PermissionId = defaultPermission.Id,
                Permission = defaultPermission
            };

            _context.UsersPermissions.Add(newUserPermissions);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task UpdateUser(int id, User user) {

            if (id == user.Id) {

                // Encrypt the users password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Entry(user).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> DeleteUser(int id) {

            var user = await _context.Users.FindAsync(id);

            if (user == null) {

                return new User {

                    UserName = String.Empty,
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    NickName = string.Empty,
                    Email = string.Empty,
                    Password = string.Empty
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
