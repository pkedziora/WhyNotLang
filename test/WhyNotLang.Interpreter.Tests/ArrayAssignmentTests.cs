using System;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class ArrayAssignmentTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public ArrayAssignmentTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public void ExecutesArrayAssignementWithNumberExpression()
        {
            _executor.Initialise(@"
                var x[10]
                x[0] := 1
            ");
            
            _executor.ExecuteAll();

            var actual = _programState.CurrentScope.Arrays["x"][0].Value;

            Assert.Equal(1, actual);
        }
        
        [Fact]
        public void Executes2ArrayAssignementsWithComplexExpressions()
        {
            _executor.Initialise(@"
                var x[10]
                x[0] := 1
                x[1] := (10 * (4/4) + (2 - 1))
            ");
            
            _executor.ExecuteAll();

            var actual0 = _programState.CurrentScope.Arrays["x"][0].Value;
            var actual1 = _programState.CurrentScope.Arrays["x"][1].Value;

            Assert.Equal(1, actual0);
            Assert.Equal(11, actual1);
        }

        [Fact]
        public void ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise(@"
                var abc:=100
                abc := (10 * (4/4) + (2 - 1))");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ThrowsDuringArrayAssignementWhenUndeclared()
        {
            _executor.Initialise(@"
                x[0] := 2
            ");

            Assert.Throws<ArgumentException>(() => _executor.ExecuteAll());
        }
        
        [Fact]
        public void ThrowsDuringArrayAssignementWhenOutOfRange()
        {
            _executor.Initialise(@"
                var x[10]
                x[10] := 2
            ");

            Assert.Throws<ArgumentException>(() => _executor.ExecuteAll());
        }
        
        [Fact]
        public void ArrayValueCanBeRetrieved()
        {
            _executor.Initialise(@"
                var x[10]
                x[1] := 2
                var y:= x[1] 
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ArrayValueUsedInExpressionWithHighPrecedenceOnTheRight()
        {
            _executor.Initialise(@"
                var x[10]
                x[1] := 2
                var y:= 1 + x[1] * 3 
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(7, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ArrayCanBePassedByReferenceToFunctionAndModified()
        {
            _executor.Initialise(@"
                function foo(array)
                begin
                    array[0] := 100
                end
                var bar[10]
                foo(bar)         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetArrayItem("bar", 0);

            var expected = new ExpressionValue(100, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GlobalArrayValueAssignedInFunction()
        {
            _executor.Initialise(@"
                global x[10]

                function foo()
                begin
                    x[1] := 2
                end

                foo()
                var y:= 1 + x[1] * 3 
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(7, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void LocalArrayHidesGlobalArrayInFunction()
        {
            _executor.Initialise(@"
                global x[10]

                function foo()
                begin
                    var x[20]
                    x[1] := 2
                end
                x[1] := 1
                foo()
                var y:= x[1] 
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
    }
}