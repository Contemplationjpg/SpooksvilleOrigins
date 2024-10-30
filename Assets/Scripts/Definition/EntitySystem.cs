using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "New EntityDatabase", menuName = "Entity/EntityDatabase")]
public class EntitySystem : ScriptableObject
{
    public EntityType[] entityDatabase;
    public static EntitySystem instance;

    public void RefreshDatabase()
    {
        instance = this;
    }

     public EntityType GetEntity(int entityID)
    {   
        EntityType getEntity = CreateInstance<EntityType>();

        if (entityID < entityDatabase.Length)
        {
            getEntity = entityDatabase[entityID];
            Debug.Log("entity at ID " + entityID + " is " + getEntity.entityName);
        }
        
        return getEntity;
    }

    public EntityType GetEntity(string entityName)
    {
        EntityType getEntity = CreateInstance<EntityType>();
        
        for (int i = 0; i < entityDatabase.Length;i++)
        {
            if (entityName == entityDatabase[i].entityName)
            {
                getEntity = entityDatabase[i];
                return getEntity;
            }
        }

        return getEntity;
    }

    public int GetEntityID(EntityType entity)
    {
        int searchEntityID = 0;

        for(int i = 0; i < entityDatabase.Length-1;i++)
        {
            if (entityDatabase[i].entityName == entity.entityName)
            {
                searchEntityID = i;
                Debug.Log("ID for " + entity.entityName + " is " + searchEntityID);
                return searchEntityID;
            }
        }
        return searchEntityID;
    }
}
