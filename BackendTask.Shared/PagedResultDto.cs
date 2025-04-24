namespace BackendTask.Shared
{
    [Serializable]
    public class PagedResultDto<T>
    {
        /// <summary>
        /// Total count of Items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of items in page
        /// </summary>
        public int PagesSize { get; set; }

        /// <summary>
        /// Items of the current page if specified
        /// </summary>
        public IReadOnlyList<T> Items { get; set; }

        public PagedResultDto()
        {
            Items = new List<T>();
        }

        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public PagedResultDto(PagedResultDto<T> model)
        {
            Items = model.Items;
            TotalCount = model.TotalCount;
            PagesSize = model.PagesSize;
        }
    }
}
