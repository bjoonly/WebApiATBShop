
using ATBShop.Helpers;
using ATBShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;

namespace ATBShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadImage([FromBody] ImageViewModel image)
        {
            try
            {

                var img = image.Base64.FromBase64StringToImage();
                string randomFileName = Path.GetRandomFileName() + "{0}.jpg";
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFileName);

                img.Save(String.Format(dir, ""), ImageFormat.Jpeg);

                var img100x100 = img.Resize(100, 100);
                img100x100.Save(String.Format(dir, "_100x100"), ImageFormat.Jpeg);

                var img250x250 = img.Resize(250, 250);
                img250x250.Save(String.Format(dir, "_250x250"), ImageFormat.Jpeg);

                return Ok(new { fileName = String.Format(randomFileName, "") });
            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new { Title = ex.Message });
            }
        }
    }
}
