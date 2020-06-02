using System;
using System.ComponentModel.DataAnnotations;

namespace Comp.Survey.Core.Entities
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
