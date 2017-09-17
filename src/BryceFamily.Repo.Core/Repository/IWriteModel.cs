using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Repository
{
    public interface IWriteModel<TEntity, TId>
    {
        TId Save(TEntity entity);

        void Delete(TId entityId);
    }
}
