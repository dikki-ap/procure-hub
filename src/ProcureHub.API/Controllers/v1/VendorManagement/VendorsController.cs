using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcureHub.Modules.VendorManagement.Application.Commands.ApproveVendor;
using ProcureHub.Modules.VendorManagement.Application.Commands.BlacklistVendor;
using ProcureHub.Modules.VendorManagement.Application.Commands.DeleteVendorDocument;
using ProcureHub.Modules.VendorManagement.Application.Commands.ReinstateVendor;
using ProcureHub.Modules.VendorManagement.Application.Commands.SuspendVendor;
using ProcureHub.Modules.VendorManagement.Application.Commands.UploadVendorDocument;
using ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorById;
using ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorDocuments;
using ProcureHub.Modules.VendorManagement.Application.Queries.GetVendorList;
using ProcureHub.Modules.VendorManagement.Domain.Enums;
using ProcureHub.SharedKernel.Abstractions;
using ProcureHub.SharedKernel.Common;

namespace ProcureHub.API.Controllers.v1.VendorManagement;

/// <summary>Admin vendor management — internal users only.</summary>
[ApiController]
[Route("api/v1/vendors")]
[Authorize(Policy = "RequireInternal")]
public class VendorsController : ControllerBase
{
    private readonly IMediator           _mediator;
    private readonly ICurrentUserService _currentUser;

    public VendorsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator    = mediator;
        _currentUser = currentUser;
    }

    /// <summary>List all vendors for a company.</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetList(
        [FromQuery] Guid companyId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVendorListQuery(companyId), ct);
        return Ok(ApiResponse.Ok(result));
    }

    /// <summary>Get vendor detail by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVendorByIdQuery(id), ct);
        return Ok(ApiResponse.Ok(result));
    }

    /// <summary>Get all documents for a vendor.</summary>
    [HttpGet("{id:guid}/documents")]
    public async Task<ActionResult<ApiResponse<object>>> GetDocuments(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVendorDocumentsQuery(id), ct);
        return Ok(ApiResponse.Ok(result));
    }

    /// <summary>Approve a pending vendor.</summary>
    [HttpPost("{id:guid}/approve")]
    [Authorize(Policy = "RequireMasterData")]
    public async Task<ActionResult<ApiResponse<object>>> Approve(Guid id, CancellationToken ct)
    {
        var approvedById = _currentUser.UserId ?? Guid.Empty;
        await _mediator.Send(new ApproveVendorCommand(id, approvedById), ct);
        return Ok(ApiResponse.Ok("Vendor approved."));
    }

    /// <summary>Suspend an active vendor.</summary>
    [HttpPost("{id:guid}/suspend")]
    [Authorize(Policy = "RequireMasterData")]
    public async Task<ActionResult<ApiResponse<object>>> Suspend(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new SuspendVendorCommand(id), ct);
        return Ok(ApiResponse.Ok("Vendor suspended."));
    }

    /// <summary>Blacklist a vendor.</summary>
    [HttpPost("{id:guid}/blacklist")]
    [Authorize(Policy = "RequireMasterData")]
    public async Task<ActionResult<ApiResponse<object>>> Blacklist(
        Guid id, [FromBody] BlacklistRequest request, CancellationToken ct)
    {
        var blacklistedById = _currentUser.UserId ?? Guid.Empty;
        await _mediator.Send(new BlacklistVendorCommand(id, request.Reason, blacklistedById), ct);
        return Ok(ApiResponse.Ok("Vendor blacklisted."));
    }

    /// <summary>Reinstate a suspended or blacklisted vendor.</summary>
    [HttpPost("{id:guid}/reinstate")]
    [Authorize(Policy = "RequireMasterData")]
    public async Task<ActionResult<ApiResponse<object>>> Reinstate(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new ReinstateVendorCommand(id), ct);
        return Ok(ApiResponse.Ok("Vendor reinstated."));
    }

    /// <summary>Upload a document for a vendor.</summary>
    [HttpPost("{id:guid}/documents")]
    [Authorize(Policy = "RequireMasterData")]
    public async Task<ActionResult<ApiResponse<object>>> UploadDocument(
        Guid id, [FromForm] UploadDocumentRequest request, CancellationToken ct)
    {
        await using var stream = request.File.OpenReadStream();
        var docId = await _mediator.Send(new UploadVendorDocumentCommand(
            id,
            request.DocumentType,
            request.DocumentNumber,
            stream,
            request.File.FileName,
            request.File.ContentType,
            request.ExpiredAt,
            request.IssuedAt,
            request.Notes), ct);

        return CreatedAtAction(nameof(GetDocuments), new { id }, ApiResponse.Ok(new { id = docId }));
    }

    /// <summary>Delete a vendor document.</summary>
    [HttpDelete("{id:guid}/documents/{documentId:guid}")]
    [Authorize(Policy = "RequireMasterData")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteDocument(
        Guid id, Guid documentId, CancellationToken ct)
    {
        var deletedById = _currentUser.UserId ?? Guid.Empty;
        await _mediator.Send(new DeleteVendorDocumentCommand(documentId, deletedById), ct);
        return Ok(ApiResponse.Ok("Document deleted."));
    }
}

public record BlacklistRequest(string Reason);

public class UploadDocumentRequest
{
    public IFormFile    File           { get; set; } = null!;
    public DocumentType DocumentType   { get; set; }
    public string?      DocumentNumber { get; set; }
    public DateOnly?    ExpiredAt      { get; set; }
    public DateOnly?    IssuedAt       { get; set; }
    public string?      Notes          { get; set; }
}
