using Entities;
using Microsoft.AspNetCore.Mvc;

namespace DTO;

public class User
{
    [FromHeader]
    public string Name { get; set; } = string.Empty;

    [FromHeader]
    public UserType Type { get; set; } = UserType.None;
}