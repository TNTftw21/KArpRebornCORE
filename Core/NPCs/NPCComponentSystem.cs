using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;

using KArpReborn.Core.NPCs.Components;

namespace KArpReborn.Core.NPCs
{
    public class NPCComponentSystem : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private List<Component> components = new List<Component>();
        private NPC npc;

        public override void SetDefaults(NPC npc)
        {
            this.npc = npc;
        }

        public T TryGetComponent<T>(Func<T> initializer) where T : Component {
            T comp = GetComponent<T>();
            if (comp == null)
                comp = AddComponent(initializer.Invoke());
            return comp;
        }

        public T GetComponent<T>() where T : Component {
            Component comp;
            //Search for a matching component, starting at the end
            for (int i = components.Count - 1; i >= 0; i--)
            {
                comp = components[i];
                if (comp is T)
                    return (T) comp;
            }
            return default(T);
        }

        public T[] GetComponents<T>() where T : Component {
            List<T> comps = new List<T>(components.Count);
            Component comp;
            for (int i = 0; i < components.Count; i++)
            {
                comp = components[i];
                if (comp is T)
                    comps.Add((T) comp);
            }
            return comps.ToArray();
        }

        public T[] GetComponentsExact<T>() where T : Component {
            List<T> comps = new List<T>(components.Count);
            Component comp;
            for (int i = 0; i < components.Count; i++)
            {
                comp = components[i];
                if (comp.GetType() == typeof(T))
                    comps.Add((T) comp);
            }
            return comps.ToArray();
        }

        public Component GetComponentExact<T>() where T : Component {
            Component comp;
            for (int i = components.Count - 1; i >= 0; i--)
            {
                comp = components[i];
                if (comp.GetType() == typeof(T))
                    return comp;
            }
            return null;
        }

        public T AddComponent<T>(T comp) where T : Component {
            this.components.Add(comp);
            comp.npc = npc;
            return comp;
        }

        public void RemoveComponent(Component comp) {
            components.Remove(comp);
        }

        public void RemoveComponents<T>() where T : Component {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T)
                    components.RemoveAt(i);
            }
        }

        public void RemoveComponentsExact<T>() where T : Component {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == typeof(T))
                    components.RemoveAt(i);
            }
        }
    }
}
