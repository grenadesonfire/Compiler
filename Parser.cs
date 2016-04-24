using System;
using System.Collections.Generic;
using System.Xml;

namespace Compiler
{
    class ProgToken
    {
        Lexeme.LexemeType type;
        string innerText;
        public ProgToken(Lexeme lex)
        {
            innerText = lex.value;
            type = lex.type;
        }
    }
    internal class Parser
    {
        List<Lexeme> _lexemes;
        List<ProgToken> _parsedoutput;
        XmlDocument output;
        XmlElement _parent;
        internal void TransferTokens(List<Lexeme> _output)
        {
            _lexemes = new List<Lexeme>(_output);
            _parsedoutput = new List<ProgToken>();
        }

        internal void Parse()
        {
            output = new XmlDocument();
            Class();
            output.AppendChild(_parent);
        }
        #region PROGRAM_STRUCTURE
        private void Class()
        {
            _parent = output.CreateElement("class");
            //'class'
            Consume();
            //className
            Consume();
            //'{'
            Consume();
            //classVarDec*
            while (Peek().value == "static" ||
                Peek().value == "field")
            {
                ClassVarDec();
            }
            //subroutineDec*
            while (Peek().value == "constructor" ||
                Peek().value == "function" ||
                Peek().value == "method")
            {
                SubroutineDec();
            }
            //'}'
            Consume();
            output.AppendChild(_parent);
        }
        private void ClassVarDec()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("classVarDec");
            //(static | field)
            Consume();
            //type
            Type();
            //varName
            VarName();
            //(',', varName)*
            while (Peek().value == ",")
            {
                //','
                Consume();
                //varName
                VarName();
            }
            //';'
            Consume();
            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void Type()
        {
            Consume();
        }
        private void SubroutineDec()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("subroutineDec");

            //('constructor' | 'function' | 'method')
            Consume();
            //('void' | type )
            if(Peek().value == "void")
            {
                Consume();
            }
            else
            {
                Type();
            }
            //subroutineName
            Consume();
            //'('
            Consume();
            //parameterList
            ParameterList();
            //')'
            Consume();
            //subroutineBody
            SubroutineBody();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void ParameterList()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("parameterList");

            string firstTok = Peek().value;
            if (!isType())
            {
                myParent.AppendChild(_parent);
                _parent = myParent;
                return;
            }

            //type
            Type();
            //varName
            VarName();
            while (Peek().value == ",")
            {
                //','
                Consume();
                //type
                Type();
                //varName
                VarName();
            }
            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void SubroutineBody()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("subroutineBody");
            //'{'
            Consume();
            //varDec*
            while (Peek().value == "var")
            {
                VarDec();
            }
            //statements
            Statements();
            //'}'
            Consume();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void VarDec()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("varDec");

            //'var'
            Consume();
            //type
            Type();
            //varName
            VarName();
            //(',', varName)*
            while(Peek().value == ",")
            {
                //','
                Consume();
                //varName
                VarName();
            }
            //';'
            Consume();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void VarName()
        {
            Consume();
        }
        #endregion
        #region STATEMENTS
        private void Statements()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("statements");
            while (isStatement())
            {
                Statement();
            }

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void Statement()
        {
            Lexeme start = Peek();
            switch (start.value)
            {
                case "let":
                    LetStatement();
                    break;
                case "if":
                    IfStatement();
                    break;
                case "while":
                    WhileStatement();
                    break;
                case "do":
                    DoStatement();
                    break;
                case "return":
                    ReturnStatement();
                    break;
            }
        }
        private void LetStatement()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("letStatement");
            //'let'
            Consume();
            //varName
            VarName();
            if(Peek().value == "[")
            {
                //'['
                Consume();
                //expression
                Expression();
                //']'
                Consume();
            }
            //'='
            Consume();
            //expression
            Expression();
            //';'
            Consume();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void IfStatement()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("ifStatement");
            //'if'
            Consume();
            //'('
            Consume();
            //expression
            Expression();
            //')'
            Consume();
            //'{'
            Consume();
            //statements
            Statements();
            //'}'
            Consume();
            if(Peek().value == "else")
            {
                //'else'
                Consume();
                //'{'
                Consume();
                //statements
                Statements();
                //'}'
            }

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void WhileStatement()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("whileStatement");

            //'while'
            Consume();
            //'('
            Consume();
            //expression
            Expression();
            //')'
            Consume();
            //'{'
            Consume();
            //statements
            Statements();
            //'}'
            Consume();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void DoStatement()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("doStatement");

