using System;

namespace YuckQi.Data.DocumentDb.MongoDb.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseAttribute : Attribute
    {
        public String Name { get; }

        public DatabaseAttribute(String name)
        {
            Name = name;
        }
    }
}
