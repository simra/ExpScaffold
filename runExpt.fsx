open System
open System.IO
#load @"expUtil\expUtil.fsx"
#load @"expUtil\runProc.fsx"
open expUtil
open runProc

// TODO: 
// pass-through command-line args and include them in any commit message.
// git add any changes automagically.
// collect a description.

// EDIT THESE FOR YOUR EXPERIMENT.
let resultFile = "results.txt"
let experimentCommand = sprintf "echo %%DATE%% %%TIME%% > %s" resultFile
let expFolder = "Experiments"
let verbosity=1

if not (Directory.Exists(expFolder)) then Directory.CreateDirectory(expFolder)|>ignore

let timestamp = DateTime.Now.ToString("yyyyMMddhhmm")
let workingTag = sprintf "%s/%s" expFolder timestamp
      
if repoIsReady() then 
    gitCommit(sprintf "Running experiment %s" timestamp)
    gitTag timestamp (sprintf "Tagged experiment %s" timestamp) // necessary?
    gitSubmod(workingTag) // submodule or something else?

    runProc "cmd.exe" (sprintf "/c %s" experimentCommand) (Some workingTag) |> ignore

    let expectedResult=sprintf @"%s\%s" workingTag resultFile
    if File.Exists(expectedResult) then
        let currentDir=Directory.GetCurrentDirectory()
        Directory.SetCurrentDirectory(workingTag)
        gitAdd(resultFile) |> ignore
        gitCommit(sprintf "Experiment %s results." workingTag) |> ignore
    //    gitPush() |> ignore -- TODO: should we push the results back up?  
        if verbosity>0 then 
            File.ReadAllLines resultFile
            |> Seq.iter (printfn "%s")
        // Concat to global result file?
        Directory.SetCurrentDirectory(currentDir)




