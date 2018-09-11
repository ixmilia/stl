IxMilia.Stl
===========

A portable .NET library for reading and writing STL files.  Clone and build
locally or directly consume the
[NuGet package](http://www.nuget.org/packages/IxMilia.Stl/).

[![Build Status](https://dev.azure.com/ixmilia/public/_apis/build/status/Stl?branchName=master)](https://dev.azure.com/ixmilia/public/_build/latest?definitionId=21)

## Usage

Open an STL file:

``` C#
using System.IO;
using IxMilia.Stl;
// ...

StlFile stlFile;
using (FileStream fs = new FileStream(@"C:\Path\To\File.stl", FileMode.Open))
{
    stlFile = StlFile.Load(fs);
}

// Now check the `stlFile.SolidName` and `stlFile.Triangles` properties.
```

Save an STL file:

``` C#
using System.IO;
using IxMilia.Stl;
// ...

StlFile stlFile = new StlFile();
stlFile.SolidName = "my-solid";
stlFile.Triangles.Add(new StlTriangle(new StlNormal(1, 0, 0), new StlVertex(0, 0, 0), new StlVertex(1, 0, 0), new StlVertex(1, 1, 0)));
// ...

using (FileStream fs = new FileStream(@"C:\Path\To\File.stl", FileMode.Open))
{
    stlFile.Save(fs);
}
```

## Building locally

Requirements to build locally are:

- [Latest .NET Core SDK](https://github.com/dotnet/cli/releases)  As of this writing the following was also required on Ubuntu 14.04:

`sudo apt-get install dotnet-sharedframework-microsoft.netcore.app-1.0.3` 
