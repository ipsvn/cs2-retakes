﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace RetakesPlugin.Modules.Managers;

public class BreakerManager
{
    private readonly bool _isBreakerEnabled;
    private readonly bool _shouldOpenDoors;
    
    public BreakerManager(bool? isBreakerEnabled, bool? shouldOpenDoors)
    {
        _isBreakerEnabled = isBreakerEnabled ?? false;
        _shouldOpenDoors = shouldOpenDoors ?? false;
    }

    public void Handle()
    {
        if (!_isBreakerEnabled)
        {
            return;
        }
        
        DestroyBreakables();
        
        if (_shouldOpenDoors)
        {
            OpenDoors();
        }
    }
    
    private static void DestroyBreakables()
    {
        var breakableEntities = 
            Utilities.FindAllEntitiesByDesignerName<CBreakable>("func_breakable")
                .Concat(Utilities.FindAllEntitiesByDesignerName<CBreakable>("func_breakable_surf"))
                .Concat(Utilities.FindAllEntitiesByDesignerName<CBreakable>("prop_dynamic"))
            ;

        foreach (var breakableEntity in breakableEntities)
        {
            breakableEntity.AcceptInput("Break");
        }
        
        var breakableProps = Utilities.FindAllEntitiesByDesignerName<CBreakableProp>("prop.breakable.01")
            .Concat(Utilities.FindAllEntitiesByDesignerName<CBreakableProp>("prop.breakable.02"));
        
        foreach (var breakableProp in breakableProps)
        {
            breakableProp.AcceptInput("break");
        }

        if (Server.MapName == "de_vertigo" || Server.MapName == "de_cache" || Server.MapName == "de_nuke")
        {
            var dynamicProps = Utilities.FindAllEntitiesByDesignerName<CDynamicProp>("prop_dynamic");
            
            foreach (var dynamicProp in dynamicProps)
            {
                dynamicProp.AcceptInput("Break");
            }
        }

        switch (Server.MapName)
        {
            case "de_nuke":
                var buttonEntities = Utilities.FindAllEntitiesByDesignerName<CBaseButton>("func_button");
                
                foreach (var buttonEntity in buttonEntities)
                {
                    buttonEntity.AcceptInput("Kill");
                }
                break;
        }
    }

    private static void OpenDoors()
    {
        var doorEntities = Utilities.FindAllEntitiesByDesignerName<CPropDoorRotating>("prop_door_rotating");

        foreach (var doorEntity in doorEntities)
        {
            doorEntity.AcceptInput("open");
        }
    }
}
