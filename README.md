# CEFSharp-in-GameMaker-Studio
An implementation of CEFSharp (Chromium Embedded Framework, C#) in GameMaker Studio. 
----
**Set up**  

When you open the source (GM: S), you will need to do a few things for set up. For a start, you need to download the CefSharp binaries either from the souce folder or the Example Executables folder (link: [here](https://github.com/GameMakerDevsSlack/CEFSharp-in-GameMaker-Studio/tree/master/Example%20Executable/CEFSharp) ) and copy the path to CefSharp.exe within that folder, replacing the blank string.  
**Release**  
Ensure that the CEFSharp folder is bundled with the game. Also, ensure that `GAME_FILENAME` is set to the executable name (and that if you use the object directly from the project, set RELEASE to 1). 
