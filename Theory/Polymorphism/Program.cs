/* In general, polymorphism is an object's ability to take on many forms.
 * In programming, this comes in two major forms: downcasting and method overloading.
 * When downcasting or upcasting, one instance can be used in many forms.
 * With method overloading, one method group (name) can represent many signatures.
 * These allow for clean solutions to specific problems.
 */

using System;

static class Program
{
    static void Main()
    {
        Example1();
        Example2();
    }
    
    #region Polymorphism with Downcasting

    // An interface which requires derived types to have a name.
    interface INamed
    {
        string Name { get; }
    }

    // People have a name - let's format it with their first and last names.
    class Person : INamed
    {
        public string Name => $"{FirstName} {LastName}";
        
        public string FirstName;
        public string LastName;
    }

    // Apples don't really have names, but we can at least refer to them as apples.
    class Apple : INamed
    {
        public string Name => "Apple";
    }

    static void Example1()
    {
        var josh = new Person {FirstName = "Josh", LastName = "TS"};
        var apple = new Apple();
        
        // At first glance, these 2 classes have nothing to do with each other.
        // That's where the interface becomes useful. If we downcast both types
        // to their shared interface, we can use them in a shared form to allow for
        // operations you couldn't normally do.
        
        // Like adding them to a type-safe array together.
        var collection = new INamed[] {josh, apple};
        // Or printing out their names in an easy way:
        foreach (var named in collection)
        {
            Console.WriteLine(named.Name); 
            
            // Note that we only have access to what is defined in the current form.
            // Console.WriteLine(named.FirstName); // Invalid - not all INamed have a FirstName field.
        }
        
        // With polymorphism, you can use more specific types in less specific forms as needed 
        // to group them with other instances with shared parent classes or interfaces.
    }
    
    #endregion
    
    #region Method Overloading

    // Consider this method "DoSomething".
    static void DoSomething() => Console.WriteLine("Hello!");
    
    // It takes no parameters and returns nothing.
    
    // Now consider this method, also named "DoSomething".
    static void DoSomething(string s) => Console.WriteLine(s);
    
    // They share the same name, but not the same signature. Due to this, the compiler
    // will know which method to invoke based on the provided arguments. (See example)
    
    // Note that return values aren't included in the signature - a signature defines
    // what the compiler needs to find the function definition given a name and argument list.

    // The following is invalid - the compiler wouldn't know which method to choose given an empty parameter list.
    // static string DoSomething() => "Hello!"; 
    
    // You can, however mix and match return types given the signatures are different:
    static string DoSomething(int i) => i.ToString();
    
    static void Example2()
    {
        DoSomething(); // Invokes the first one
        DoSomething("Hello from overloading!"); // Invokes the second

        // The compiler allows this because this signature is associated with a definition with a return value.
        var str = DoSomething(5); 
    }

    #endregion
}