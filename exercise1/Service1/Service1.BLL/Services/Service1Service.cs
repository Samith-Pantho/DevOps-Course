using Service1.BLL.Repositories;
using Service1.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service1.BLL.Services
{
    public class Service1Service : IService1Repository
    {
        private readonly IDalService1Repository _service1;

        public Service1Service(IDalService1Repository service1)
        {
            this._service1 = service1;
        }

        public string getStatus()
        {
            return _service1.getStatus();
        }
        public string getLogs()
        {
            return _service1.getLogs();
        }
        public bool deleteLogs()
        {
            return _service1.deleteLogs();
        }
    }
}
