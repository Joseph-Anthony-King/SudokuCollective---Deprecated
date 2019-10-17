namespace SudokuCollective.Domain.Interfaces {

    public interface IUserApp : IEntityBase {

        public int UserId { get; set; }
        public int AppId { get; set; }
    }
}
