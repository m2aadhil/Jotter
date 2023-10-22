using AutoMapper;
using Jotter.Models;
using Jotter.Models.DTO;

namespace Jotter.Configs
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<NoteRequestDTO, Note>();
            CreateMap<Note, NoteResponseDTO>();
        }
    }
}
