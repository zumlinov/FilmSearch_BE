using FilmSearchClasses.Dtos.SearchParams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmSearchClasses.Helpers
{
    public static class SearchParamConverter
    {
        public static Dictionary<string, string> PrepareSearchParams(SearchParam[] searchParams)
        {
            var outgoingParam = new Dictionary<string, string>();

            if (!searchParams.Any())
                return outgoingParam;

            foreach (var searchParam in searchParams)
            {
                switch (searchParam)
                {
                    case TitleSearchParam:
                        outgoingParam.Add("s", (searchParam as TitleSearchParam).Title);
                        break;
                    case YearOfReleaseSearchParam:
                        outgoingParam.Add("y", (searchParam as YearOfReleaseSearchParam).YearOfRelease);
                        break;
                    case MovieTypeSearchParam:
                        outgoingParam.Add("type", (searchParam as MovieTypeSearchParam).Type.ToString());
                        break;
                    default:
                        throw new ArgumentException($"Unsuported serch param: {searchParam.GetType().Name}");
                }
            }

            return outgoingParam;
        }
    }
}
