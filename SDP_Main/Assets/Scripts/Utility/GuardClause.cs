using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuardClause
{
    /// <summary>
    /// This method checks if the type of object is not null, it will throw unity debug error message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="currentType"></param>
    /// <param name="message"></param>
    /// <returns>returns current type of the object.</returns>
    public static T InspectGuardClause<T>(T currentType, string message)
    {
        if (currentType == null)
        {
            Debug.LogError(message + nameof(T));
            return default(T);
        }
        return currentType;
    }
    /// <summary>
    /// This method checks the GuardClause of the type, if the object type is null it will throw unity debug error message. parameter only requires error type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="C"></typeparam>
    /// <param name="currentType"></param>
    /// <param name="error"></param>
    /// <returns>returns current type of the object.</returns>
    public static T InspectGuardClause<T,C>(T currentType, ErrorTypes error)
    {
        if (currentType == null)
        {
            switch (error)
            {
                case ErrorTypes.NullRef:
                    {
                        Debug.LogError("[" + error.ToString() + "]: Missing " + currentType.GetType().Name + ", this reference is required to run the current script.");
                        return default(T);
                    }
                case ErrorTypes.MissingComponent:
                    {
                        Debug.LogError("[" + error.ToString() + "]: Missing Component of " + currentType.GetType().Name + ", this Component requires "+nameof(C));
                        return default(T);
                    }
                default:
                    {
                        Debug.LogError("[" + error.ToString() + "]: Unexpected error occured please check your syntax");
                        return default(T);
                    }
            }
        }
        return currentType;
    }
}
