IxMilia.Stl
===========

A portable .NET library for reading and writing STL files.  Clone and build
locally or directly consume the
[NuGet package](http://www.nuget.org/packages/IxMilia.Stl/).

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

- [Visual Studio 2015](https://www.visualstudio.com)
- [.NET Core SDK](https://www.microsoft.com/net/download#core)
