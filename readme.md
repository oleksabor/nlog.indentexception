I've asked a question about [exception layout renderer at SO](https://stackoverflow.com/questions/46565639/nlog-exception-layout-to-format-exception-type-message-and-stack-trace)

However seems that writing custom layout renderer is not so complex
So here i will keep IndentExceptionLayoutRenderer.cs source code

[![NuGet](https://img.shields.io/nuget/v/NLog.IndentException.svg?maxAge=259200)](https://www.nuget.org/packages/NLog.IndentException/)

This renderer allows to show exception using readable format (from my point of view) in the log (file or console). Additionally it does not log full exception stack trace if it is logged second time (it happens if exception is rethrown like inner exception).

src\NLog.config is a sample configuration that shows how layout can be parameterized for console and file logging

Below you can find IndentException class properties that can help you to adjust exception logging

| property name | description |
|-----|------|
|Indent | what character(s) are placed before exception in the log (default is tab)|
StackTraceIndent | indent between each stack trace line (default is two tab characters)
BeforeType | is written before exception type name (default [)
AfterType | is written after exception type name (default ])
Separator | separator between exception type and message
LogStack | to log stack trace or not (for console logger e.g.)


#### console log sample
Here is console output for layout like `layout="${level} ${message}${onexception:${newline}${IndentException:LogStack=false:separator=&#x9;:beforeType=:aftertype=}}"`
there is no stack trace shown (because of `LogStack=false`)
```
Error tryException failure
        ArgumentException       outer exception
        KeyNotFoundException    innerException
Error failed to start NLogTest
        ArgumentException       bad try
        ArgumentException       outer exception
        KeyNotFoundException    innerException
```

#### file log sample
Below you can find file logging sample using layout `layout="[${threadid}] ${longdate} ${level} (${logger}) ${message}${onexception:${newline}${IndentException}}`

`outer exception` and `inner exception` stack trace is logged only once. Error type and message are logged only if error processing code tries to log the same exception second time

```
[1] 2019-01-27 19:56:57.7258 Debug (YourNamespace.NLog.Extention.Test.Program) starting
[1] 2019-01-27 19:56:57.7769 Error (YourNamespace.NLog.Extention.Test.Classes.UnitOfWork) tryException failure
	[ArgumentException] outer exception
		at YourNamespace.NLog.Extention.Test.Classes.UnitOfWork.outerException()
		at YourNamespace.NLog.Extention.Test.Classes.UnitOfWork.tryException()
	[KeyNotFoundException] innerException
		at YourNamespace.NLog.Extention.Test.Classes.UnitOfWork.innerException()
		at YourNamespace.NLog.Extention.Test.Classes.UnitOfWork.outerException()
[1] 2019-01-27 19:56:57.7769 Error (YourNamespace.NLog.Extention.Test.Program) failed to start NLogTest
	[ArgumentException] bad try
		at YourNamespace.NLog.Extention.Test.Classes.UnitOfWork.tryException()
		at YourNamespace.NLog.Extention.Test.Program.Main(String[] args)
	[ArgumentException] outer exception
	[KeyNotFoundException] innerException
[1] 2019-01-27 19:56:57.8344 Debug (YourNamespace.NLog.Extention.Test.Program) the end
```