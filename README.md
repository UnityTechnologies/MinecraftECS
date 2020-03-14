# MinecraftECS with Unity 2019.3.3 & Latest ECS Packages
> This is an upgrade to Unity 2019.3.3 forked from the offical Unity MinecraftECS repository. The original Unity version was 2018.2.19.

The code is about how to create a Minecraft-like environment with huge number of blocks using [ECS (Entity Component System)](https://docs.unity3d.com/Packages/com.unity.entities@0.8/manual/index.html).

![Imgur](https://imgur.com/NDtCOQg.png)

## Required Packages 
| Name            | Version           |
|-----------------|-------------------|
| Entities        | preview.8 - 0.8.0 |
| Hybrid Renderer | preview.8 - 0.4.0 |
| Burst           | preview.6 - 1.3.0 |

## Usage (recommended steps)
1. Click "Download Unity Hub" at the [Unity official download website](https://unity3d.com/get-unity/download). 
> If you are using this for the first time, you may be promted to login / register a Unity account and activate a license. Click "Activate New License" -> "Unity Personal" should be fine. Just follow the instructions. :)
2. Install Unity version 2019.3.3 by clicking the green button next to "Unity 2019.3.3" with the text "Unity Hub" [here](https://unity3d.com/get-unity/download/archive). 
3. Clone this respository by entering this line in your terminal (You can also download it as a zip file):
```
git clone https://github.com/tina1998612/MinecraftECS.git
```
4. Open Unity Hub and click the "ADD" button on the top right corner to load our project, then select our project folder.
5. After adding the project, it should appear on the first line under the "Projects" section. Double click to open it in the Unity Editor.
> If you find that the importing process is running for too long, it is completely normal :P
6. After opening Unity Editor, remember to install the packages listed above. In the top menu bar, click "Window" -> "Package Manager", then "Advanced" -> "Show Preview Packages", and enter the package name in the search bar. The install button is on the bottom right. 
Close the package manager window after you finish installing.
7. In Unity Editor, go to the "Project" tab at the bottom, double click the "Scenes" folder, and then double click the first scene (the leftmost black cube). 
8. Press `Ctrl + P` to play the scene (or `Cmd + P` if you are using Mac). Press `Ctrl + P` again to exit play mode. 
9. In the left panel called "Hierarchy", click on the "GameSettings" object to see the "Inspector" on the right. You can customize the size of the Minecraft world you'd like to generate there (by changing the value of the "Chunk Base").
10. Finally, press `Ctrl + P` to play the scene with your customized settings! 

## Notes 
Feel free to let me know if you encounter any problem when running the project. Suggestions on how to optimize the code are also highly welcomed. 




*-- <br>
Happy coding, <br>
tinaaaaalee* 🍀

