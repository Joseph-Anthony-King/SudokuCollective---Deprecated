namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {
    
    public class UpdateUserRolesRO : BaseRequestRO {

        public int[] RoleIds { get; set; }
    }
}
