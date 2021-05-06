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

        static string HeaderFor(string fileName)
        {
            if (fileName.Contains("/FreneticUtilities/") && !fileName.Contains("/FGETests/"))
            {
                return FRENUTIL_MIT_LICENSE;
            }
            else if (fileName.Contains("/FreneticScript/"))
            {
                return FS_MIT_LICENSE;
            }
            else if (fileName.Contains("/FreneticGameEngine/"))
            {
                return FGE_LICENSE;
            }
            else
            {
                return VOX_LICENSE;
            }
        }

        public static StringBuilder SectionBuilderHelper = new StringBuilder();

        static string GetExistingHeader(string[] fileContent, out int firstRealLine)
        {
            SectionBuilderHelper.Clear();
            for (int i = 0; i < fileContent.Length; i++)
            {
                string line = fileContent[i];
                if (string.IsNullOrWhiteSpace(line) || (line.StartsWith("//") && !line.StartsWith("///")))
                {
                    SectionBuilderHelper.Append(line).Append("\r\n");
                }
                else
                {
                    firstRealLine = i;
                    return SectionBuilderHelper.ToString().Trim();
                }
            }
            firstRealLine = 0;
            return "";
        }

        static void GetUsings(string[] fileContent, List<string> list, int startIndex, out int firstCodeLine)
        {
            firstCodeLine = startIndex;
            for (int i = startIndex; i < fileContent.Length; i++)
            {
                string line = fileContent[i];
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("using "))
                {
                    list.Add(line);
                }
                else
                {
                    firstCodeLine = i;
                    return;
                }
            }
        }

        public static List<string> UsingStatics = new List<string>(), UsingClasses = new List<string>(), NormalUsings = new List<string>();

        public static string[] RequiredUsings = new string[]
        {
            "using System;",
            "using System.Collections.Generic;",
            "using System.Text;",
            "using System.Linq;"
        };

        static string GetSortedUsings(List<string> usings)
        {
            if (!usings.Any())
            {
                return "";
            }
            SectionBuilderHelper.Clear();
            UsingStatics.Clear();
            UsingClasses.Clear();
            NormalUsings.Clear();
            foreach (string line in usings)
            {
                if (line.StartsWith("using static "))
                {
                    UsingStatics.Add(line);
                }
                else if (line.Contains("="))
                {
                    UsingClasses.Add(line);
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    NormalUsings.Add(line);
                }
            }
            foreach (string required in RequiredUsings)
            {
                if (!NormalUsings.Contains(required))
                {
                    NormalUsings.Add(required);
                }
            }
            NormalUsings = NormalUsings.OrderBy(s =>
            {
                string mainPart = s["using ".Length..];
                if (mainPart.StartsWith("System"))
                {
                    return 0;
                }
                else if (mainPart.StartsWith("FreneticUtilities"))
                {
                    return 10;
                }
                else if (mainPart.StartsWith("FGECore"))
                {
                    return 20;
                }
                else if (mainPart.StartsWith("FGEGraphics"))
                {
                    return 30;
                }
                else if (mainPart.StartsWith("FreneticScript"))
                {
                    return 40;
                }
                else if (mainPart.StartsWith("Bepu"))
                {
                    return 50;
                }
                else if (mainPart.StartsWith("OpenTK"))
                {
                    return 60;
                }
                else if (mainPart.StartsWith("Voxalia.Shared"))
                {
                    return 70;
                }
                else if (mainPart.StartsWith("Voxalia.Server"))
                {
                    return 80;
                }
                else if (mainPart.StartsWith("Voxalia.Client"))
                {
                    return 90;
                }
                return 100;
            }).ThenBy(s => s).ToList();
            foreach (string line in NormalUsings)
            {
                SectionBuilderHelper.Append(line).Append("\r\n");
            }
            if (UsingStatics.Any())
            {
                SectionBuilderHelper.Append("\r\n");
                foreach (string line in UsingStatics.OrderBy(s => s))
                {
                    SectionBuilderHelper.Append(line).Append("\r\n");
                }
            }
            if (UsingClasses.Any())
            {
                SectionBuilderHelper.Append("\r\n");
                foreach (string line in UsingClasses.OrderBy(s => s))
                {
                    SectionBuilderHelper.Append(line).Append("\r\n");
                }
            }
            SectionBuilderHelper.Append("\r\n");
            return SectionBuilderHelper.ToString();
        }

        static void Apply(string[] fileList, bool fixUsings)
        {
            Console.WriteLine($"Scanning {fileList.Length} files...");
            int untouched = 0, skipped = 0, modified = 0, sorted = 0;
            List<string> usings = new List<string>();
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
                string existingHeader = GetExistingHeader(fullOriginalContent, out int firstRealLine);
                string header = HeaderFor(fileName);
                bool changedHeader = false;
                if (string.IsNullOrWhiteSpace(existingHeader))
                {
                    Console.WriteLine($"File {file} was missing header.");
                    changedHeader = true;
                }
                else if (existingHeader != header.Trim())
                {
                    Console.WriteLine($"File {file} has pre-existing different header, may need to be checked if unique-header intended.");
                    changedHeader = true;
                }
                string altUsings = "";
                bool didSort = false;
                if (fixUsings)
                {
                    usings.Clear();
                    GetUsings(fullOriginalContent, usings, firstRealLine, out firstRealLine);
                    string originalUsings = string.Join("\r\n", usings);
                    altUsings = GetSortedUsings(usings);
                    didSort = originalUsings != altUsings;
                }
                string content = string.Join("\r\n", fullOriginalContent[firstRealLine..]);
                if (changedHeader)
                {
                    modified++;
                }
                if (didSort)
                {
                    sorted++;
                }
                if (!changedHeader && !didSort)
                {
                    untouched++;
                    // Still rewrite anyway, in case of missing NL@EOF or encoding issues.
                }
                File.WriteAllBytes(file, UTF8.GetBytes($"{header}{altUsings}{content}\r\n"));
            }
            Console.WriteLine($"For scan of {fileList.Length}, modified {modified} headers, skipped {skipped}, sorted usings for {sorted}, and left untouched {untouched}");
        }

        static void Main(string[] args)
        {
            Apply(Directory.GetFiles("./", "*.cs", SearchOption.AllDirectories), true);
            Apply(Directory.GetFiles("./", "*.fs", SearchOption.AllDirectories), false);
            Apply(Directory.GetFiles("./", "*.vs", SearchOption.AllDirectories), false);
            Apply(Directory.GetFiles("./", "*.geom", SearchOption.AllDirectories), false);
            Apply(Directory.GetFiles("./", "*.inc", SearchOption.AllDirectories), false);
        }
    }
}
