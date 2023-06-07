namespace StudyingTelegramApi.Models {
    public class Paging {
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool PasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool SsLastPage { get; set; }
        public int FirstItemOnPage { get; set; }
        public int LastItemOnPage { get; set; }
    }

    public class Group {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
    }

    public class GroupResponse {
        //public Paging Paging { get; set; }
        public List<Group> Data { get; set; }
    }
}
