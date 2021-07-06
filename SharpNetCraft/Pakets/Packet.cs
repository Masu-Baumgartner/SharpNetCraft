using System;
using System.Diagnostics;
using System.Threading;

using SharpNetCraft.Packets;
using SharpNetCraft.Utils;

namespace SharpNetCraft
{
	public abstract class Packet : IPacket<MinecraftStream>
	{
		public bool ForceUnencrypted { get; set; } = false;
		public Stopwatch Stopwatch { get; } = new Stopwatch();

		public int PacketId { get; set; } = -1;

		public abstract void Decode(MinecraftStream stream);

		public abstract void Encode(MinecraftStream stream);
		
		public abstract void PutPool();
		
		public virtual void Reset()
		{
			ResetPacket();

			//_encodedMessage = null;
			//Bytes = null;
			Stopwatch.Restart();

			/*_writer?.Close();
			_reader?.Close();
			_buffer?.Close();
			_writer = null;
			_reader = null;
			_buffer = null;*/
		}

		protected virtual void ResetPacket()
		{
		}
	}

	public abstract class Packet<TPacket> : Packet, IDisposable where TPacket : Packet<TPacket>, new()
	{		
		private static readonly PacketPool<TPacket> Pool = new PacketPool<TPacket>(() => new TPacket());
		private bool _isPermanent;
		private bool _isPooled;
		private long _referenceCounter;
		public bool IsPooled
		{
			get { return _isPooled; }
		}
		public long ReferenceCounter
		{
			get { return _referenceCounter; }
			set { _referenceCounter = value; }
		}


		public TPacket MarkPermanent(bool permanent = true)
		{
			if (!_isPooled) throw new Exception("Tried to make non pooled item permanent");
			_isPermanent = permanent;

			return (TPacket) this;
		}

		public TPacket AddReferences(long numberOfReferences)
		{
			if (_isPermanent) return (TPacket) this;

			if (!_isPooled) throw new Exception("Tried to reference count a non pooled item");
			Interlocked.Add(ref _referenceCounter, numberOfReferences);

			return (TPacket) this;
		}

		public TPacket AddReference(Packet<TPacket> item)
		{
			if (_isPermanent) return (TPacket) this;

			if (!item.IsPooled) throw new Exception("Item template needs to come from a pool");

			Interlocked.Increment(ref item._referenceCounter);
			return (TPacket) item;
		}

		public TPacket MakePoolable(long numberOfReferences = 1)
		{
			_isPooled = true;
			_referenceCounter = numberOfReferences;
			return (TPacket) this;
		}


		public static TPacket CreateObject(long numberOfReferences = 1)
		{
			TPacket item = Pool.GetObject();
			item._isPooled = true;
			item._referenceCounter = numberOfReferences;
			item.Stopwatch.Restart();
			return item;
		}
		
		public override void PutPool()
		{
			if (_isPermanent) return;

			if (!IsPooled)
			{
				Logger.GetLogger().Warn($"Tried pooling non-pooled packet 0x{PacketId:x2} NULL, IsPooled={_isPooled}, IsPermanent={_isPermanent}, Refs={_referenceCounter}");
				return;
			}

			long counter = Interlocked.Decrement(ref _referenceCounter);
			if (counter > 0) return;

			if (counter < 0)
			{
				Logger.GetLogger().Error($"Pooling error. Added pooled object too many times. 0x{PacketId:x2} NULL, IsPooled={IsPooled}, IsPooled={_isPermanent}, Refs={_referenceCounter}");
				return;
			}

			Reset();

			_isPooled = false;

			//Pool.PutObject((T) this);
		}

		public void Dispose()
		{
			PutPool();
		}
	}
}
