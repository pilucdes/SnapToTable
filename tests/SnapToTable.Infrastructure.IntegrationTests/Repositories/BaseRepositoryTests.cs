using System.Linq.Expressions;
using Shouldly;
using SnapToTable.Domain.Common;
using SnapToTable.Infrastructure.IntegrationTests.Fixtures;
using SnapToTable.Infrastructure.IntegrationTests.Models;
using SnapToTable.Infrastructure.Repositories;
using Xunit;
using SortDirection = SnapToTable.Domain.Common.SortDirection;

namespace SnapToTable.Infrastructure.IntegrationTests.Repositories;

public class BaseRepositoryTests : BaseTest
{
    private const string CollectionName = $"{nameof(EntityTest)}s";
    private readonly BaseRepository<EntityTest> _repository;

    public BaseRepositoryTests(FixtureBaseTest fixture) : base(fixture)
    {
        _repository = new BaseRepository<EntityTest>(Database, CollectionName);
    }

    [Fact]
    public async Task AddAsync_GivenNewEntity_ShouldAddNewEntity()
    {
        // Arrange
        var newEntity = new EntityTest("added", 1);
        var initialCreationDate = DateTime.UtcNow;

        // Act
        await _repository.AddAsync(newEntity);

        // Assert
        var addedEntity = await _repository.GetByIdAsync(newEntity.Id);

        newEntity.Id.ShouldNotBe(Guid.Empty);
        addedEntity.ShouldNotBeNull();
        addedEntity.Id.ShouldBe(newEntity.Id);
        addedEntity.Name.ShouldBe(newEntity.Name);
        addedEntity.Number.ShouldBe(newEntity.Number);

        addedEntity.CreatedAt.ShouldBe(initialCreationDate, tolerance: TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task AddRangeAsync_GivenNewEntities_ShouldAddEntities()
    {
        // Arrange
        var entity1 = new EntityTest("added1", 1);
        var entity2 = new EntityTest("added2", 2);
        var sortOrder = new SortDescriptor<EntityTest>(e => e.Number, SortDirection.Ascending);
        var initialCreationDate = DateTime.UtcNow;

        // Act
        await _repository.AddRangeAsync([entity1, entity2]);

        // Assert
        var pagedEntities = await _repository.GetPagedAsync(1, 5, p => p, sortOrder: sortOrder);
        pagedEntities.TotalPages.ShouldBe(1);
        pagedEntities.TotalCount.ShouldBe(2);
        pagedEntities.Items.ShouldNotBeEmpty();
        pagedEntities.Items[0].Id.ShouldBe(entity1.Id);
        pagedEntities.Items[0].Name.ShouldBe(entity1.Name);
        pagedEntities.Items[0].CreatedAt.ShouldBe(initialCreationDate, tolerance: TimeSpan.FromSeconds(1));

        pagedEntities.Items[1].Id.ShouldBe(entity2.Id);
        pagedEntities.Items[1].Name.ShouldBe(entity2.Name);
        pagedEntities.Items[1].CreatedAt.ShouldBe(initialCreationDate, tolerance: TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task AddRangeAsync_GivenNoEntities_ShouldNotAddEntities()
    {
        // Arrange
        var entity1 = new EntityTest("added1", 1);
        var sortOrder = new SortDescriptor<EntityTest>(e => e.Number, SortDirection.Ascending);
        var initialCreationDate = DateTime.UtcNow;

        // Act
        await _repository.AddRangeAsync([]);

        // Assert
        var pagedEntities = await _repository.GetPagedAsync(1, 5, p => p, sortOrder: sortOrder);
        pagedEntities.TotalPages.ShouldBe(0);
        pagedEntities.TotalCount.ShouldBe(0);
    }
    
    [Fact]
    public async Task GetByIdAsync_GivenExistingEntity_ShouldGetRelatedEntity()
    {
        // Arrange
        var seededEntities = await SeedEntitiesAsync(1);
        var entityToFind = seededEntities.First();

        // Act
        var existingEntity = await _repository.GetByIdAsync(entityToFind.Id);

        // Assert
        existingEntity.ShouldNotBeNull();
        existingEntity.Id.ShouldBe(entityToFind.Id);
        existingEntity.CreatedAt.ShouldBe(entityToFind.CreatedAt, tolerance: TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetByIdAsync_GivenNonExistentId_ShouldReturnNull()
    {
        // Arrange
        await SeedEntitiesAsync(1);

        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetPagedAsync_WhenEntitiesExist_ShouldReturnCorrectlyPaginatedAndSortedPages()
    {
        // Arrange
        int totalEntities = 10;
        int pageSize = 5;
        await SeedEntitiesAsync(totalEntities);
        var sortOrder = new SortDescriptor<EntityTest>(e => e.Number, SortDirection.Ascending);

        // Act
        var page1Result = await _repository.GetPagedAsync(1, pageSize, sortOrder: sortOrder);
        var page2Result = await _repository.GetPagedAsync(2, pageSize, sortOrder: sortOrder);

        // Assert - Page 1
        page1Result.ShouldNotBeNull();
        page1Result.Page.ShouldBe(1);
        page1Result.PageSize.ShouldBe(pageSize);
        page1Result.TotalCount.ShouldBe(totalEntities);
        page1Result.TotalPages.ShouldBe(2);
        page1Result.Items.Count.ShouldBe(pageSize);
        page1Result.Items.Select(item => item.Number).ShouldBe([1, 2, 3, 4, 5]);

        // Assert - Page 2
        page2Result.ShouldNotBeNull();
        page2Result.Items.Count.ShouldBe(pageSize);
        page2Result.Items.Select(item => item.Number).ShouldBe([6, 7, 8, 9, 10]);

        // Assert - No overlap between pages
        var page1Ids = page1Result.Items.Select(item => item.Id);
        page2Result.Items.ShouldAllBe(item => !page1Ids.Contains(item.Id));
    }

    [Fact]
    public async Task GetPagedAsync_WhenNoSortIsProvided_ReturnsResultsSortedByIdDescending()
    {
        // Arrange
        await SeedEntitiesAsync(5);

        // Act
        var pagedResult = await _repository.GetPagedAsync(1, 5);

        // Assert
        pagedResult.Items.Count.ShouldBe(5);
        var actualIds = pagedResult.Items.Select(item => item.Id).ToList();

        var expectedSortedIds = pagedResult.Items.OrderByDescending(i => i.Id).Select(i => i.Id).ToList();
        actualIds.ShouldBe(expectedSortedIds, "Results should be sorted by Id descending by default.");
    }

    [Fact]
    public async Task GetPagedAsync_WhenProjectionIsProvided_ShouldReturnCorrectlyMappedData()
    {
        // Arrange
        var seededEntities = await SeedEntitiesAsync(5);
        var originalFirstEntity = seededEntities.First(e => e.Number == 1);

        Expression<Func<EntityTest, EntityProjectedTest>> projection = entity =>
            new EntityProjectedTest(entity.Id, entity.CreatedAt, entity.Name);

        var sortOrder = new SortDescriptor<EntityTest>(e => e.Number, SortDirection.Ascending);

        // Act
        var pagedResult = await _repository.GetPagedAsync(1, 5, projection, sortOrder: sortOrder);

        // Assert
        pagedResult.ShouldNotBeNull();
        pagedResult.Items.Count.ShouldBe(5);
        pagedResult.TotalCount.ShouldBe(5);
        pagedResult.Items.First().ShouldBeOfType<EntityProjectedTest>();

        var firstProjectedItem = pagedResult.Items.First();
        firstProjectedItem.Id.ShouldBe(originalFirstEntity.Id);
        firstProjectedItem.Name.ShouldBe(originalFirstEntity.Name);
    }

    [Fact]
    public async Task GetPagedAsync_WhenFilterIsProvided_ShouldReturnCorrectlyFilteredData()
    {
        // Arrange
        await SeedEntitiesAsync(10);

        // Act
        var pagedResult = await _repository.GetPagedAsync(1, 5, filter: e => e.Number > 8);

        // Assert
        pagedResult.ShouldNotBeNull();
        pagedResult.Items.Count.ShouldBe(2); // Entities with number 9 and 10
        pagedResult.TotalCount.ShouldBe(2);

        var numbers = pagedResult.Items.Select(i => i.Number).ToList();
        numbers.ShouldContain(9);
        numbers.ShouldContain(10);
    }

    [Fact]
    public async Task GetPagedAsync_WhenNoEntitiesExist_ShouldReturnEmptyPage()
    {
        // Act
        var pagedResult = await _repository.GetPagedAsync(1, 10);

        // Assert
        pagedResult.ShouldNotBeNull();
        pagedResult.Items.ShouldBeEmpty();
        pagedResult.TotalCount.ShouldBe(0);
        pagedResult.TotalPages.ShouldBe(0);
        pagedResult.Page.ShouldBe(1);
    }

    private async Task<List<EntityTest>> SeedEntitiesAsync(int count)
    {
        var entities = Enumerable.Range(1, count)
            .Select(i => new EntityTest($"Entity Name {i}", i))
            .ToList();

        await SeedDatabaseWithManyAsync(entities, CollectionName);

        return entities;
    }
}