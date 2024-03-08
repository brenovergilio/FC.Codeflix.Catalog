using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Entity.Exceptions;
using FC.Codeflix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
  private readonly CreateCategoryTestFixture _fixture;

  public CreateCategoryTest(CreateCategoryTestFixture fixture)
  {
    _fixture = fixture;
  }

  [Fact(DisplayName = nameof(CreateCategory))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategory()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );
    var input = _fixture.GetInput();

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );

    unitOfWorkMock.Verify(
      unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(input.IsActive);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);
  }

  [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
  [Trait("Application", "CreateCategory - Use Cases")]
  [MemberData(nameof(GetInvalidInputs))]
  public async void ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
  {
    var useCase = new UseCases.CreateCategory(
      _fixture.GetRepositoryMock().Object,
      _fixture.GetUnitOfWorkMock().Object
    );

    Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);
    await task.Should()
      .ThrowAsync<EntityValidationException>()
      .WithMessage(exceptionMessage);
  }

  public static IEnumerable<object[]> GetInvalidInputs()
  {
    var fixture = new CreateCategoryTestFixture();
    var invalidInputsList = new List<object[]>();

    var invalidInputShortName = fixture.GetInput();
    invalidInputShortName.Name = invalidInputShortName.Name[..2];
    invalidInputsList.Add([
      invalidInputShortName,
      "Name should be at least 3 characters long"
    ]);

    var invalidInputTooLongName = fixture.GetInput();
    var tooLongNameForCategory = fixture.Faker.Commerce.ProductName();
    while(tooLongNameForCategory.Length <= 255)
        tooLongNameForCategory = $"{tooLongNameForCategory} {fixture.Faker.Commerce.ProductName()}";

    invalidInputTooLongName.Name = tooLongNameForCategory;
    invalidInputsList.Add([
      invalidInputTooLongName,
      "Name should be less or equal 255 characters long"
    ]);

    var invalidInputDescriptionNull = fixture.GetInput();
    invalidInputDescriptionNull.Description = null!;
    invalidInputsList.Add([
      invalidInputDescriptionNull,
      "Description should not be null"
    ]);

    var invalidInputTooLongDescription = fixture.GetInput();
    var tooLongDescriptionForCategory = fixture.Faker.Commerce.ProductDescription();
    while(tooLongDescriptionForCategory.Length <= 10_000)
        tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {fixture.Faker.Commerce.ProductDescription()}";

    invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
    invalidInputsList.Add([
      invalidInputTooLongDescription,
      "Description should be less or equal 10000 characters long"
    ]);

    return invalidInputsList;
  }

  [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategoryWithOnlyName()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );
    var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );

    unitOfWorkMock.Verify(
      unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().BeEmpty();
    output.IsActive.Should().BeTrue();
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);
  }

  
  [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategoryWithOnlyNameAndDescription()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );
    var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );

    unitOfWorkMock.Verify(
      unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().BeTrue();
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);
  }
}