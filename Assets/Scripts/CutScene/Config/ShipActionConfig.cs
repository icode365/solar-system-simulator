using DG.Tweening;
using Scripts.CutScene;
using SpaceShip;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipActionConfig", menuName = "CutScene/Create Ship Action Config")]
public class ShipActionConfig : ActionConfigBase
{
    public override Sequence GetSequence(CutSceneContext ctx)
    {
        var ship = ctx.Resolve<Transform>();
        ship.GetComponent<SpaceShipController>().enabled = false;
        return null;
    }

    public override void WrapUp(CutSceneContext ctx)
    {
        var ship = ctx.Resolve<Transform>();
        ship.GetComponent<SpaceShipController>().enabled = true;
    }
}