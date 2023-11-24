using AccesData.DTOs.BookDTOs;
using AccesData.InputsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _bookService;



        public BookController(IBookService bookService)
        {

            _bookService = bookService;
        }


        [Authorize]
        [HttpPost("create")]

        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest bookinput)
        {

            if (string.IsNullOrEmpty(bookinput.name_book)) return BadRequest("Name is required");

            try
            {

                CreateBookDTO bookCreated = await _bookService.CreateBookService(bookinput.name_book);
                if (bookCreated.msg == "The name is already in use") return Conflict("The name is already in use");
                if (bookCreated.msg == "server error") return StatusCode(500, "server error");
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
