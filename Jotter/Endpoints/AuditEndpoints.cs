using AutoMapper;
using Jotter.Models;
using Jotter.Models.DTO;
using Jotter.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using static Jotter.Models.Enum.Enum;

namespace Jotter.Endpoints
{
    public static class AuditEndpoints

    {
        public static void ConfigureAudits(this WebApplication app)
        {
            app.MapGet("/api/audits", GetAllAudits)
                .Produces<ResponseDTO>(200);

            app.MapGet("/api/audit", GetAuditByNoteId)
            .Produces<ResponseDTO>(200)
             .Produces(400);

        }

        private async static Task<IResult> GetAllAudits(IAuditRepository _auditRepo, IMapper _mapper)
        {
            ResponseDTO response = new();
            response.Result = _mapper.Map<List<AuditResponseDTO>>(await _auditRepo.GetAuditsAsync(GetLoggedInUser().Id));
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }

        private async static Task<IResult> GetAuditByNoteId(IAuditRepository _auditRepo, IMapper _mapper, ILogger<Program> _logger, [FromQuery] string noteId)
        {
            ResponseDTO response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            var audit = await _auditRepo.GetAuditsByNoteIdAsync(GetLoggedInUser().Id, noteId);
            if (audit == null)
            {
                _logger.LogWarning($"Note id {noteId} not found");
                return Results.BadRequest(response);
            }

            response.Result = _mapper.Map<List<AuditResponseDTO>>(audit);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Results.Ok(response);
        }


        private static User GetLoggedInUser()
        {
            return new User { Id = "1", Email = "guestuser@gmail.com", UserName = "GuestUser" };
        }

    }

}
