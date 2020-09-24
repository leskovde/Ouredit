using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Models
{
    public class SearchContext
    {
        /// <summary>
        /// Model representing serching text and booleans for serching options.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="matchCase"></param>
        /// <param name="matchWholeWord"></param>
        /// <param name="useRegex"></param>
        public SearchContext(
            string searchText,
            bool matchCase = false,
            bool matchWholeWord = false,
            bool useRegex = false)
        {
            SearchText = searchText;
            MatchCase = matchCase;
            MatchWholeWord = matchWholeWord;
            UseRegex = useRegex;
        }

        public string SearchText { get; }

        public bool MatchCase { get; }

        public bool MatchWholeWord { get; }

        public bool UseRegex { get; }
    }
}
