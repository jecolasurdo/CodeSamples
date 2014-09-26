namespace someCompany.Backup.BackupSpecs

open System

module HelperFunctions =
    let splitCommand (s:string) = s.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)