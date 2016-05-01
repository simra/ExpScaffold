#load @"expUtil\expUtil.fsx"
#load @"expUtil\runProc.fsx"
open expUtil
open runProc
open System
open System.IO

// TODO: 
// pass-through command-line args and include them in any commit message.
// git add any changes automagically.
// collect a description.

// EDIT THESE FOR YOUR EXPERIMENT.
let resultFile = "results.txt"
let experimentCommand = sprintf "echo %%DATE%% %%TIME%% > %s" resultFile
let verbosity=1

let workingTag = DateTime.Now.ToString("yyyyMMddhhmm")

      
if repoIsReady() then 
    gitCommit(sprintf "Running experiment %s" workingTag)
    gitTag workingTag (sprintf "Tagged experiment %s" workingTag)
    gitSubmod(workingTag) // submodule or something else?

    runProc "cmd.exe" (sprintf "/c %s" experimentCommand) (Some workingTag) |> ignore

    let expectedResult=sprintf @"%s\%s" workingTag resultFile
    if File.Exists(expectedResult) then
        Directory.SetCurrentDirectory(workingTag)
        gitAdd(resultFile) |> ignore
        gitCommit(sprintf "Experiment %s results." workingTag) |> ignore
    //    gitPush() |> ignore -- TODO: should we push the results back up?  
        if verbosity>0 then 
            File.ReadAllLines resultFile
            |> Seq.iter (printfn "%s")
        // Concat to global result file?
        Directory.SetCurrentDirectory("..")




