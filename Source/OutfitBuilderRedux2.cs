using Verse;
using RimWorld;
using System.Collections.Generic;

namespace OBR2
{
    [StaticConstructorOnStartup]
    public static class OutfitBuilderRedux2
    {
        static OutfitBuilderRedux2()
        {
            ModLog.Log("Outfit Builder Redux^2 loaded successfully!");
        }

        public static void OutfitBuilderRedux_Do(Pawn pawn, bool overwrite)
        {
            OutfitPolicyGameComponent component = Current.Game.GetComponent<OutfitPolicyGameComponent>();
            ApparelPolicy apparelPolicy = component.OutfitAssignedToPawn(pawn);
            if (apparelPolicy == null)
            {
                apparelPolicy = component.CreateNewOutfit(pawn);
            }
            if (overwrite)
            {
                apparelPolicy.filter.SetDisallowAll();
            }
            else if (pawn.outfits.CurrentApparelPolicy.label != pawn.Name.ToStringShort.CapitalizeFirst())
            {
                apparelPolicy.filter.CopyAllowancesFrom(pawn.outfits.CurrentApparelPolicy.filter);
            }
            foreach (Apparel item in pawn.apparel.WornApparel)
            {
                apparelPolicy.filter.SetAllow(item.def, allow: true);
            }
            pawn.outfits.CurrentApparelPolicy = apparelPolicy;
            pawn.outfits.forcedHandler.Reset();
        }

        public static IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>> Button_GenerateMenu(Pawn pawn)
        {
            foreach (ApparelPolicy outfit in Current.Game.outfitDatabase.AllOutfits)
            {
                yield return new Widgets.DropdownMenuElement<ApparelPolicy>
                {
                    option = new FloatMenuOption(outfit.label, delegate
                    {
                        pawn.outfits.CurrentApparelPolicy = outfit;
                    }),
                    payload = outfit
                };
            }
            yield return new Widgets.DropdownMenuElement<ApparelPolicy>
            {
                option = new FloatMenuOption(string.Format("{0}...", "AssignTabEdit".Translate()), delegate
                {
                    Find.WindowStack.Add(new Dialog_ManageApparelPolicies(pawn.outfits.CurrentApparelPolicy));
                })
            };
        }
    }
}
