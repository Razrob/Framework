using Framework.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[FrameworkSystem(State.Menu)]
public class StandProductsVisualFiller : FSystemFoundation
{
    [InjectModel] private AAAModel AAAModel;

    [Executable] protected override void OnInitialize()
    {
        //AAAModel.BBBModel = new List<BBBModel>();

        //AAAModel.BBBModel.Add(new BBBModel { Value = 11 });
        //AAAModel.BBBModel.Add(new BBBModel { Value = 22 });
        //AAAModel.BBBModel.Add(new BBBModel { Value = 33 });

        //FrameworkCommander.SaveModel();

        Debug.Log(AAAModel.BBBModel[0].CCModel);

        //foreach (BBBModel bBBModel in AAAModel.BBBModel)
        //    bBBModel.OnInject();

    }
}

[Serializable]
[InternalModel(true)]
public class AAAModel : InternalModel
{
    [InjectModel] public List<BBBModel> BBBModel;
}


[Serializable]
[InternalModel(true)]
public class BBBModel : InternalModel
{
    //[InjectSelector] private ComponentSelector<MovementComponent> movementComponents;
    [InjectField] private GameObject prefab;
    [InjectModel] public CCModel CCModel;
    public int Value;

    protected override void OnInject()
    {
        Debug.Log(CCModel);
    }
}

[Serializable]
[InternalModel(true)]
public class CCModel : InternalModel
{
    public int value;
    [InjectSelector] private ComponentSelector<MovementComponent> movementComponents;

    public void Debugf()
    {
        Debug.Log(movementComponents.Components.Count());
    }
}

