using System;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface ICompUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}
