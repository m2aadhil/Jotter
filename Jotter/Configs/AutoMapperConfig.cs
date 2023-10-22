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
            CreateMap<Audit, AuditResponseDTO>().ForMember(d => d.Event,
                op => op.MapFrom(o =>  o.Event.ToString() ));
            ;
        }
    }
}
