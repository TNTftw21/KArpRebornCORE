using System;
using System.Linq;
using System.Reflection;

using Terraria;

namespace KArpRebornCORE
{
    public class CallHelper
    {
        public static object Call(params object[] args)
        {
            if (args.Length < 1 || args[0].GetType() != typeof(string))
                throw new ArgumentNullException("args[0] must be the name of a method in CallHelper!");

            object[] methodArgs = new object[0];
            if (args.Length > 1)
            //Get the parameters passed to us in args
                methodArgs = new ArraySegment<object>(args, 1, args.Length - 1).Array;
            //Get all of the methods we can Call
            MethodInfo[] methods = typeof(CallHelper).GetMethods();
            foreach (MethodInfo method in methods) {
                //args[0] should be the string name of the target method
                if (method.Name != args[0] as string)
                    continue;
                    
                ParameterInfo[] pars = method.GetParameters();
                //Make sure our args length matches the target
                if (methodArgs.Length != pars.Length)
                    continue;

                //Ensure that this method's parameters match the parameters in args
                for (int i = 0; i < pars.Length; i++) {
                    if (!pars[i].GetType().IsAssignableFrom(args[i+1].GetType()))
                        continue;
                }
                return method.Invoke(null, methodArgs);
            }
            //If we made it this far, then we never found the method we're looking for.
            string signature = (args[0] as string) + $"({ String.Join(", ", methodArgs.Select(o => o.GetType().Name)) })";

            throw new InvalidOperationException("No method exists with signature " + signature);
        }

        public static int LevelUp(int player)
        {
            Players.KArpPlayer c = Main.player[player].GetModPlayer<Players.KArpPlayer>();
            c.LevelUp();
            return c.level;
        }

        public static float ApplyDifficultyFactor(string id, float factor)
        {
            KArpRebornCOREMain.Mod.difficultyFactorTracker[id] = factor;
            return KArpRebornCOREMain.Mod.DifficultyFactor;
        }
    }
}