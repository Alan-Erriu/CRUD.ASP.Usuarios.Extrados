using AccesData.DTOs.RentBookDTOs;
using AccesData.InputsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentBookController : ControllerBase
    {

        private IRentBookService _rentBookService;


        public RentBookController(IRentBookService rentBookService)
        {

            _rentBookService = rentBookService;

        }

        [Authorize(Roles = "User")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateRentBook([FromBody] CreatRentBookControlleRequest RentBookRequest)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdInt = int.Parse(userId);
                if (userIdInt != RentBookRequest.id_user) return Unauthorized("Invalid user ID");
                CreateRentBookDTO bookCreated = await _rentBookService.CreateRentBookService(RentBookRequest);
                if (bookCreated.msg == "server error") return StatusCode(500, "server error");
                if (bookCreated.msg == "book not found") return NotFound("book not found");
                return Ok(bookCreated);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new book {Ex.Message}");
                return StatusCode(500, "server error:");
            }


        }


    }
}
