open System
open System.IO
#load @"expUtil\expUtil.fsx"
#load @"expUtil\runProc.fsx"
open expUtil
open runProc

// EDIT THESE FOR YOUR EXPERIMENT.
let resultFile = "results.txt"
// experimentCommand will be run in a shell under Experiments\[timestamp]
let experimentCommand = sprintf "echo %%DATE%% %%TIME%% > %s" resultFile
let expFolder = "Experiments"
let verbosity=1

if not (Directory.Exists(expFolder)) then Directory.CreateDirectory(expFolder)|>ignore

let timestamp = DateTime.Now.ToString("yyyyMMddHHmm")
let workingTag = sprintf "%s/%s" expFolder timestamp

let message = argsOrDefault "-m" (sprintf "Running experiment %s" timestamp)
      
if repoIsReady() then 
    gitCommit(message)
    gitTag timestamp (sprintf "Tagged experiment %s" timestamp) // necessary?
    gitClone "." workingTag // submodule or something else?

    runProc "cmd.exe" (sprintf "/c %s" experimentCommand) (Some workingTag)     
    |> fun (out,err) ->
        out |> Seq.iter (eprintfn "OUTPUT: %s") 
        err |> Seq.iter (eprintfn "ERROR: %s")

    let expectedResult=sprintf @"%s\%s" workingTag resultFile
    if File.Exists(expectedResult) then
        let currentDir=Directory.GetCurrentDirectory()
        Directory.SetCurrentDirectory(workingTag)
        gitAdd(resultFile) |> ignore
        gitCommit(sprintf "Experiment %s results." timestamp) |> ignore
    //    gitPush() |> ignore -- TODO: should we push the results back up?  
        if verbosity>0 then 
            File.ReadAllLines resultFile
            |> Seq.iter (printfn "%s")
        // Concat to global result file?
        Directory.SetCurrentDirectory(currentDir)




