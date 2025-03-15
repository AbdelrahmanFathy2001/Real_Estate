namespace Project.BLL.Specifications
{
    public class UserSpecParams
    {

        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        private string? search;

        public string Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

    }
}
