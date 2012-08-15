// Created by Joshua Flanagan
// http://flimflan.com/blog
// May 2004
//
// You may freely use this code as you wish, I only ask that you retain my name in the source code

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BitmapToIcon
{
    /// <summary>
    /// Provides methods for converting between the bitmap and icon formats
    /// </summary>
    public class Converter
    {
        public static Icon BitmapToIcon(Bitmap b)
        {
            var ico = BitmapToIconHolder(b);
            Icon newIcon;
            using (var bw = new BinaryWriter(new MemoryStream()))
            {
                ico.Save(bw);
                bw.BaseStream.Position = 0;
                newIcon = new Icon(bw.BaseStream);
            }
            return newIcon;
        }

        public static IconHolder BitmapToIconHolder(Bitmap b)
        {
            var bmp = new BitmapHolder();
            using (var stream = new MemoryStream())
            {
                b.Save(stream, ImageFormat.Bmp);
                stream.Position = 0;
                bmp.Open(stream);
            }
            return BitmapToIconHolder(bmp);
        }

        public static IconHolder BitmapToIconHolder(BitmapHolder bmp)
        {
            var mapColors = (bmp.Info.InfoHeader.BiBitCount <= 24);
            //Hashtable uniqueColors = new Hashtable(maximumColors);
            // actual colors is probably nowhere near maximum, so dont try to initialize the hashtable
            var uniqueColors = new Hashtable();

            int sourcePosition = 0;
            int numPixels = bmp.Info.InfoHeader.BiHeight*bmp.Info.InfoHeader.BiWidth;
            var indexedImage = new byte[numPixels];
            byte colorIndex;

            if (mapColors)
            {
                for (var i = 0; i < indexedImage.Length; i++)
                {
                    //TODO: currently assumes source bitmap is 24bit color
                    //read 3 bytes, convert to color
                    var pixel = new byte[3];
                    Array.Copy(bmp.ImageData, sourcePosition, pixel, 0, 3);
                    sourcePosition += 3;

                    var color = new RGBQUAD(pixel);
                    if (uniqueColors.Contains(color))
                    {
                        colorIndex = Convert.ToByte(uniqueColors[color]);
                    }
                    else
                    {
                        if (uniqueColors.Count > byte.MaxValue)
                        {
                            throw new NotSupportedException(
                                String.Format("The source image contains more than {0} colors.", byte.MaxValue));
                        }
                        colorIndex = Convert.ToByte(uniqueColors.Count);
                        uniqueColors.Add(color, colorIndex);
                    }
                    // store pixel as an index into the color table
                    indexedImage[i] = colorIndex;
                }
            }
            else
            {
                // added by Pavel Janda on 14/11/2006
                if (bmp.Info.InfoHeader.BiBitCount == 32)
                {
                    for (var i = 0; i < indexedImage.Length; i++)
                    {
                        //TODO: currently assumes source bitmap is 32bit color with alpha set to zero
                        //ignore first byte, read another 3 bytes, convert to color
                        var pixel = new byte[4];
                        Array.Copy(bmp.ImageData, sourcePosition, pixel, 0, 4);
                        sourcePosition += 4;

                        var color = new RGBQUAD(pixel[0], pixel[1], pixel[2], pixel[3]);
                        if (uniqueColors.Contains(color))
                        {
                            colorIndex = Convert.ToByte(uniqueColors[color]);
                        }
                        else
                        {
                            if (uniqueColors.Count > byte.MaxValue)
                            {
                                throw new NotSupportedException(
                                    String.Format("The source image contains more than {0} colors.", byte.MaxValue));
                            }
                            colorIndex = Convert.ToByte(uniqueColors.Count);
                            uniqueColors.Add(color, colorIndex);
                        }
                        // store pixel as an index into the color table
                        indexedImage[i] = colorIndex;
                    }
                    // end of addition
                }
                else
                {
                    //TODO: implement converting an indexed bitmap
                    throw new NotImplementedException("Unable to convert indexed bitmaps.");
                }
            }

            ushort bitCount = GetBitCount(uniqueColors.Count);
            // *** Build Icon ***
            var ico = new IconHolder {IconDirectory = {Entries = new IconDirEntry[1]}};
            //TODO: is it really safe to assume the bitmap width/height are bytes?
            ico.IconDirectory.Entries[0].Width = (byte) bmp.Info.InfoHeader.BiWidth;
            ico.IconDirectory.Entries[0].Height = (byte) bmp.Info.InfoHeader.BiHeight;
            ico.IconDirectory.Entries[0].BitCount = bitCount; // maybe 0?
            ico.IconDirectory.Entries[0].ColorCount = (uniqueColors.Count > byte.MaxValue)
                                                          ? (byte) 0
                                                          : (byte) uniqueColors.Count;
            //HACK: safe to assume that the first imageoffset is always 22
            ico.IconDirectory.Entries[0].ImageOffset = 22;
            ico.IconDirectory.Entries[0].Planes = 0;
            ico.IconImages[0].Header.BiBitCount = bitCount;
            ico.IconImages[0].Header.BiWidth = ico.IconDirectory.Entries[0].Width;
            // height is doubled in header, to account for XOR and AND
            ico.IconImages[0].Header.BiHeight = ico.IconDirectory.Entries[0].Height << 1;
            ico.IconImages[0].XOR = new byte[ico.IconImages[0].NumBytesInXor()];
            ico.IconImages[0].AND = new byte[ico.IconImages[0].NumBytesInAnd()];
            ico.IconImages[0].Header.BiSize = 40; // always
            ico.IconImages[0].Header.BiSizeImage = (uint) ico.IconImages[0].XOR.Length;
            ico.IconImages[0].Header.BiPlanes = 1;
            ico.IconImages[0].Colors = BuildColorTable(uniqueColors, bitCount);
            //BytesInRes = biSize + colors * 4 + XOR + AND
            ico.IconDirectory.Entries[0].BytesInRes = (uint) (ico.IconImages[0].Header.BiSize
                                                              + (ico.IconImages[0].Colors.Length*4)
                                                              + ico.IconImages[0].XOR.Length
                                                              + ico.IconImages[0].AND.Length);

            // copy image data
            var bytePosXOR = 0;
            var bytePosAND = 0;
            byte transparentIndex = indexedImage[0];
            //initialize AND
            ico.IconImages[0].AND[0] = byte.MaxValue;

            int pixelsPerByte;
            int[] shiftCounts;

            switch (bitCount)
            {
                case 1:
                    pixelsPerByte = 8;
                    shiftCounts = new[] {7, 6, 5, 4, 3, 2, 1, 0};
                    break;
                case 4:
                    pixelsPerByte = 2;
                    shiftCounts = new[] {4, 0};
                    break;
                case 8:
                    pixelsPerByte = 1;
                    shiftCounts = new[] {0};
                    break;
                default:
                    throw new NotSupportedException("Bits per pixel must be 1, 4, or 8");
            }
            int bytesPerRow = ico.IconDirectory.Entries[0].Width/pixelsPerByte;
            int padBytes = bytesPerRow%4;
            if (padBytes > 0)
                padBytes = 4 - padBytes;

            sourcePosition = 0;
            for (var row = 0; row < ico.IconDirectory.Entries[0].Height; ++row)
            {
                for (var rowByte = 0; rowByte < bytesPerRow; ++rowByte)
                {
                    byte currentByte = 0;
                    for (var pixel = 0; pixel < pixelsPerByte; ++pixel)
                    {
                        byte index = indexedImage[sourcePosition++];
                        var shiftedIndex = (byte) (index << shiftCounts[pixel]);
                        currentByte |= shiftedIndex;
                    }
                    ico.IconImages[0].XOR[bytePosXOR] = currentByte;
                    ++bytePosXOR;
                }
                // make sure each scan line ends on a long boundary
                bytePosXOR += padBytes;
            }

            for (var i = 0; i < indexedImage.Length; i++)
            {
                var index = indexedImage[i];
                var bitPosAND = 128 >> (i%8);
                if (index != transparentIndex)
                    ico.IconImages[0].AND[bytePosAND] ^= (byte) bitPosAND;
                if (bitPosAND == 1)
                {
                    // need to start another byte for next pixel
                    if (bytePosAND%2 == 1)
                    {
                        //TODO: fix assumption that icon is 16px wide
                        //skip some bytes so that scanline ends on a long barrier
                        bytePosAND += 3;
                    }
                    else
                    {
                        bytePosAND += 1;
                    }
                    if (bytePosAND < ico.IconImages[0].AND.Length)
                        ico.IconImages[0].AND[bytePosAND] = byte.MaxValue;
                }
            }
            return ico;
        }

        private static ushort GetBitCount(int uniqueColorCount)
        {
            if (uniqueColorCount <= 2)
                return 1;
            
            if (uniqueColorCount <= 16)
                return 4;
            
            return (ushort) (uniqueColorCount <= 256 ? 8 : 24);
        }

        private static RGBQUAD[] BuildColorTable(Hashtable colors, ushort bpp)
        {
            //RGBQUAD[] colorTable = new RGBQUAD[colors.Count];
            //HACK: it looks like the color array needs to be the max size based on bitcount
            int numColors = 1 << bpp;
            var colorTable = new RGBQUAD[numColors];
            foreach (RGBQUAD color in colors.Keys)
            {
                int colorIndex = Convert.ToInt32(colors[color]);
                colorTable[colorIndex] = color;
            }
            return colorTable;
        }
    }
}