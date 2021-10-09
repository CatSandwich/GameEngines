/* Generics are a special type or method definition that take other types as 
 * parameters. These type parameters can then be used as any other type inside 
 * of that definition. This allows for a more generic use of the type or method.
 */

using System;
using System.Collections;

static class Program
{
    // Theory split into 3 parts - read top down for clarity.
    static void Main()
    {
        Example1();
        Example2();
        Example3();
    }
    
    #region Generic Types
    // This is a generic class definition. The type parameter is "T".
    class Generic<T>
    {
        // This is a public field of type "T". It will be a different type depending on the type parameter.
        public T Data;
    }
    
    // Custom type for demonstration later.
    class CustomType
    {
        public int I;
        public string S;
    }
    
    public static void Example1()
    {
        // Create an instance of Generic with the type argument int.
        var gInt = new Generic<int>();
        // Set the integer field to 1.
        gInt.Data = 1;
        
        // Create an instance of Generic with the type argument string.
        var gStr = new Generic<string>();
        gStr.Data = "Hello, generics!";
        
        // Create an instance of Generic with the type argument CustomType.
        var gCus = new Generic<CustomType>();
        gCus.Data = new CustomType();
        gCus.Data.I = 1;
        gCus.Data.S = "Hello, generics!";
    }
    #endregion
    
    #region Generic Type Constraints
    // If you want your generic classes to require certain functionality from their
    // arguments to work, you can apply constraints on your type parameters.

    // A generic definition with a restriction - the type argument must derive from CustomType.
    class GenericWithConstraint<T> where T : CustomType
    {
        public T Data;

        public void DoSomething()
        {
            // Can reference "I" because any type deriving from CustomType will have that field.
            Console.WriteLine(Data.I);
        }
    }

    // Type derived from CustomType for demonstration.
    class MoreCustomType : CustomType
    {
        public float F;
    }
    
    static void Example2()
    {
        // Valid because MoreCustomType derives from CustomType:
        var genericWithConstraint = new GenericWithConstraint<MoreCustomType>(); 
        // Invalid because int does not derive from CustomType:
        // var genericWithConstraintInvalid = new GenericWithConstraint<int>();
    }
    #endregion
    
    #region Generic Methods
    // Methods can also be generic. This allows their return types and arguments to use
    // a type parameter.
    
    // Returns the default value of the specified type.
    public static T GetDefault<T>() => default(T);

    // Converts the data to a string and prints it to console.
    public static void Print<T>(T data)
    {
        Console.WriteLine(data.ToString());
    }

    // You can also add constraints to generic methods.
    public static void PrintCollection<T>(T collection) where T : IEnumerable
    {
        // Enumeration is valid because T must be enumerable.
        foreach (var data in collection) 
        {
            Console.WriteLine(data.ToString());
        }
    }

    public static void Example3()
    {
        // The generic methods are called like so:
        var s = GetDefault<string>();
        var i = GetDefault<int>();
        
        // Sometimes the compiler can infer the type parameter based on what you pass in:
        Print("Hello world!"); // No type parameter needed - compiler knows it's string.
        
        // You can still specify it if you want though - sometimes it helps Intellisense when filling in the arguments.
        Print<string>("Hello world!"); 
        
        PrintCollection(new int[]{1, 2, 3, 4}); // Again no type parameter needed. Compiler infers int[].
    }
    #endregion
}

