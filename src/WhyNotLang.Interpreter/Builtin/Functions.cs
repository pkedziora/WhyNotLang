using System;
using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Builtin
{
    public class Functions
    {
        public static ExpressionValue ToNumber(ExpressionValue str)
        {
            if (str.Type != ExpressionValueTypes.String)
            {
                throw new Exception("String expected");
            }
            
            return new ExpressionValue(int.Parse((string)str.Value), ExpressionValueTypes.Number);
        }
        
        public static ExpressionValue ToString(ExpressionValue number)
        {
            if (number.Type != ExpressionValueTypes.Number)
            {
                throw new Exception("Number expected");
            }
            
            return new ExpressionValue(number.Value.ToString(), ExpressionValueTypes.String);
        }
        
        public static ExpressionValue Writeln(ExpressionValue str)
        {
            if (str.Type != ExpressionValueTypes.String)
            {
                throw new Exception("String expected");
            }
            
            Console.WriteLine(str.Value);
            return ExpressionValue.Empty;
        }
        
        public static ExpressionValue Readln()
        {

            var str = Console.ReadLine();
            return new ExpressionValue(str, ExpressionValueTypes.String);
        }
    }
}