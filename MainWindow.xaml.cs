using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Compiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Wonderfully simple Logging.
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Processing elements
        Tokenizer _tokenizer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeProcessingElements();
        }

        private void InitializeProcessingElements()
        {
            _tokenizer = new Tokenizer();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Jack Files | *.jack";
            if (ofd.ShowDialog() == true)
            {
                JackFile_TextBox.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void Tokenize_Click(object sender, RoutedEventArgs e)
        {
            _tokenizer.Process(JackFile_TextBox.Text);
        }
    }
}
