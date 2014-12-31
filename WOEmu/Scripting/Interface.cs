using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

using WOEmu.Objects;

using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;

namespace WOEmu.Scripting
{
    public static class ScriptingInterface
    {
        public static void logTxt(string txt)
        {
            WO.Core.Logger.Logger.AppendLine(txt);
        }

        public static void Init(string RootDir)
        {
            compiler = new BooCompiler();
            compiler.Parameters.Pipeline = new CompileToMemory();
            compiler.Parameters.Ducky = true;

            gameModuleList = new List<string>();
            objectModuleList = new List<string>();
            moduleFileList = new List<string>();

            objectNames = new List<string>();

            #region Game directory
            //Game directory
            DirectoryInfo dirInfGame = new DirectoryInfo(RootDir + "/Game");
            FileInfo[] modFilesGame = dirInfGame.GetFiles("*.boo");
            foreach (FileInfo fileInf in modFilesGame)
            {
                string filename = fileInf.Name;
                WO.Core.Logger.Logger.printDebug("Game Script: " + filename);

                try
                {
                    moduleFileList.Add(RootDir + "/Game/" + filename);
                    string[] tokens = filename.Split(new char[] { '.' });
                    gameModuleList.Add(tokens[0]);
                    compiler.Parameters.Input.Add(new FileInput(RootDir + "/Game/" + filename));
                }
                catch (EndOfStreamException)
                {
                    WO.Core.Logger.Logger.printWarning("Corrupted script file '" + filename + "'.");
                }
            }
            #endregion
            #region Objects directory
            DirectoryInfo dirInfObj = new DirectoryInfo(RootDir + "/Objects");
            FileInfo[] modFilesObj = dirInfObj.GetFiles("*.boo");
            foreach (FileInfo fileInf in modFilesObj)
            {
                string filename = fileInf.Name;
                WO.Core.Logger.Logger.printDebug("Object Script: " + filename);

                try
                {
                    moduleFileList.Add(RootDir + "/Objects/" + filename);
                    string[] tokens = filename.Split(new char[] { '.' });
                    objectModuleList.Add(tokens[0]);
                    compiler.Parameters.Input.Add(new FileInput(RootDir + "/Objects/" + filename));
                }
                catch (EndOfStreamException)
                {
                    WO.Core.Logger.Logger.printWarning("Corrupted script file '" + filename + "'.");
                }
            }
            #endregion

            context = compiler.Run();
            if (context.GeneratedAssembly == null)
            {
                foreach (CompilerError err in context.Errors)
                    WO.Core.Logger.Logger.printWarning(err.ToString());

                WO.Core.Logger.Logger.printWarning("Not continuing...");
                while (true) { }
            }

            foreach (string mod in objectModuleList)
            {
                Type module = context.GeneratedAssembly.GetType(mod + "Module");
                FieldInfo inf = module.GetField("__ObjectName");
                
                
            }
        }

        public static List<Object> runGameHook(string name, params object[] par)
        {
            List<Object> buf = new List<object>();
            foreach (string mod in gameModuleList)
            {
                Type module = context.GeneratedAssembly.GetType(mod + "Module");
                MethodInfo method = module.GetMethod(name);
                buf.Add(method.Invoke(null, par));
            }
            return buf;
        }



        public static void runObjectHook(ObjectBase b, string name, params object[] par)
        {
            foreach (string mod in objectModuleList)
            {
                Type module = context.GeneratedAssembly.GetType(mod + "Module");
                MethodInfo method = module.GetMethod(name);
                method.Invoke(null, par);
            }
        }

        public static List<string> gameModuleList;
        public static List<string> objectModuleList;
        public static List<string> moduleFileList;
        public static BooCompiler compiler;
        public static CompilerContext context;

        public static List<string> objectNames;
    }

}
