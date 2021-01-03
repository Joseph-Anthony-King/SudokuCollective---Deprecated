using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class SolveRequest : ISolveRequest
    {
        public int[] FirstRow { get; set; }
        public int[] SecondRow { get; set; }
        public int[] ThirdRow { get; set; }
        public int[] FourthRow { get; set; }
        public int[] FifthRow { get; set; }
        public int[] SixthRow { get; set; }
        public int[] SeventhRow { get; set; }
        public int[] EighthRow { get; set; }
        public int[] NinthRow { get; set; }

        public SolveRequest() : base()
        {
            FirstRow = new int[9];
            SecondRow = new int[9];
            ThirdRow = new int[9];
            FourthRow = new int[9];
            FifthRow = new int[9];
            SixthRow = new int[9];
            SeventhRow = new int[9];
            EighthRow = new int[9];
            NinthRow = new int[9];
        }
    }
}
