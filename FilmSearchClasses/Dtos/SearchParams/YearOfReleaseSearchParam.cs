namespace FilmSearchClasses.Dtos.SearchParams
{
    public class YearOfReleaseSearchParam : SearchParam
    {
        public YearOfReleaseSearchParam()
        {
            CriteriaType = Enums.SearchCriteriaType.YearOfReleased;
        }

        public string YearOfRelease { get; set; }
    }
}
