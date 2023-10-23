using FluentValidation;
using Jotter.Models.DTO;

namespace Jotter.Validations
{
    public class NoteRequestValidator:  AbstractValidator<NoteRequestDTO>
    {
        public NoteRequestValidator()
        {
            RuleFor(model => model.Title).NotEmpty();
            RuleFor(model => model.Content).NotEmpty();
        }
    }
}
