XnaUnitySpriteBatch
===================

Provides a Xna SpriteBatch implementation for Unity use, allowing for quick and rough porting of Xna projects to Unity.

Related reading:
http://shadowmint.blogspot.com/2012/11/display-textured-2d-quad-in-unity-using.html
http://docs.unity3d.com/Documentation/Manual/UsingDLL.html

Most of the code is based on MonoGame, we reuse classes where possible and replace them when necessary.
Some classes are cludged around for features that we do not support.
The csproj expect that MonoGame is checked out adjacent to it, and references the MonoGame files via links.

To use this with unity creates a new solution file reference this, a new solution for your game, build and then place the DLLs in the unity assets folder.
See the unity documentation linked above for more details.

List of classes that we have replaced:
Audio/SoundEffect
Content/ContentManager
Graphics/Effect/Effect
Graphics/GraphicsDevice
Graphics/States/*
Graphics/SpriteBatch (this is based on a copy/paste of the MonoGame implementation)
Graphics/Texture2D
Media/Song
Game
GraphicsDeviceManager

To use:
Set up a unity GameObject
In the constructor create your game object and call UnityInitialise
Add a FixedUpdate function and call UnityFixedUpdate
Add a Update function and call UnityUpdate