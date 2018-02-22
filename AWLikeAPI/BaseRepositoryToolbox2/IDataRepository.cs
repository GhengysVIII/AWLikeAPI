using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.BaseRepository
{
    public interface IDataRepository<TKey, TEntity>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TKey Key);
        TEntity Insert(TEntity Entity);
        void Update(TEntity Entity);
        void Delete(TKey Key);
    }
}
