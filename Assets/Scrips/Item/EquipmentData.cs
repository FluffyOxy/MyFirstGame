using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(LogOnImport = true)]
public class EquipmentData : ScriptableObject
{
	public List<ExcelEquipmentData> NewEquipment; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> Equipment; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> Sheet3; // Replace 'EntityType' to an actual type that is serializable.
}
