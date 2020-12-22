using System;

namespace YuckQi.Data.Abstract
{
    internal interface IDeleted
    {
        DateTime? DeletionMomentUtc { get; set; }
    }
}