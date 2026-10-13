namespace ClinicManagement.Model.ViewModels
{
    public class TreeViewModel
    {
        public string? Title { get; set; }
        public string? Key { get; set; }
        public bool Expanded { get; set; }
        public string? Icon { get; set; }
        public bool IsLeaf { get; set; }
        public List<TreeViewModel>? Children { get; set; }
    }
}
