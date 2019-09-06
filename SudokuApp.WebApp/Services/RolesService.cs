using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.Models.Enums;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services {

    public class RolesService : IRolesService {

        private readonly ApplicationDbContext _context;

        public RolesService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<Role>> GetRole(int id) {

            var role = await _context.Roles.SingleOrDefaultAsync(d => d.Id == id);

            if (role == null) {

                return new Role {
                    
                    Name = string.Empty,
                    RoleLevel = RoleLevel.NULL,
                    Users = new List<UserRole>()
                };
            }

            return role;
        }

        public async Task<ActionResult<IEnumerable<Role>>> GeRoles() {

            return await _context.Roles.Include(r => r.Users).ToListAsync();
        }

        public async Task<Role> CreateRole(string name, RoleLevel roleLevel) {

            Role role = new Role() { Name = name, RoleLevel = roleLevel };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task UpdateRole(int id, Role role) {

            if (id == role.Id) {

                _context.Entry(role).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Role> DeleteRole(int id) {

            var role = await _context.Roles.FindAsync(id);

            if (role == null) {

                return new Role {
                    
                    Name = string.Empty,
                    RoleLevel = RoleLevel.NULL,
                    Users = new List<UserRole>()
                };
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return role;
        }
    }
}