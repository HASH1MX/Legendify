using LoLChampionApi.Models;
using LoLChampionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoLChampionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChampionsController : ControllerBase
{
    private readonly IChampionService _championService;

    public ChampionsController(IChampionService championService)
    {
        _championService = championService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Champion>>> GetAll()
    {
        var champions = await _championService.GetAllAsync();
        return Ok(champions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Champion>> GetById(int id)
    {
        var champion = await _championService.GetByIdAsync(id);

        if (champion is null)
        {
            return NotFound();
        }

        return Ok(champion);
    }

    [HttpPost]
    public async Task<ActionResult<Champion>> Create(Champion champion)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdChampion = await _championService.CreateAsync(champion);

        return CreatedAtAction(nameof(GetById), new { id = createdChampion.Id }, createdChampion);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Champion champion)
    {
        if (id != champion.Id)
        {
            return BadRequest("Route id does not match champion id.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated = await _championService.UpdateAsync(id, champion);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _championService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
