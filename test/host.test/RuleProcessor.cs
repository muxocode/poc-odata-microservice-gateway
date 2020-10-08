using crossapp.rule;
using host.domain.rules;
using host.test.clases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace host.test
{
    public class RuleProcessorTest
    {
        [Fact]
        public async Task NULL()
        {
            // Arrange
            var ruleProcessor = new RuleProcessor<TestUser>(null);

            // Act
            var result = await ruleProcessor.CheckRules(new TestUser());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Correct()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new CorrectRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = await ruleProcessor.CheckRules(new TestUser());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Correct_Array()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new CorrectRule(), new CorrectRule(), new CorrectRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = await ruleProcessor.CheckRules(new TestUser());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Wrong()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new WrongRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = await ruleProcessor.CheckRules(new TestUser());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Wrong_Array()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new WrongRule(), new WrongRule(), new WrongRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = await ruleProcessor.CheckRules(new TestUser());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Mix()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new CorrectRule(), new WrongRule(), new CorrectRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = await ruleProcessor.CheckRules(new TestUser());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Exception()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new ExceptionRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = ruleProcessor.CheckRules(new TestUser());

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(()=> result);
        }

        [Fact]
        public async Task Exception_Array()
        {
            // Arrange
            var myParams = new List<IRule<TestUser>>() { new ExceptionRule(), new ExceptionRule(), new ExceptionRule(), new ExceptionRule() };
            var ruleProcessor = new RuleProcessor<TestUser>(myParams);

            // Act
            var result = ruleProcessor.CheckRules(new TestUser());

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => result);
        }
    }
}
