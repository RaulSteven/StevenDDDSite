namespace Steven.Domain.ViewModels
{
    public class FilterGroupModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Source { get; set; }
        public string FilterGroups { get; set; }
        //public FilterGroup FilterGroup { get; set; }

        public string SourceProperties { get; set; }

    }
}
