using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class AppRequest : IAppRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public string Name { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
        public bool InDevelopment { get; set; }

        public AppRequest() : base()
        {
            Name = string.Empty;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
            InDevelopment = false;

            if (PageListModel == null)
            {
                PageListModel = new PageListModel();
            }
        }
    }
}
