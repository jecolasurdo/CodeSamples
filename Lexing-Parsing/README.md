Lexing and Parsing
==================

The code found here is part of a project where I was to write a command line utility that would provide some basic file backup functionality for a few systems.

At the time I started this project I had started to gain some experience with F#. I had written lexer-parsers in C# before, but I felt that a functional language, in particular some of the object types present in F#, would lend themselves to the lex/parse process quite nicely.

The specifications for the parser can be found here: https://github.com/jecolasurdo/CodeSamples/blob/master/Lexing-Parsing/Specifications/ParserSpecs.fs

The specifications for the backup utility that uses the parser's output can be found here:
https://github.com/jecolasurdo/CodeSamples/blob/master/Lexing-Parsing/Specifications/BackupSpecs.fs

And the implementation of the parser is here: https://github.com/jecolasurdo/CodeSamples/blob/master/Lexing-Parsing/Parser.fs

This parser implements a recursive decent algorithm. Wanting to use recursion is another factor that led me to implement this in F# rather than C#. Inspection of the resulting IL code showed that F# does indeed optimize tail calls. (In this case, the compiler actual rewrites the recursion iteratively.)

There are a number of things that I think I would do differently if I did this project again, but I am glad to say that it works quite reliably.

Note that I have removed a few things from the supplied code:
 - Integration tests that prove functions such as https://github.com/jecolasurdo/CodeSamples/blob/master/Lexing-Parsing/Program.fs#L127 are not included in this sample.
 - I have ommitted the implementation of the service manager. Found here: https://github.com/jecolasurdo/CodeSamples/blob/master/Lexing-Parsing/Program.fs#L151
