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
using System.Xml;

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
        Parser _parser;

        private FileInfo jackProgram;

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
                jackProgram = new FileInfo(ofd.FileName);
                JackFile_TextBox.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void SaveTokens_Click(object sender, RoutedEventArgs e)
        {
            string tokenOutput = jackProgram.FullName.Substring(
                0,
                jackProgram.FullName.Length-5)+"T_.xml";

            _tokenizer.SaveToXML(tokenOutput);
        }

        private void Tokenize_Click(object sender, RoutedEventArgs e)
        {
            _tokenizer.Process(JackFile_TextBox.Text);
            Token_TextBox.Text = _tokenizer.FormattedTokenString();
        }

        private void Parse_Click(object sender, RoutedEventArgs e)
        {
            _parser = new Parser();
            _parser.TransferTokens(_tokenizer._output);
            _parser.Parse();
            string tokenOutput = jackProgram.FullName.Substring(
                0,
                jackProgram.FullName.Length - 5) + "_.xml";
            _parser.SaveToXML(tokenOutput);

            Parsed_TextBox.Text = FormatXML(_parser.GetXML());
        }

        private string FormatXML(string xml)
        {
            //http://stackoverflow.com/questions/1702893/display-xml-in-a-wpf-textbox
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var stringBuilder = new StringBuilder();
            var xmlWriterSettings = new XmlWriterSettings
            { Indent = true, OmitXmlDeclaration = true };
            doc.Save(XmlWriter.Create(stringBuilder, xmlWriterSettings));
            return stringBuilder.ToString();
        }
    }
}
