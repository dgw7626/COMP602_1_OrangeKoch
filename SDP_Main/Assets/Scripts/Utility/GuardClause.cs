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
        if (currentType.Equals(null))
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
    public static T InspectGuardClause<T,C>(T currentType,  string parameterName, ErrorType error)
    {
        if (currentType.Equals(null))
        {
            switch (error)
            {
                case ErrorType.NullRef:
                    {
                        Debug.LogError("[Custom." + ErrorType.NullRef.ToString() + "]: Parameter Name: " + parameterName + ", Type: (" + currentType.GetType() + ") , -----HERE-----" +
                         "\n \t\t[Comment]: Missing " + currentType.GetType() + ", this reference is required to run the current script.");
                        return default(T);
                    }
                case ErrorType.MissingComponent:
                    {
                        Debug.LogError("[Custom." + ErrorType.NullRef.ToString() + "]: Parameter Name: " + parameterName + ", Type: (" + currentType.GetType() + ") , -----HERE-----" +
                        "\n \t\t[Comment]: Missing " + currentType.GetType() + ", Missing Component of (" + currentType.ToString() + "), this Component requires " + nameof(C));
                        return default(T);
                    }
                default:
                    {
                        Debug.LogWarning("[Custom." + error.ToString() + "]: Unexpected error occured please check your syntax");
                        return default(T);
                    }
            }
        }
        return currentType;
    }
    /// <summary>
    /// This method checks the GuardClause of the type, if the object type is null it will throw unity debug error message. parameter only requires error type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="currentType"></param>
    /// <returns>returns current type of the object.</returns>
    public static T InspectGuardClauseNullRef<T>(T currentType, string parameterName)
    {
        if (ReferenceEquals(currentType, null))
        {
            Debug.LogError("[Custom." + ErrorType.NullRef.ToString() + "]: Parameter Name: " + parameterName + ", Type: (" + typeof(T) + ") , -----HERE-----" +
                "\n \t\t[Comment]: Missing " + typeof(T) + ", this reference is required to run the current script.");
            return default(T);
        }
        return currentType;
    }

}
