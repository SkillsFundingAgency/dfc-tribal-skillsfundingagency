using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddStopWordModel
    {
        [LanguageRequired]
        [LanguageDisplay("Stop Word")]
        [LanguageStringLength(64, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your course title.")]
        public String StopWord { get; set; }
    }

    public class ListStopWordsModel
    {
        public List<ListStopWordsItemModel> Items { get; set; }

        public ListStopWordsModel()
        {
            Items = new List<ListStopWordsItemModel>();
        }
    }

    public class ListStopWordsItemModel
    {
        [LanguageDisplay("Stop Word")]
        public String StopWord { get; set; }

        public ListStopWordsItemModel() { }

        public ListStopWordsItemModel(String stopWord) : this()
        {
            this.StopWord = stopWord;
        }
    }
}