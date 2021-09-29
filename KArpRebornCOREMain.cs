using System;
using System.IO;
using System.Collections.Generic;

using Terraria.UI;

using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace KArpRebornCORE
{
	public class KArpRebornCOREMain : Mod
	{

		public static KArpRebornCOREMain Mod { get; private set; }
		public Proxies.ServerProxy proxy { get; private set; }
		public Dictionary<string, float> difficultyFactorTracker { get; private set; }
		public float DifficultyFactor { get {
			float value = 1.0f;
			foreach (float i in difficultyFactorTracker.Values)
				value *= i;
			return value;
		} }

		public KArpRebornCOREMain()
		{
			Mod = this;
			if (Terraria.Main.dedServ)
				proxy = new Proxies.ServerProxy();
			else
				proxy = new Proxies.ClientProxy();
		}

		public override object Call(params object[] args)
		{
			try {
				return CallHelper.Call(args);
			} catch (Exception e) {
				Logger.Error("Error in Call: " + e);
				return null;
			}
		}

		public override void Load()
		{
			difficultyFactorTracker = new Dictionary<string, float>();
			proxy.Load();
			Logger.InfoFormat("{0} successfully loaded", Name);
		}

		public override void PostSetupContent()
		{
			proxy.PostSetupContent();
		}

		public override void Unload()
		{
			Mod = null;
			proxy.Unload();
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (proxy.GetType() == typeof(Proxies.ClientProxy))
				(proxy as Proxies.ClientProxy).UpdateUI(gameTime);
			
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			if (proxy.GetType() == typeof(Proxies.ClientProxy))
				(proxy as Proxies.ClientProxy).ModifyInterfaceLayers(layers);
		}


		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			Networking.Handler.HandlePacket(reader, whoAmI);
		}
	}
}