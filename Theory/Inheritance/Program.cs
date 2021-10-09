/* Inheritance is an object oriented concept where one type definition
 * re-uses / builds on another. It gives all of the same functionality
 * of the base/parent class to the derived/child class.
 */

using System;

static class Program
{
    static void Main()
    {
        Example1();
        Example2();
        Example3();
    }
    
    #region Basics
    // Just a normal type definition.
    class Parent
    {
        public int FieldFromParent;
    }

    // A type definition that inherits from Parent.
    class Child : Parent
    {
        public int FieldFromChild;
    }

    static void Example1()
    {
        var child = new Child();
        
        // You can access fields declared in the child class.
        child.FieldFromChild = 0; 
        // You can also access fields declared in the parent class.
        child.FieldFromParent = 1; 
    }
    #endregion
    
    #region Protected Members
    // As a reminder, public members can be accessed outside of the type definition, 
    // whereas private members can only be accessed within the type definition.
    // With inheritance comes another access modifier: protected.
    // Protected member can only be accessed within the type definition or derived type definitions.
    class ChildWithEncapsulation
    {
        public int Public;
        protected int _protected;
        private int _private;
    }

    class ParentAccessingFields : ChildWithEncapsulation
    {
        void DoSomething()
        {
            Console.WriteLine(Public); // Valid - can be accessed anywhere
            Console.WriteLine(_protected); // Valid - can be accessed in the base type definition or derived
            // Console.WriteLine(_private); // Invalid - can only be accessed in the base type definition
        }
    }

    static void Example2()
    {
        var parent = new ParentAccessingFields();
        parent.Public = 0; // Valid - can be accessed anywhere
        // parent._protected = 0; // Invalid - no longer inside the base or derived type definition
        // parent._private = 0; // Still invalid - still outside of the base type definition
    }
    #endregion
    
    #region Downcasting
    static void Example3()
    {
        // Say we have an instance of Child:
        var child = new Child();
        
        // Because the child class has all of the functionality of the parent class, 
        // the compiler lets us convert the child instance to an instance of parent:
        var parent = (Parent) child;
        // parent.FieldFromChild = 1; // Invalid - this field can no longer be referenced.

        // This is called downcasting - you're casting an instance to a lower type in the hierarchy.
    }
    #endregion
}