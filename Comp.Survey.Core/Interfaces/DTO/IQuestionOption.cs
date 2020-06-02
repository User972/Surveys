using System;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface IQuestionOption
    {
        Guid Id { get; set; }
        string Text { get; set; }
    }
}
