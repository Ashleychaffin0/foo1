open System
open System.Text.RegularExpressions
open System.Net
open System.IO

//---------------------------------------------------------------------------------------

type SiSource =
    | Inline                    // For testing, to avoid web overhead
    | FromWeb

//---------------------------------------------------------------------------------------

// Configuration parameters
let SiDataSource = SiSource.FromWeb

let SiUrl = "http://live.sysinternals.com/"

let SiDirectory = @"G:\LRS-8500\bin\SysInt\"

//---------------------------------------------------------------------------------------

let GetHttp (url: string) =
    let req = System.Net.WebRequest.Create url
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    resp.Close()
    html

//---------------------------------------------------------------------------------------
    
let GetHttpBinary (url: string) =
    let client = new WebClient()
    let buf = client.DownloadData url
    buf

//---------------------------------------------------------------------------------------

let GetHttpBinary_obsolete (url: string) =          // TODO:
    let req = System.Net.WebRequest.Create url
    let resp = req.GetResponse()
    let ContentLength = resp.Headers.["Content-Length"]
    let stream = resp.GetResponseStream()
    let rdr = new BinaryReader(stream)
    let html = rdr.ReadBytes(Convert.ToInt32(ContentLength))
    resp.Close()
    html

//---------------------------------------------------------------------------------------

let GetSiData() =     
    match SiDataSource with
    | SiSource.FromWeb -> GetHttp SiUrl
    | SiSource.Inline -> @"<html><head><META http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""><title>live.sysinternals.com - /</title></head><body><H1>live.sysinternals.com - /</H1><hr>
 
