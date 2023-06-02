/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Corey Knigth	            *
 * Student ID: 		21130891		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */

/// <summary>
/// Used by any state machine to run the state.
/// </summary>
public abstract class AbstractState
{
    public abstract AbstractState RunState(System.Object param);
}
