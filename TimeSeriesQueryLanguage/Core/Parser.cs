using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Functions;

namespace TimeSeriesQueryLanguage.Core
{
    public class TimeSeriesQueryLanguageParser
    {
        Tokenizer? _tokenizer;

        public TimeSeriesQueryLanguageParser()
        {
        }

        public TimeSeriesQueryLanguageParser(string text)
        {
            Set(text);
        }

        public TimeSeriesQueryLanguageParser Set(string text)
        {
            _tokenizer = new Tokenizer(text);
            return this;
        }

        public AbstractNode? Parse()
        {
            switch (_tokenizer?.Token)
            {
                case Token.Agg:
                case Token.FId:
                case Token.Number:
                    return ParseLeafNode();
                case Token.Mult:
                case Token.Div:
                case Token.Add:
                case Token.And:
                case Token.Or:
                case Token.V1mV2:
                case Token.V1lV2:
                case Token.Scale:
                case Token.V1inV2V3:
                    return ParseFunctionNode(_tokenizer.Token);
                default:
                    break;
            }
            _tokenizer?.NextToken();
            return _tokenizer?.Token == Token.EOF ? null : Parse();
        }

        public AbstractNode ParseLeafNode()
        {
            switch (_tokenizer?.Token)
            {
                case Token.Agg:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == Token.OpenParens)
                    {
                        _tokenizer.NextToken();
                        AggFn aggFn = AggFn.Avg; AggCl aggCl = AggCl.Cl0; AggTs aggTsSlideTo = AggTs.M0; AggTs aggTsFrame = AggTs.M1;
                        while (_tokenizer.Token != Token.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case Token.Comma: break;
                                case Token.AggFn: aggFn = _tokenizer.AggFn; break;
                                case Token.AggCl: aggCl = _tokenizer.AggCl; break;
                                case Token.AggTsSlideTo: aggTsSlideTo = _tokenizer.AggTsSlideTo; break;
                                case Token.AggTsFrame: aggTsFrame = _tokenizer.AggTsFrame; break;
                                default:
                                    throw new Exception($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new AggNode(aggFn, aggCl, aggTsSlideTo, aggTsFrame);
                    }
                    else
                    {
                        throw new Exception("ParseLeafNode Token.Agg no open parans");
                    }
                case Token.FId:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == Token.OpenParens)
                    {
                        _tokenizer.NextToken();
                        decimal i = 0;
                        while (_tokenizer.Token != Token.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case Token.Number: i = _tokenizer.Number; break;
                                default:
                                    throw new Exception($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new FIdNode(i);
                    }
                    else
                    {
                        throw new Exception("ParseLeafNode Token.Agg no open parans");
                    }
                case Token.Number:
                    return new NumberNode(_tokenizer.Number);
            }
            throw new Exception($"ParseLeafNode _tokenizer.Token{_tokenizer?.Token}: Invalid syntax");
        }

        public AbstractNode ParseFunctionNode(Token token)
        {
            _tokenizer?.NextToken();
            if (_tokenizer?.Token == Token.OpenParens)
            {
                _tokenizer.NextToken();
                List<AbstractNode> args = new List<AbstractNode>();
                while (_tokenizer.Token != Token.CloseParens)
                {
                    switch (_tokenizer.Token)
                    {
                        case Token.Agg:
                        case Token.FId:
                        case Token.Number:
                            args.Add(ParseLeafNode());
                            break;
                        case Token.Comma:
                            break;
                        case Token.Mult:
                        case Token.Div:
                        case Token.Add:
                        case Token.And:
                        case Token.Or:
                        case Token.V1mV2:
                        case Token.V1lV2:
                        case Token.Scale:
                        case Token.V1inV2V3:
                            args.Add(ParseFunctionNode(_tokenizer.Token));
                            break;
                        default:
                            throw new Exception($"ParseFunctionNode unexpected token: {_tokenizer.Token}");
                    }
                    _tokenizer.NextToken();
                }
                switch (token)
                {
                    case Token.Mult: return new MultiplicationNode(args);
                    case Token.Div: return new DivisionNode(args);
                    case Token.Add: return new AddNode(args);
                    case Token.And: return new AndNode(args);
                    case Token.Or: return new OrNode(args);
                    case Token.V1mV2: return new V1mV2Node(args);
                    case Token.V1lV2: return new V1lV2Node(args);
                    case Token.Scale: return new ScaleNode(args);
                    case Token.V1inV2V3: return new V1inV2V3Node(args);
                }
                throw new Exception("ParseFunctionNode not a valid token");
            }
            else
            {
                throw new Exception("ParseFunctionNode no open parens");
            }
        }
    }
}
