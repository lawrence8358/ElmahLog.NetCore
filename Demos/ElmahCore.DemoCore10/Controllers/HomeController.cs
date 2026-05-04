using System;
using System.Diagnostics;
using ElmahCore.DemoCore10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ElmahCore.DemoCore10.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogTrace("Test");
        _logger.LogDebug("Test");
        _logger.LogError("Test");
        _logger.LogInformation("Test");
        _logger.LogWarning("Test");
        _logger.LogCritical(new InvalidOperationException("Test"), "Test");

        ElmahExtensions.RaiseError(new Exception("test2"));

        var r = 0;
        // ReSharper disable once IntDivisionByZero
        var d = 100 / r;
        return View();
    }

    public void TestMethod(string p1, int p2)
    {
        this.LogParams((nameof(p1), p1), (nameof(p2), p2));
    }

    public IActionResult Privacy()
    {
        throw new Exception("Test");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}