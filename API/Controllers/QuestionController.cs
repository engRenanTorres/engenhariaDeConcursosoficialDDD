using API.Extensions.HttpExtensions;
using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
  private readonly ILogger<QuestionController> _logger;
  private readonly IQuestionService _questionService;

  public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
  {
    _logger = logger;
    _questionService = questionService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAllFull([FromQuery] QuestionParams pagingParams)
  {
    var questions = await _questionService.GetAllComplete(pagingParams);
    Response.AddPaginationHeader(
      questions.CurrentPage,
      questions.PageSize,
      questions.TotalCount,
      questions.TotalPages
    );
    return Ok(questions);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(int id)
  {
    var questions = await _questionService.GetFullById(id);
    return Ok(questions);
  }

  [HttpGet("LastId")]
  public async Task<ActionResult> GetCount()
  {
    int id = await _questionService.GetCount();
    return Ok(id);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateQuestionDTO questionDto)
  {
    var question = await _questionService.Create(questionDto);
    var actionName = nameof(GetFullById);
    var LastIdCreated = await _questionService.GetLastId();
    return CreatedAtAction(actionName, new { Id = LastIdCreated }, question);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteQuestion(int id)
  {
    _logger.LogInformation("Delete Question Controller has been called.");

    await _questionService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> PatchQuestion(
    int id,
    [FromBody] UpdateQuestionDTO updateQuestionDTO
  )
  {
    _logger.LogInformation("PatchQuestions has been called.");

    var updatedQuestion = await _questionService.Patch(id, updateQuestionDTO);

    return Ok(updatedQuestion);
  }
}
