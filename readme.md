I've asked a question about [exception layout renderer at SO](https://stackoverflow.com/questions/46565639/nlog-exception-layout-to-format-exception-type-message-and-stack-trace)

However seems that writing custom layout renderer is not so complex
So here i will keep IndentExceptionLayoutRenderer.cs source code

This renderer allows to show exception using readable format (from my point of view) in the log (file or console)

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


