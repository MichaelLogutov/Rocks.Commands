using System;
using System.Threading.Tasks;
using FluentAssertions;
using Rocks.Commands.Extensions;
using Rocks.Commands.Tests.Decorators.Async.Commands;
using SimpleInjector;
using Xunit;

namespace Rocks.Commands.Tests.Decorators.Async
{
    public class Tests
    {
        [Fact]
        public async Task RegisterCommandsDecorator_Always_RegistersAndUsesOpenGenericDecorators ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>), container, lifestyle);

            var command = new TestDecoratableAsyncCommand { Number = 1 };


            // act
            var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


            // assert
            command.Number.Should ().Be (3);
            result.Should ().Be (2);
        }


        [Fact]
        public async Task RegisterCommandsDecorator_DoesNotAppliesDecoratorToNotApplicableCommands ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>), container, lifestyle);

            var command = new TestNotDecoratableAsyncCommand { Number = 1 };


            // act
            var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


            // assert
            command.Number.Should ().Be (2);
            result.Should ().Be (2);
        }


        [Fact]
        public void RegisterCommandsDecorator_TwoRegistrations_Verifies ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>), container, lifestyle);
            CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>), container, lifestyle);


            // act
            Action act = () => container.Verify ();


            // assert
            act.ShouldNotThrow ();
        }


        [Fact]
        public void GetAllDecorators_NoDecorators_ReturnsNothing ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);


            // act
            var result = CommandsLibrary.CommandsProcessor.GetAllAsyncDecorators<TestDecoratableAsyncCommand> ();


            // assert
            result.Should ().BeEmpty ();
        }


        [Fact]
        public void GetAllDecoratorsGenericTypes_OneDecorator_ReturnsIt ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>), container, lifestyle);


            // act
            var result = CommandsLibrary.CommandsProcessor.GetAllAsyncDecoratorsGenericTypes<TestDecoratableAsyncCommand> ();


            // assert
            result.Should ().Equal (typeof (TestAsyncDecorator<,>));
        }
    }
}