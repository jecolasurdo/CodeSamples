namespace BackupSpecs

open NUnit.Framework
open somecompany.Backup
open somecompany.Backup.HelperFunctions
open System

[<TestFixture>]
type ``Backup Specifications``() =

    [<Category("General")>]
    [<Test>]
    member this.``When the entire operation completes successfully, one email is sent.``() =
        let notifierCalled = ref 0;
        let notifier options mailMessage = notifierCalled:= !notifierCalled + 1
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeService"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !notifierCalled
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("General")>]
    [<Test>]
    member this.``When the entire operation completes successfully, no logging is done.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = calls:= !calls + 1
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeService"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Source Directory Specifications")>]
    [<Test>]
    member this.``If the source directory cannot be found, one email is sent.``() =
        let notifierCalled = ref 0;
        let notifier options mailMessage = notifierCalled:= !notifierCalled + 1
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  FailedToCopyFiles "A test error."
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !notifierCalled
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Source Directory Specifications")>]
    [<Test>]
    member this.``If the source directory cannot be found, the error is logged once.``() =
        let loggerCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = loggerCalls:= !loggerCalls + 1
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  FailedToCopyFiles "A test error."
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !loggerCalls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Source Directory Specifications")>]
    [<Test>]
    member this.``If the source directory cannot be found, no attempt is made to purge files.``() =
        let purgerCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = 
            purgerCalls:= !purgerCalls + 1
            PurgedFiles
        let directoryCopier options =  FailedToCopyFiles "A test error."
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !purgerCalls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Source Directory Specifications")>]
    [<Test>]
    member this.``When directories are being copied, the program attempts once to purge files.``() =
        let purgerCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = 
            purgerCalls:= !purgerCalls + 1
            PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !purgerCalls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If a service is not specified, no action is taken to locate a service.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | LocateService -> 
                calls:= !calls + 1
                ActionSucceeded options
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)  

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If a service is not specified, no action is taken to stop a service.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | LocateService -> 
                calls:= !calls + 1
                ActionSucceeded options
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)   

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If a service is not specified, no action is taken to start a service.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | StartService -> 
                calls:= !calls + 1
                ActionSucceeded options
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)   

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be located, the error is logged once.``() =
        let loggerCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = loggerCalls:= !loggerCalls + 1
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | LocateService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !loggerCalls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)        

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be located, one email is sent.``() =
        let calls = ref 0;
        let notifier options mailMessage = calls:= !calls + 1
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | LocateService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls) 

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be located, no attempt is made to copy files.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  
            calls:= !calls + 1
            CopiedFiles
        let serviceManager action options = 
            match action with
            | LocateService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls) 

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be located, no attempt is made to purge files.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = 
            calls:= !calls + 1
            PurgedFiles
        let directoryCopier options = CopiedFiles
        let serviceManager action options = 
            match action with
            | LocateService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be stopped, one email is sent.``() =
        let calls = ref 0;
        let notifier options mailMessage = calls:= !calls + 1
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options = CopiedFiles
        let serviceManager action options = 
            match action with
            | StopService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be stopped, the error is logged once.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = calls:= !calls + 1
        let filePurger destination days = PurgedFiles
        let directoryCopier options = CopiedFiles
        let serviceManager action options = 
            match action with
            | StopService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be stopped, no attempt is made to copy files.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options = 
            calls:= !calls + 1
            CopiedFiles
        let serviceManager action options = 
            match action with
            | StopService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be stopped, no attempt is made to purge files.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days =
            calls:= !calls + 1
            PurgedFiles
        let directoryCopier options = CopiedFiles
        let serviceManager action options = 
            match action with
            | StopService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 0
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be started, one email is sent.``() =
        let calls = ref 0;
        let notifier options mailMessage = calls:= !calls + 1
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options = CopiedFiles
        let serviceManager action options = 
            match action with
            | StartService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If the specified service cannot be started, the error is logged once.``() =
        let calls = ref 0;
        let notifier options mailMessage = ()
        let logger string = calls:= !calls + 1
        let filePurger destination days = PurgedFiles
        let directoryCopier options = CopiedFiles
        let serviceManager action options = 
            match action with
            | StartService -> ActionFailed "Test error."
            | _ -> ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !calls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If a service has been stopped, and all other operations succeed, one attempt is made to start the service.``() =
        let serviceStopCalls = ref 0;
        let serviceStartCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | StopService -> serviceStopCalls:= !serviceStopCalls + 1
            | StartService -> serviceStartCalls:= !serviceStartCalls + 1
            | _ -> ()
            ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let expectedStopCalls = 1
        let actualStopCalls = !serviceStopCalls

        let expectedStartCalls = 1
        let actualStartCalls = !serviceStartCalls

        Assert.AreEqual(expectedStopCalls, actualStopCalls)
        Assert.AreEqual(expectedStartCalls, actualStartCalls)        

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If a service has been stopped, and the file copy fails, one attempt is made to start the service.``() =
        let serviceStopCalls = ref 0;
        let serviceStartCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = PurgedFiles
        let directoryCopier options =  FailedToCopyFiles "Test Error"
        let serviceManager action options = 
            match action with
            | StopService -> serviceStopCalls:= !serviceStopCalls + 1
            | StartService -> serviceStartCalls:= !serviceStartCalls + 1
            | _ -> ()
            ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let expectedStopCalls = 1
        let actualStopCalls = !serviceStopCalls

        let expectedStartCalls = 1
        let actualStartCalls = !serviceStartCalls

        Assert.AreEqual(expectedStopCalls, actualStopCalls)
        Assert.AreEqual(expectedStartCalls, actualStartCalls) 

    [<Category("Service Management Specifications")>]
    [<Test>]
    member this.``If a service has been stopped, and the file purge fails, one attempt is made to start the service.``() =
        let serviceStopCalls = ref 0;
        let serviceStartCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days = FailedToPurgeFiles "Test Error"
        let directoryCopier options =  CopiedFiles
        let serviceManager action options = 
            match action with
            | StopService -> serviceStopCalls:= !serviceStopCalls + 1
            | StartService -> serviceStartCalls:= !serviceStartCalls + 1
            | _ -> ()
            ActionSucceeded options

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com -v SomeServiceName"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let expectedStopCalls = 1
        let actualStopCalls = !serviceStopCalls

        let expectedStartCalls = 1
        let actualStartCalls = !serviceStartCalls

        Assert.AreEqual(expectedStopCalls, actualStopCalls)
        Assert.AreEqual(expectedStartCalls, actualStartCalls)  

    [<Category("Destination File Purge Specifications")>]
    [<Test>]
    member this.``If the max days value is specified, an attempt is made to purge files in the destination directory.``() =
        let purgeCalled = ref false;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days =
             purgeCalled:= true
             PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        Assert.IsTrue(!purgeCalled)

    [<Category("Destination File Purge Specifications")>]
    [<Test>]
    member this.``If the max days value is not specified, no attempt is made to purge file in the destination directory.``() =
        let purgeCalled = ref false;
        let notifier options mailMessage = ()
        let logger string = ()
        let filePurger destination days =
             purgeCalled:= true
             PurgedFiles
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        Assert.IsFalse(!purgeCalled)

    [<Category("Destination File Purge Specifications")>]
    [<Test>]
    member this.``If the destination files cannot be purged, the error is logged once.``() =
        let loggerCalls = ref 0;
        let notifier options mailMessage = ()
        let logger string = loggerCalls:= !loggerCalls + 1
        let filePurger destination days = FailedToPurgeFiles "An error message."
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !loggerCalls
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)

    [<Category("Destination File Purge Specifications")>]
    [<Test>]
    member this.``If the destination files cannot be purged, one email is sent.``() =
        let notifierCalled = ref 0;
        let notifier options mailMessage = notifierCalled:= !notifierCalled + 1
        let logger string = ()
        let filePurger destination days = FailedToPurgeFiles "An error message."
        let directoryCopier options =  CopiedFiles
        let serviceManager a b = ActionSucceeded b

        let command = @"-o c:\backups -d e:\my backups -m 7 -t mail.somecompany.net -e help@somecompany.com -s some.person@somecompany.com -f help@somecompany.com"
        command
        |> splitCommand
        |> fun args -> execute args notifier logger directoryCopier filePurger serviceManager
        |> ignore

        let numberOfExpectedCalls = 1
        let numberOfActualCalls = !notifierCalled
        Assert.AreEqual(numberOfExpectedCalls, numberOfActualCalls)
