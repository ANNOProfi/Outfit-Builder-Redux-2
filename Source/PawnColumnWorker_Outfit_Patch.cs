using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace OBR2
{
    [HarmonyPatch(typeof(PawnColumnWorker_Outfit))]
    internal class PawnColumnWorker_Outfit_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("DoCell")]
        private static bool Prefix(Rect rect, Pawn pawn, PawnTable table, PawnColumnWorker_Outfit __instance)
        {
            if (pawn.outfits.forcedHandler.SomethingIsForced)
            {
                if (pawn.outfits == null)
                {
                    return true;
                }
                float x = rect.x;
                float y = rect.y + 2f;
                float width = Mathf.FloorToInt(rect.width * 0.5f - 4f);
                float num = Mathf.FloorToInt(rect.width * 0.25f - 4f);
                float height = rect.height - 4f;
                _ = pawn.outfits.forcedHandler.SomethingIsForced;
                Rect rect2 = new Rect(x, y, width, height);
                if (pawn.IsQuestLodger())
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(rect2, "Unchangeable".Translate().Truncate(rect2.width));
                    TooltipHandler.TipRegionByKey(rect2, "QuestRelated_Outfit");
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                else
                {
                    Widgets.Dropdown(rect2, pawn, (Func<Pawn, ApparelPolicy>)((Pawn p) => p.outfits.CurrentApparelPolicy), (Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>>)OutfitBuilderRedux2.Button_GenerateMenu, pawn.outfits.CurrentApparelPolicy.label.Truncate(rect2.width), (Texture2D)null, pawn.outfits.CurrentApparelPolicy.label, (Texture2D)null, (Action)null, paintable: true);
                }
                x += rect2.width + 4f;
                Rect rect3 = new Rect(x, y, num + 3f, height);
                if (Widgets.ButtonText(rect3, "OBR.ClearForcedApparel".Translate().Truncate(rect3.width)))
                {
                    pawn.outfits.forcedHandler.Reset();
                }
                if (Mouse.IsOver(rect3))
                {
                    TooltipHandler.TipRegion(rect3, new TipSignal(delegate
                    {
                        string text = "ForcedApparel".Translate() + ":\n";
                        foreach (Apparel item in pawn.outfits.forcedHandler.ForcedApparel)
                        {
                            text = text + "\n   " + item.LabelCap;
                        }
                        return text;
                    }, pawn.GetHashCode() * 612));
                }
                x += rect3.width + 4f;
                Rect rect4 = new Rect(x, y, num + 2f, height);
                if (Widgets.ButtonText(rect4, "OBR.Save".Translate().Truncate(rect4.width)))
                {
                    if (Event.current.button == 0)
                    {
                        OutfitBuilderRedux2.OutfitBuilderRedux_Do(pawn, overwrite: true);
                    }
                    else if (Event.current.button == 1)
                    {
                        OutfitBuilderRedux2.OutfitBuilderRedux_Do(pawn, overwrite: false);
                    }
                }
                if (Mouse.IsOver(rect4))
                {
                    TooltipHandler.TipRegion(rect4, new TipSignal(() => "Left click: Save outfit (overwrites)\nRight click: Update outfit", pawn.GetHashCode() * 612));
                }
                return false;
            }
            return true;
        }
    }
}