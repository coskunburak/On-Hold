using System.Collections.Generic;
using OnHold.Core;

namespace OnHold.Domain
{
    public sealed class SupportedSet
    {
        sealed class Entry { public double Mass, X, Z, Changed; public bool Contact, Included; }
        readonly Dictionary<string, Entry> entries = new Dictionary<string, Entry>();
        public double Mass { get; private set; }
        public double CentreX { get; private set; }
        public double CentreZ { get; private set; }
        public int Count { get; private set; }
        public void Observe(string id, double mass, double x, double z, bool contact, bool secured, double now, FoundationConfig c)
        {
            if (!Numbers.Id(id) || !Numbers.Finite(mass) || mass <= 0 || !Numbers.Finite(x) || !Numbers.Finite(z)) return;
            if (!entries.TryGetValue(id, out var e)) { e = new Entry { Changed = now, Contact = contact }; entries.Add(id, e); }
            if (e.Contact != contact) { e.Contact = contact; e.Changed = now; }
            e.Mass = mass; e.X = x; e.Z = z;
            if (secured) e.Included = true;
            else if (now - e.Changed >= (contact ? c.SupportEnter : c.SupportExit)) e.Included = contact;
        }
        public void Remove(string id) => entries.Remove(id);
        public bool Contains(string id) => entries.TryGetValue(id, out var e) && e.Included;
        public void Calculate(double baseMass)
        {
            double sumX = 0, sumZ = 0; Mass = baseMass; Count = 0;
            foreach (var e in entries.Values) if (e.Included) { Mass += e.Mass; sumX += e.Mass * e.X; sumZ += e.Mass * e.Z; Count++; }
            CentreX = Mass > 0 ? sumX / Mass : 0; CentreZ = Mass > 0 ? sumZ / Mass : 0;
        }
    }
}
