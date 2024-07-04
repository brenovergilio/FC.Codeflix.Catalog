using Entity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Entity.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;

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

        var useCase = new UseCase.CreateCategory(
          repositoryMock.Object,
          unitOfWorkMock.Object
        );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
          repository => repository.Insert(
            It.IsAny<Entity.Category>(),
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

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
      nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
      parameters: 12,
      MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async void ThrowWhenCantInstantiate(CreateCategoryInput input, string exceptionMessage)
    {
        var useCase = new UseCase.CreateCategory(
          _fixture.GetRepositoryMock().Object,
          _fixture.GetUnitOfWorkMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);
        await task.Should()
          .ThrowAsync<EntityValidationException>()
          .WithMessage(exceptionMessage);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyName()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCase.CreateCategory(
          repositoryMock.Object,
          unitOfWorkMock.Object
        );
        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
          repository => repository.Insert(
            It.IsAny<Entity.Category>(),
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

        var useCase = new UseCase.CreateCategory(
          repositoryMock.Object,
          unitOfWorkMock.Object
        );
        var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
          repository => repository.Insert(
            It.IsAny<Entity.Category>(),
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