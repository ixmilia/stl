// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;

namespace IxMilia.Stl
{
    public class StlFile
    {
        public string SolidName { get; set; }

        public List<StlTriangle> Triangles { get; private set; }

        public StlFile()
        {
            Triangles = new List<StlTriangle>();
        }

#if HAS_FILESYSTEM_ACCESS
        public void Save(string path, bool asAscii = true)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                Save(stream, asAscii);
            }
        }
#endif

        public void Save(Stream stream, bool asAscii = true)
        {
            var writer = new StlWriter();
            writer.Write(this, stream, asAscii);
        }

#if HAS_FILESYSTEM_ACCESS
        public static StlFile Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return Load(stream);
            }
        }
#endif

        public static StlFile Load(Stream stream)
        {
            var file = new StlFile();
            var reader = new StlReader(stream);
            file.SolidName = reader.ReadSolidName();
            file.Triangles = reader.ReadTriangles();
            return file;
        }
    }
}
