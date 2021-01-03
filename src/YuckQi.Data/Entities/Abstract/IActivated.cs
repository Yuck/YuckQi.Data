using System;

namespace YuckQi.Data.Entities.Abstract
{
    public interface IActivated
    {
        DateTime? ActivationMomentUtc { get; set; }
    }
}