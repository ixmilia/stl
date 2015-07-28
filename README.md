IxMilia.Stl
===========

A portable .NET library for reading and writing STL files.

### Usage

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
