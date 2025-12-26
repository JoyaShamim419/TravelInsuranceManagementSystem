
using Microsoft.AspNetCore.Mvc;

public class UserDashboardController : Controller
{
    public IActionResult Dashboard()
    {
        return View(); // Views/UserDashboard/Dashboard.cshtml
    }


    public IActionResult Claims()
    {
        return View(viewName: "~/Views/UserDashboard/Claims.cshtml");
    }

    // Claim create view stored at Views/UserDashboard/ClaimCreate.cshtml
    public IActionResult ClaimCreate()
    {
        return View(viewName: "~/Views/UserDashboard/ClaimCreate.cshtml");
    }


    public IActionResult Policies()
    {
        return View("~/Views/UserDashboard/Policies.cshtml");
    }
    public IActionResult RaiseTicket()
    {
        return View("~/Views/UserDashboard/RaiseTicket.cshtml");
    }

   



}

