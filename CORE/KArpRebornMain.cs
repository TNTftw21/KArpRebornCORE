using Terraria.ModLoader;
using System.Collections.Generic;
using System.IO;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace KArpReborn.CORE
{
	public class KArpRebornCOREMain : Mod
	{

		public static KArpRebornCOREMain Mod { get; private set; }
		public CORE.Proxies.CommonProxy proxy { get; private set; }

		public KArpRebornCOREMain()
		{
			Mod = this;
			if (Terraria.Main.dedServ)
				proxy = new CORE.Proxies.ServerProxy();
			else
				proxy = new CORE.Proxies.ClientProxy();
		}

		public override void Load()
		{
			Logger.InfoFormat("{0} successfully loaded", Name);
			proxy.Load();
		}

		public override void Unload()
		{
			Mod = null;
			proxy.Unload();
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (proxy.GetType() == typeof(CORE.Proxies.ClientProxy))
				(proxy as CORE.Proxies.ClientProxy).UpdateUI(gameTime);
			
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			if (proxy.GetType() == typeof(CORE.Proxies.ClientProxy))
				(proxy as CORE.Proxies.ClientProxy).ModifyInterfaceLayers(layers);
		}


		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			CORE.Networking.Handler.HandlePacket(reader, whoAmI);
		}
	}
}