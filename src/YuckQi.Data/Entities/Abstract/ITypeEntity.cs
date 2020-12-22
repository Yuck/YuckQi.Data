using System;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Entities.Abstract
{
    // TODO: Handler/Repository to work with this by "Identifier"
    internal interface ITypeEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        Guid Identifier { get; set; }
        string Name { get; set; }
    }
}