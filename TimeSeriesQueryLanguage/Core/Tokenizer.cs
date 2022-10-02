using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;

namespace TimeSeriesQueryLanguage.Core
{
    public class Tokenizer<TAggFn,TAggCl> where TAggFn : Enum where TAggCl : Enum
    {
        char[] _MathQualifiers = new char[] { '-', '.' };
        char[] _Operations = new char[] { '*', '/', '+', '<', '>', '.', '&', '|' };

        readonly string _Command;
        char _CurrentChar;
        int _CommandCharIndex = 0;

        public Tokenizer(string command)
        {
            _Command = command;
            NextChar();
            NextToken();
        }

        public Token Token;
        public decimal Number = 0;
        public TAggFn? AggFn = default;
        public TAggCl? AggCl = default;
        public AggTs AggTsSlideTo;
        public AggTs AggTsFrame;

        public void NextChar()
        {
            _CurrentChar = _CommandCharIndex < _Command.Length ? _Command[_CommandCharIndex] : '\0';
            _CommandCharIndex++;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(_CurrentChar))
            {
                NextChar();
            }

            if (_CurrentChar == '\0')
            {
                Token = Token.EOF;
            }
            else if (_CurrentChar == '(')
            {
                Token = Token.OpenParens;
                NextChar();
            }
            else if (_CurrentChar == ')')
            {
                Token = Token.CloseParens;
                NextChar();
            }
            else if (_CurrentChar == ',')
            {
                Token = Token.Comma;
                NextChar();
            }
            else if (char.IsDigit(_CurrentChar) || _MathQualifiers.Contains(_CurrentChar))
            {
                var sb = new StringBuilder();
                while (char.IsDigit(_CurrentChar) || _MathQualifiers.Contains(_CurrentChar))
                {
                    sb.Append(_CurrentChar);
                    NextChar();
                }
                Token = Token.Number; Number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
            }
            else if (char.IsLetter(_CurrentChar) || _Operations.Contains(_CurrentChar))
            {
                var sb = new StringBuilder();
                while (char.IsLetterOrDigit(_CurrentChar) || _Operations.Contains(_CurrentChar))
                {
                    sb.Append(_CurrentChar);
                    NextChar();
                }
                var sbs = sb.ToString();
                var dot = sbs.IndexOf(".");
                if (sbs == "*")
                {
                    Token = Token.Mult;
                }
                else if (sbs == "/")
                {
                    Token = Token.Div;
                }
                else if (sbs == "+")
                {
                    Token = Token.Add;
                }
                else if (sbs == "&")
                {
                    Token = Token.And;
                }
                else if (sbs == "|")
                {
                    Token = Token.Or;
                }
                else if (sbs == "ag")
                {
                    Token = Token.Agg;
                }
                else if (sbs == "tid")
                {
                    Token = Token.Tid;
                }
                else if (sbs == "fid")
                {
                    Token = Token.FId;
                }
                else if (sbs == ">")
                {
                    Token = Token.V1mV2;
                }
                else if (sbs == "<")
                {
                    Token = Token.V1lV2;
                }
                else if (sbs == "sc")
                {
                    Token = Token.Scale;
                }
                else if (sbs == "in")
                {
                    Token = Token.V1inV2V3;
                }
                else if (Enum.GetNames(typeof(TAggFn)).Contains(sbs))
                {
                    Token = Token.AggFn; AggFn = (TAggFn)Enum.Parse(typeof(TAggFn), sbs);
                }
                else if (Enum.GetNames(typeof(TAggCl)).Contains(sbs))
                {
                    Token = Token.AggCl; AggCl = (TAggCl)Enum.Parse(typeof(TAggCl), sbs);
                }
                else if (sbs.StartsWith("To.") && dot > 0)
                {
                    Token = Token.AggTsSlideTo; AggTsSlideTo = (AggTs)Enum.Parse(typeof(AggTs), sbs.Substring(dot + 1));
                }
                else if (sbs.StartsWith("Fr.") && dot > 0)
                {
                    Token = Token.AggTsFrame; AggTsFrame = (AggTs)Enum.Parse(typeof(AggTs), sbs.Substring(dot + 1));
                }
                else
                    throw new Exception($"{_Command} : Bad language syntax");
            }
        }
    }

}
