using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = "AdminOnly")]
    public class ParentController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ParentResponseDTO>>> GetAll()
        {
            try
            {
                var parents = await _parentService.GetAll();
                return Ok(parents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ParentResponseDTO>> GetByUserId(int userId)
        {
            try
            {
                var parent = await _parentService.GetParentByUserId(userId);
                if (parent == null)
                    return NotFound(new { message = "Parent not found" });

                return Ok(parent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParentResponseDTO>> GetById(int id)
        {
            try
            {
                var parent = await _parentService.GetParentById(id);
                if (parent == null)
                    return NotFound(new { message = "Parent not found" });

                return Ok(parent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ParentResponseDTO>> Create([FromBody] ParentRequestDTO parentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdParent = await _parentService.CreateParentWithUser(parentDTO);
                return CreatedAtAction(nameof(GetById), new { id = createdParent.Id }, createdParent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParentResponseDTO>> Update(int id, [FromBody] ParentRequestDTO parentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedParent = await _parentService.Update(id, parentDTO);
                return Ok(updatedParent);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ParentResponseDTO>> Delete(int id)
        {
            try
            {
                var deletedParent = await _parentService.Delete(id);
                return Ok(deletedParent);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
} 