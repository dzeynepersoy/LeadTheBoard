namespace LeadTheBoard.Shared.Models.MainPage
{
    public class MainPageModel
    {
        public MainPageUserModel Users { get; set; } = new();

        public MainPageTaskModel Tasks { get; set; } = new();

        public MainPageProductModel Products { get; set; } = new();

        public MainPageMachinesModel Machines { get; set; } = new();

        public MainPageTaskStatistics TaskStatistics { get; set; } = new();


    }

}
