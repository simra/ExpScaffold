module expUtil
open System
open System.Diagnostics


// roughly equivalent to posix system()
let system (str:string) =
    use proc = Process.Start(str)
    proc.WaitForExit();
    proc.ExitCode = 0

// take the first argument after 'switch'. If switch isn't present return defaultstr
let argsOrDefault switch defaultStr =
    let ix=Array.IndexOf(fsi.CommandLineArgs, switch)
    if ix>=0 && ix<fsi.CommandLineArgs.Length-1 then 
        fsi.CommandLineArgs.[ix+1]
    else defaultStr

let gitCmd = @"C:\Program Files\Git\cmd\git.exe"
let runGit args = sprintf "%s %s" gitCmd args |> system


// todo: adds and/or tagging/branching.
let gitCommit message = runGit (sprintf "-m \"%s\"" message) 
let gitTag tag message = runGit (sprintf "tag -a %s -m \"%s\"" tag message) 
let gitSubmod workingDir = runGit (sprintf "submodule add . %s" workingDir) 
let gitAdd fn = runGit (sprintf "add %s" fn)
let gitPush () = runGit "push"