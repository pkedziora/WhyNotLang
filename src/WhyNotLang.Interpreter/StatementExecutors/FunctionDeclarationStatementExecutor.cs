using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class FunctionDeclarationStatementExecutor : IStatementExecutor
    {
        private readonly IExecutor _mainExecutor;

        public FunctionDeclarationStatementExecutor(IExecutor mainExecutor)
        {
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var functionDeclaration = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as FunctionDeclarationStatement;
            if (functionDeclaration == null)
            {
                throw new WhyNotLangException("Function declaration expected");
            }

            var functionName = functionDeclaration.Name.Value;
            _mainExecutor.ProgramState.DeclareFunction(functionName, functionDeclaration);

            return await Task.FromResult(ExpressionValue.Empty);
        }
    }
}