<pre>         Friday, May 30, 2008  3:55 PM          668 <A HREF=""/About_This_Site.txt"">About_This_Site.txt</A><br> Wednesday, December 17, 2008  9:11 PM       313200 <A HREF=""/accesschk.exe"">accesschk.exe</A><br> Wednesday, November 01, 2006  1:06 PM       174968 <A HREF=""/AccessEnum.exe"">AccessEnum.exe</A><br>      Thursday, July 12, 2007  5:26 AM        50379 <A HREF=""/AdExplorer.chm"">AdExplorer.chm</A><br>    Monday, November 26, 2007 12:21 PM       422952 <A HREF=""/ADExplorer.exe"">ADExplorer.exe</A><br> Wednesday, November 07, 2007  9:13 AM       401616 <A HREF=""/ADInsight.chm"">ADInsight.chm</A><br>   Tuesday, November 20, 2007 12:25 PM      1049640 <A HREF=""/ADInsight.exe"">ADInsight.exe</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/adrestore.exe"">adrestore.exe</A><br>  Thursday, February 19, 2009 11:36 PM        &lt;dir&gt; <A HREF=""/aspnet_client/"">aspnet_client</A><br> Wednesday, November 01, 2006  1:06 PM       154424 <A HREF=""/Autologon.exe"">Autologon.exe</A><br>   Tuesday, December 16, 2008  4:46 PM        49244 <A HREF=""/autoruns.chm"">autoruns.chm</A><br>   Tuesday, February 03, 2009  6:32 PM       647552 <A HREF=""/autoruns.exe"">autoruns.exe</A><br>   Tuesday, February 03, 2009  6:32 PM       540032 <A HREF=""/autorunsc.exe"">autorunsc.exe</A><br>   Wednesday, August 06, 2008  4:27 PM       845864 <A HREF=""/Bginfo.exe"">Bginfo.exe</A><br> Wednesday, November 01, 2006  1:06 PM       154424 <A HREF=""/Cacheset.exe"">Cacheset.exe</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/Clockres.exe"">Clockres.exe</A><br>  Tuesday, September 30, 2008  7:33 PM       198184 <A HREF=""/Contig.exe"">Contig.exe</A><br>Wednesday, September 03, 2008 12:08 PM       185896 <A HREF=""/Coreinfo.exe"">Coreinfo.exe</A><br>Wednesday, September 27, 2006  5:04 PM        10104 <A HREF=""/ctrl2cap.amd.sys"">ctrl2cap.amd.sys</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/ctrl2cap.exe"">ctrl2cap.exe</A><br>    Sunday, November 21, 1999  5:20 PM         2864 <A HREF=""/ctrl2cap.nt4.sys"">ctrl2cap.nt4.sys</A><br>    Sunday, November 21, 1999  6:46 PM         2832 <A HREF=""/ctrl2cap.nt5.sys"">ctrl2cap.nt5.sys</A><br> Thursday, September 15, 2005  8:49 AM        68539 <A HREF=""/dbgview.chm"">dbgview.chm</A><br>  Wednesday, October 15, 2008  8:25 AM       461680 <A HREF=""/Dbgview.exe"">Dbgview.exe</A><br> Wednesday, November 01, 2006  9:06 PM       158520 <A HREF=""/DEFRAG.EXE"">DEFRAG.EXE</A><br>    Thursday, August 21, 2008  8:30 AM       118824 <A HREF=""/Desktops.exe"">Desktops.exe</A><br>         Monday, May 14, 2007  7:42 AM        87424 <A HREF=""/diskext.exe"">diskext.exe</A><br> Wednesday, November 01, 2006  1:06 PM       191288 <A HREF=""/Diskmnt.exe"">Diskmnt.exe</A><br>    Monday, December 08, 2003  9:40 AM         9519 <A HREF=""/Diskmnt.hlp"">Diskmnt.hlp</A><br> Wednesday, November 01, 2006  1:06 PM       224056 <A HREF=""/Diskmon.exe"">Diskmon.exe</A><br>    Monday, December 08, 2003  9:40 AM         9519 <A HREF=""/DISKMON.HLP"">DISKMON.HLP</A><br> Wednesday, November 01, 2006  1:06 PM       236400 <A HREF=""/DiskView.exe"">DiskView.exe</A><br>   Thursday, October 14, 1999 12:45 PM        11728 <A HREF=""/DMON.SYS"">DMON.SYS</A><br> Wednesday, December 10, 2008  2:40 PM       221040 <A HREF=""/du.exe"">du.exe</A><br> Wednesday, November 01, 2006  1:05 PM       146232 <A HREF=""/efsdump.exe"">efsdump.exe</A><br>        Friday, July 28, 2006  8:32 AM         7005 <A HREF=""/Eula.txt"">Eula.txt</A><br>    Monday, November 06, 2006 11:55 AM       748344 <A HREF=""/Filemon.exe"">Filemon.exe</A><br>     Thursday, March 20, 2003  4:26 PM        14619 <A HREF=""/FILEMON.HLP"">FILEMON.HLP</A><br>  Thursday, February 05, 2009 11:04 PM        &lt;dir&gt; <A HREF=""/Files/"">Files</A><br>   Tuesday, November 18, 2008  1:15 PM       417136 <A HREF=""/handle.exe"">handle.exe</A><br>   Tuesday, November 18, 2008  5:04 AM           16 <A HREF=""/healthmonitoring.html"">healthmonitoring.html</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/hex2dec.exe"">hex2dec.exe</A><br>       Tuesday, July 24, 2007  2:58 PM        95616 <A HREF=""/junction.exe"">junction.exe</A><br> Wednesday, November 01, 2006  1:06 PM       154424 <A HREF=""/ldmdump.exe"">ldmdump.exe</A><br> Wednesday, November 01, 2006  1:06 PM       170808 <A HREF=""/Listdlls.exe"">Listdlls.exe</A><br> Wednesday, November 01, 2006  1:07 PM       383800 <A HREF=""/livekd.exe"">livekd.exe</A><br> Wednesday, November 01, 2006  1:06 PM       154424 <A HREF=""/LoadOrd.exe"">LoadOrd.exe</A><br> Wednesday, November 01, 2006  1:06 PM       195384 <A HREF=""/logonsessions.exe"">logonsessions.exe</A><br> Wednesday, November 01, 2006  1:05 PM       146232 <A HREF=""/movefile.exe"">movefile.exe</A><br> Wednesday, November 01, 2006  1:06 PM       228152 <A HREF=""/newsid.exe"">newsid.exe</A><br> Wednesday, November 01, 2006  1:05 PM       122680 <A HREF=""/ntfsinfo.exe"">ntfsinfo.exe</A><br> Wednesday, November 01, 2006  1:06 PM       215928 <A HREF=""/pagedfrg.exe"">pagedfrg.exe</A><br>        Sunday, July 23, 2000  6:58 PM         8419 <A HREF=""/pagedfrg.hlp"">pagedfrg.hlp</A><br>      Friday, August 29, 2008  2:10 PM       155960 <A HREF=""/pdh.dll"">pdh.dll</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/pendmoves.exe"">pendmoves.exe</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/PHYSMEM.EXE"">PHYSMEM.EXE</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/pipelist.exe"">pipelist.exe</A><br>        Friday, July 30, 1999  3:28 PM          422 <A HREF=""/PORTMON.CNT"">PORTMON.CNT</A><br> Wednesday, November 01, 2006  1:07 PM       363320 <A HREF=""/portmon.exe"">portmon.exe</A><br>     Monday, January 31, 2000  8:20 AM        43428 <A HREF=""/PORTMON.HLP"">PORTMON.HLP</A><br>      Friday, August 31, 2007  5:36 AM        72138 <A HREF=""/procexp.chm"">procexp.chm</A><br>   Tuesday, February 03, 2009  6:32 PM      3550592 <A HREF=""/procexp.exe"">procexp.exe</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/ProcFeatures.exe"">ProcFeatures.exe</A><br>   Tuesday, December 09, 2008  5:38 PM        60772 <A HREF=""/procmon.chm"">procmon.chm</A><br> Wednesday, December 10, 2008  2:40 PM      2901872 <A HREF=""/Procmon.exe"">Procmon.exe</A><br>   Thursday, January 03, 2008 10:40 AM       234536 <A HREF=""/psexec.exe"">psexec.exe</A><br>    Monday, December 04, 2006  4:53 PM       105264 <A HREF=""/psfile.exe"">psfile.exe</A><br>    Monday, December 04, 2006  4:53 PM       187184 <A HREF=""/psgetsid.exe"">psgetsid.exe</A><br>        Monday, July 09, 2007 10:23 AM       243072 <A HREF=""/Psinfo.exe"">Psinfo.exe</A><br>    Monday, December 04, 2006  4:53 PM       187184 <A HREF=""/pskill.exe"">pskill.exe</A><br>    Monday, December 04, 2006  4:53 PM       125744 <A HREF=""/pslist.exe"">pslist.exe</A><br>    Monday, December 04, 2006  4:53 PM       105264 <A HREF=""/psloggedon.exe"">psloggedon.exe</A><br>    Monday, December 04, 2006  4:53 PM       113456 <A HREF=""/psloglist.exe"">psloglist.exe</A><br>    Monday, December 04, 2006  4:53 PM       105264 <A HREF=""/pspasswd.exe"">pspasswd.exe</A><br>  Wednesday, January 09, 2008  3:36 PM       107560 <A HREF=""/psservice.exe"">psservice.exe</A><br>    Monday, December 04, 2006  4:53 PM       207664 <A HREF=""/psshutdown.exe"">psshutdown.exe</A><br>    Monday, December 04, 2006  4:53 PM       187184 <A HREF=""/pssuspend.exe"">pssuspend.exe</A><br>  Saturday, February 10, 2007  8:46 AM        64126 <A HREF=""/Pstools.chm"">Pstools.chm</A><br>   Tuesday, November 06, 2007  8:17 AM           39 <A HREF=""/psversion.txt"">psversion.txt</A><br>   Tuesday, February 13, 2007  5:19 PM          214 <A HREF=""/ReadMe-SysinternalsUtilitiesIndex.url"">ReadMe-SysinternalsUtilitiesIndex.url</A><br> Wednesday, November 01, 2006  1:06 PM       162616 <A HREF=""/RegDelNull.exe"">RegDelNull.exe</A><br> Wednesday, November 01, 2006  1:05 PM       146232 <A HREF=""/Reghide.exe"">Reghide.exe</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/regjump.exe"">regjump.exe</A><br> Wednesday, November 01, 2006  1:07 PM       707384 <A HREF=""/Regmon.exe"">Regmon.exe</A><br>    Tuesday, October 22, 2002  7:48 AM        15031 <A HREF=""/REGMON.HLP"">REGMON.HLP</A><br> Wednesday, December 07, 2005  2:19 PM       102160 <A HREF=""/RootkitRevealer.chm"">RootkitRevealer.chm</A><br> Wednesday, November 01, 2006  1:07 PM       334720 <A HREF=""/RootkitRevealer.exe"">RootkitRevealer.exe</A><br> Wednesday, November 01, 2006  1:06 PM       166712 <A HREF=""/sdelete.exe"">sdelete.exe</A><br> Wednesday, November 01, 2006  1:07 PM       260976 <A HREF=""/ShareEnum.exe"">ShareEnum.exe</A><br> Wednesday, February 27, 2008  5:51 PM       103464 <A HREF=""/ShellRunas.exe"">ShellRunas.exe</A><br> Thursday, September 25, 2008  9:11 AM       222760 <A HREF=""/sigcheck.exe"">sigcheck.exe</A><br>       Friday, April 27, 2007  9:17 AM        87424 <A HREF=""/streams.exe"">streams.exe</A><br>      Tuesday, April 24, 2007 10:38 AM        91520 <A HREF=""/strings.exe"">strings.exe</A><br> Wednesday, November 01, 2006  1:05 PM       150328 <A HREF=""/sync.exe"">sync.exe</A><br>   Thursday, October 12, 2006  5:40 PM       716800 <A HREF=""/SysInternalsBluescreen.scr"">SysInternalsBluescreen.scr</A><br>   Thursday, January 03, 2008 10:40 AM       132136 <A HREF=""/tcpvcon.exe"">tcpvcon.exe</A><br>     Monday, October 30, 2006  9:32 AM        40016 <A HREF=""/tcpview.chm"">tcpview.chm</A><br>  Wednesday, January 09, 2008  3:38 PM       148520 <A HREF=""/Tcpview.exe"">Tcpview.exe</A><br>   Monday, September 02, 2002 12:13 PM         7983 <A HREF=""/TCPVIEW.HLP"">TCPVIEW.HLP</A><br>   Tuesday, November 18, 2008  1:22 AM        &lt;dir&gt; <A HREF=""/Tools/"">Tools</A><br> Wednesday, November 01, 2006  1:05 PM       154424 <A HREF=""/Volumeid.exe"">Volumeid.exe</A><br> Wednesday, November 01, 2006  1:06 PM       158520 <A HREF=""/whois.exe"">whois.exe</A><br>   Tuesday, November 18, 2008  1:10 AM        &lt;dir&gt; <A HREF=""/WindowsInternals/"">WindowsInternals</A><br> Wednesday, November 01, 2006  1:06 PM       207672 <A HREF=""/Winobj.exe"">Winobj.exe</A><br>  Thursday, December 30, 1999 10:26 AM         7653 <A HREF=""/WINOBJ.HLP"">WINOBJ.HLP</A><br>   Tuesday, February 03, 2009  6:32 PM       252288 <A HREF=""/ZoomIt.exe"">ZoomIt.exe</A><br></pre><hr></body></html>"

