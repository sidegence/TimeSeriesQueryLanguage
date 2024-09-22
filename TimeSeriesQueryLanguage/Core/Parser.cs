using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Functions;

namespace TimeSeriesQueryLanguage.Core
{
    public class TimeSeriesQueryLanguageParser<TAggFn, TAggCl> where TAggFn : Enum where TAggCl : Enum
    {
        Tokenizer<TAggFn, TAggCl>? _tokenizer;

        readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        readonly Array _functionEnum = Enum.GetValues(typeof(FunctionEnum));
        readonly Array _argFunctionEnum = Enum.GetValues(typeof(ArgFunctionEnum));
        readonly Array _aggTimeIntervalEnum = Enum.GetValues(typeof(AggTimeIntervalEnum));
        readonly Array _TAggFn = Enum.GetValues(typeof(TAggFn));
        readonly Array _TAggCl = Enum.GetValues(typeof(TAggCl));

        public TimeSeriesQueryLanguageParser()
        {
        }

        public TimeSeriesQueryLanguageParser(string text)
        {
            Set(text);
        }

        public string Get()
        {
            return _tokenizer?.GetCommand() ?? "";
        }

        public TimeSeriesQueryLanguageParser<TAggFn, TAggCl> Set(string text)
        {
            _tokenizer = new Tokenizer<TAggFn, TAggCl>(text);
            return this;
        }

        public TimeSeriesQueryLanguageParser<TAggFn, TAggCl> Random(int maxLevels)
        {
            var text = Rnd(maxLevels);
            return Set(text);
        }

        public AbstractNode? Parse()
        {
            switch (_tokenizer?.Token)
            {
                case TokenEnum.Agg:
                case TokenEnum.FId:
                case TokenEnum.Number:
                case TokenEnum.HourOfDay:
                case TokenEnum.DayOfWeek:
                case TokenEnum.DayOfMonth:
                case TokenEnum.MonthOfYear:
                    return ParseLeafNode();
                case TokenEnum.Mult:
                case TokenEnum.Div:
                case TokenEnum.Add:
                case TokenEnum.And:
                case TokenEnum.Or:
                case TokenEnum.V1mV2:
                case TokenEnum.V1lV2:
                case TokenEnum.Scale:
                case TokenEnum.V1inV2V3:
                case TokenEnum.V1V2V3Inc:
                case TokenEnum.V1V2V3Dec:
                    return ParseFunctionNode(_tokenizer.Token);
                default:
                    break;
            }
            _tokenizer?.NextToken();
            return _tokenizer?.Token == TokenEnum.EOF ? null : Parse();
        }

