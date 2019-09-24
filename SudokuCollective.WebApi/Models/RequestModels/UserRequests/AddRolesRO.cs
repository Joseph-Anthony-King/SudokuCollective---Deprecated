namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {
    
    public class UpdateUserRolesRO : BaseRequestRO {

        public int UserId { get; set; }
        public int[] RoleIds { get; set; }
    }
}
