namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {
    
    public class UpdateRoleRO {

        public int UserId { get; set; }
        public int[] RoleIds { get; set; }
    }
}
