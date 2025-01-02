using AutoMapper;
using cinema.api.Models;
using cinema.context;
using Microsoft.AspNetCore.Mvc;

namespace cinema.api.Controllers.Public;

[ApiController]
[Route("api/user/categories")]
[ApiExplorerSettings(GroupName = "User")]
public class CategoriesController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(CinemaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoryDto>> Get()
    {
        var result = _context.Categories.ToList();

        var resultDto = _mapper.Map<List<CategoryDto>>(result);

        return Ok(resultDto);
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryDto> Get(Guid id)
    {
        var category = _context.Categories.FirstOrDefault(m => m.Id == id);
        if (category is null) return NotFound("Kategoria o podanym ID nie została znaleziona.");

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return Ok(categoryDto);
    }
}