            //'do'
            Consume();
            //subroutineCall
            SubRoutineCall();
            //';'
            Consume();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void ReturnStatement()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("returnStatement");

            //'return'
            Consume();
            //expression?
            if (isExpression())
            {
                Expression();
            }
            //';'
            Consume();

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        #endregion STATEMENTS
        #region EXPRESSION
        private void Expression()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("expression");

            //term
            Term();
            while (isOp())
            {
                //op
                Consume();
                //term
                Term();
            }

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void Term()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("term");

            Lexeme top = Peek();
            Lexeme next = Peek(1);
            if(top.type == Lexeme.LexemeType.integerConstant)
            {
                Consume();
            }
            else if(top.type == Lexeme.LexemeType.stringConstant)
            {
                Consume();
            }
            else if(isKeyWordConstant())
            {
                Consume();
            }
            //varname '[' expression ']'
            else if(next.value == "[")
            {
                //varName
                VarName();
                //'['
                Consume();
                //expression
                Expression();
                //']'
                Consume();
            }
            //subroutineCall
            else if(top.type == Lexeme.LexemeType.identifier && 
                next.value == ".")
            {
                SubRoutineCall();
            }
            else if(top.value == "(")
            {
                //'('
                Consume();
                //expression
                Expression();
                //')'
                Consume();
            }
            else if (isUnaryOp())
            {
                // '-' | '~'
                Consume();
                //term
                Term();
            }
            //varName
            else
            {
                VarName();
            }

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        private void SubRoutineCall()
        {
            Lexeme next = Peek(1);
            if(next.value == "(")
            {
                //subroutineName
                Consume();
                //'('
                Consume();
                //expressionList
                ExpressionList();
                //')'
                Consume();
            }
            else if(next.value == ".")
            {
                //(className | varName)
                Consume();
                //'.'
                Consume();
                //subroutineName
                Consume();
                //'('
                Consume();
                //expressionList
                ExpressionList();
                //')'
                Consume();
            }
        }
        private void ExpressionList()
        {
            XmlElement myParent = _parent;
            _parent = output.CreateElement("expressionList");
            if (isExpression())
            {
                //expression
                Expression();
                while(Peek().value == ",")
                {
                    //','
                    Consume();
                    //expression
                    Expression();
                }
            }

            myParent.AppendChild(_parent);
            _parent = myParent;
        }
        #endregion
        #region Lexeme Check Functions
        private bool isExpression()
        {
            return isTerm();
        }
        private bool isTerm()
        {
            Lexeme top = Peek();
            return top.type == Lexeme.LexemeType.integerConstant ||
                top.type == Lexeme.LexemeType.stringConstant ||
                isKeyWordConstant() ||
                isUnaryOp() ||
                top.type == Lexeme.LexemeType.identifier ||
                top.value == "(";
        }
        private bool isUnaryOp()
        {
            return Peek().value == "~" ||
                Peek().value == "-";
        }
        private bool isKeyWordConstant()
        {
            return isKeyWordConstant(Peek());
        }
        private bool isKeyWordConstant(Lexeme top)
        {
            return top.value == "true" ||
                top.value == "false" ||
                top.value == "null" ||
                top.value == "this";
        }
        private bool isOp()
        {
            switch (Peek().value)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "&":
                case "|":
                case "<":
                case ">":
                case "=":
                    return true;
                default:
                    return false;
            }
        }
        private bool isType()
        {
            return Peek().value == "int" ||
                Peek().value == "char" ||
                Peek().value == "boolean" ||
                Peek().type == Lexeme.LexemeType.identifier;
        }
        private bool isStatement()
        {
            return Peek().value == "let" ||
                Peek().value == "if" ||
                Peek().value == "while" ||
                Peek().value == "do" ||
                Peek().value == "return";
        }
        #endregion
        #region LEXEME_MANAGEMENT
        private void Consume()
        {
            Lexeme lex = GetNextLexeme();

            XmlElement xElement = output.CreateElement(lex.type.ToString());
            xElement.InnerText = lex.value;
            _parent.AppendChild(xElement);

            _parsedoutput.Add(new ProgToken(lex));
            
        }
        private Lexeme Peek(int index = 0)
        {
            return _lexemes[index];
        }
        #endregion
        internal void SaveToXML(string tokenOutput)
        {
            output.Save(tokenOutput);
        }

        private Lexeme GetNextLexeme()
        {
            Lexeme ret = _lexemes[0];
            _lexemes.RemoveAt(0);
            return ret;
        }

        public string GetXML()
        {
            return output.OuterXml;
        }
    }
}