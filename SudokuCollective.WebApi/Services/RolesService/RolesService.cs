using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.RoleRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class RolesService : IRolesService {

        private readonly DatabaseContext _context;

        public RolesService(DatabaseContext context) {

            _context = context;
        }

        public async Task<RoleResult> GetRole(
            int id, bool fullRecord = false) {

            var roleTaskResult = new RoleResult();

            try {

                var role = new Role();

                if (fullRecord) {

                    role = await _context.Roles
                        .Include(r => r.Users)
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (role == null) {

                        roleTaskResult.Message = "Role not found";

                        return roleTaskResult;
                    }

                    foreach (var r in role.Users) {

                        r.User = await _context.Users
                            .Where(u => u.Id == r.UserId)
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync();

                        r.User.Games = await _context.Games
                            .Where(g => g.User.Id == r.UserId)
                            .Include(g => g.SudokuMatrix)
                            .ToListAsync();                    
                        
                        foreach (var game in r.User.Games) {

                            game.SudokuMatrix = 
                                await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                        }
                    }

                    roleTaskResult.Success = true;
                    roleTaskResult.Role = role;

                } else {

                    role = await _context.Roles
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (role == null) {

                        roleTaskResult.Message = "Role not found";

                        return roleTaskResult;
                    }

                    roleTaskResult.Success = true;
                    roleTaskResult.Role = role;
                }

                return roleTaskResult;

            } catch (Exception e) {

                roleTaskResult.Message = e.Message;

                return roleTaskResult;
            }
        }

        public async Task<RolesResult> GetRoles(
            bool fullRecord = false) {

            var roleListTaskResult = new RolesResult();

            try {

                var roles = new List<Role>();

                if (fullRecord) {

                    roles = await _context.Roles
                        .Where(r => r.Id != 1 && r.Id != 2)
                        .Include(r => r.Users)
                        .ToListAsync();

                    foreach (var role in roles) {

                        foreach (var r in role.Users) {

                            r.User = await _context.Users
                                .Where(u => u.Id == r.UserId)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync();

                            r.User.Games = await _context.Games
                                .Where(g => g.User.Id == r.UserId)
                                .Include(g => g.SudokuMatrix)
                                .ToListAsync();                    
                            
                            foreach (var game in r.User.Games) {

                                game.SudokuMatrix = 
                                    await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                            }
                        }
                    }

                    roleListTaskResult.Success = true;
                    roleListTaskResult.Roles = roles;

                } else {

                    roles = await _context.Roles
                        .Where(r => r.Id != 1 && r.Id != 2)
                        .ToListAsync();

                    roleListTaskResult.Success = true;
                    roleListTaskResult.Roles = roles;
                }

                return roleListTaskResult;

            } catch (Exception e) {

                roleListTaskResult.Message = e.Message;

                return roleListTaskResult;
            }
        }

        public async Task<RoleResult> CreateRole(string name, 
            RoleLevel roleLevel) {

            var roleTaskResult = new RoleResult();

            try {

                Role role = new Role() { Name = name, RoleLevel = roleLevel };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                
                roleTaskResult.Success = true;
                roleTaskResult.Role = role;

                return roleTaskResult;

            } catch (Exception e) {

                roleTaskResult.Message = e.Message;

                return roleTaskResult;
            }
        }

        public async Task<BaseResult> UpdateRole(int id, UpdateRoleRequest updateRoleRO) {

            var baseTaskResult = new BaseResult();
            
            try {

                if (id == updateRoleRO.Id) {

                    var role = await _context.Roles
                        .Where(d => d.Id == updateRoleRO.Id)
                        .FirstOrDefaultAsync();

                    if (role != null) {

                        baseTaskResult.Message = "Role not found";

                        return baseTaskResult;
                    }

                    role.Name = updateRoleRO.Name;
                    role.RoleLevel = updateRoleRO.RoleLevel;

                    _context.Roles.Update(role);
                    
                    await _context.SaveChangesAsync();

                    baseTaskResult.Success = true;
                }

                return baseTaskResult;

            } catch (Exception e) {

                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<BaseResult> DeleteRole(int id) {

            var baseTaskResult = new BaseResult();

            try {

                var role = await _context.Roles.FindAsync(id);

                if (role != null) {

                    baseTaskResult.Message = "Role not found";

                    return baseTaskResult;
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult;

            } catch (Exception e) {

                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }
    }
}
