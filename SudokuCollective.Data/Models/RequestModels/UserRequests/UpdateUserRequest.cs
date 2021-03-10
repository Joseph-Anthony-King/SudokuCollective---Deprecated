using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateUserRequest : IUpdateUserRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPaginator Paginator { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }

        public UpdateUserRequest() : base()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            Email = string.Empty;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
