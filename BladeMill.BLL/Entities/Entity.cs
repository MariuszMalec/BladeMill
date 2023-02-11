using System;

namespace BladeMill.BLL.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
