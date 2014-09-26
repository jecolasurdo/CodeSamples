namespace SomeCompany.BackUp

open Microsoft.FSharp.Reflection

module Parser =

    type MandatoryToken =
        |SOURCE
        |DESTINATION
        |SMTPSERVER
        |FROMEMAIL
        |SUCCESSEMAIL
        |FAILUREEMAIL

    type Token = 
        |MANDATORY of MandatoryToken
        |MAXDAYS
        |SERVICE
        |UNKNOWNARG
        |VALUE of string

    type tokenListStatus =
        | Complete of List<Token>
        | Incomplete of string

    let tokenize (args : string[]) = 
        let extractMandatoryTokenNameSet (tokenList : list<Token>) =
            let guf x = FSharpValue.GetUnionFields(x,x.GetType())
            tokenList
            |> List.filter (fun x ->
                                let uci, _ = guf x
                                if uci.Name = "MANDATORY" then true else false)
            |> List.map (fun x ->
                            let _ , obj = guf x
                            obj |> Array.toList |> List.head)
            |> List.map (fun x -> 
                            let uci, _ = guf x
                            uci.Name)
            |> Set.ofList

        let suppliedTokens = 
            [for x in args do
                let token = 
                    match x with
                    | "-o" -> MANDATORY SOURCE
                    | "-v" -> SERVICE
                    | "-d" -> MANDATORY DESTINATION
                    | "-m" -> MAXDAYS
                    | "-t" -> MANDATORY SMTPSERVER
                    | "-e" -> MANDATORY FROMEMAIL
                    | "-s" -> MANDATORY SUCCESSEMAIL
                    | "-f" -> MANDATORY FAILUREEMAIL
                    | y when y.StartsWith("-") -> UNKNOWNARG
                    | _ -> VALUE x
                yield token]

        let mandatoryTokenNames = (FSharpType.GetUnionCases(typeof<MandatoryToken>)
                                    |> Array.map (fun s -> s.Name)
                                    |> Set.ofArray)

        if Set.isSubset mandatoryTokenNames (extractMandatoryTokenNameSet suppliedTokens) then
            Complete suppliedTokens
        else
            Incomplete "One or more required arguments are missing from the command."

    type Options = {
        Source : string;
        Service : string option;
        Destination : string;
        MaxDays : int option;
        SmtpServer : string;
        FromEmail : string;
        SuccessEmail : string;
        FailureEmail : string;
        }

    let isWholeNumber s = String.forall (fun c -> System.Char.IsDigit(c)) s

    // Strips VALUE tokens from top of list, returning the rest of the list
    let returnNonValueTail tokenList =
        tokenList
        |>List.toSeq
        |>Seq.skipWhile (fun t -> match t with VALUE y -> true | _ -> false)
        |>Seq.toList

    // Takes VALUE tokens from the top of the list, contatenates their associated strings and returns the contatenated string.
    let returnConcatHeadValues tokenList =
        tokenList
        |> List.toSeq
        |> Seq.takeWhile (fun t -> match t with VALUE y -> true | _ -> false)
        |> Seq.fold (fun acc elem -> match elem with VALUE y -> acc + " " + y | _ -> acc) " "
        |> fun s -> s.Trim()

    type Result = 
        |Success of Options
        |Failure of string

    let rec parseTokenListRec tokenList optionsSoFar =
        match tokenList with
        | [] -> Success optionsSoFar
        | MANDATORY SOURCE::t -> 
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with Source = (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the source argument."
        | SERVICE::t ->
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with Service = Some (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the service argument."
        | MANDATORY DESTINATION::t ->
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with Destination = (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the destination argument."
        | MAXDAYS::t ->
            match t with
            |VALUE x::tt when (isWholeNumber x) -> parseTokenListRec tt {optionsSoFar with MaxDays = Some (int x)}
            | _ -> Failure "Expected a whole number to be supplied after the maxdays argument."
        | MANDATORY SMTPSERVER::t ->
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with SmtpServer = (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the smtp server argument."
        | MANDATORY FROMEMAIL::t ->
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with FromEmail = (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the from email argument."
        | MANDATORY SUCCESSEMAIL::t ->
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with SuccessEmail = (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the success email argument."
        | MANDATORY FAILUREEMAIL::t ->
            match t with
            | VALUE x::tt -> parseTokenListRec (returnNonValueTail t) {optionsSoFar with FailureEmail = (returnConcatHeadValues t)}
            | _ -> Failure "Expected a value after the failure email argument."
        | VALUE x::t -> Failure (sprintf "Encountered a value ('%s') without an associated argument." x)
        | UNKNOWNARG::t -> Failure ("Encountered an unknown argument.")

    let parseArgs args =
        let tokenList = tokenize(args)
        match tokenList with
        | Incomplete x -> Failure x
        | Complete tokenList ->
            let defaultOptions = {
                Source = "";
                Service = None;
                Destination = "";
                MaxDays = None;
                SmtpServer = "";
                FromEmail = "";
                SuccessEmail = "";
                FailureEmail = "";
                }

            parseTokenListRec tokenList defaultOptions