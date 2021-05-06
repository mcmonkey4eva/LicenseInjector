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
                             + "// This code is Copyright (C) Frenetic LLC under the terms of a strict license.\r\n"
                             + "// See README.md or LICENSE.txt in the Voxalia source root for the contents of the license.\r\n"
                             + "// If neither of these are available, assume that neither you nor anyone other than the copyright holder\r\n"
                             + "// hold any right or permission to use this software until such time as the official license is identified.\r\n"
                             + "//\r\n"
                             + "\r\n";

        const string FGE_LICENSE = "//\r\n"
                             + "// This file is part of the Frenetic Game Engine, created by Frenetic LLC.\r\n"
                             + "// This code is Copyright (C) Frenetic LLC under the terms of a strict license.\r\n"
                             + "// See README.md or LICENSE.txt in the FreneticGameEngine source root for the contents of the license.\r\n"
                             + "// If neither of these are available, assume that neither you nor anyone other than the copyright holder\r\n"
                             + "// hold any right or permission to use this software until such time as the official license is identified.\r\n"
                             + "//\r\n"
                             + "\r\n";

        const string FS_MIT_LICENSE = "//\r\n"
                             + "// This file is part of FreneticScript, created by Frenetic LLC.\r\n"
                             + "// This code is Copyright (C) Frenetic LLC under the terms of the MIT license.\r\n"
                             + "// See README.md or LICENSE.txt in the FreneticScript source root for the contents of the license.\r\n"
                             + "//\r\n"
                             + "\r\n";

        const string FRENUTIL_MIT_LICENSE = "//\r\n"
                             + "// This file is part of Frenetic Utilities, created by Frenetic LLC.\r\n"
                             + "// This code is Copyright (C) Frenetic LLC under the terms of the MIT license.\r\n"
                             + "// See README.md or LICENSE.txt in the FreneticUtilities source root for the contents of the license.\r\n"
                             + "//\r\n"
                             + "\r\n";

        public static UTF8Encoding UTF8 = new UTF8Encoding(false);

        static void Apply(string[] fileList)
        {
            Console.WriteLine($"Scanning {fileList.Length} files...");
            int untouched = 0, skipped = 0, modified = 0;
            StringBuilder existingHeaderBuilder = new StringBuilder();
            foreach (string file in fileList)
            {
                string fileName = file.Replace('\\', '/');
                if (fileName.EndsWith("/GlobalSuppressions.cs") || fileName.Contains("/bin/") || fileName.Contains("/obj/"))
                {
                    skipped++;
                    continue;
                }
                string[] fullOriginalContent = File.ReadAllLines(file);
                if (fullOriginalContent.Length <= 1)
                {
                    skipped++;
                    Console.WriteLine($"Skipping empty file {file}.");
                    continue;
                }
                int firstRealLine = 0;
                for (int i = 0; i < fullOriginalContent.Length; i++)
                {
                    string line = fullOriginalContent[i];
                    if (string.IsNullOrWhiteSpace(line) || (line.StartsWith("//") && !line.StartsWith("///")))
                    {
                        existingHeaderBuilder.Append(line).Append("\r\n");
                    }
                    else
                    {
                        firstRealLine = i;
                        break;
                    }
                }
                string content = string.Join("\r\n", fullOriginalContent[firstRealLine..]);
                string existingHeader = existingHeaderBuilder.ToString().Trim();
                existingHeaderBuilder.Clear();
                string header;
                if (file.Contains("FreneticUtilities") && !file.Contains("FGETests"))
                {
                    header = FRENUTIL_MIT_LICENSE;
                }
                else if (file.Contains("FreneticScript"))
                {
                    header = FS_MIT_LICENSE;
                }
                else if (file.Contains("FreneticGameEngine"))
                {
                    header = FGE_LICENSE;
                }
                else
                {
                    header = VOX_LICENSE;
                }
                if (string.IsNullOrWhiteSpace(existingHeader))
                {
                    Console.WriteLine($"File {file} was missing header.");
                    modified++;
                }
                else if (existingHeader != header.Trim())
                {
                    Console.WriteLine($"File {file} has pre-existing different header, may need to be checked if unique-header intended.");
                    modified++;
                }
                else
                {
                    untouched++;
                    // Still rewrite anyway, in case of missing NL@EOF or encoding issues.
                }
                File.WriteAllBytes(file, UTF8.GetBytes($"{header}{content}\r\n"));
            }
            Console.WriteLine($"For scan of {fileList.Length}, modified {modified}, skipped {skipped}, and left untouched {untouched}");
        }

        static void Main(string[] args)
        {
            Apply(Directory.GetFiles("./", "*.cs", SearchOption.AllDirectories));
            Apply(Directory.GetFiles("./", "*.fs", SearchOption.AllDirectories));
            Apply(Directory.GetFiles("./", "*.vs", SearchOption.AllDirectories));
        }
    }
}
