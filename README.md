# SharpNetCraft
A minecraft protocol library in C#

### Whats is this for? ###
This is a library written in c# for making apps
which uses minecrafts protocol. A example project is already included in this repo

### License ###
My project use other libraries and repos

> https://github.com/kennyvv/Alex
> 
> https://github.com/NiclasOlofsson/MiNET
> 
> https://github.com/mstefarov/fNbt
> 
> https://github.com/innocenzi/MojangSharp
> 
> https://www.bouncycastle.org/
>
> Newtonsoft.JSON (Added via Nugget)

For using this library please check out their licenses

### Example ###
#### Connect to a server (Offlinemode Server) ####
```csharp
MCPacketFactory.Load();
MinecraftClient client = new MinecraftClient();
client.SetOfflineUsername("Test User");
client.Connect("127.0.0.1", 25565);
```

#### Connect to a server (Onlineode Server) ####
```csharp
MCPacketFactory.Load();
MinecraftClient client = new MinecraftClient();
client.Login("email@example.com", "password");
client.Connect("127.0.0.1", 25565);
```

#### Reading incomming packets ####
For reading packets you need to create a hook aka. a packethandler
Example:
```csharp
public class MyHook : IPacketHandler
{
  public void HandleHandshake(Packet packet)
  {}
  
  public void HandleLogin(Packet packet)
  {}
  
  public void HandlePlay(Packet packet)
  {
    if(packet is ChatMessagePacket)
    {
      ChatMessagePacket cmp = (ChatMessagePacket)packet;
      
      if(cmp.Position == ChatMessagePacket.Chat)
      {
        Console.WriteLine("Message from server: " + cmp.Message);
      }
    }
  }

  public void HandleStatus(Packet packet)
  {}
}
```
This example shows all chat messages from a player in the console.
To activate this "Hook" you need to set it on the MinecraftClient object before
connecting
```csharp
client.hook = new MyHook();
// Then connect
```

### Minecraft Version ###
This project is for the minecraft version 1.17
I will update it when 1.18 comes out
