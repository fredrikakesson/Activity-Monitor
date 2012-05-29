// Created by Joshua Flanagan
// http://flimflan.com/blog
// May 2004
//
// You may freely use this code as you wish, I only ask that you retain my name in the source code

using System;
using System.IO;

namespace BitmapToIcon
{
    /// <summary>
    /// Provides an in-memory representation of the device independent bitmap format
    /// </summary>
    /// <remarks>
    /// Based on documentation at http://www.whisqu.se/per/docs/graphics52.htm
    /// </remarks>
    public class BitmapHolder
    {
        public BitmapFileHeader FileHeader;
        public byte[] ImageData;
        public BitmapInfo Info;

        public void Open(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            Open(File.OpenRead(filename));
        }

        public void Open(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                FileHeader.Populate(br);
                Info.Populate(br);
                if (Info.InfoHeader.BiSizeImage > 0)
                    ImageData = br.ReadBytes((int) Info.InfoHeader.BiSizeImage);
                else
                    ImageData = br.ReadBytes((int) (br.BaseStream.Length - br.BaseStream.Position));
            }
        }

        public void Save(string filename)
        {
            Save(File.OpenWrite(filename));
        }

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
            {
                FileHeader.Save(bw);
                Info.Save(bw);
                bw.Write(ImageData);
            }
        }
    }
}