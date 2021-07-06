using System;
using SharpNetCraft.Utils;
using fNbt;
using SharpNetCraft.Utils.Data;

namespace SharpNetCraft.Pakets.Play
{
	public class RespawnPacket : Packet<RespawnPacket>
	{
		public NbtCompound Dimension;
		public byte        Difficulty;
		public GameMode    Gamemode, PreviousGamemode;
		public string      WorldName;
		public long        HashedSeed;
		public bool        IsDebug, IsFlat, CopyMetadata;
		
		public override void Decode(MinecraftStream stream)
		{
			Dimension = stream.ReadNbtCompound();
			WorldName = stream.ReadString();
			HashedSeed = stream.ReadLong();
			Gamemode = new GameMode(stream.ReadByte());
			PreviousGamemode = new GameMode(stream.ReadByte());
			IsDebug = stream.ReadBool();
			IsFlat = stream.ReadBool();
			CopyMetadata = stream.ReadBool();
		}

		public override void Encode(MinecraftStream stream)
		{
			throw new NotImplementedException();
		}
	}
}
