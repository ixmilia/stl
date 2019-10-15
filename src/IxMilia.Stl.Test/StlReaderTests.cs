// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace IxMilia.Stl.Test
{
    public class StlReaderTests
    {
        [Fact]
        public void ReadOneVertexTest()
        {
            var file = FromString(@"
solid foo
  facet normal 1 2 3
    outer loop
      vertex 4 5 6
      vertex 7 8 9
      vertex 10 11 12
    endloop
  endfacet
endsolid foo
");
            Assert.Equal("foo", file.SolidName);
            Assert.Single(file.Triangles);
            Assert.Equal(new StlNormal(1, 2, 3), file.Triangles[0].Normal);
            Assert.Equal(new StlVertex(4, 5, 6), file.Triangles[0].Vertex1);
            Assert.Equal(new StlVertex(7, 8, 9), file.Triangles[0].Vertex2);
            Assert.Equal(new StlVertex(10, 11, 12), file.Triangles[0].Vertex3);
        }

        [Fact]
        public void ReadTwoVerticiesTest()
        {
            var file = FromString(@"
solid foo
  facet normal 1 2 3
    outer loop
      vertex 4 5 6
      vertex 7 8 9
      vertex 10 11 12
    endloop
  endfacet
  facet normal 13 14 15
    outer loop
      vertex 16 17 18
      vertex 19 20 21
      vertex 22 23 24
    endloop
  endfacet
endsolid foo
");
            Assert.Equal("foo", file.SolidName);
            Assert.Equal(2, file.Triangles.Count);
            Assert.Equal(new StlNormal(1, 2, 3), file.Triangles[0].Normal);
            Assert.Equal(new StlVertex(4, 5, 6), file.Triangles[0].Vertex1);
            Assert.Equal(new StlVertex(7, 8, 9), file.Triangles[0].Vertex2);
            Assert.Equal(new StlVertex(10, 11, 12), file.Triangles[0].Vertex3);
            Assert.Equal(new StlNormal(13, 14, 15), file.Triangles[1].Normal);
            Assert.Equal(new StlVertex(16, 17, 18), file.Triangles[1].Vertex1);
            Assert.Equal(new StlVertex(19, 20, 21), file.Triangles[1].Vertex2);
            Assert.Equal(new StlVertex(22, 23, 24), file.Triangles[1].Vertex3);
        }

        [Fact]
        public void ReadAsciiStlNoLengthOrPositionStreamTest()
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                writer.WriteLine(@"
solid foo
  facet normal 1 2 3
    outer loop
      vertex 4 5 6
      vertex 7 8 9
      vertex 10 11 12
    endloop
  endfacet
  facet normal 13 14 15
    outer loop
      vertex 16 17 18
      vertex 19 20 21
      vertex 22 23 24
    endloop
  endfacet
endsolid foo
".Trim());
                writer.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                using (var stream = new StreamWithNoLengthOrPosition(ms))
                {
                    var file = StlFile.Load(stream);
                    Assert.Equal("foo", file.SolidName);
                    Assert.Equal(2, file.Triangles.Count);
                    Assert.Equal(new StlNormal(1, 2, 3), file.Triangles[0].Normal);
                    Assert.Equal(new StlVertex(4, 5, 6), file.Triangles[0].Vertex1);
                    Assert.Equal(new StlVertex(7, 8, 9), file.Triangles[0].Vertex2);
                    Assert.Equal(new StlVertex(10, 11, 12), file.Triangles[0].Vertex3);
                    Assert.Equal(new StlNormal(13, 14, 15), file.Triangles[1].Normal);
                    Assert.Equal(new StlVertex(16, 17, 18), file.Triangles[1].Vertex1);
                    Assert.Equal(new StlVertex(19, 20, 21), file.Triangles[1].Vertex2);
                    Assert.Equal(new StlVertex(22, 23, 24), file.Triangles[1].Vertex3);
                }
            }
        }

        [Fact]
        public void ReadBinaryStlNoLengthOrPositionStreamTest()
        {
            var binarySize =
                80 +                    // header
                sizeof(int) +           // number of triangles
                2 *                     // 2 verticies
                (
                    sizeof(float) * 3 +     // normal vector
                    sizeof(float) * 3 +     // vertex 1
                    sizeof(float) * 3 +     // vertex 2
                    sizeof(float) * 3 +     // vertex 3
                    sizeof(short)           // attribute byte count
                );
            var binaryList = new List<byte>();

            binaryList.AddRange(new byte[80]); // header
            binaryList.AddRange(BitConverter.GetBytes(2)); // 2 triangles

            // first triangle
            binaryList.AddRange(BitConverter.GetBytes(1.0f)); // normal.X
            binaryList.AddRange(BitConverter.GetBytes(2.0f)); // normal.Y
            binaryList.AddRange(BitConverter.GetBytes(3.0f)); // normal.Z
            binaryList.AddRange(BitConverter.GetBytes(4.0f)); // vertex1.X
            binaryList.AddRange(BitConverter.GetBytes(5.0f)); // vertex1.Y
            binaryList.AddRange(BitConverter.GetBytes(6.0f)); // vertex1.Z
            binaryList.AddRange(BitConverter.GetBytes(7.0f)); // vertex2.X
            binaryList.AddRange(BitConverter.GetBytes(8.0f)); // vertex2.Y
            binaryList.AddRange(BitConverter.GetBytes(9.0f)); // vertex2.Z
            binaryList.AddRange(BitConverter.GetBytes(10.0f)); // vertex3.X
            binaryList.AddRange(BitConverter.GetBytes(11.0f)); // vertex3.Y
            binaryList.AddRange(BitConverter.GetBytes(12.0f)); // vertex3.Z
            binaryList.AddRange(BitConverter.GetBytes((short)0)); // attribute byte count

            // second triangle
            binaryList.AddRange(BitConverter.GetBytes(13.0f)); // normal.X
            binaryList.AddRange(BitConverter.GetBytes(14.0f)); // normal.Y
            binaryList.AddRange(BitConverter.GetBytes(15.0f)); // normal.Z
            binaryList.AddRange(BitConverter.GetBytes(16.0f)); // vertex1.X
            binaryList.AddRange(BitConverter.GetBytes(17.0f)); // vertex1.Y
            binaryList.AddRange(BitConverter.GetBytes(18.0f)); // vertex1.Z
            binaryList.AddRange(BitConverter.GetBytes(19.0f)); // vertex2.X
            binaryList.AddRange(BitConverter.GetBytes(20.0f)); // vertex2.Y
            binaryList.AddRange(BitConverter.GetBytes(21.0f)); // vertex2.Z
            binaryList.AddRange(BitConverter.GetBytes(22.0f)); // vertex3.X
            binaryList.AddRange(BitConverter.GetBytes(23.0f)); // vertex3.Y
            binaryList.AddRange(BitConverter.GetBytes(24.0f)); // vertex3.Z
            binaryList.AddRange(BitConverter.GetBytes((short)0)); // attribute byte count

            Assert.Equal(binarySize, binaryList.Count);

            using (var ms = new MemoryStream())
            {
                ms.Write(binaryList.ToArray(), 0, binaryList.Count);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                using (var stream = new StreamWithNoLengthOrPosition(ms))
                {
                    var file = StlFile.Load(stream);
                    Assert.Equal(2, file.Triangles.Count);

                    Assert.Equal(new StlNormal(1.0f, 2.0f, 3.0f), file.Triangles[0].Normal);
                    Assert.Equal(new StlVertex(4.0f, 5.0f, 6.0f), file.Triangles[0].Vertex1);
                    Assert.Equal(new StlVertex(7.0f, 8.0f, 9.0f), file.Triangles[0].Vertex2);
                    Assert.Equal(new StlVertex(10.0f, 11.0f, 12.0f), file.Triangles[0].Vertex3);

                    Assert.Equal(new StlNormal(13.0f, 14.0f, 15.0f), file.Triangles[1].Normal);
                    Assert.Equal(new StlVertex(16.0f, 17.0f, 18.0f), file.Triangles[1].Vertex1);
                    Assert.Equal(new StlVertex(19.0f, 20.0f, 21.0f), file.Triangles[1].Vertex2);
                    Assert.Equal(new StlVertex(22.0f, 23.0f, 24.0f), file.Triangles[1].Vertex3);
                }
            }
        }

        [Fact]
        public void ReadAsciiTriangleWithNoNormalTest()
        {
            var file = FromString(@"
solid foo
  facet normal 0 0 0
    outer loop
      vertex 0 0 0
      vertex 1 0 0
      vertex 0 1 0
    endloop
  endfacet
endsolid foo
");

            // should be auto-corrected to the positive z axis
            Assert.Equal(new StlNormal(0, 0, 1), file.Triangles.Single().Normal);
        }

        [Fact]
        public void ReadBinaryTriangleWithNoNormalTest()
        {
            var binaryList = new List<byte>();
            binaryList.AddRange(new byte[80]); // header
            binaryList.AddRange(BitConverter.GetBytes(1)); // 1 triangle
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // normal.X
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // normal.Y
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // normal.Z
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex1.X
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex1.Y
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex1.Z
            binaryList.AddRange(BitConverter.GetBytes(1.0f)); // vertex2.X
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex2.Y
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex2.Z
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex3.X
            binaryList.AddRange(BitConverter.GetBytes(1.0f)); // vertex3.Y
            binaryList.AddRange(BitConverter.GetBytes(0.0f)); // vertex3.Z
            binaryList.AddRange(BitConverter.GetBytes((short)0)); // attribute byte count
            using (var ms = new MemoryStream())
            {
                ms.Write(binaryList.ToArray(), 0, binaryList.Count);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                var file = StlFile.Load(ms);

                // should be auto-corrected to the positive z axis
                Assert.Equal(new StlNormal(0, 0, 1), file.Triangles.Single().Normal);
            }
        }

        [Fact]
        public void ReadDecimalSeparatorCultureTest()
        {
            var existingCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
                var stl = FromString(@"
solid foo
  facet normal 0 0 1.0
    outer loop
      vertex 0 0 0
      vertex 1 0 0
      vertex 0 1 0
    endloop
  endfacet
endsolid foo
");
                Assert.Equal(new StlNormal(0.0f, 0.0f, 1.0f), stl.Triangles.Single().Normal);
            }
            finally
            {
                CultureInfo.CurrentCulture = existingCulture;
            }
        }

        private StlFile FromString(string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content.Trim());
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return StlFile.Load(stream);
        }
    }
}
