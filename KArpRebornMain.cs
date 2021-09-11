using Terraria.ModLoader;
using System.Collections.Generic;
using System.IO;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace KArpReborn
{
	public class KArpRebornMain : Mod
	{

		public static KArpRebornMain Mod { get; private set; }
		public Core.Proxies.CommonProxy proxy { get; private set; }

		public KArpRebornMain()
		{
			Mod = this;
			if (Terraria.Main.dedServ)
				proxy = new Core.Proxies.ServerProxy();
			else
				proxy = new Core.Proxies.ClientProxy();
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
			if (proxy.GetType() == typeof(Core.Proxies.ClientProxy))
				(proxy as Core.Proxies.ClientProxy).UpdateUI(gameTime);
			
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			if (proxy.GetType() == typeof(Core.Proxies.ClientProxy))
				(proxy as Core.Proxies.ClientProxy).ModifyInterfaceLayers(layers);
		}


		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			Core.Networking.Handler.HandlePacket(reader, whoAmI);
		}
	}
}