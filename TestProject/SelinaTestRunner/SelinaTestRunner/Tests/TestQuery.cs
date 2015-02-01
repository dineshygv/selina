using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SelinaTestRunner
{
    class TestQuery
    {
        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        public List<string> GetTestNames(string assemblyRelPath, string @namespace)
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            Type[] typelist = GetTypesInNamespace(Assembly.ReflectionOnlyLoadFrom(assemblyRelPath), @namespace);
            List<string> result = new List<string>();
            foreach (Type type in typelist)
            {
                if (type.CustomAttributes.ToList().Any(ca => ca.ToString().Contains("TestClassAttribute")))
                {
                    var methods = type.GetMethods();
                    foreach (var method in methods)
                    {
                        if (method.CustomAttributes.ToList().Any(ca => ca.ToString().Contains("TestMethodAttribute")))
                        {
                            result.Add(type.Name + "." + method.Name);
                        }
                    }
                }
            }
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomain_ReflectionOnlyAssemblyResolve;
            return result;
        }

    }
}
