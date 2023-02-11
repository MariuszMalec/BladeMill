using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.BLL.Entities
{
    public interface IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
