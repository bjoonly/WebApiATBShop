
using ATBShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATBShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult>UploadImage([FromBody]ImageViewModel image)
        {
            try
            {

            var bytes = Convert.FromBase64String(image.base64);
            string randomFileName = Path.GetRandomFileName() + ".jpg";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFileName);
            using (var imageFile = new FileStream(path, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return Ok(new {fileName= randomFileName });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new { Title = ex.Message });
            }
        }
    }
}
