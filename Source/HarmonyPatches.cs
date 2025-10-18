using System.Reflection;
using HarmonyLib;
using Verse;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
	static HarmonyPatches()
	{
		new Harmony("RimWorld.ANNOProfi.OBR2").PatchAll(Assembly.GetExecutingAssembly());
	}
}