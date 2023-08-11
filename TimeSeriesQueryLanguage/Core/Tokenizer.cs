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

        public string GetCommand()
        {
            return _Command;
        }

        public TokenEnum Token;
        public decimal Number = 0;
        public TAggFn? AggFn = default;
        public TAggCl? AggCl = default;
        public AggTimeIntervalEnum AggTsFr;
        public AggTimeIntervalEnum AggTsTo;

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
                Token = TokenEnum.EOF;
            }
            else if (_CurrentChar == '(')
            {
                Token = TokenEnum.OpenParens;
                NextChar();
            }
            else if (_CurrentChar == ')')
            {
                Token = TokenEnum.CloseParens;
                NextChar();
            }
            else if (_CurrentChar == ',')
            {
                Token = TokenEnum.Comma;
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
                Token = TokenEnum.Number; Number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
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
                    Token = TokenEnum.Mult;
                }
                else if (sbs == "/")
                {
                    Token = TokenEnum.Div;
                }
                else if (sbs == "+")
                {
                    Token = TokenEnum.Add;
                }
                else if (sbs == "&")
                {
                    Token = TokenEnum.And;
                }
                else if (sbs == "|")
                {
                    Token = TokenEnum.Or;
                }
                else if (sbs == "ag")
                {
                    Token = TokenEnum.Agg;
                }
                else if (sbs == "fid")
                {
                    Token = TokenEnum.FId;
                }
                else if (sbs == ">")
                {
                    Token = TokenEnum.V1mV2;
                }
                else if (sbs == "<")
                {
                    Token = TokenEnum.V1lV2;
                }
                else if (sbs == "sc")
                {
                    Token = TokenEnum.Scale;
                }
                else if (sbs == "in")
                {
                    Token = TokenEnum.V1inV2V3;
                }
                else if (Enum.GetNames(typeof(TAggFn)).Contains(sbs))
                {
                    Token = TokenEnum.AggFn; AggFn = (TAggFn)Enum.Parse(typeof(TAggFn), sbs);
                }
                else if (Enum.GetNames(typeof(TAggCl)).Contains(sbs))
                {
                    Token = TokenEnum.AggCl; AggCl = (TAggCl)Enum.Parse(typeof(TAggCl), sbs);
                }
                else if (sbs.StartsWith("Fr.") && dot > 0)
                {
                    Token = TokenEnum.AggTsFr; AggTsFr = (AggTimeIntervalEnum)Enum.Parse(typeof(AggTimeIntervalEnum), sbs.Substring(dot + 1));
                }
                else if (sbs.StartsWith("To.") && dot > 0)
                {
                    Token = TokenEnum.AggTsTo; AggTsTo = (AggTimeIntervalEnum)Enum.Parse(typeof(AggTimeIntervalEnum), sbs.Substring(dot + 1));
                }
                else
                    throw new Exception($"{_Command} : Bad language syntax");
            }
        }
    }
}
