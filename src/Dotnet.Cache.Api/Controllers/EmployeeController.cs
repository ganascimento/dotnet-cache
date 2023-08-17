using Bogus;
using Dotnet.Cache.Api.Domain;
using Dotnet.Cache.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Cache.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var result = await _employeeService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _employeeService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync()
    {
        try
        {
            var employee = new Faker<EmployeeEntity>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.BirthDate, f => f.Date.Between(new DateTime(1950, 01, 01), new DateTime(2010, 12, 31)))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## #####-####"))
                .RuleFor(u => u.Document, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(u => u.Company, f => f.Company.CompanyName())
                .Generate();

            var result = await _employeeService.CreateAsync(employee);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}