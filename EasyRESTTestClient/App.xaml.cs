using EasyRESTFramework.Client;
using EasyRESTTestClient.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EasyRESTTestServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private EasyRESTClient _restClient = null;// new EasyRESTClient("http://localhost:51032/api/");
        private WsContext _wsContext = null;
        private TestClientDataContext _dataContext = null;

        public App()
        {
            _restClient = new EasyRESTClient("http://localhost:51032/api/");
            _wsContext = new WsContext(_restClient);
            _dataContext = new TestClientDataContext(_wsContext);
        }

        public TestClientDataContext CustomDataContext
        {
            get
            {
                return _dataContext;
            }
        }
        
    }
}
