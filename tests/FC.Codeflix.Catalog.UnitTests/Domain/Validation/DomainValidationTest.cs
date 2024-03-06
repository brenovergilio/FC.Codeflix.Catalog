using Bogus;
using FC.Codeflix.Catalog.Domain.Entity.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
  private Faker Faker { get; set; } = new Faker();
 
  [Fact(DisplayName = nameof(NotNullOk))]
  [Trait("Domain", "DomainValidation - Validation")]
  public void NotNullOk()
  {
    var value = Faker.Commerce.ProductName();
    Action action = () => DomainValidation.NotNull(value, "Value");

    action.Should().NotThrow();
  }

  [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
  [Trait("Domain", "DomainValidation - Validation")]
  public void NotNullThrowWhenNull()
  {
    string? value = null;
    Action action = () => DomainValidation.NotNull(value, "FieldName");

    action.Should()
      .Throw<EntityValidationException>()
      .WithMessage("FieldName should not be null");
  }

  [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
  [Trait("Domain", "DomainValidation - Validation")]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData(null)]
  public void NotNullOrEmptyThrowWhenEmpty(string? target)
  {
    Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

    action.Should()
      .Throw<EntityValidationException>()
      .WithMessage("FieldName should not be null or empty");
  }

  [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
  [Trait("Domain", "DomainValidation - Validation")]
  public void NotNullOrEmptyOk()
  {
    var target = Faker.Commerce.ProductName();

    Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

    action.Should().NotThrow();
  }
}