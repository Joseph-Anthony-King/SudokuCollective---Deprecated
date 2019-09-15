using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class SudokuSolution : ISudokuSolution {

        public int Id { get; set; }
        public List<int> SolutionList { get; set; }
        public List<int> FirstRow { get => GetValues(0, 9); }
        public List<int> SecondRow { get => GetValues(9, 9); }
        public List<int> ThirdRow { get => GetValues(18, 9); }
        public List<int> FourthRow { get => GetValues(27, 9); }
        public List<int> FifthRow { get => GetValues(36, 9); }
        public List<int> SixthRow { get => GetValues(45, 9); }
        public List<int> SeventhRow { get => GetValues(54, 9); }
        public List<int> EighthRow { get => GetValues(63, 9); }
        public List<int> NinthRow { get => GetValues(72, 9); }
        public virtual Game Game { get; set; }

        public SudokuSolution() {

            SolutionList = new List<int>();
        }

        [JsonConstructor]
        public SudokuSolution(List<int> intList) : this() {

            SolutionList = intList;
        }

        private List<int> GetValues (int skipValue, int takeValue) {
            
            return SolutionList.Skip(skipValue).Take(takeValue).ToList();
        }
    }
}
