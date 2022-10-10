namespace CarsApi.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int entitiesPerPage { get; set; } = 10;
        private readonly int pageMaxSize = 50;
        public int EntitiesPerPage
        {
            get => entitiesPerPage;
            set
            {
                entitiesPerPage = (value > pageMaxSize) ? entitiesPerPage : value;
            }
        }
    }
}
