namespace SudokuApp.WebApi.Models.RequestObjects.UserRequests {
    
    public class UpdateRoleRO {

        public int UserId { get; set; }
        public int[] RoleIds { get; set; }
    }
}
