using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WOEmu.Config
{
    /// <summary>
    /// Holds values from the config file.
    /// </summary>
    public class Configuration
    {
        public void Load(string filename)
        {
            Vars = new List<ConfigVar>();
            FileStream f = File.Open(filename, FileMode.Open);
            StreamReader r = new StreamReader(f);

            while (!r.EndOfStream)
            {
                string line = r.ReadLine();

                if (line == "")
                    continue;

                if (line[0] == '#')
                    continue;

                string[] tokens = line.Split(new char[] { '=' });

                string var = tokens[0].Trim();
                string value = tokens[1].Trim();

                ConfigVar c = new ConfigVar(var, value);

                Vars.Add(c);
            }
        }

        public string GetValue(string name)
        {
            foreach (ConfigVar c in Vars)
            {
                if (c.Name == name)
                {
                    return c.Value;
                }
            }
            throw new NullReferenceException("Could not find config line '" + name + "'.");
        }

        public List<ConfigVar> Vars;
    }

    public class ConfigVar
    {
        public ConfigVar(string n, string v)
        {
            Name = n;
            Value = v;
        }

        public string Name;
        public string Value;
    }
}
