LicenseInjector
---------------

This is the little tool I wrote to automatically ensure the correct license header is present in Voxalia, [FreneticGameEngine](https://github.com/FreneticLLC/FreneticGameEngine), [FreneticScript](https://github.com/FreneticLLC/FreneticScript), and [FreneticUtilities](https://github.com/FreneticLLC/FreneticUtilities).

It also can sort the `using` statements at the top of C# files to keep them clean and orderly (alphabetically, but with `System` at the top and a few other custom intentional alternate orderings based largely on project hierarchy and usual prominence).

This also does an file format cleanup, forcing all files to Windows-style `\r\n` line endings (I work on Windows in Visual Studio so this is unfortunately needed), ensuring new-line-at-end-of-file, and encoding as UTF-8 (no BOM... I'm somewhat confused how any files ended up with a BOM, considering "UTF-8 With BOM" is not a standard anywhere ever, nor even a functional thing that makes any sense, but... Microsoft works in mysterious ways, and so those showed up in a few files).

### Status and Usability

This isn't particularly extensible or configurable in any way on its own. It is, however, very quick and easy - all the code is in one short file.

If you want to use it for any projects other than the above listed ones, you would have to fork it and modify the code to fit your own needs and include your own headers and/or special sorting requirements.

### Licensing pre-note:

This is an open source project, provided entirely freely, for everyone to use and contribute to.

If you make any changes that could benefit the community as a whole, please contribute upstream.

### The short of the license is:

You can do basically whatever you want (as long as you give credit), except you may not hold any developer liable for what you do with the software.

### The long version of the license follows:

Previous License

Copyright (C) 2016-2021 Alex "mcmonkey" Goodwin, All Rights Reserved.

The MIT License (MIT)

Copyright (c) 2021 Alex "mcmonkey" Goodwin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
