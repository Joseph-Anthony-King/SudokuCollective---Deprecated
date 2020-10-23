namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface ISolveRequest : IBaseRequest
    {
        int UserId { get; set; }
        int Minutes { get; set; }
        int[] FirstRow { get; set; }
        int[] SecondRow { get; set; }
        int[] ThirdRow { get; set; }
        int[] FourthRow { get; set; }
        int[] FifthRow { get; set; }
        int[] SixthRow { get; set; }
        int[] SeventhRow { get; set; }
        int[] EighthRow { get; set; }
        int[] NinthRow { get; set; }
    }
}
