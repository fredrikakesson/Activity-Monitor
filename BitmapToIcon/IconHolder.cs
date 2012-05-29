// Created by Joshua Flanagan
// http://flimflan.com/blog
// May 2004
//
// You may freely use this code as you wish, I only ask that you retain my name in the source code

using System;
using System.Drawing;
using System.IO;

namespace BitmapToIcon
{
    /// <summary>
    /// Provides an in-memory representation of the device independent bitmap format
    /// </summary>
    /// <remarks>
    /// Based on documentation at http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwui/html/msdn_icons.asp
    /// </remarks>
    public class IconHolder
    {
        public IconDir IconDirectory;
        public IconImage[] IconImages;

        public IconHolder()
        {
            IconDirectory.Reserved = 0;
            IconDirectory.ResourceType = 1;
            IconDirectory.EntryCount = 1;
            IconImages = new[] {new IconImage()};
        }

        public void Open(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            Open(File.OpenRead(filename));
        }

        public void Open(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                IconDirectory.Populate(br);
                IconImages = new IconImage[IconDirectory.EntryCount];
                // Loop through and read in each image
                for (var i = 0; i < IconImages.Length; i++)
                {
                    // Seek to the location in the file that has the image
                    //  SetFilePointer( hFile, pIconDir->idEntries[i].dwImageOffset, NULL, FILE_BEGIN );
                    br.BaseStream.Seek(IconDirectory.Entries[i].ImageOffset, SeekOrigin.Begin);
                    // Read the image data
                    //  ReadFile( hFile, pIconImage, pIconDir->idEntries[i].dwBytesInRes, &dwBytesRead, NULL );
                    // Here, pIconImage is an ICONIMAGE structure. Party on it :)
                    IconImages[i] = new IconImage();
                    IconImages[i].Populate(br);
                }
            }
        }

        public void Save(string filename)
        {
            using (var bw = new BinaryWriter(File.OpenWrite(filename)))
            {
                Save(bw);
            }
        }

        public void Save(BinaryWriter bw)
        {
            IconDirectory.Save(bw);
            for (var i = 0; i < IconImages.Length; i++)
                IconImages[i].Save(bw);
        }

        public Icon ToIcon()
        {
            Icon newIcon;
            using (var bw = new BinaryWriter(new MemoryStream()))
            {
                Save(bw);
                bw.BaseStream.Position = 0;
                newIcon = new Icon(bw.BaseStream);
            }
            return newIcon;
        }
    }
}