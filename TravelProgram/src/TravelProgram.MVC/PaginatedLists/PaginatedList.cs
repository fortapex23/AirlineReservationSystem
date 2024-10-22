namespace TravelProgram.MVC.PaginatedLists
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> datas, int count, int page, int pagesize)
        {
            AddRange(datas);
            ActivePage = page;
            TotalPage = (int)Math.Ceiling(count / (double)pagesize);
        }

        public int ActivePage { get; set; }
        public int TotalPage { get; set; }
        public bool Next { get => ActivePage != TotalPage; }
        public bool Prev { get => ActivePage > 1; }

        public static PaginatedList<T> Create(IQueryable<T> query, int page, int pagesize)
        {
            return new PaginatedList<T>(query.Skip((page - 1) * pagesize).Take(pagesize).ToList(), query.Count(), page, pagesize);
        }
    }
}
