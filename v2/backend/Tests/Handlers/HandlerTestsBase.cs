using System;
using System.Collections.Generic;
using System.Threading;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Api.Data;
using Api.Mappings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.Handlers;

public class HandlerTestsBase : IDisposable
{
    protected readonly IMapper Mapper;
    protected readonly ApplicationDbContext Db;
    protected readonly CancellationToken CancellationToken = new();
    protected readonly IConfiguration Configuration;
    protected readonly Mock<IAmazonCognitoIdentityProvider> IdentityClientMock;
    protected readonly Mock<CognitoUserPool> UserPool;

    protected HandlerTestsBase()
    {
        var inMemorySettings = new Dictionary<string, string> { {"AWSCognito:ClientId", "FakeClientId"} };
        Configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        IdentityClientMock = new Mock<IAmazonCognitoIdentityProvider>();
        UserPool = new Mock<CognitoUserPool>();

        var options  = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(new Guid().ToString())
            .Options;
        Db = new ApplicationDbContext(options);
        Db.Database.EnsureCreated();
        
        DatabaseInitializer.Initialize(Db);
        
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ApplicationProfile());
        });
        Mapper = mappingConfig.CreateMapper();
    }
    
    public void Dispose()
    {
        Db.Database.EnsureDeleted();
        Db.Dispose();
        GC.SuppressFinalize(this);
    }
}
