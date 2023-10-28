namespace Scrapper.Domain.Abstractions
{
    public class PageResult<T>
    {
        /// <summary>
        /// Gets or sets the total number of records.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of records per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public List<T> Items { get; set; }
        
        public PageResult()
        {
            Items = new List<T>();
        }
    }
}
