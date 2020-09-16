using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAlgorithm
{
    public static class Key
    {
        private static readonly int[] LEFT_SHIFTS_ARRAY = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
        private static readonly int[] PC2 = {14 ,17 ,11 ,24 ,1 ,5,
                                            3 ,28, 15 ,6 ,21 ,10,
                                            23 ,19 ,12 ,4 ,26 ,8,
                                            16 ,7 ,27 ,20 ,13 ,2,
                                            41, 52 ,31 ,37 ,47 ,55,
                                            30 ,40 ,51 ,45 ,33 ,48,
                                            44 ,49 ,39 ,56 ,34 ,53,
                                            46 ,42 ,50 ,36 ,29 ,32};

        private static readonly int[] PC1 = {57, 49 ,41 ,33 ,25 ,17 ,9,
                                            1, 58 ,50 ,42 ,34 ,26 ,18,
                                            10 ,2 ,59 ,51 ,43 ,35 ,27,
                                            19 ,11 ,3 ,60 ,52 ,44 ,36,
                                            63 ,55 ,47 ,39 ,31 ,23 ,15,
                                            7, 62 ,54 ,46 ,38 ,30 ,22,
                                            14 ,6 ,61 ,53 ,45 ,37 ,29,
                                            21 ,13 ,5 ,28 ,20 ,12 ,4                                            };
        public static List<BitArray> GenerateKey(BitArray keyIn)
        {
            BitArray cArray=new BitArray(28);
            BitArray dArray = new BitArray(28);
            List<BitArray> keyOut = new List<BitArray>();
            keyIn=HelpDES.Permute(keyIn, PC1, 56);   //teraz ma rozmiar 56
            HelpDES.CutInHalf(keyIn, cArray, dArray);
            for(int i=0;i<16;i++)
            {
                LeftShifts(cArray, LEFT_SHIFTS_ARRAY[i]);
                LeftShifts(dArray, LEFT_SHIFTS_ARRAY[i]);
                keyOut.Add(HelpDES.Permute(HelpDES.Connect(cArray, dArray),PC2,48));
            }
            return keyOut;
        }
 
        
        /// <summary>
        /// Metoda przesuwa ciąg bitów w BitArray o zadaną liczbę w lewo.
        /// </summary>
        /// <param name="bitArray">Tablica bitów do przesunięcia</param>
        /// <param name="numberOfLeftShifts">Ilość przesunięć</param>
        public static void LeftShifts(BitArray bitArray, int numberOfLeftShifts)
        {
            for (int n = 0; n < numberOfLeftShifts; n++)
            {
                bool bit1 = bitArray.Get(bitArray.Length - 1);
                bool bit2;
                for (int i = bitArray.Length - 1; i >= 0; i--)
                {
                    if (i == 0)
                    { bitArray.Set(bitArray.Length - 1, bit1); }
                    else
                    {
                        bit2 = bitArray.Get(i - 1);
                        bitArray.Set(i - 1, bit1);
                        bit1 = bit2;
                    }
                }
            }
        }

    }
}
