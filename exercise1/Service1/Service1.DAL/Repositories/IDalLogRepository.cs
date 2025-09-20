using System;
using System.Collections.Generic;
using System.Text;

namespace Service1.DAL.Repositories
{
    public interface IDalLogRepository
    {
        public void SaveLog(string message);
    }
}
