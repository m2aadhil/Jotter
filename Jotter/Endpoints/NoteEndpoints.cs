﻿using AutoMapper;
using Jotter.Configs;
using Jotter.Models;
using Jotter.Models.DTO;
using Jotter.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Jotter.Models.Enum.Enum;

namespace Jotter.Endpoints
{
    public static class NoteEndpoints
    {
        public static void ConfigureNotes(this WebApplication app)
        {

            //todo -> add filters

            app.MapGet("/api/note", GetAllNotes)
                .WithName("Getnotes").Produces<ResponseDTO>(200);

            app.MapGet("/api/note/{id}", GetNote)
            .Produces<ResponseDTO>(200)
             .Produces(400);

            app.MapPost("/api/note", CreateNote)
            .Accepts<NoteRequestDTO>("application/json")
            .Produces<ResponseDTO>(201)
            .Produces(400);


            app.MapPut("/api/note/{id}", UpdateNote)
            .Accepts<NoteRequestDTO>("application/json")
            .Produces<ResponseDTO>(201)
            .Produces(400);

            app.MapDelete("/api/note/{id}", DeleteNote)
            .Accepts<NoteRequestDTO>("application/json")
            .Produces<ResponseDTO>(201)
            .Produces(400);
        }


        private async static Task<IResult> GetAllNotes(INoteRepository _noteRepo, IMapper _mapper)
        {
            ResponseDTO response = new();
            response.Result = _mapper.Map<List<NoteResponseDTO>>(await _noteRepo.GetNotesAsync(MockLoggedInUser.getUserId()));
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }

        private async static Task<IResult> GetNote(INoteRepository _noteRepo, IAuditRepository _auditRepo, IMapper _mapper, string id, ILogger<Program> _logger)
        {
            ResponseDTO response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            var note = _mapper.Map<NoteResponseDTO>(await _noteRepo.GetNoteIfExists(id));
            if (note == null)
            {
                _logger.LogWarning($"Note id {id} not found");
                response.ErrorMessages.Add($"Note id {id} not found");
                return Results.BadRequest(response);
            }

            response.Result = note;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            _auditRepo.CreateAudit(new Audit { Event = NoteEvent.Viewed, NoteId = note.Id, UserId = MockLoggedInUser.getUserId(), });
            return Results.Ok(response);
        }

        private async static Task<IResult> CreateNote(INoteRepository _noteRepo, IAuditRepository _auditRepo, IMapper _mapper,
              [FromBody] NoteRequestDTO noteRequestDTO)
        {
            ResponseDTO response = new();

            Note note = _mapper.Map<Note>(noteRequestDTO);
            note.UserId = MockLoggedInUser.getUserId();
            await _noteRepo.CreateNoteAsync(note);
            NoteResponseDTO noteDTO = _mapper.Map<NoteResponseDTO>(note);

            response.Result = noteDTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;

            _auditRepo.CreateAudit(new Audit { Event = NoteEvent.Created, NoteId = note.Id, UserId = MockLoggedInUser.getUserId(), });

            return Results.Ok(response);
        }

        private async static Task<IResult> UpdateNote(INoteRepository _noteRepo, IAuditRepository _auditRepo, IMapper _mapper, string id,
              [FromBody] NoteRequestDTO noteRequestDTO, ILogger<Program> _logger)
        {
            ResponseDTO response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            if (await _noteRepo.GetNoteIfExists(id) == null)
            {
                _logger.LogWarning($"Note id {id} not found");
                response.ErrorMessages.Add($"Note id {id} not found");
                return Results.BadRequest(response);
            }

            Note note = _mapper.Map<Note>(noteRequestDTO);
            note.LastUpdatedAt = DateTime.UtcNow;
            await _noteRepo.UpdateNoteAsync(id, note);
            NoteResponseDTO noteDTO = _mapper.Map<NoteResponseDTO>(note);
            response.Result = noteDTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;

            _auditRepo.CreateAudit(new Audit { Event = NoteEvent.Updated, NoteId = note.Id, UserId = MockLoggedInUser.getUserId(), });

            return Results.Ok(response);
        }

        private async static Task<IResult> DeleteNote(INoteRepository _noteRepo, IAuditRepository _auditRepo, string id, ILogger<Program> _logger)
        {
            ResponseDTO response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            if (await _noteRepo.GetNoteIfExists(id) == null)
            {
                _logger.LogWarning($"Note id {id} not found");
                response.ErrorMessages.Add($"Note id {id} not found");
                return Results.BadRequest(response);
            }
            await _noteRepo.DeleteNoteAsync(id);
            response.Result = id;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            _auditRepo.CreateAudit(new Audit { Event = NoteEvent.Deleted, NoteId = id, UserId = MockLoggedInUser.getUserId(), });

            return Results.Ok(response);
        }
    }
}