//---------------------------------------------------------------------------------------
    
let reParseLine =
    // e.g. Wednesday, December 17, 2008  9:11 PM       313200 <A HREF="/accesschk.exe">accesschk.exe</A>
    // new Regex(@"(?<day>[^ ]+) (?<date>.+)", RegexOptions.Compiled)
    new Regex(@"
   (?<day>[^ ]+)                # Wednesday,
   \s+
   (?<date>[^,]+)               # December 17,
   ,
   \s+
   (?<year>\S+)                 # 2008
   \s+
   (?<time>\S+)                 # 9:11
   \s+
   (?<AMPM>\S+)                 # PM
   \s+
   (?<size>\S+)                 # 313200 (or &lt;dir&gt;)
   \s+
   (?<APrefix>\<A\s+HREF=""/)   # <A HREF=""/
   (?<href>[^""]+)              # accesschk.exe
",
        RegexOptions.Compiled ||| RegexOptions.IgnorePatternWhitespace)

//---------------------------------------------------------------------------------------
        
let DownloadAndWriteFile filename date =
    let path = SiDirectory + filename
    try
        let html = GetHttpBinary (SiUrl + filename)
        File.WriteAllBytes(SiDirectory + filename, html)
        File.SetCreationTime(path, date)
    with
        | err -> printfn "\t*** Error - %s" err.Message

