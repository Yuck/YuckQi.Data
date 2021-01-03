using System;

namespace YuckQi.Data.Entities.Abstract
{
    public interface IDeleted
    {
        DateTime? DeletionMomentUtc { get; set; }
    }
}