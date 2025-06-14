using RimWorld;
using Verse;

namespace Armorshield;

public class CompShieldRanged : CompShield
{
	public override bool CompAllowVerbCast(Verb verb)
	{
		return true;
	}
}
