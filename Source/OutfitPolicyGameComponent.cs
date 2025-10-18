using System.Collections.Generic;
using RimWorld;
using Verse;

namespace OBR2
{
    public class OutfitPolicyGameComponent : GameComponent
    {
        private static Dictionary<Pawn, int> storedOutfits = new Dictionary<Pawn, int>();

        private static List<Pawn> allPawns = new List<Pawn>();

        private static List<int> outfitIdentifiers = new List<int>();

        public OutfitPolicyGameComponent(Game game)
        {
            List<Pawn> list = new List<Pawn>();
            foreach (Pawn key in storedOutfits.Keys)
            {
                if (key.DestroyedOrNull())
                {
                    list.Add(key);
                }
            }
            foreach (Pawn item in list)
            {
                storedOutfits.Remove(item);
            }
        }

        public ApparelPolicy OutfitAssignedToPawn(Pawn pawn)
        {
            if (storedOutfits == null)
            {
                storedOutfits = new Dictionary<Pawn, int>();
            }
            if (storedOutfits.ContainsKey(pawn))
            {
                return Current.Game.outfitDatabase.AllOutfits.Find((ApparelPolicy s) => s.id == storedOutfits[pawn]);
            }
            return null;
        }

        public ApparelPolicy CreateNewOutfit(Pawn pawn)
        {
            ApparelPolicy apparelPolicy = Current.Game.outfitDatabase.MakeNewOutfit();
            apparelPolicy.label = pawn.Name.ToStringShort.CapitalizeFirst();
            storedOutfits.Add(pawn, apparelPolicy.id);
            return apparelPolicy;
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref storedOutfits, "storedOutfits", LookMode.Reference, LookMode.Value, ref allPawns, ref outfitIdentifiers);
        }
    }
}