/* Extension methods are syntactic sugar that let you use static methods like
 * instance methods. This is helpful when working with types that you didn't define,
 * but you need certain functionality that isn't there. A static method taking the
 * instance as a parameter would work, but it looks less clean.
 */

// Pretend this class definition is built in. You cannot change this definition no matter what.

public class Thing
{
    public int I;
}

// In your case, you constantly need to square the value of I. Your options are:

// Hardcode:
// instance.I = instance.I * instance.I;

// Make a re-usable static method
static class MethodHolder
{
    public static void SquareThing(Thing inst)
    {
        inst.I *= inst.I;
    }
}

// Some magical third option:

// Inside of a static class
static class ThingExtension
{
    // Create a static method with the keyword 'this' in front of the first parameter.
    // This allows the method to be used like an instance method on the first argument's type.
    public static void Square(this Thing thing) 
    {
        thing.I *= thing.I;
    }

    // If you need extra parameters, they work perfectly fine.
    public static void Multiply(this Thing thing, int operand)
    {
        thing.I *= operand;
    }
}

static class Program
{
    static void Main()
    {
        var thing = new Thing();
        
        // To square the field:
        // Option 1: Hardcoded. Fine in this example but for more complex operations it's a pain.
        thing.I *= thing.I;
        
        // Option 2: Static method. Much better, but looks less nice than an instance method.
        MethodHolder.SquareThing(thing);

        // Option 3: Magical third method (extension method). Perfection.
        thing.Square();
        
        // It looks like an instance method, but it's just syntactic sugar for a static method.
        // The first parameter is automatically filled with the instance.
        
        // Further parameters are passed in as normal.
        thing.Multiply(3);
    }
}