﻿namespace app.api.Helpers.Users
{
    public class UserParams
    {
        private const int MAX_PAGE_SIZE = 40;
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value; }
        }
    }
}