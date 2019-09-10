namespace SudokuApp.WebApp.Models.RequestObjects.SolveRequests {
    
    public class SolveRequestsRO {

        public int UserId { get; set; }
        public int Minutes { get; set; }
        public int[] FirstRow { get; set; }
        public int[] SecondRow { get; set; }
        public int[] ThirdRow { get; set; }
        public int[] FourthRow { get; set; }
        public int[] FifthRow { get; set; }
        public int[] SixthRow { get; set; }
        public int[] SeventhRow { get; set; }
        public int[] EighthRow { get; set; }
        public int[] NinthRow { get; set; }
    }
}