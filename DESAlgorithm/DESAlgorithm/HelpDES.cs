using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAlgorithm
{

    public static class HelpDES
    {

        /// <summary>
        /// Metoda wykonuje permutacje.
        /// </summary>
        /// <param name="bitArray">Tablica bitów do permutacji</param>
        /// <param name="permutedChoiceArray">Tablica na podstawie której jest wykonywana permutacja</param>
        /// <param name="length">Dlugość tablicy po premutacji</param>
        public static BitArray Permute(BitArray bitArray, int[] permutedChoiceArray, int length)
        {
            BitArray permutedBitArray = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                permutedBitArray[i] = bitArray[permutedChoiceArray[i] - 1];
            }
            return permutedBitArray;
        }

        /// <summary>
        /// Metoda odwraca kolejność bitów w tablicy.
        /// </summary>
        /// <param name="array"></param>
        public static void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        public static void Reverse(List<BitArray> list)
        {
            int length = list.Count;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                BitArray bits = list[i];
                list[i] = list[length - i - 1];
                list[length - i - 1] = bits;
            }
        }


        /// <summary>
        /// Metoda zwraca 64 bitowy klucz z BitArray w postaci tablicy intów.
        /// </summary>
        /// <param name="bitArray">Tablica bitów, z której zapisany jest klucz</param>
        /// <returns></returns>
        private static int[] GetArrayFromBinary(BitArray bitArray, int length)
        {
            int i = 0;
            int[] key = new int[length];
            foreach (var bit in bitArray)
                key[i++] = (bool)bit ? 1 : 0;
            return key;
        }

        /// <summary>
        /// Metoda tnie tablicę w połowie i zapisuje w dwóch tablicach
        /// </summary>
        /// <param name="sourceArray">Tablica do przecięcia</param>
        /// <param name="cArray"></param>
        /// <param name="dArray"></param>
        public static void CutInHalf(BitArray sourceArray, BitArray cArray, BitArray dArray)
        {
            for (int i = 0; i < sourceArray.Length / 2; i++)
            {
                cArray[i] = sourceArray[i];
                dArray[i] = sourceArray[i + sourceArray.Length / 2];
            }
        }

        /// <summary>
        /// Metoda łaczy dwie tablice
        /// </summary>
        /// <param name="cArray"></param>
        /// <param name="dArray"></param>
        /// <returns></returns>
        public static BitArray Connect(BitArray cArray, BitArray dArray)
        {
            BitArray outArray = new BitArray(cArray.Length * 2);
            for (int i = 0; i < cArray.Length; i++)
            {
                outArray[i] = cArray[i];
                outArray[i + cArray.Length] = dArray[i];
            }
            return outArray;
        }

        public static BitArray GetBinaryArrayFromFile(string path, int _byte)
        {
            BinaryReader binaryReader;
            try
            {
                binaryReader = new BinaryReader(new FileStream(path, FileMode.Open));
                byte[] readsByte = new byte[_byte];
                BitArray bitArray;
                ReadBytes(binaryReader, ref readsByte, false);
                
                byte[] readsByteReverse = new byte[8];
                for(int i=0;i<8;i++)
                { readsByteReverse[i] = readsByte[7 - i]; }
                bitArray = new BitArray(readsByteReverse);
                Reverse(bitArray);
                binaryReader.Close();
                return bitArray;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        /// <summary>
        /// Metoda czyta readsByte.Length bajtów z BinaryReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="readsByte">Tablica, do której zapisywane są przeczytane bajty. </param>
        /// <returns></returns>
        public static int ReadBytes(BinaryReader reader, ref byte[] readsByte, bool isDechiper)
        {
            int count;
            count = reader.Read(readsByte, 0, readsByte.Length);
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                

                BitArray padding = Function.NumberToBinary(8 - count, 8);
                Reverse(padding);
                for (int i = count; i < 8; i++)
                {
                    readsByte[i] = BitArrayToByteArray(padding)[0];
                }

                if (reader.BaseStream.Length % 8 == 0 && isDechiper) 
                {
                    return -1;
                }
                if (reader.BaseStream.Length % 8 == 0 && count == 0 && !isDechiper)
                {
                    count = -1;
                }

                
            }
            return count;

        }

        public static void WriteBytes(BinaryWriter writer, BitArray bitArray)
        {
            byte[] array = BitArrayToByteArray(bitArray);
            byte[] readsByteReverse = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            { readsByteReverse[i] = array[(array.Length-1)-i]; }
            writer.Write(readsByteReverse);
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}
