namespace Dating.API.Helpers
{
    public class MessageParams
    {
        private const int MaXPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 12;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaXPageSize) ? MaXPageSize : value; }
        }

        public int UserId { get; set; }

        public string MessageContainer { get; set; } = "Unread";

    }
}