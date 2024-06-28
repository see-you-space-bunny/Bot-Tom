using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charsheet.LevelGame.Enums;

namespace Charsheet.LevelGame;

public static class ActorExtensions
{
    #region Actor Extensions
    public static T ChangeClass<T>(this T actor, ClassName className) where T : Actor
    {
        actor.ChangeClass(className);
        return actor;
    }

    public static T AddToResource<T>(this T actor, Resource resource,int value) where T : Actor
    {
        actor.AddToResource(resource,value);
        return actor;
    }

    public static T SetResource<T>(this T actor, Resource resource,int value) where T : Actor
    {
        actor.SetResource(resource,value);
        return actor;
    }
    #endregion
}