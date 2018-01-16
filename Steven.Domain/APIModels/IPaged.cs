namespace MLS.Domain.APIModels
{
    public interface IPaged
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
        int TotalPageCount { get; set; } 
    }
}