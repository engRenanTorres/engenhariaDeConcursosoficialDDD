using Apllication.DTOs;
using Apllication.Repositories;
using Application.DTOs;
using Domain.Entities.Questions;

namespace Apllication.Services.Interfaces;

public interface IQuestionService
{
  public Task<Question> Create(CreateQuestionDTO questionDto);
  public Task Delete(int id);
  public Task<IEnumerable<ViewQuestionDto?>> GetAllComplete();

  public Task<ViewQuestionDto?> GetFullById(int id);

  public Task<int> GetCount();
  public Task<int> GetLastId();
  public Task<Question> PatchQuestion(int id, UpdateQuestionDTO updateQuestionDTO);
}
