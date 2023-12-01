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


        [Authorize(Roles = "Admin")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest bookinput)
        {



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
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{nameBook}")]

        public async Task<IActionResult> DeleteBookByNameBook(string nameBook)
        {

            if (string.IsNullOrEmpty(nameBook)) return BadRequest("Name Book is required");

            try
            {
                //falta codear una tercera respuesta para errores del servidor
                int bookDeleted = await _bookService.DeleteBookByNameService(nameBook);
                if (bookDeleted == 0) return BadRequest("Book Not Found");
                return Ok("Book successfully deleted");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting book {Ex.Message}");
                return StatusCode(500, "server error:");
            }


        }

    }
}
