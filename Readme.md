# CliCommander

**This repository consists of 2 projects:**

### RawCli
A fork of [CliWrap](https://github.com/Tyrrrz/CliWrap) that enables outputting native process output at the cost of output redirection.

```csharp
CommandResult result = RawCli.Wrap("docker")
    .WithArguments(a => a
        .Add("build")
        .Add("--progress")
        .Add("tty"))
    .Execute();
```

### CliCommander
A shared API between `CliWrap` and `RawCli` that enables the creation of commands that can be executed by either library.

```csharp
var command = CliCommander.Wrap("docker")
    .WithArguments(a => a
        .Add("build")
        .Add("--progress")
        .Add("auto"));

// Docker will build with progress=plain since TTY is not supported when output is redirected.
BufferedCommandResult bufferedResult = command.ToCliWrap()
    .ExecuteBuffered();

// Docker will build with progress=tty since the output is not redirected.
CommandResult result = command.ToRawCli()
    .Execute();
```