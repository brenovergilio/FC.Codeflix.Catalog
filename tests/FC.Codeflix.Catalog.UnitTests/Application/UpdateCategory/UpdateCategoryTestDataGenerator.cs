﻿using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();

        for(int indice = 0; indice < times; indice++)
        {
            var exampleCategory = fixture.GetExempleCategory();
            var exampleInput = new UpdateCategoryInput(
               exampleCategory.Id,
               fixture.GetValidCategoryName(),
               fixture.GetValidCategoryDescription(),
               fixture.GetRandomBoolean()
            );

            yield return new object[]
            {
                exampleCategory,
                exampleInput
            };
        }
    }
}
