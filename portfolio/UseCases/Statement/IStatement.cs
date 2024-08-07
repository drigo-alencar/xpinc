using Microsoft.AspNetCore.Mvc;

namespace UseCases;

public interface IStatement
{
    Task<IDictionary<string, int>> Find(string name);
}