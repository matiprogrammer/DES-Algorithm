using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAlgorithm
{
    public static class Function
    {
        private static readonly int[] E_BIT_SELECTION_TABLE = { 32, 1 ,2 ,3 ,4 ,5,
                                                                4 ,5 ,6 ,7 ,8 ,9,
                                                                8 ,9 ,10 ,11 ,12 ,13,
                                                                12 ,13 ,14 ,15 ,16 ,17,
                                                                16 ,17 ,18 ,19 ,20 ,21,
                                                                20 ,21 ,22 ,23 ,24 ,25,
                                                                24 ,25 ,26 ,27 ,28 ,29,
                                                                28 ,29 ,30 ,31 ,32 ,1};
        private static readonly int[] P_TABLE = {16,7,20,21,
                                                29,12,28,17,
                                                1,15,23,26,
                                                5,18,31,10,
                                                2,8,24,14,
                                                32,27,3,9,
                                                19,13,30,6,
                                                22,11,4,25};

        /// <summary>
        /// Głowna funkcja f
        /// </summary>
        /// <param name="bitArray">32-bitowa tablica</param>
        /// <returns></returns>
        public static BitArray FFunction(BitArray bitsR, BitArray bitsK)
        {
            List<int[,]> sBoxes = new List<int[,]> { SBOX.S1, SBOX.S2, SBOX.S3, SBOX.S4, SBOX.S5, SBOX.S6, SBOX.S7, SBOX.S8 };
            BitArray array= EFunction(bitsR);
            BitArray array2 = new BitArray(0);
            array.Xor(bitsK);
            for(int i=0;i<8;i++)
            {
                   array2= Add(array2, SFunction(new BitArray(new bool[] { array[ (i * 6)], array[ 1 + (i * 6)], array[ 2 + (i * 6)], array[ 3 + (i * 6)], array[ 4 + (i * 6)], array[5 + (i * 6)] }), sBoxes[i]));
            }
            return PFunction(array2);
        }
        /// <summary>
        /// Dodaje 2 tablice i zwraca rezultat
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="toAdd"></param>
        /// <returns></returns>
        public static BitArray Add(BitArray dest, BitArray toAdd)
        {
            BitArray bitsOut = new BitArray(dest.Length + toAdd.Length);

            for(int i=0;i<dest.Length;i++)
            {
                bitsOut[i] = dest[i];
            }
            for(int i=0;i<toAdd.Length; i++)
            {
                bitsOut[i + dest.Length] = toAdd[i];
            }
            return bitsOut;
        }
        /// <summary>
        /// Wykonuje funkcję E na tablicy bitów
        /// </summary>
        /// <param name="bitArray"> 32-bitowa tablica BitArray</param>
        /// <returns> Zwraca 48-bitową talice</returns>
        public static BitArray EFunction(BitArray bitArray) 
        {
            BitArray outArray = new BitArray(48);
            for(int i=0;i<outArray.Length;i++)
            {
                outArray[i] = bitArray[E_BIT_SELECTION_TABLE[i]-1];
            }
            return outArray;
        }
        /// <summary>
        /// Metoda przyjmuje 6-bitowa tablice i zwraca 4-bitową
        /// </summary>
        /// <param name="bitarray"></param>
        /// <param name="sBox"></param>
        /// <returns></returns>
        public static BitArray SFunction(BitArray bitarray, int[,] sBox)  //UWAGA: najmniej znaczacy bit na koncu tabicy
        {
            BitArray rowBits = new BitArray(new bool[] { bitarray[0], bitarray[5] });
            BitArray columnBits = new BitArray(new bool[] { bitarray[1], bitarray[2], bitarray[3], bitarray[4] });
            
            return NumberToBinary(sBox[BinaryToNumber(rowBits), BinaryToNumber(columnBits)],4); 
        }
        /// <summary>
        /// Przyjmuje 32-bitową tablice, robi permutacje i zwraca 32-bitowa tablice.
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public static BitArray PFunction(BitArray bitArray)
        {
            BitArray outArray = new BitArray(32);
            for (int i = 0; i < outArray.Length; i++)
            {
                outArray[i] = bitArray[P_TABLE[i] - 1];
            }
            return outArray;
        }
        public static int BinaryToNumber(BitArray bits)
        {
            int number = 0;
            for (int i = bits.Length - 1; i >= 0; i--)
            {
                number += (bits[i]) ? (int)Math.Pow(2, (bits.Length - 1) - i) : 0;
            }
            return number;
        }
        /// <summary>
        /// Zwraca tablice bitów o długości okreslonej w parametrze.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static BitArray NumberToBinary (int number, int length)
        {
            BitArray b= new BitArray(new int[] { number });
            HelpDES.Reverse(b);
           BitArray outArray = new BitArray(length);
            for(int i=0;i< length; i++)
            {
                outArray[i] = b[(b.Length-length) + i];
            }
            return outArray;
        }
    }
}
