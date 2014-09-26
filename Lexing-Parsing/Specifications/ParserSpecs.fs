namespace BackupSpecs

open NUnit.Framework
open SomeCompany.Backup.Parser
open SomeCompany.BackupSpecs.HelperFunctions
open System

[<TestFixture>]
type ``Parser Specifications``() = 
    
    [<Test>]
    member this.``The parser normally handles all standard tokens.``() =
        let command = @"-o c:\backups -v serviceName -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult -> 
            let expectedResult = Success {Source = @"c:\backups";
                                          Service = Some "serviceName";
                                          Destination = @"e:\my backups";
                                          MaxDays = Some 7; 
                                          SmtpServer = "mail.somecompany.net";
                                          FromEmail = "help@somecompany.com";
                                          SuccessEmail = "some.person@somecompany.com";
                                          FailureEmail = "help@somecompany.com"}
            Assert.AreEqual(expectedResult,actualResult)

    [<Test>]
    member this.``The parser succeeds with optional arguments omitted.``() =
        let command = @"-o c:\backups -d e:\my backups -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult -> 
            let expectedResult = Success {Source = @"c:\backups";
                                          Service = None;
                                          Destination = @"e:\my backups";
                                          MaxDays = None; 
                                          SmtpServer = "mail.somecompany.net";
                                          FromEmail = "help@somecompany.com";
                                          SuccessEmail = "some.person@somecompany.com";
                                          FailureEmail = "help@somecompany.com"}
            Assert.AreEqual(expectedResult,actualResult)

    [<Test>]
    member this.``Arguments are commutative``() =
        let a = parseArgs (splitCommand @"-o c:\backups -r registryvalue -z -v serviceName -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com")
        let b = parseArgs (splitCommand @"-t mail.somecompany.net -r registryvalue -z -m 7 -v serviceName -d e:\my backups -f help@somecompany.com -e help@somecompany.com -s some.person@somecompany.com  -o c:\backups")
        Assert.AreEqual(a,b)

    [<Test>]
    member this.``Unknown argument, with no value, at the beginning returns failure.``() =
        //-q is not a supported argument
        let command = @"-q -o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Encountered an unknown argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Unknown argument, with no value, after a known argument returns failure.``() =
        //-q is not a supported argument
        let command = @"-o -q c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Expected a value after the source argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Unknown argument, with no value, after the value of a known argument returns failure.``() =
        //-q is not a supported argument
        let command = @"-o c:\backups -q -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Encountered an unknown argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    //-q is not a supported argument
    member this.``Unknown argument, with no value, at end returns failure.``() =
        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -q"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Encountered an unknown argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Unknown argument, with value, at the beginning returns failure.``() =
        let command = @"-q notreal -o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Encountered an unknown argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Unknown argument, with value, after a known argument returns failure.``() =
        //-q is not a supported argument
        let command = @"-o -q notreal c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Expected a value after the source argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Unknown argument, with value, after the value of a known argument returns failure.``() =
        //-q is not a supported argument
        let command = @"-o c:\backups -q notreal -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Encountered an unknown argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Unknown argument, with value, at end returns failure.``() =
        //-q is not a supported argument
        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -q notreal"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Encountered an unknown argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Duplicate argument, with no value, returns failure.``() =
        //Success email argument is duplicated
        let command = @"-o c:\backups -s -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Failure "Expected a value after the success email argument."
            Assert.AreEqual(expectedResult, actualResult)

    [<Test>]
    member this.``Duplicate argument, with value, returns success with second value applied.``() =
        //Success email argument is duplicated
        let command = @"-o c:\backups -s some.person@somecompany.com -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s duplicateValue -f help@somecompany.com"
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult ->
            let expectedResult = Success {Source = @"c:\backups";
                                          Service = None;
                                          Destination = @"e:\my backups";
                                          MaxDays = Some 7; 
                                          SmtpServer = "mail.somecompany.net";
                                          FromEmail = "help@somecompany.com";
                                          SuccessEmail = "duplicateValue";
                                          FailureEmail = "help@somecompany.com"}
            Assert.AreEqual(expectedResult, actualResult)
    
    [<TestCase(@"-v serviceName -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com")>] //Missing source argument
    [<TestCase(@"-o c:\backups -v serviceName -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com")>] // Missing destination argument
    [<TestCase(@"-o c:\backups -v serviceName -d e:\my backups -m 7 -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com")>] // Missing SMTP argument
    [<TestCase(@"-o c:\backups -v serviceName -d e:\my backups -m 7 -t mail.somecompany.net -s some.person@somecompany.com -f help@somecompany.com")>] // Missing from email argument
    [<TestCase(@"-o c:\backups -v serviceName -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -f help@somecompany.com")>] // Missing success email argument
    [<TestCase(@"-o c:\backups -v serviceName -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com")>] // Missing failure email argument
    member this.``If a mandatory argument is excluded, failure is returned.``(command: string) =
        command
        |> splitCommand
        |> parseArgs
        |> fun actualResult -> 
            let expectedResult = Failure "One or more required arguments are missing from the command."
            Assert.AreEqual(expectedResult,actualResult)