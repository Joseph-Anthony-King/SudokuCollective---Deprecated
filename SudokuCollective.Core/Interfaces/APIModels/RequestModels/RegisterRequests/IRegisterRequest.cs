namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IRegisterRequest : IBaseRequest
    {
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string NickName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
