using System.Text.Json;
using Dotnet.Cache.Api.Domain;
using Dotnet.Cache.Api.Infra.Repositories.interfaces;
using Dotnet.Cache.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Dotnet.Cache.Api.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly string ALL_KEY = "Company_All_Key";

    public CompanyService(ICompanyRepository companyRepository, IDistributedCache distributedCache)
    {
        _companyRepository = companyRepository;
        _distributedCache = distributedCache;
    }

    public async Task<CompanyEntity?> GetByIdAsync(string id)
    {
        var cacheMember = await _distributedCache.GetStringAsync(id);

        CompanyEntity? company;
        if (string.IsNullOrEmpty(cacheMember))
        {
            await Task.Delay(1500);
            company = await _companyRepository.GetByIdAsync(id);

            if (company != null)
            {
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10));
                await _distributedCache.SetStringAsync(id, JsonSerializer.Serialize(company), options);
            }

            return company;
        }

        company = JsonSerializer.Deserialize<CompanyEntity>(cacheMember);
        return company;
    }

    public async Task<List<CompanyEntity>?> GetAllAsync()
    {
        var cacheMember = await _distributedCache.GetStringAsync(ALL_KEY);

        List<CompanyEntity>? companies;
        if (string.IsNullOrEmpty(cacheMember))
        {
            await Task.Delay(1500);
            companies = await _companyRepository.GetAllAsync();

            if (companies != null && companies.Count > 0)
            {
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10));
                await _distributedCache.SetStringAsync(ALL_KEY, JsonSerializer.Serialize(companies), options);
            }

            return companies;
        }

        companies = JsonSerializer.Deserialize<List<CompanyEntity>>(cacheMember);
        return companies;
    }

    public async Task<CompanyEntity> CreateAsync(CompanyEntity entity)
    {
        await _distributedCache.RemoveAsync(ALL_KEY);
        return await _companyRepository.CreateAsync(entity);
    }
}