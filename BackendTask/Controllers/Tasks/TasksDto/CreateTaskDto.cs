using BackendTask.Shared;
using BackendTask.Shared.Enums;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BackendTask.API.Controllers.Tasks.TasksDto
{
    public class CreateTaskDto : IValidatableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityLevel Priority { get; set; }
        public int CategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var factory = validationContext.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(CommonResource));
            var results = new List<ValidationResult>();

            // Validate Title
            if (string.IsNullOrWhiteSpace(Title))
            {
                results.Add(new ValidationResult(
                    localizer["Title is required"],
                    new[] { nameof(Title) }));
            }
            else if (Title.Length > 100)
            {
                results.Add(new ValidationResult(
                    localizer["Title cannot exceed 100 characters"],
                    new[] { nameof(Title) }));
            }

            // Validate Description
            if (Description != null && Description.Length > 500)
            {
                results.Add(new ValidationResult(
                    localizer["Description cannot exceed 500 characters"],
                    new[] { nameof(Description) }));
            }

            // Validate DueDate
            if (DueDate == default)
            {
                results.Add(new ValidationResult(
                    localizer["Due date is required"],
                    new[] { nameof(DueDate) }));
            }

            // Validate Priority
            if (!Enum.IsDefined(typeof(PriorityLevel), Priority))
            {
                results.Add(new ValidationResult(
                    localizer["Priority is required"],
                    new[] { nameof(Priority) }));
            }

            // Validate CategoryId
            if (CategoryId <= 0)
            {
                results.Add(new ValidationResult(
                    localizer["Category is required"],
                    new[] { nameof(CategoryId) }));
            }

            return results;
        }
    }
}
