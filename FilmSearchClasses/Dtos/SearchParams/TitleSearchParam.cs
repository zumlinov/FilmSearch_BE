namespace FilmSearchClasses.Dtos.SearchParams
{
    public class TitleSearchParam : SearchParam
    {
        public TitleSearchParam()
        {
            CriteriaType = Enums.SearchCriteriaType.Title;
        }

        public string Title { get; set; }
    }
}
