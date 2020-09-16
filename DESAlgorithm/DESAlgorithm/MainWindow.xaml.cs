using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DESAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<BitArray> listKey=null;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadKey_Click(object sender, RoutedEventArgs e)
        {
            string path = LoadFile();
            KeyFile.Text = path;
            listKey = Key.GenerateKey(HelpDES.GetBinaryArrayFromFile(path,8));
        }

        public BitArray ArrayToBitArray(int[] intArray)
        {
            bool[] boolArray = new bool[intArray.Length];
            for (int i = 0; i < intArray.Length; i++)
            {
                boolArray[i] = (intArray[i] == 1) ? true : false;
            }
            return new BitArray(boolArray);
        }

        private void Encipher_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_Encipher;
            worker.ProgressChanged += worker_EncipherProgressChanged;

            worker.RunWorkerAsync();
        }

        void worker_Encipher(object sender, DoWorkEventArgs e)
        {
            string path = LoadFile();
            Queue.Encipher(path, path.Replace(".bin","Encrypted.bin"), listKey,false, sender);
        }
        void worker_Decipher(object sender, DoWorkEventArgs e)
        {
            string path = LoadFile();
            Queue.Decipher(path, path.Replace("Encrypted.bin", "Decrypted.bin"), this.listKey,true, sender);
        }

        private void Decipher_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_Decipher;
            worker.ProgressChanged += worker_EncipherProgressChanged;

            worker.RunWorkerAsync();
            
        }

        private string LoadFile()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Wybierz plik";
            op.Filter = "bin files (*.bin) | *.bin| jpg files(*.jpg) | *.jpg| All files(*.*) | *.*";
            string path = "";
            if (op.ShowDialog() == true)
                path = op.FileName;
            return path;
        }

        void worker_EncipherProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status.Value = e.ProgressPercentage;
        }
    }
}
