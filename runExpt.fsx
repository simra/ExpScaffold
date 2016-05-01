#load @"expUtil\expUtil.fsx"
open expUtil
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

gitCommit(workingTag)
gitTag workingTag (sprintf "Tagged experiment %s" workingTag)
gitSubmod(workingTag)

// set working directory
Directory.SetCurrentDirectory(workingTag)

system experimentCommand

if File.Exists(@".\results.txt") then
    gitAdd("results.txt") |> ignore
    gitCommit(sprintf "Experiment %s results." workingTag) |> ignore
    gitPush() |> ignore
    if verbosity>0 then 
        File.ReadAllLines resultFile
        |> Seq.iter (printfn "%s")
    // Concat to global result file?

Directory.SetCurrentDirectory("..")




