using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace USC.GISResearchLab.Common.Core.IOs.Writers
{
    public class BinaryFileWriter
    {
        public static void WriteByteArrayToFile(byte[] byteImage, string fileLocation)
        {
            using (BinaryWriter binWriter =
            new BinaryWriter(File.Open(fileLocation, FileMode.Create)))
            {
                binWriter.Write(byteImage);
            }
        }

        // Convert an object to a byte array // from http://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
    }

}
