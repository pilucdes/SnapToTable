using MongoDB.Driver;
using Shouldly;
using SnapToTable.Infrastructure.IntegrationTests.Fixtures;
using SnapToTable.Infrastructure.IntegrationTests.Models;
using SnapToTable.Infrastructure.Repositories;
using Xunit;

namespace SnapToTable.Infrastructure.IntegrationTests.Repositories;

public class BaseRepositoryTests : BaseTest
{
    public BaseRepositoryTests(FixtureBaseTest fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task AddAsync_GivenNewEntity_ShouldAddNewEntity()
    {
        var collectionName = $"{nameof(EntityTest)}s";
        var baseRepo = new BaseRepository<EntityTest>(Database, collectionName);

        EntityTest newEntity = new EntityTest("added", 1);
        await baseRepo.AddAsync(newEntity);

        var filter = Builders<EntityTest>.Filter.Eq("_id", newEntity.Id);
        var addedEntity = await Database.GetCollection<EntityTest>(collectionName).Find(filter).FirstOrDefaultAsync();

        newEntity.Id.ShouldNotBe(Guid.Empty);
        addedEntity.ShouldNotBeNull();
        addedEntity.Id.ShouldBe(newEntity.Id);
        addedEntity.Name.ShouldBe(newEntity.Name);
        addedEntity.Number.ShouldBe(newEntity.Number);
        addedEntity.CreatedAt.ToString("u").ShouldBe(newEntity.CreatedAt.ToString("u"));
    }

    [Fact]
    public async Task GetByIdAsync_GivenExistingEntity_ShouldGetRelatedEntity()
    {
        var collectionName = $"{nameof(EntityTest)}s";
        EntityTest newEntity = new EntityTest("existing", 2);
        await Database.GetCollection<EntityTest>(collectionName).InsertOneAsync(newEntity);
        
        var baseRepo = new BaseRepository<EntityTest>(Database, collectionName);

        var existingEntity = await baseRepo.GetByIdAsync(newEntity.Id);

        existingEntity.ShouldNotBeNull();
        existingEntity.Id.ShouldBe(newEntity.Id);
        existingEntity.Name.ShouldBe(newEntity.Name);
        existingEntity.Number.ShouldBe(newEntity.Number);
        existingEntity.CreatedAt.ToString("u").ShouldBe(newEntity.CreatedAt.ToString("u"));
    }
}