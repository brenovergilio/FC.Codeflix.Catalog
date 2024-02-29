namespace FC.Codeflix.Catalog.Domain.Entity.Exceptions;

public class EntityValidationException(string? message) : Exception(message)
{
}