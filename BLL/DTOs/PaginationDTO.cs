namespace BLL.DTOs
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class PaginationParams
    {
        private int pageNumber = 1;
        private int pageSize = 10;

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value < 1 ? 10 : value > 100 ? 100 : value;
        }
    }
}
