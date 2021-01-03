using System;

namespace YuckQi.Data.Entities.Abstract
{
    public interface IRevised
    {
        DateTime RevisionMomentUtc { get; set; }
    }
}