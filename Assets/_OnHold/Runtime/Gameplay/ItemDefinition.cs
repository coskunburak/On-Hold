using UnityEngine;
using OnHold.Core;

namespace OnHold.Gameplay
{
    [CreateAssetMenu(menuName = "On Hold/Item definition")]
    public sealed class ItemDefinition : ScriptableObject
    {
        public string DefinitionId, DisplayNameKey, ProvenanceId = "original.greybox.1";
        public float MassKg = 80;
        public int BaseValue = 100;
        public GameObject Prefab;
        public Vector3[] Grips = { new Vector3(-1, .1f, 0), new Vector3(1, .1f, 0) };
        public Vector3 BoundsCenter, BoundsSize = Vector3.one;
        public void Validate()
        {
            if (!Numbers.Id(DefinitionId) || !Numbers.Id(DisplayNameKey) || ProvenanceId != "original.greybox.1" ||
                !Numbers.Finite(MassKg) || MassKg <= 0 || MassKg > 1000 || BaseValue < 0 || Prefab == null || Grips == null || Grips.Length != 2 ||
                !SafeMath.Finite(BoundsSize) || !SafeMath.Finite(BoundsCenter) || BoundsSize.x <= 0 || BoundsSize.y <= 0 || BoundsSize.z <= 0)
                throw new System.ArgumentException("Invalid content: " + name);
            foreach (var grip in Grips) if (!SafeMath.Finite(grip)) throw new System.ArgumentException("Invalid grip");
        }
        public Vector3 Corner(int i) => BoundsCenter + Vector3.Scale(BoundsSize * .5f, new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1));
    }
    public static class SafeMath
    {
        public static bool Finite(Vector3 v) => Numbers.Finite(v.x) && Numbers.Finite(v.y) && Numbers.Finite(v.z);
        public static bool Finite(Quaternion q) => Numbers.Finite(q.x) && Numbers.Finite(q.y) && Numbers.Finite(q.z) && Numbers.Finite(q.w);
    }
}
