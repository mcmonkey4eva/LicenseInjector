using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LicenseInjector
{
    class Program
    {
        const string VOX_LICENSE = "//\r\n"
                             + "// This file is part of the game Voxalia, created by Frenetic LLC.\r\n"
                             + "// This code is Copyright (C) 2016-2017 Frenetic LLC under the terms of a strict license.\r\n"
                             + "// See README.md or LICENSE.txt in the source root for the contents of the license.\r\n"
                             + "// If neither of these are available, assume that neither you nor anyone other than the copyright holder\r\n"
                             + "// hold any right or permission to use this software until such time as the official license is identified.\r\n"
                             + "//\r\n"
                             + "\r\n";

        const string FGE_LICENSE = "//\r\n"
                             + "// This file is created by Frenetic LLC.\r\n"
                             + "// This code is Copyright (C) 2017 Frenetic LLC under the terms of a strict license.\r\n"
                             + "// See README.md or LICENSE.txt in the source root for the contents of the license.\r\n"
                             + "// If neither of these are available, assume that neither you nor anyone other than the copyright holder\r\n"
                             + "// hold any right or permission to use this software until such time as the official license is identified.\r\n"
                             + "//\r\n"
                             + "\r\n";

        const string FGE2_LICENSE = "//\r\n"
                             + "// This file is created by Frenetic LLC.\r\n"
                             + "// This code is Copyright (C) 2016-2017 Frenetic LLC under the terms of a strict license.\r\n"
                             + "// See README.md or LICENSE.txt in the source root for the contents of the license.\r\n"
                             + "// If neither of these are available, assume that neither you nor anyone other than the copyright holder\r\n"
                             + "// hold any right or permission to use this software until such time as the official license is identified.\r\n"
                             + "//\r\n"
                             + "\r\n";

        static void Apply(string[] f)
        {
            foreach (string file in f)
            {
                string t = File.ReadAllText(file);
                while (t.StartsWith("//") || t.StartsWith("\n") || t.StartsWith("\r\n"))
                {
                    int newline = t.IndexOf('\n');
                    t = t.Substring(newline + 1);
                }
                if (file.Contains("FreneticGameEngine") && (file.Contains("FreneticScript") || file.Contains("FreneticDataSyntax")))
                {
                    t = FGE_LICENSE + t;
                }
                else if (file.Contains("FreneticGameEngine"))
                {
                    t = FGE_LICENSE + t;
                }
                else
                {
                    t = VOX_LICENSE + t;
                }
                File.WriteAllText(file, t);
            }
        }

        static void Main(string[] args)
        {
            Apply(Directory.GetFiles("./", "*.cs", SearchOption.AllDirectories));
            Apply(Directory.GetFiles("./", "*.fs", SearchOption.AllDirectories));
            Apply(Directory.GetFiles("./", "*.vs", SearchOption.AllDirectories));
        }
    }
}
