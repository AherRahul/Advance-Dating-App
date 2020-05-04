using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dating.API.Controllers
{
    [AllowAnonymous]
    public class Fallback: Controller
    {
        public IActionResult Index() {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }        
    }
}