﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace RawCli.Tests;

public class CredentialsSpecs
{
    [SkippableFact(Timeout = 15000)]
    public async Task I_can_execute_a_command_as_a_different_user()
    {
        Skip.IfNot(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "Starting a process as another user is only supported on Windows."
        );

        // We can't really test the happy path, but we can at least verify
        // that the credentials have been passed by getting an exception.

        // Arrange
        var cmd = Raw.CliWrap("dotnet")
            .WithArguments(a => a.Add(Dummy.Program.FilePath))
            .WithCredentials(c => c
                .SetUserName("user123")
                .SetPassword("pass123")
                .LoadUserProfile()
            );

        // Act & assert
        await Assert.ThrowsAsync<Win32Exception>(() => cmd.WithStandardOutputToNull().ExecuteAsync());
    }

    [SkippableFact(Timeout = 15000)]
    public async Task I_can_execute_a_command_as_a_different_user_under_the_specified_domain()
    {
        Skip.IfNot(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "Starting a process as another user is only supported on Windows."
        );

        // We can't really test the happy path, but we can at least verify
        // that the credentials have been passed by getting an exception.

        // Arrange
        var cmd = Raw.CliWrap("dotnet")
            .WithArguments(a => a.Add(Dummy.Program.FilePath))
            .WithCredentials(c => c
                .SetDomain("domain123")
                .SetUserName("user123")
                .SetPassword("pass123")
                .LoadUserProfile()
            );

        // Act & assert
        await Assert.ThrowsAsync<Win32Exception>(() => cmd.WithStandardOutputToNull().ExecuteAsync());
    }

    [SkippableFact(Timeout = 15000)]
    public async Task I_cannot_execute_a_command_as_a_different_user_on_a_system_that_does_not_support_it()
    {
        Skip.If(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "Starting a process as another user is only supported on Windows."
        );

        // Arrange
        var cmd = Raw.CliWrap("dotnet")
            .WithArguments(a => a.Add(Dummy.Program.FilePath))
            .WithCredentials(c => c
                .SetUserName("user123")
                .SetPassword("pass123")
            );

        // Act & assert
        await Assert.ThrowsAsync<NotSupportedException>(() => cmd.WithStandardOutputToNull().ExecuteAsync());
    }
}