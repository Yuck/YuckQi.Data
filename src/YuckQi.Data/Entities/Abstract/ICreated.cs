using System;

namespace YuckQi.Data.Entities.Abstract
{
    public interface ICreated
    {
        DateTime CreationMomentUtc { get; set; }
    }
}