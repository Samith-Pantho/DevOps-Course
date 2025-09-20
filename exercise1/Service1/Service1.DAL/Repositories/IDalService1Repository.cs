using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service1.DAL.Repositories
{
    public interface IDalService1Repository
    {
        public string getStatus();
        public string getLogs();
        public bool deleteLogs();
    }
}
