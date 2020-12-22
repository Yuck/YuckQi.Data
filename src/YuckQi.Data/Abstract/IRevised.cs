using System;

namespace YuckQi.Data.Abstract
{
    internal interface IRevised
    {
        DateTime RevisionMomentUtc { get; set; }
    }
}