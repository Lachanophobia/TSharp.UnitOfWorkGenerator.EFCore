using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSharp.UnitOfWorkGenerator.DataAccess.Entities
{
    public partial class BaseEntity : IBaseEntity
    {
        public DateTime CreatedDate { get; set; }

    }
}
