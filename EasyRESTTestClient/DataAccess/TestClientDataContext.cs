using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Abstractions;

namespace EasyRESTTestClient.DataAccess
{
    public class TestClientDataContext
    {
        private IWsContext _wsContext = null;
        private IStudentRepository _studentRepository;

        public TestClientDataContext(IWsContext wsContext)
        {
            this._wsContext = wsContext;
            //allocate repository based on the context
            _studentRepository = new StudentsRepository(_wsContext);
        }

        public IStudentRepository Students
        {
            get
            {
                return _studentRepository;
            }
            private set
            {
                _studentRepository = value;
            }
        }

        public async Task SaveAll()
        {
            await _wsContext.SaveAllAsync();
        }
    }
}
