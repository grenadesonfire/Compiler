using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class Lexeme
    {
        private LexemeType type { get; }
        private string value { get; }

        public Lexeme(string num, LexemeType iNT_CONST)
        {
            value = num;
            type = iNT_CONST;
        }

        public enum LexemeType
        {
            KEYWORD,
            SYMBOL,
            IDENTIFIER,
            INT_CONST,
            STRING_CONST
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
                /*sb.Append(s[i]);
                if(sb.ToString() == "//")
                {
                    ParseLineComment(s, start + i+1, out i);
                }*/

                //We see a letter character
                char c = s[i];
                if (char.IsLetter(s[i]))
                {
                    string word = ParseWord(s, i, out i);
                    _output.Add(new Lexeme(word, Lexeme.LexemeType.IDENTIFIER));
                }
                //We see a number character
                else if (char.IsNumber(s[i]))
                {
                    string num = ParseNumber(s, i,out i);
                    _output.Add(new Lexeme(num, Lexeme.LexemeType.INT_CONST));
                }
                else
                {
                    string symbol = ParseSymbol(s, i, out i);
                    if(!string.IsNullOrEmpty(symbol))
                    {
                        _output.Add(new Lexeme(symbol, Lexeme.LexemeType.KEYWORD));
                    }
                }
            }
        }

        private string ParseSymbol(string s, int start, out int i)
        {
            i = start + 1;
            //Line comment
            if (s[start - 1] == '/' && s[start] == '/')
            {
                ConsumeUntilNewline(s, start + 2, out i);
                return "";
            }
            //Block Comment
            else if(s[start-1] == '/' && s[start] == '*')
            {
                ConsumeUntilEndOfComment(s, start + 2, out i);
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
            i++;
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
    }
}