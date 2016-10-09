using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LicenseInjector
{
    class Program
    {
        const string LICENSE = "//\r\n"
                             + "// This file is part of the game Voxalia, created by FreneticXYZ.\r\n"
                             + "// This code is Copyright (C) 2016 FreneticXYZ under the terms of the MIT license.\r\n"
                             + "// See README.md or LICENSE.txt for contents of the MIT license.\r\n"
                             + "// If these are not available, see https://opensource.org/licenses/MIT\r\n"
                             + "//\r\n"
                             + "\r\n";

        static void Main(string[] args)
        {
            foreach (string file in Directory.GetFiles("./", "*.cs", SearchOption.AllDirectories))
            {
                string t = File.ReadAllText(file);
                while (t.StartsWith("//") || t.StartsWith("\n") || t.StartsWith("\r\n"))
                {
                    int newline = t.IndexOf('\n');
                    t = t.Substring(newline + 1);
                }
                t = LICENSE + t;
                File.WriteAllText(file, t);
            }
        }
    }
}
