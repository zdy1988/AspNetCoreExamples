using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public interface IRepository
    {
        string GetInfo();
    }

    public class EFRepository : IRepository
    {
        public string GetInfo()
        {
            return "load data form ef！";
        }
    }

    public class DapperRepository : IRepository
    {
        public string GetInfo()
        {
            return "load data form dapper！";
        }
    }

    public class OperationService
    {
        private readonly IRepository _repository;
        public OperationService(IRepository repository)
        {
            _repository = repository;
        }

        public string GetList()
        {
            return _repository.GetInfo();
        }
    }   
}
