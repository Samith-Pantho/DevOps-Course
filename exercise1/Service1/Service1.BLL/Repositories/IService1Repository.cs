using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service1.BLL.Repositories
{
    public interface IService1Repository
    {
        public string getStatus();
        public string getLogs();
        public bool deleteLogs();
    }
}
