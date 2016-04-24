using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Compiler
{
    class Lexeme
    {
        public LexemeType type { get; }
        public string value { get; }

        public Lexeme(string num, LexemeType iNT_CONST)
        {
            value = num;
            type = iNT_CONST;
        }

        public enum LexemeType
        {
            keyword,
            identifier,
            symbol,
            integerConstant,
            stringConstant
        }
        public override string ToString()
        {
            return type + " " + value;
        }
    }
    class Token
    {
        enum KeyWordType
        {
            CLASS,
            METHOD,
            FUNCTION,
            CONSTRUCTOR,
            INT,
            BOOLEAN,
            CHAR,
            VOID,
            VAR,
            STATIC,
            FIELD,
            LET,
            DO,
            IF,
            ELSE,
            WHILE,
            RETURN,
            TRUE,
            FALSE,
            NULL,
            THIS
        }
    }
    internal class Tokenizer
    {
        private string originalFile { get; set; }
        public List<Lexeme> _output;
        public XmlDocument tokenDoc { get; private set; }
        public List<string> _keywords = new List<string>()
        {
            "class",
            "constructor",
            "function",
            "method",
            "field",
            "static",
            "var",
            "int",
            "char",
            "boolean",
            "void",
            "true",
            "false",
            "null",
            "this",
            "let",
            "do",
            "if",
            "else",
            "while",
            "return"
        };
        public Tokenizer()
        {
            _output = new List<Lexeme>();
        }
        internal void Process(string text)
        {
            originalFile = text;
            _output.Clear();
            //string[] tmpLines = originalFile.Split(new string[] { "\r","\n"},StringSplitOptions.RemoveEmptyEntries);
            //foreach(string s in tmpLines)
            //{
            ParseLine(text,0);
            //}
        }

        private void ParseLine(string s,int start)
        {
            for(int i = start; i < s.Length;)
            {
                //We see a letter character
                char c = s[i];
                if (char.IsLetter(s[i]))
                {
                    string word = ParseWord(s, i, out i);
                    Lexeme.LexemeType type = Lexeme.LexemeType.identifier;
                    if (isKeyWord(word))
                    {
                        type = Lexeme.LexemeType.keyword;
                    }
                    _output.Add(new Lexeme(word, type));
                }
                //We see a number character
                else if (char.IsNumber(s[i]))
                {
                    string num = ParseNumber(s, i,out i);
                    _output.Add(new Lexeme(num, Lexeme.LexemeType.integerConstant));
                }
                //We see a quote
                else if(s[i] == '"')
                {
                    string word = ParseStringConstant(s, i, out i);
                    _output.Add(new Lexeme(word, Lexeme.LexemeType.stringConstant));
                }
                else
                {
                    string symbol = ParseSymbol(s, i, out i);
                    if(!string.IsNullOrEmpty(symbol))
                    {
                        _output.Add(new Lexeme(symbol, Lexeme.LexemeType.symbol));
                    }
                }
            }
        }

        private bool isKeyWord(string word)
        {
            if (_keywords.Contains(word))
                return true;
            return false;
        }

        private string ParseSymbol(string s, int start, out int i)
        {
            i = start + 1;
            //Line comment
            if (s[i - 1] == '/' && s[i] == '/')
            {
                ConsumeUntilNewline(s, i + 1, out i);
                return "";
            }
            //Block Comment
            else if(s[i-1] == '/' && s[i] == '*')
            {
                ConsumeUntilEndOfComment(s, i + 1, out i);
                return "";
            }
            //Whitespacing
            else if(char.IsWhiteSpace(s[start]))
            {
                return "";
            }
            else
            {
                return s.Substring(i-1, 1);
            }
        }

        private void ConsumeUntilEndOfComment(string s, int v, out int i)
        {
            i = v;
            while(!(s[i] == '*' && s[i+1] == '/'))
            {
                i++;
            }
            i+=2;
        }

        private string ParseNumber(string s, int start,out int i)
        {
            StringBuilder sb = new StringBuilder();
            i = start;
            while(i < s.Length && char.IsNumber(s[i]))
            {
                sb.Append(s[i]);
                i++;
            }
            return sb.ToString();
        }

        private string ParseWord(string s, int start,out int i)
        {
            StringBuilder sb = new StringBuilder();
            i = start;
            while (i < s.Length && char.IsLetter(s[i]))
            {
                sb.Append(s[i]);
                i++;
            }
            return sb.ToString();
        }

        private string ParseStringConstant(string s, int start, out int i)
        {
            i = start+1;
            while(i < s.Length && s[i] != '"')
            {
                i++;
            }
            string ret = s.Substring(start+1, i - start -1);
            i++;
            return ret;
        }

        private string ConsumeUntilNewline(string s, int start,out int end)
        {
            end = start;
            StringBuilder sb = new StringBuilder();
            sb.Append(s[end]);
            while (end < s.Length && s[end] != '\r' && s[end] != '\n')
            {
                sb.Append(s[end]);
                end++;
            }
            end++;
            return sb.ToString();
        }

        public string FormattedTokenString()
        {
            string ret = "";
            foreach(Lexeme s in _output)
            {
                ret += s.ToString()+Environment.NewLine;
            }
            return ret;
        }

        internal void SaveToXML(string tokenOutput)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement root = xDoc.CreateElement("tokens");
            foreach(Lexeme l in _output)
            {
                XmlElement xNode = xDoc.CreateElement(l.type.ToString());
                xNode.InnerText = " "+l.value+" ";
                root.AppendChild(xNode);
            }
            xDoc.AppendChild(root);
            tokenDoc = xDoc;
            xDoc.Save(tokenOutput);
        }
    }
}