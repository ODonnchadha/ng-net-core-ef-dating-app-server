namespace app.api.Helpers
{
    public class Params
    {
        public const int MAX_PAGE_SIZE = 40;
        public int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value; }
        }
        public int UserId { get; set; }
    }
}
