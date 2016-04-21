using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class Token
    {
        enum TokenType
        {
            KEYWORD,
            SYMBOL,
            IDENTIFIER,
            INT_CONST,
            STRING_CONST
        }
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
        public List<string> _output;
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
            for(int i = start; i < s.Length; i++)
            {
                /*sb.Append(s[i]);
                if(sb.ToString() == "//")
                {
                    ParseLineComment(s, start + i+1, out i);
                }*/

                //We see a letter character
                if (char.IsLetter(s[i]))
                {
                    ParseWord(s, start, out i);
                }
                //We see a number character
                else if (char.IsNumber(s[i]))
                {
                    ParseNumber(s, start,out i);
                }
                else
                {
                    ParseSymbol(s, start, out i);
                }
            }
        }

        private void ParseSymbol(string s, int start, out int i)
        {
            i = start + 1;
        }

        private void ParseNumber(string s, int start,out int i)
        {
            i = start + 1;
        }

        private void ParseWord(string s, int start,out int i)
        {
            i = start + 1;
            if(s[start] == '/' && s[start+1] == '/')
            {
                ConsumeUntilNewline(s,start+2,out i);
            }
        }

        private void ConsumeUntilNewline(string s, int start,out int end)
        {
            end = start;
            while (end < s.Length && s[end] != '\n' && s[end] != '\n')
            {
                end++;
            }
            end++;
        }
    }
}