//---------------------------------------------------------------------------------------

let ProcessLine line =
    let matches = reParseLine.Match line
    // TODO: Check that the match worked
    printfn "Processing %A" matches.Groups.["href"]
    let sDate = sprintf "%A %A %A %A" matches.Groups.["date"] matches.Groups.["year"] matches.Groups.["time"] matches.Groups.["AMPM"]
    let date = DateTime.Parse sDate
    let size = ref 0
    let bOK = Int32.TryParse(matches.Groups.["size"].ToString(), size)
    let filename = sprintf "%A" matches.Groups.["href"]
//    if filename = "autoruns.exe" then       // TODO:
    if true then                            // Use previous line for testing
        // Check CreationDate and date. If equal, skip the download etc
        let path = SiDirectory + filename
        if File.Exists path then
            let fileCreationDate = File.GetCreationTime(path)
            if fileCreationDate < date then
                printfn "\t* Updated from %A to %A" fileCreationDate date
                DownloadAndWriteFile filename date
        else
            DownloadAndWriteFile filename date
    ()

//---------------------------------------------------------------------------------------

let Main() =
    let SiData = GetSiData()
    // Each line in the HTML fits into one of 3 classes (after being Trim'med):
    //  1) Starts with "<" (e.g. "<html>"). We don't want this.
    //  2) Represents a subdirectory, and has "&lt;dir&gt;". We don't want this either.
    //  3) Everything else. These we want.
    let lines = 
        SiData.Split([| "<br>" |], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.filter (fun line -> not ((line.StartsWith("<")) || (line.Contains("&lt;dir&gt;"))))
        |> Array.map (fun txt -> txt.Trim())
      
    for line in lines do
        ProcessLine line

//---------------------------------------------------------------------------------------

[<EntryPoint>]
Directory.CreateDirectory(SiDirectory) |> ignore
do Main()
printfn "Press any key to finish..."
let x = Console.ReadKey()



// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

// [<EntryPoint>]
// let main argv = 
//   printfn "%A" argv
//     0 // return an integer exit code

