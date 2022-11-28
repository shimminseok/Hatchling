using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hachling_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            DBController db = new DBController("");
            ServerManager server = new ServerManager(80, db);

            while (server.MainProcess())
            {
                server.CloseSocketProcess();
            }
            server.ReleaseServer();
        }

    }
}
