/* Interfaces are type definitions that don't implement functionality.
 * They require that inheriting types implement them. This goes hand-in-hand
 * with polymorphism.
 */

using System;

static class Program
{
    static void Main()
    {
        Example1();
    }
    
    #region Basics
    // This is an interface for types that do stuff. It has the definition DoStuff()
    interface IDoesStuff // Name starts with 'I' by convention.
    {
        void DoStuff();
    }

    // Below are two type definitions that implement the interface above.
    // They are required to have a definition for DoStuff with the same
    // signature or there will be a compiler error.
    
    class DoesStuff : IDoesStuff // Same syntax as inheritance
    {
        public int I; 
        public void DoStuff() => Console.WriteLine("This type prints this message!");
    }

    class DoesOtherStuff : IDoesStuff
    {
        public int OtherI;
        public void DoStuff() => Console.WriteLine("This type prints a different message!");
    }
    
    // Note that methods implemented from interfaces must be public.
    // If they weren't, it would defeat the purpose. Interfaces define
    // a contract of what functionality a class has. If the functionality
    // isn't publicly accessible, then the interface provides nothing.

    interface ISomeOtherInterface
    {
        // Since properties are just compiled into methods, they're valid in interfaces:
        int SomePropertyWithGetter { get; }
        string SomePropertyWithSetter { set; }
        float SomePropertyWithBoth { get; set; }
    }

    // Here is what an implementation of that interface might look like:
    class SomeOtherClass : ISomeOtherInterface
    {
        public int SomePropertyWithGetter => 2;
        
        public string SomePropertyWithSetter
        {
            set => _someInternalString = value;
        }

        private string _someInternalString = "";
        
        public float SomePropertyWithBoth
        {
            get => _encapsulatedFloat; 
            set => _encapsulatedFloat = value;
        }

        private float _encapsulatedFloat = 0f;
    }
    
    
    static void Example1()
    {
        var doesStuff = new DoesStuff();
        doesStuff.I = 1;
        doesStuff.DoStuff(); // Valid with or without the interface

        var doesOtherStuff = new DoesOtherStuff();
        doesOtherStuff.OtherI = 2;
        doesOtherStuff.DoStuff(); // Also valid with or without the interface

        // You can also downcast types to their interfaces:
        var iDoesStuff = (IDoesStuff) doesStuff;
        // But you can't instantiate an interface since it's not an implementation of a type; only definitions.
        // var invalid = new IDoesStuff(); // Invalid

        // So far there's nothing special here. The interface doesn't 
        // do anything for us until we get into polymorphism.
    }
    #endregion
}