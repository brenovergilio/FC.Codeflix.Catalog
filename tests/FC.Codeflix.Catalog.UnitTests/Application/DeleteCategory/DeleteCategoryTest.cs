using Moq;
using FluentAssertions;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category;

namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory;

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
    var categoryExample = _fixture.GetValidCategory();

    repositoryMock.Setup(repository => repository.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);

    var input = new DeleteCategoryInput(categoryExample.Id);
    var useCase = new UseCase.DeleteCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(repository => repository.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
    repositoryMock.Verify(repository => repository.Delete(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
    unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);
  }
}