using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.Models.Enums;

namespace SudokuApp.WebApp.Services.Interfaces {
    
    public interface IRolesService {

        Task<ActionResult<Role>> GetRole(int id);
        Task<ActionResult<IEnumerable<Role>>> GeRoles();
        Task<Role> CreateRole(string name, RoleLevel roleLevel);
        Task UpdateRole(int id, Role difficulty);
        Task<Role> DeleteRole(int id);
    }}