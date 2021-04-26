using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call save change from db context
        /// </summary>
        void Commit();
    }
}
