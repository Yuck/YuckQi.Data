namespace YuckQi.Data.Abstract
{
    internal interface IEntity<TKey> where TKey : struct
    {
        TKey Key { get; set; }
    }
}