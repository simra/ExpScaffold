module expUtil
open System
open System.IO
open System.Diagnostics

#load "runProc.fsx"
open runProc

// roughly equivalent to posix system()
(*
let system (cmd:string, args:string) =
    eprintfn "Running \"%s\" %s" cmd args
    let p=new ProcessStartInfo()
    p.FileName<-cmd
    p.Arguments<-args
    p.CreateNoWindow<-true
    p.UseShellExecute<-false
    p.RedirectStandardOutput<-true
    p.RedirectStandardError<-true
    use proc = Process.Start(p)
      
    let out=proc.StandardOutput.ReadToEnd()
    proc.WaitForExit();
    (out,err,proc.ExitCode) *)

// take the first argument after 'switch'. If switch isn't present return defaultstr
let argsOrDefault switch defaultStr =
    let ix=Array.IndexOf(fsi.CommandLineArgs, switch)
    if ix>=0 && ix<fsi.CommandLineArgs.Length-1 then 
        fsi.CommandLineArgs.[ix+1]
    else defaultStr

let gitCmd = @"C:\Program Files\Git\cmd\git.exe"
let runGitWithOutput args = runProc gitCmd args None
let runGit args = 
    runGitWithOutput args
    |> fun (err,out) ->
        err|>Seq.iter (eprintfn "%s")
        out|>Seq.iter (eprintfn "%s")

        
// todo: adds and/or tagging/branching.
let gitCommit message = runGit (sprintf "commit -m \"%s\"" message) 
let gitTag tag message = runGit (sprintf "tag -a %s -m \"%s\"" tag message) 
let gitSubmod workingDir = runGit (sprintf "submodule add \"%s\" %s" (Directory.GetCurrentDirectory()) workingDir) 
let gitAdd fn = runGit (sprintf "add %s" fn)
let gitPush () = runGit "push"
let gitStatus() = runGitWithOutput "status"

let repoIsReady() =
    gitStatus()
    |> fun (out,err) ->
        out
        |> Seq.filter(fun s -> not (s.Contains(".gitmodules")) && s.Trim().StartsWith("modified:") && not (s.Contains("(untracked content)")) && not (s.Contains("(new commits)"))) 
        |> fun s ->
            if (not (Seq.isEmpty s)) then
                eprintfn "git add these changes first:"
                s|>Seq.iter (printfn "%s")
                false
            else  true