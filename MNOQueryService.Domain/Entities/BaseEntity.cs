using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNOQueryService.Domain.Entities
{
    public abstract class BaseEntity<TKey>
    {
        protected BaseEntity()
        {
            Id = default(TKey);
        }
        protected BaseEntity(TKey id)
        {
            Id = id;
        }
        public virtual TKey Id { get; protected set; }
    }
}
