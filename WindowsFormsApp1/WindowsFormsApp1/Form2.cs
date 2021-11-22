using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{ 
    public partial class Form2 : Form
    {
       public int Flag = 0;// флаг для переключения AES/DES функций
                           // 0- AES 1-DES
        private byte[] iv3 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
        public Form2()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Flag == 0)//AES
            {

                // Open file to Encrypt
                openFileDialog1.Title = "Select the source file to encrypt";
                openFileDialog1.Filter = "All files (*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog1.FileName;
                    button1.Enabled = true;
                }

            }

            else if (Flag == 1)//DES 
            {
                // Open file to Encrypt
                openFileDialog1.Title = "Select the source file to encrypt";
                openFileDialog1.Filter = "All files (*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog1.FileName;
                    button1.Enabled = true;
                }

            }

        }

        //private void button2_Click(object sender, EventArgs e)//мне это не нужно
        //{
        //    if (Flag == 0)//AES
        //    {
        //        // Open file to Encrypt
        //        openFileDialog1.Title = "Select the source file to decrypt";
        //        openFileDialog1.Filter = "Encrypted Files (*.enc)|*.enc";
        //
        //        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            textBox2.Text = openFileDialog1.FileName;
        //            button2.Enabled = true;
        //        }
        //    }
        //    else if (Flag == 1)//DES 
        //    {
        //        // Open file to Encrypt
        //        openFileDialog1.Title = "Select the source file to decrypt";
        //        openFileDialog1.Filter = "Encrypted Files (*.enc)|*.enc";
        //
        //        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            textBox2.Text = openFileDialog1.FileName;
        //            button2.Enabled = true;
        //        }
        //    }
        //}
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (Flag == 0)// AES
            {
                if (EncryptData(textBox1.Text, textBox1.Text + ".aesenrypt", textBox3.Text) == true)
                    MessageBox.Show(this, "Done!", "Encryption Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, "The encryption process failed!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (Flag == 1) //DES
            {
                if (EncryptData(textBox1.Text, textBox1.Text + ".desencrypt", textBox3.Text) == true)
                    MessageBox.Show(this, "Done!", "Encryption Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, "The encryption process failed!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button4_Click(object sender, EventArgs e)// здаесь заменить текстбокс со втрого на первый
        {
            if (Flag == 0)//AES
            {
                string textBox1Name = textBox1.Text.Replace(".aesenrypt", "");
                if (DecryptData(textBox1.Text, textBox1Name, textBox3.Text) == true)
                    MessageBox.Show(this, "Done!", "Decryption Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, "The decryption process failed!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (Flag == 1)//DES
            {
                string textBox1Name = textBox1.Text.Replace(".desencrypt", "");
                if (DecryptData(textBox1.Text, textBox1Name, textBox3.Text) == true)
                    MessageBox.Show(this, "Done!", "Decryption Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, "The decryption process failed!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        public bool EncryptData(String sourceFile, String destinationFile, String cryptoKey)
        {
            CryptoStream cryptoStream = null;

                try
                {
                    //Откройте файл источника и назначения, используя объект потока файлов
                    FileStream inFileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
                    FileStream outFileStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
                    outFileStream.SetLength(0);

                    //Создаем переменные, чтобы помочь с чтением и записью .
                    byte[] bin = new byte[100]; // Это промежуточное хранилище для шифрования.
                    long rdlen = 0;  // Это общее количество написанных байтов.
                    long totlen = inFileStream.Length; // Это общая длина входного файла. 
                    int len;   //Это количество байтов, которые будут написаны одновременно .

                    if (Flag == 0)//AES
                    {
                        //Создаем объект поставщика услуг AES и назначаем ключ и вектор к нему. 
                        AesManaged AESProvider = new AesManaged();

                        AESProvider.Key = ASCIIEncoding.ASCII.GetBytes(cryptoKey);
                        // Устанавливаем вектор инициализации.
                        AESProvider.IV = this.iv;
                        ICryptoTransform AESEncrypt = AESProvider.CreateEncryptor(AESProvider.Key, AESProvider.IV);

                        // Создаем класс CryptoStream и пишем зашифрованные 
                        cryptoStream = new CryptoStream(outFileStream, AESEncrypt, CryptoStreamMode.Write);
                    }
                    else if (Flag == 1)//DES
                    {
                        //Создаем объект поставщика услуг DES и назначаем ключ и вектор к нему.
                        DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider();

                        DESProvider.Key = ASCIIEncoding.ASCII.GetBytes(cryptoKey);
                        // Устанавливаем вектор инициализации.
                        DESProvider.IV = this.iv3;
                        ICryptoTransform DESEncrypt = DESProvider.CreateEncryptor(DESProvider.Key, DESProvider.IV);

                        // Создаем класс CryptoStream и пишем зашифрованные
                         cryptoStream = new CryptoStream(outFileStream, DESEncrypt, CryptoStreamMode.Write);
                    }   

                    // Читаем из входного файла,затем шифруем и записываем в выходной файл.
                    while (rdlen < totlen)
                    {
                        len = inFileStream.Read(bin, 0, 100);
                        cryptoStream.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }


                    // Закрыть обработчики потока 
                    cryptoStream.Close();
                    inFileStream.Close();
                    outFileStream.Close();
                    return true;
                }

                catch (Exception e)
                {
                    MessageBox.Show(this, e.ToString(), "Encryption Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                      
        }


        // The DecryptData method will decrypt the given file using the AES algorithm
        public bool DecryptData(String sourceFile, String destinationFile, String cryptoKey)
        {
            CryptoStream cryptoStream = null;
                try
                {
                    // Откройте файл источника и назначения, используя объект потока файлов
                    FileStream inFileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
                    FileStream outFileStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
                    outFileStream.SetLength(0);

                    //Создаем переменные, чтобы помочь с чтением и записью .
                    byte[] bin = new byte[100]; // Это промежуточное хранилище для расшифрования.
                    long rdlen = 0;  // Это общее количество написанных байтов.
                    long totlen = inFileStream.Length; // Это общая длина входного файла. 
                    int len;   //Это количество байтов, которые будут написаны одновременно .

                if (Flag == 0)
                {
                    //Создаем объект поставщика услуг AES и назначаем ключ и вектор к нему.
                    AesManaged AESProvider = new AesManaged();
                    AESProvider.Key = ASCIIEncoding.ASCII.GetBytes(cryptoKey);
                    // Устанавливаем вектор инициализации.
                    AESProvider.IV = this.iv;
                    ICryptoTransform AESDecrypt = AESProvider.CreateDecryptor(AESProvider.Key, AESProvider.IV);

                     cryptoStream = new CryptoStream(outFileStream, AESDecrypt, CryptoStreamMode.Write);
                }
                else if (Flag == 1) 
                {
                    //Создаем объект поставщика услуг DES и назначаем ключ и вектор к нему.
                    DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider();
                    DESProvider.Key = ASCIIEncoding.ASCII.GetBytes(cryptoKey);
                    // Устанавливаем вектор инициализации.
                    DESProvider.IV = this.iv3;
                    ICryptoTransform DESDecrypt = DESProvider.CreateDecryptor(DESProvider.Key, DESProvider.IV);

                     cryptoStream = new CryptoStream(outFileStream, DESDecrypt, CryptoStreamMode.Write);
                }

                    //CryptoStream cryptoStream = new CryptoStream(outFileStream, AESDecrypt, CryptoStreamMode.Write);

                    // Читаем из входного файла,затем шифруем и записываем в выходной файл.
                    while (rdlen < totlen)
                    {
                        len = inFileStream.Read(bin, 0, 100);
                        cryptoStream.Write(bin, 0, len);
                        rdlen = rdlen + len;
                    }

                    // Закрыть обработчики потока 
                    cryptoStream.Close();
                    inFileStream.Close();
                    outFileStream.Close();
                    return true;
                }

                catch (Exception e)
                {
                    MessageBox.Show(this, e.ToString(), "Decryption Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

            


        }

        private void dESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flag = 1;// DES
            textBox3.Text = "TfG5H6ip";
        }

        private void aESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flag = 0;// AES
            textBox3.Text = "awsPay165FcfyTf6";
        }
    }
}
