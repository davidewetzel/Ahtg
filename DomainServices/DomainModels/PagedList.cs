namespace DomainServices.DomainModels
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }

        public int TotalItemCount { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
