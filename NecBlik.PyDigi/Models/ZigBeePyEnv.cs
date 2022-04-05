using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.PyDigi.Models
{
    public class ZigBeePyEnv
    {
        private static ZigBeePyEnv instance = null;
        public static ZigBeePyEnv Instance
        {
            get
            {
                if(ZigBeePyEnv.instance==null)
                {
                    ZigBeePyEnv.instance = new ZigBeePyEnv();
                }
                return ZigBeePyEnv.instance;
            }
            private set
            {
                ZigBeePyEnv.instance = value;
            }
        }


        private ZigBeePyEnv()
        {
            string pathToVirtualEnv = Resources.Resources.PythonVirtualEnvPath;
            var path = Environment.GetEnvironmentVariable("PATH").TrimEnd(';');
            path = string.IsNullOrEmpty(path) ? pathToVirtualEnv : path + ";" + pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Runtime.PythonDLL = pathToVirtualEnv + Resources.Resources.PythonDLLSubpath;
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();
        }

        ~ZigBeePyEnv()
        {
            PythonEngine.Shutdown();
        }

        public static void Initialize()
        {
            if(ZigBeePyEnv.instance==null)
            {
                ZigBeePyEnv.instance = new ZigBeePyEnv();
            }
        }

        public static PyModule NewScope()
        {
            using (Py.GIL())
                return Py.CreateScope();
        }

        public static PyModule NewInitializedScope()
        {
            using(Py.GIL())
            {
                var scope = NewScope();
                InitializeScope(scope);
                return scope;
            }
        }

        public static void InitializeScope(PyModule scope)
        {
            using (Py.GIL())
            {
                scope = Py.CreateScope();
                var initScript = File.ReadAllText(Resources.Resources.PyDigiScriptsLocation + "/" + Resources.Resources.ScriptInitialize_py)
                    .Replace("{$path$}", "\"" + Resources.Resources.PyDigiScriptsLocation + "\"");
                scope.Exec(initScript);
            }
        }
    }
}
