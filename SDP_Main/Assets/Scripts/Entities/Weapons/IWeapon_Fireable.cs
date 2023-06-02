/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Hanul Rheem		            *
 * Student ID: 		20109218		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using UnityEngine;
/// <summary>
/// This is interface class for the IWeapon_Fireable that has Fire and Hit method in default.
/// </summary>
public interface IWeapon_Fireable
{
    /// <summary>
    /// Fire method that instantiates bullet and shoot, the parameter requires shooting position.
    /// </summary>
    /// <param name="origin"></param>
    public void Fire(Transform origin);
    /// <summary>
    /// Hit method is for debugging method to identify the transform name and any status of the object.
    /// </summary>
    /// <param name="origin"></param>
    public void Hit(Transform origin);
 
}
