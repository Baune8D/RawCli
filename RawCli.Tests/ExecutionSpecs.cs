﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace RawCli.Tests;

public class ExecutionSpecs
{
    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_get_the_exit_code_and_execution_time()
    {
        // Arrange
        var cmd = Raw.CliWrap("dotnet").WithArguments(Dummy.Program.FilePath);

        // Act
        var result = await cmd.WithStandardOutputToNull().ExecuteAsync();

        // Assert
        result.ExitCode.Should().Be(0);
        result.RunTime.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_get_the_associated_process_ID()
    {
        // Arrange
        var cmd = Raw.CliWrap("dotnet").WithArguments(Dummy.Program.FilePath);

        // Act
        var task = cmd.WithStandardOutputToNull().ExecuteAsync();

        // Assert
        task.ProcessId.Should().NotBe(0);

        await task;
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_with_a_configured_awaiter()
    {
        // Arrange
        var cmd = Raw.CliWrap("dotnet").WithArguments(Dummy.Program.FilePath);

        // Act & assert
        await cmd.WithStandardOutputToNull().ExecuteAsync().ConfigureAwait(false);
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_on_large_stdout_and_stderr()
    {
        // Arrange
        var cmd = Raw.CliWrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("generate binary")
                .Add("--target").Add("all")
                .Add("--length").Add(100_000)
            );

        // Act & assert
        await cmd.WithStandardOutputToNull().ExecuteAsync();
    }

    [Fact(Timeout = 15000)]
    public void I_cannot_execute_a_command_on_a_file_that_does_not_exist()
    {
        // Arrange
        var cmd = Raw.CliWrap("I_do_not_exist.exe");

        // Act & assert

        // Should throw synchronously
        // https://github.com/Tyrrrz/CliWrap/issues/139
        Assert.ThrowsAny<Win32Exception>(() => cmd.WithStandardOutputToNull().ExecuteAsync());
    }
}