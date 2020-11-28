using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ds.test.impl
{
    public static class Library
    {
        abstract class APlugin:IPlugin 
        {
            public string name;
            public string version;
            public System.Drawing.Image image;
            public string Plugin
            {
                get
                {
                    return name;
                }
            }
            public string Version
            {
                get
                {
                    return version;
                }
            }
            public System.Drawing.Image Image 
            {
                get
                {
                    return image;
                }
            }
            public virtual int Run(int input1, int input2)
            {
                return 0;
            }
        }
        class Summ:APlugin
        {
            public Summ()
            {
                name = "summ of two ints";
                version = "1.0";
                image = null;
            }
            public override int Run(int input1, int input2)
            {
                return (input1+input2);
            }
        }
        class Differience:APlugin
        {
            public Differience()
            {
                name = "diff of two ints";
                version = "1.0";
                image = null;
            }
            public override int Run(int input1, int input2)
            {
                return (input1 - input2);
            }
        }
        class Multiplication : APlugin
        {
            public Multiplication()
            {
                name = "Multiplication of two ints";
                version = "1.0";
                image = null;
            }
           public override int Run(int input1, int input2)
            {
                return (input1 * input2);
            }
        }
        class Division : APlugin
        {
            public Division()
            {
                name = "Division(without residue) of two ints";
                version = "1.0";
                image = null;
            }
            public override int Run(int input1, int input2)
            {
                return (input1 / input2);
            }
        }

        public interface IPlugin
        {
            string Plugin { get; }
            string Version { get; }
            System.Drawing.Image Image { get; }
            int Run(int iput1, int input2);
        }
        interface PluginFactory //интерфейс из задания
        {
            int PluginsCount { get; }
            string[] GetPluginNames { get; }
            IPlugin GetPlugin(string pluginName, ref int errorCode);
        }
        public static class Plugins // статичная оболочка для фабрики
        {
            public static IFactory factory { get; private set; } static Plugins()
            {
                factory = new IFactory();
            }
        }
        public class IFactory : PluginFactory // Фабрика
        {
            public int PluginsCount
            {
                get
                {
                    int count = 0;
                    foreach (Type t in typeof(APlugin).Assembly.GetExportedTypes())
                    {
                        count++;
                    }
                    
                    return count;
                }
            }
            public string[] GetPluginNames
            {
                get
                {
                    List <string> names = new List<string>();
                    foreach (Type t in typeof(APlugin).Assembly.GetExportedTypes())
                    {
                        names.Add(t.ToString());
                    }
                    return names.ToArray();
                }
            }
            public IPlugin GetPlugin(string pluginName, ref int errorCode)
            {
                if (pluginName.Length < 3)
                {
                    errorCode = 1; //Недостаточная длинна
                    return null;
                }
                foreach (Type type in
                        Assembly.GetAssembly(typeof(APlugin)).GetTypes()
                        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(APlugin))))
                {
                    if (type.ToString().Contains(pluginName))
                        return (IPlugin)Activator.CreateInstance(type);
                }
                errorCode = 2;// Не найдено ни одного вхождения
                return null;
            }
        }
    }
}

