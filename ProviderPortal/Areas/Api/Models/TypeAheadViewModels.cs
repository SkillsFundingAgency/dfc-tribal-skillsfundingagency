namespace Tribal.SkillsFundingAgency.ProviderPortal.Areas.Api.Models
{
    public class TypeAheadProviderResult
    {
        // ReSharper disable InconsistentNaming
        public string id { get; set; }
        public int? ukprn { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string town { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public bool deleted { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class TypeaheadUserResult
    {
        // ReSharper disable InconsistentNaming
        //public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    ///     Model for the typeahead partial view
    /// </summary>
    public class TypeaheadViewModel
    {
        /// <summary>
        ///     Array of the JavaScript field names to include in the searchable text.
        /// </summary>
        public string[] SearchFields;

        /// <summary>
        ///     An optional URL where all the data can be prefetched from, this data is cached locally.
        /// </summary>
        public string PrefetchUrl { get; set; }

        /// <summary>
        ///     An optional URL where data can be looked up, use %QUERY in the URL to get the current search terms.
        /// </summary>
        public string RemoteUrl { get; set; }

        /// <summary>
        ///     When true this forces the browser's cached copy of the data to be cleared, this is useful during development.
        /// </summary>
        public bool ClearPrefetchCache { get; set; }

        /// <summary>
        ///     The jQuery selector of the input field to attach to.
        /// </summary>
        public string JQuerySelector { get; set; }

        /// <summary>
        ///     The ID of the hidden field to receive the selected item ID.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The Json object field to display in the text box.
        /// </summary>
        public string DisplayKey { get; set; }

        /// <summary>
        ///     The Json object field containing the item ID.
        /// </summary>
        public string ValueKey { get; set; }

        /// <summary>
        ///     HTML of a message to display if the current search yields no results.
        /// </summary>
        public string EmptyMessage { get; set; }

        /// <summary>
        ///     JavaScript statement constructing the HTML to display as a suggestion in the list of search results. This should be
        ///     in the form of a function that returns a string, e.g. "function (a) {return '
        ///     <p>a.name<br /><small>a.otherstuff</small></p>';}"
        /// </summary>
        public string Suggestion { get; set; }

        /// <summary>
        ///     Determines whether to use a data-typeahead-name attribute on the textbox for the "Name" parameter
        /// </summary>
        public bool UseAttributeForName { get; set; }

        /// <summary>
        ///     Determines whether the show a "Your query did not return any results" message
        /// </summary>
        public bool HideEmptyMessage { get; set; }

        /// <summary>
        ///     Specified the number of suggestions to show, default value is 15 suggestions.
        /// </summary>
        public int SuggestionLimit { get; set; }

        /// <summary>
        ///     JavaScript statements to execute when the typeahead:selected event is fired. The JavaScript variable obj contains
        ///     the typeahead object and datum contains the selected suggestion."
        /// </summary>
        public string OnSelect { get; set; }

        /// <summary>
        ///     JavaScript statements to execute when the typeahead:autocompleted event is fired. The JavaScript variable obj
        ///     contains the typeahead object and datum contains the selected suggestion."
        /// </summary>
        public string OnAutoComplete { get; set; }

        /// <summary>
        ///     JavaScript statements to execute when the typeahead:change event is fired. The JavaScript variable obj contains the
        ///     typeahead object and datum contains the selected suggestion."
        /// </summary>
        public string OnChange { get; set; }
    }

    public class TypeAheadAwardingOrganisationResult
    {
        public string LearningAimAwardOrgCode { get; set; }
        public string AwardOrgName { get; set; }
    }

    public class TypeAheadCourseLanguageResult
    {
        public string Language { get; set; }
    }

    public class TypeAheadQualificationTitleResult
    {
        public string QualificationTitle { get; set; }
    }

    public class TypeAheadLearningAimResult
    {
        // ReSharper disable InconsistentNaming
        public string LearningAimRefId { get; set; }
        public string LearningAimTitle { get; set; }
        public string Qualification { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class TypeAheadVenueLocationResult
    {
        // ReSharper disable InconsistentNaming
        public int VenueLocationId { get; set; }
        public string LocationName { get; set; }
        public string ParentVenueLocation { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class TypeAheadLearnDirectClassificationResult
    {
        public string LearnDirectClassificationRef { get; set; }
        public string LearnDirectClassSystemCodeDesc { get; set; }
    }

    public class TypeAheadStandardsAndFrameworksResult
    {
        public int? FrameworkId { get; set; }
        public int? StandardCodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}