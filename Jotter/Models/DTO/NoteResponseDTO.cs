namespace Jotter.Models.DTO
{
    public class NoteResponseDTO
    {
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
