# Dotnet Cache

This project was developed to test different ways of dealing with caching.

## Resources used

To develop this project, was used:

- DotNet 7
- InMemoryCache
- Redis
- MongoDB

## Cache, what is?

In the context of a backend application, a cache refers to a mechanism used to temporarily store and manage frequently accessed data in order to improve application performance and efficiency. This helps reduce the need to repeatedly retrieve the same data from slower data sources such as databases or external APIs. Instead, cached data can be quickly retrieved directly from memory or from a cached database, which is much faster.

## But why use cache?

Caching is crucial in applications for enhancing performance and user experience. By storing frequently accessed data in memory, caching reduces the need to fetch information from slower sources like databases or APIs. This leads to faster data retrieval, lower latency, and decreased server load. Applications become more scalable, handle increased user traffic efficiently, and mitigate the impact of external dependencies.

## Cache types

### In memory cache

In-memory cache is a data storage technique where frequently accessed information is temporarily held in the computer's main memory (RAM) for quick retrieval. This accelerates data access, reducing the need to repeatedly fetch data from slower sources.

### Distributed cache

Distributed cache is a system that stores data across multiple servers, enabling quick and efficient data access for applications. It enhances performance by distributing frequently used information across a network, reducing the load on individual servers and data sources. This approach boosts application responsiveness and scalability, making it ideal for high-traffic scenarios.

<p align="start">
  <img src="./assets/redis.png" width="200" />
</p>

## Test

To run this project you need docker installed on your machine, see the docker documentation [here](https://www.docker.com/).

Having all the resources installed, run the command in a terminal from the root folder of the project and wait some seconds to build project image and download the resources:
`docker-compose up -d`

In terminal show this:

```console
[+] Running 4/4
 ✔ Network dotnet-cache_default      Created                              0.9s
 ✔ Container dotnet-cache-redis-1    Started                              1.6s
 ✔ Container dotnet-cache-mongodb-1  Started                              2.0s
 ✔ Container cache_app               Started                              3.6s
```

After this, access the link below:

- Swagger project [click here](http://localhost:5000/swagger)

### Stop Application

To stop, run: `docker-compose down`

## How implement

### In memory cache

In `Program.cs` add:

```c#
builder.Services.AddMemoryCache();
```

To use cache during get data, apply:

```c#
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMemoryCache _memoryCache;

    public EmployeeService(IEmployeeRepository employeeRepository, IMemoryCache memoryCache)
    {
        _employeeRepository = employeeRepository;
        _memoryCache = memoryCache;
    }

    public async Task<EmployeeEntity?> GetByIdAsync(string id)
    {
        if (_memoryCache.TryGetValue<EmployeeEntity>(id, out EmployeeEntity? cachevalue))
            return cachevalue;

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee != null)
            _memoryCache.Set<EmployeeEntity>(id, employee, TimeSpan.FromSeconds(10));

        return employee;
    }
}
```

---

### Distributed cache

In `Program.cs` add:

```c#
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});
```

In `appsettings.json` add:

```json
"Redis": {
    "ConnectionString": "localhost:6379"
}
```

To use, apply:

```c#
public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IDistributedCache _distributedCache;

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
}
```
