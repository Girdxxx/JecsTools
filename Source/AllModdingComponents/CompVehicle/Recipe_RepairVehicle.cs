﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using System.Linq;

namespace CompPilotable
{
    internal class Recipe_RepairVehicle : RecipeWorker
    {
        [DebuggerHidden]
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            List<Hediff> brokenParts = pawn.health.hediffSet.hediffs.FindAll((Hediff x) => x is Hediff_Injury);
            if (brokenParts != null && brokenParts.Count > 0)
            {
                foreach (Hediff brokenPart in brokenParts)
                {
                    if (brokenPart.Part != null)
                        yield return brokenPart.Part;
                }
            }
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
        {
            if (pawn != null)
            {
                foreach (BodyPartRecord rec in pawn.health.hediffSet.GetInjuredParts())
                {
                    foreach (Hediff_Injury current in from injury in pawn.health.hediffSet.GetHediffs<Hediff_Injury>() where injury.Part == rec select injury)
                    {
                        if (rec == part) current.Heal((int)current.Severity + 1);
                    }
                }
            }
            //pawn.health.AddHediff(this.recipe.addsHediff, part, null);
            //ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
        }
    }
}
