using Microsoft.AspNetCore.Http;
using Solution.Business.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Common.ViewModel
{
    public class ClassVM
    {
        public string? ClassId { get; set; }

        [Required(ErrorMessage = "Grade Level is required.")]
        public string? GradeLevel { get; set; }

        [Required(ErrorMessage = "Program Details are required.")]
        public string? ProgramDetails { get; set; }

        [Required(ErrorMessage = "Class Time is required.")]
        public ClassTimeSlots? ClassTime { get; set; }

        [Required(ErrorMessage = "Maximum Size is required.")]
        public int? MaxSize { get; set; }
    }
}
