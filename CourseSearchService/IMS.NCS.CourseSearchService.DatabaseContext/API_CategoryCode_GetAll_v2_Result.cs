//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMS.NCS.CourseSearchService.DatabaseContext
{
    using System;
    
    public partial class API_CategoryCode_GetAll_v2_Result
    {
        public int CategoryCodeId { get; set; }
        public string CategoryCodeName { get; set; }
        public string ParentCategoryCode { get; set; }
        public string Description { get; set; }
        public bool IsSearchable { get; set; }
        public int TotalCourses { get; set; }
        public Nullable<int> Level { get; set; }
    }
}
