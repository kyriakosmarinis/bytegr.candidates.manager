using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bytegr.candidates.manager.web.Models;
using AutoMapper;
using bytegr.candidates.manager.data.Repositories;
using bytegr.candidates.manager.data.Dtos;
using System.Collections.Generic;

namespace bytegr.candidates.manager.web.Controllers;

public class HomeController : Controller
{
    private readonly IMapper _mapper;
    private readonly ILogger<HomeController> _logger;
    private readonly ICandidatesRepository _candidatesRepository;

    public HomeController(IMapper mapper, ILogger<HomeController> logger, ICandidatesRepository candidatesRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _candidatesRepository = candidatesRepository ?? throw new ArgumentNullException(nameof(candidatesRepository));
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Welcome";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

