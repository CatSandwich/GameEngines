/* Coroutines are a Unity concept that builds on C#'s IEnumerators.
 * They are methods that are intended to be split into parts to be
 * run at different times. This could mean over two separate frames
 * or much longer.
 */

using System.Collections;
using UnityEngine;

public class Coroutines : MonoBehaviour
{
    void Start()
    {
        Example1();
        Example2();
        Example3();
        Example4();
    }

    #region IEnumerator Basics
    
    // IEnumerator is an interface that enforces the ability to enumerate
    // over a collection. It has a few exposed fields and methods to do this.

    public void Example1()
    {
        var collection = new[] {1, 2, 3};
        var enumerator = collection.GetEnumerator();
        
        while (enumerator.MoveNext()) // Start enumerating over the collection
        {
            Debug.Log(enumerator.Current); // Print the current element
        }
    }
    
    // You can also define methods with multiple return points if it returns an enumerator.
    // The returned enumerator can be used to enumerate over the return points only running
    // code as requested.

    public IEnumerator GetCollection()
    {
        Debug.Log("GetCollection enumeration has started.");
        yield return 1;
        
        Debug.Log("GetCollection enumeration has continued to the second return statement.");
        yield return 2;
        
        Debug.Log("GetCollection enumeration has reached the last element.");
        yield return 3;
    }

    public void Example2()
    {
        // This does NOT run all of the code in GetCollection. It returns an enumerator
        // that allows you to run the code in segments defined by the return points.
        var enumerator = GetCollection();
        enumerator.MoveNext();
        Debug.Log(enumerator.Current);
        enumerator.MoveNext();
        Debug.Log(enumerator.Current);

        // Since we only called MoveNext() twice, it will only run the code up to the second return statement. 
    }

    #endregion
    
    #region Coroutines
    
    // Coroutines use this IEnumerator system to allow you to define methods that don't run all
    // at once. Unity provides types you can return from IEnumerator methods that tell the engine
    // when to continue the code's execution.

    IEnumerator MyCoroutine()
    {
        Debug.Log("Coroutine Start"); // This will be run as soon as the execution begins
        yield return new WaitForSeconds(5f); // This tells Unity to wait 5 seconds before calling MoveNext again
        Debug.Log("Coroutine has waited 5 seconds"); // This will be run after 5 seconds
    }

    void Example3()
    {
        StartCoroutine(MyCoroutine()); // This method passes the enumerator over for unity to take care of
        
        // StartCoroutine returns a Coroutine object which can stored and passed to StopCoroutine as needed.
    }
    
    // In addition to the built-in types to instruct the engine, you can create custom ones:

    // This type defines a yield instruction that has a 1 in 100 chance to continue every frame.
    class MyYieldInstruction : CustomYieldInstruction
    {
        public override bool keepWaiting => Random.Range(1, 100) != 1;
    }

    void Example4()
    {
        StartCoroutine(CustomInstructionDemo());
    }

    IEnumerator CustomInstructionDemo()
    {
        Debug.Log("CustomInstructionDemo begin");
        yield return new MyYieldInstruction();
        Debug.Log("CustomInstructionDemo end");
    }
    
    #endregion
}