        public AbstractNode ParseLeafNode()
        {
            switch (_tokenizer?.Token)
            {
                case TokenEnum.Agg:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == TokenEnum.OpenParens)
                    {
                        _tokenizer.NextToken();
                        TAggFn? aggFn = default; TAggCl? aggCl = default; AggTimeIntervalEnum aggTsFr = AggTimeIntervalEnum.Zero; AggTimeIntervalEnum aggTsTo = AggTimeIntervalEnum.M1;
                        while (_tokenizer.Token != TokenEnum.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case TokenEnum.Comma: break;
                                case TokenEnum.AggFn: aggFn = _tokenizer.AggFn; break;
                                case TokenEnum.AggCl: aggCl = _tokenizer.AggCl; break;
                                case TokenEnum.AggTsFr: aggTsFr = _tokenizer.AggTsFr; break;
                                case TokenEnum.AggTsTo: aggTsTo = _tokenizer.AggTsTo; break;
                                default:
                                    throw new ArgumentNullException($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new AggNode<TAggFn, TAggCl>(aggFn, aggCl, aggTsFr, aggTsTo);
                    }
                    else
                    {
                        throw new ArgumentNullException($"ParseLeafNode not valid CloseParens: {_tokenizer.Token}");
                    }

                case TokenEnum.FId:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == TokenEnum.OpenParens)
                    {
                        _tokenizer.NextToken();
                        decimal i = 0;
                        while (_tokenizer.Token != TokenEnum.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case TokenEnum.Number: i = _tokenizer.Number; break;
                                default:
                                    throw new ArgumentNullException($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new FIdNode<TAggFn, TAggCl>(i);
                    }
                    else
                    {
                        throw new ArgumentNullException($"ParseLeafNode not valid CloseParens: {_tokenizer.Token}");
                    }

                case TokenEnum.Number:
                    return new NumberNode(_tokenizer.Number);

                case TokenEnum.HourOfDay:
                    return new HourOfDayNode();

                case TokenEnum.DayOfWeek:
                    return new DateTimeNode();

                case TokenEnum.DayOfMonth:
                    return new DayOfMonthNode();

                case TokenEnum.MonthOfYear:
                    return new MonthOfYearNode();
            }
            throw new ArgumentNullException($"ParseLeafNode _tokenizer.Token{_tokenizer?.Token}: Invalid syntax");
        }

        public AbstractNode ParseFunctionNode(TokenEnum token)
        {
            _tokenizer?.NextToken();
            if (_tokenizer?.Token == TokenEnum.OpenParens)
            {
                _tokenizer.NextToken();
                List<AbstractNode> args = new List<AbstractNode>();
                while (_tokenizer.Token != TokenEnum.CloseParens)
                {
                    switch (_tokenizer.Token)
                    {
                        case TokenEnum.Agg:
                        case TokenEnum.FId:
                        case TokenEnum.Number:
                        case TokenEnum.HourOfDay:
                        case TokenEnum.DayOfWeek:
                        case TokenEnum.DayOfMonth:
                        case TokenEnum.MonthOfYear:
                            args.Add(ParseLeafNode());
                            break;
                        case TokenEnum.Comma:
                            break;
                        case TokenEnum.Mult:
                        case TokenEnum.Div:
                        case TokenEnum.Add:
                        case TokenEnum.And:
                        case TokenEnum.Or:
                        case TokenEnum.V1mV2:
                        case TokenEnum.V1lV2:
                        case TokenEnum.Scale:
                        case TokenEnum.V1inV2V3:
                        case TokenEnum.V1V2V3Inc:
                        case TokenEnum.V1V2V3Dec:
                            args.Add(ParseFunctionNode(_tokenizer.Token));
                            break;
                        default:
                            throw new ArgumentNullException($"ParseFunctionNode unexpected token: {_tokenizer.Token}");
                    }
                    _tokenizer.NextToken();
                }
                switch (token)
                {
                    case TokenEnum.Mult: return new MultiplicationNode(args);
                    case TokenEnum.Div: return new DivisionNode(args);
                    case TokenEnum.Add: return new AddNode(args);
                    case TokenEnum.And: return new AndNode(args);
                    case TokenEnum.Or: return new OrNode(args);
                    case TokenEnum.V1mV2: return new V1mV2Node(args);
                    case TokenEnum.V1lV2: return new V1lV2Node(args);
                    case TokenEnum.Scale: return new ScaleNode(args);
                    case TokenEnum.V1inV2V3: return new V1inV2V3Node(args);
                    case TokenEnum.V1V2V3Inc: return new V1V2V3IncNode(args);
                    case TokenEnum.V1V2V3Dec: return new V1V2V3DecNode(args);
                }
                throw new NotImplementedException("ParseFunctionNode not a valid token");
            }
            else
            {
                throw new ArgumentNullException($"ParseFunctionNode not valid OpenParens: {_tokenizer?.Token}");
            }
        }

        private string Rnd(int level)
        {
            if (level == 0)
                return RndFunction();

            level--;
            return RndArgFunction(level);
        }

        private string RndFunction()
        {
            var f = RndArrVal(_functionEnum);
            switch (f)
            {
                case FunctionEnum.Number: return $"{_random.NextDouble().ToString("0.00")}";
                case FunctionEnum.Agg: return $"ag({RndArrVal(_TAggFn)},{RndArrVal(_TAggCl)},Fr.{RndArrVal(_aggTimeIntervalEnum)},To.{RndArrVal(_aggTimeIntervalEnum)})";
                default: throw new NotImplementedException(f?.ToString());
            }
        }

        private string RndArgFunction(int level)
        {
            var af = RndArrVal(_argFunctionEnum);
            switch (af)
            {
                case ArgFunctionEnum.And: return $"&({Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.Or: return $"|({Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.V1mV2: return $"<({Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.V1lV2: return $">({Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.V1inV2V3: return $"in({Rnd(level)},{Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.V1V2V3Inc: return $"inc({Rnd(level)},{Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.V1V2V3Dec: return $"dec({Rnd(level)},{Rnd(level)},{Rnd(level)})";
                case ArgFunctionEnum.Function: return RndFunction();
                default: throw new NotImplementedException(af?.ToString());
            }
        }

        private object? RndArrVal(Array array)
        {
            return array.GetValue(_random.Next(array.Length));
        }
    }
}
