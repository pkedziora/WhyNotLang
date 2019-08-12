using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class BlockStatementExecutor : IStatementExecutor
    {
        private readonly IExecutor _mainExecutor;

        public BlockStatementExecutor(IExecutor mainExecutor)
        {
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var blockStatement = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as BlockStatement;
            if (blockStatement == null)
            {
                throw new WhyNotLangException("Block expected");
            }

            _mainExecutor.ProgramState.AddScope("Block");
            _mainExecutor.CreateNewContext(blockStatement.ChildStatements);
            var returnValue = await _mainExecutor.ExecuteAll();
            _mainExecutor.LeaveContext();
            _mainExecutor.ProgramState.RemoveScope();

            return returnValue;
        }
    }
}