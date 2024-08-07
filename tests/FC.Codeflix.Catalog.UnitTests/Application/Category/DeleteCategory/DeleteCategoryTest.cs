using Moq;
using FluentAssertions;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryExample = _fixture.GetExampleCategory();

        repositoryMock.Setup(repository => repository.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);

        var input = new UseCase.DeleteCategoryInput(categoryExample.Id);
        var useCase = new UseCase.DeleteCategory(
          repositoryMock.Object,
          unitOfWorkMock.Object
        );

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleGuid = Guid.NewGuid();

        repositoryMock.Setup(repository => repository.Get(exampleGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{exampleGuid} not found' "));

        var input = new UseCase.DeleteCategoryInput(exampleGuid);
        var useCase = new UseCase.DeleteCategory(
          repositoryMock.Object,
          unitOfWorkMock.Object
        );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repository => repository.Get(exampleGuid, It.IsAny<CancellationToken>()), Times.Once);
    }
}