using BussinessObjects.Models;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class ConnectionRepository : IConnectionRepository
    {


        public Task<Connection> AddConnection(Connection entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Connection>> GetConnections()
        {
            throw new NotImplementedException();
        }
    }
}
