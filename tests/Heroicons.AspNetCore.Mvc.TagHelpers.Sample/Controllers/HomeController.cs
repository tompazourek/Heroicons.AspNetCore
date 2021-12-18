using Microsoft.AspNetCore.Mvc;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Sample.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
