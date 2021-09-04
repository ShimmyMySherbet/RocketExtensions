# RocketExtensions
Extends the functonality of Unturned Rocketmod, including async UniTask commands.

This library is compatible with normal, unmodified rocketmod. You just need to include this library and UniTask with your plugin's libraries.

### Commands
* **Commands inherit `RocketCommand`**
* Commands are async and use UniTask, starting on the thread pool
* Command details are defined using attributes: `Aliases`, `AllowedCaller`, `CommandInfo`, and `CommandName`
* Comand name defaults to the class name without 'Command'
* Command Permissions defaults to *{AssemblyName}*.*{CommandName}*
* Allowed Caller defaults to both
* Context is passed in `(CommandContext context)` in the execute method.

### Arguments
* Arguments can be accessed through context.Arguments.Get<...>
* Strict argument: `T context.Arguments.Get<T>(int index)`
* Optional argument: `T context.Arguments.Get<T>(int index, T defaultValue)`
* With strict arguments, if the argument is missing, or is invalid for the specified type, the method will throw `ArgumentOutOfRangeException`, `InvalidCastException`, or `InvalidArgumentException`
* If the command does not handle these exceptions, it will print a relevant message to the user. 
* Supported Argument Types: `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `bool`, `Player`, `SteamPlayer`, `UnturnedPlayer`

### Misc
* Thread tools to leverage easier multi-threading even outside of UniTask commands
* `FastTaskDispatcher` for a higher-performance game thread dispatcher compared to the built-in Rocketmod one.
