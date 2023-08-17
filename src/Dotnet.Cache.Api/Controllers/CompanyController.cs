using Bogus;
using Dotnet.Cache.Api.Domain;
using Dotnet.Cache.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Cache.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var result = await _companyService.GetByIdAsync(id);
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
            var result = await _companyService.GetAllAsync();
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
            var company = new Faker<CompanyEntity>()
                .RuleFor(u => u.Name, f => f.Company.CompanyName())
                .RuleFor(u => u.FantasyName, f => f.Company.CompanyName())
                .RuleFor(u => u.Owner, f => f.Name.FullName())
                .RuleFor(u => u.Document, f => f.Phone.PhoneNumber("##############"))
                .RuleFor(u => u.Address, f => $"{f.Address.StreetName()}, {f.Address.BuildingNumber()} - {f.Address.ZipCode()} - {f.Address.Country()}")
                .Generate();

            var result = await _companyService.CreateAsync(company);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}