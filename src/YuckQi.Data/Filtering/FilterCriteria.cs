using System;

namespace YuckQi.Data.Filtering
{
    public readonly struct FilterCriteria
    {
        public String FieldName { get; }
        public FilterOperation Operation { get; }
        public Object Value { get; }

        public FilterCriteria(String fieldName, Object value) : this(fieldName, FilterOperation.Equal, value) { }

        public FilterCriteria(String fieldName, FilterOperation operation, Object value)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Operation = operation;
            Value = value;
        }
    }
}
