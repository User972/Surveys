using System;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface ISurvey
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}
