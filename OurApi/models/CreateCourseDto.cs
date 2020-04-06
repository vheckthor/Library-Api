using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OurApi.models
{
    public class CreateCourseDto:IValidatableObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description) 
            { 
                yield return new ValidationResult(
                    "The provided title and description should not be the same.",
                    new[] {"CreateCourseDto"} 
                    ); 
            }

        }
    }
}