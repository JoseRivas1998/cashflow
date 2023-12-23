using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackMoveState : GameState
{
    private const float moveTimer = 0.4f;
    private int currentSpace;
    private readonly int targetSpace;
    private readonly Player player;

    private bool atDestination;

    public FastTrackMoveState(MainGameManager mgm, in int diceSum)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        currentSpace = player.space;
        targetSpace = mgm.board.NormalizeFastTrackSpace(currentSpace + diceSum);
        int nextSpace = mgm.board.NormalizeFastTrackSpace(currentSpace + 1);
        Vector3 nextSpaceCenter = mgm.board.FastTrackSpaceCenter(nextSpace);
        PolarPoint offset = new PolarPoint(mgm.board.ratRaceSpaceCenterThreshold, Random.Range(0f, Mathf.PI * 2f));
        Vector3 targetPostion = new Vector3(nextSpaceCenter.x, player.gamePiece.transform.position.y, nextSpaceCenter.z) + offset.toVector3XZ;
        player.gamePiece.SetFastTrackSpace(nextSpace);
        LeanTween.move(player.gamePiece.gameObject, targetPostion, moveTimer)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() => NextSpace(mgm));

        atDestination = false;
        mgm.gameStateDisplay.SetText($"Moving {diceSum} space{(diceSum > 1 ? "s" : "")}");
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (atDestination)
        {
            player.space = targetSpace;
            var spaceType = mgm.board.GetFastTrackSpaceType(player.space);
            switch (spaceType)
            {
                case FastTrackSpaceType.BusinessInvestments:
                    return new BusinessInvestmentState(mgm, (BusinessInvestment)mgm.board.GetFastTrackSpace(player.space));
                case FastTrackSpaceType.HealthCare:
                    return new HealthCareState(mgm);
                case FastTrackSpaceType.Charity:
                    return new FastTrackCharityState(mgm);
                case FastTrackSpaceType.TaxAudit:
                    return new LoseHalfCashState(mgm, FastTrackSpaceType.TaxAudit);
                case FastTrackSpaceType.Divorce:
                    return new LoseHalfCashState(mgm, FastTrackSpaceType.Divorce);
                case FastTrackSpaceType.Lawsuit:
                    return new LoseHalfCashState(mgm, FastTrackSpaceType.Lawsuit);
                case FastTrackSpaceType.BadPartner:
                    return new BadPartnerState(mgm);
                case FastTrackSpaceType.ForeignOilDeal:
                    return new ForeignOilDealState(mgm);
                case FastTrackSpaceType.SoftwareCoIPO:
                case FastTrackSpaceType.BioTecCoIPO:
                    return new FastTrackCashGambleState(mgm, spaceType);
                case FastTrackSpaceType.UnforeseenRepairs:
                    return new UnforeseenRepairsState(mgm);
                case FastTrackSpaceType.CashFlowDay:
                default:
                    break;
            }
            return new FastTrackPostTurnState(mgm);
        }
        return this;
    }

    private void NextSpace(MainGameManager mgm)
    {
        currentSpace = mgm.board.NormalizeFastTrackSpace(currentSpace + 1);
        if (currentSpace == targetSpace)
        {
            atDestination = true;
            return;
        }
        int nextSpace = mgm.board.NormalizeFastTrackSpace(currentSpace + 1);
        Vector3 nextSpaceCenter = mgm.board.FastTrackSpaceCenter(nextSpace);
        PolarPoint offset = new PolarPoint(mgm.board.ratRaceSpaceCenterThreshold, Random.Range(0f, Mathf.PI * 2f));
        Vector3 targetPostion = new Vector3(nextSpaceCenter.x, player.gamePiece.transform.position.y, nextSpaceCenter.z) + offset.toVector3XZ;
        player.gamePiece.SetFastTrackSpace(nextSpace);
        LeanTween.move(player.gamePiece.gameObject, targetPostion, moveTimer)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() => NextSpace(mgm));
    }

}
