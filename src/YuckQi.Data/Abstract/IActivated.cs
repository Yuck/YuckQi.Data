using System;

namespace YuckQi.Data.Abstract
{
    internal interface IActivated
    {
        DateTime? ActivationMomentUtc { get; set; }
    }
}