﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace INIFilesReader
{
    public static class INISerializer
    {
        internal static SerializableData Deserialize(string path)
        {
            string[] lines = File.ReadAllLines(path);
            SerializableData newData = new SerializableData();
            int heading = -1;
            foreach (string l in lines)
            {
                if (l.Length == 0)
                    continue;

                if (l[0] == '[')
                {
                    newData.Headings.Add(l.Substring(1, l.Length - 2));
                    heading++;
                    newData.Values.Add(new Dictionary<string, string>());
                    continue;
                }

                if (l.Contains("="))
                {
                    if (newData.Values[heading].Keys.Contains(l.Substring(0, l.IndexOf('=')).ToLower().Trim()))
                        continue;
                    newData.Values[heading].Add(l.Substring(0, l.IndexOf('=')).ToLower().Trim(), l.Substring(l.IndexOf('=') + 1).Trim());
                }
                else
                    continue;
            }
            return newData;
        }

        internal static void Serialize(string path, SerializableData serializableData)
        {
            FileStream file = File.Create(path);
            using (StreamWriter streamWriter = new StreamWriter(file))
            {
                for (int i = 0; i < serializableData.Headings.Count; i++)
                {
                    streamWriter.WriteLine("[" + serializableData.Headings[i] + "]");
                    foreach (var pair in serializableData.Values[i])
                    {
                        streamWriter.WriteLine(pair.Key + "=" + pair.Value);
                    }
                }
            }
        }
    }
}
