using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{}