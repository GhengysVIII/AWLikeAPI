using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.BaseRepository
{
    public interface IRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        TKey Insert(TEntity Entity);

        TEntity Get(TKey Id);
        IEnumerable<TEntity> GetAll();

        bool Update(TEntity Entity);

        bool Delete(TKey Id);
        bool Delete(TEntity Entity);
    }
}
