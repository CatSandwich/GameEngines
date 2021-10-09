/* Delegates are a special type that hold a list of methods. Different delegate
 * types support holding methods of different signatures and return types.
 * You can add and remove methods from delegates or invoke them, invoking
 * all of the methods referenced by that delegate.
 */

using System;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Action exampleRunner = () => { };
        exampleRunner += Example1;
        exampleRunner += Example2;
        exampleRunner += Example3;
        exampleRunner();
    }
    
    #region Custom Delegate Types
    // In the past, you had to manually create delegate types based on their 
    // signature and return type. This one returns nothing and takes no parameters.
    delegate void MyDelegate();
    
    // This delegate could hold a reference to this method:
    static void SayHi() => Console.WriteLine("Hi");
    
    // It could NOT hold a reference to this method:
    static int DifferentReturnType() => 2;
    // Or this one:
    static void DifferentSignature(string str) => Console.WriteLine(str);

    // These delegate definitions would be needed:
    delegate int IntDel();
    delegate void StrParamDel(string str);
    
    // Class for demonstration
    class C
    {
        public void DoSomething() { }
    }
    
    static void Example1()
    {
        // Create an instance of the delegate type initially pointing to SayHi.
        MyDelegate del = SayHi;

        // Invoke all methods the delegate points to.
        del();
        // Same as the above without using the shorthand.
        del.Invoke(); 
        // The long version allows for use with the null conditional operator '?.'
        del?.Invoke(); // Invokes only if del is not null.

        StrParamDel strDel = DifferentSignature;
        strDel("Hello!"); // Delegate invocation with a parameter

        IntDel intDel = DifferentReturnType;
        var i = intDel(); // Delegate invocation using the returned value

        var c = new C();
        del += c.DoSomething; // You can add instance methods to delegates as well
    }
    #endregion
    
    #region Built-in Delegate Types
    
    // Later on came built-in generic delegate types so you don't need to make a new type each time.
    // The two most common ones are Func and Action.
    
    // Func: Generic type which returns the first type and takes the rest as parameters.
    // For example, IntDel could be Func<int>
    
    // Action: Similar, but no return type.
    // For example, MyDelegate would be just be Action. No parameters.
    // StrParamDel would be Action<string>.
    
    // These types provided a unified way to use delegates.

    static void Example2()
    {
        IntDel i = DifferentReturnType; // Custom type
        Func<int> funcInt = DifferentReturnType; // Built-in type

        StrParamDel s = DifferentSignature; // Custom type
        Action<string> actionStr = DifferentSignature; // Built-in type

        MyDelegate myDel = SayHi; // Custom type
        Action action = SayHi; // Built-in type
    }
    
    #endregion
    
    #region Anonymous Methods
    
    // Sometimes it's a hassle to fully create methods just to use with delegates.
    // In come anonymous methods, or unnamed methods. These are expressions which
    // evaluate to a method, allowing you to use them inline with delegates.

    static void Example3()
    {
        // This is a delegate expression.
        // It creates an anonymous method with the parameter list and inferred return type.
        // This initialization sets the action to a new delegate that does nothing.
        Action action = delegate () { };
        
        // You can omit the parameter list if it is empty.
        action += delegate { Console.WriteLine("Hello, world!"); };

        action(); // This will call the first delegate doing nothing, then call the second, printing hello world.
        
        // This is an example of a delegate expression with a return type and parameter.
        Func<int, int> transformer = delegate(int i) { return i * 2; };
        
        // These are all the older syntax. The newer syntax is using lambda expressions.
        // Lambda expressions use the syntax (parameters) => [body]
        // Below are the same delegates in lambda form:

        action += () => { }; // Empty body block
        action += () => Console.WriteLine("Hello, world!"); // Expression body

        transformer += i => i * 2; // Parentheses are optional
        transformer += (i) => { return i * 2; }; // Example with body block instead of an expression
        
        // Lambda expressions are much cleaner. Delegate expressions aren't seen much anymore.
    }
    
    #endregion
    
    #region Events
    
    // A common implementation of delegates is event-based programming. In event-based
    // programming, code is run when something happens, rather than checking if said thing
    // happened before running code. This shift in focus can lead to cleaner code in certain
    // situations.
    
    // With delegates, different parts of a codebase can "subscribe" to an event by adding
    // callbacks that are run when the event fires. In C#, this is done through delegates.
    // A delegate is created that is invoked when an event happens, and other parts of the 
    // codebase can add method references to the delegate to be invoked when the event fires.

    // Here is an example of an event:
    class Clock
    {
        // Create and initialize an action to hold callbacks.
        public Action ClockTicked = () => { };

        public Clock()
        {
            // Run a task in the background that fires the event every second.
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    ClockTicked();
                }
            });
        }
    }
    
    // Here is an example of subscribing to the event
    class Stopwatch
    {
        public int SecondsElapsed = 0;

        public Stopwatch()
        {
            // Create a new clock
            var clock = new Clock();
            // Subscribe to the clock's event to keep the stopwatch updated
            clock.ClockTicked += () => SecondsElapsed += 1;
        }
    }
    
    // The event pattern became so common that a keyword was created to help it: 'event'.
    // Consider the above example. The clock should only be ticked every second, which the
    // class controls. Nowhere else should the event be able to be fired from. Normally to
    // prevent unwanted use of a field, you would apply a restricting access modifier like
    // private, internal or protected. The issue with this is that the event will not be able
    // to be subscribed to if it's not accessible. The 'event' keyword fixes this by restricting
    // access to the invocation only.

    // This class is the same as the above clock, but demonstrates the event keyword.
    class Clock2
    {
        // Due to the event keyword, only this class is able to fire the event. 
        // Subscribing is still publicly available.
        public event Action ClockTicked = () => { };

        public Clock2()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    ClockTicked(); // Valid within the class
                }
            });
        }
    }

    class Stopwatch2
    {
        public int SecondsElapsed = 0;

        public Stopwatch2()
        {
            // Create a new clock
            var clock = new Clock2();
            // Subscribe to the clock's event to keep the stopwatch updated
            clock.ClockTicked += () => SecondsElapsed += 1;

            // The following would generate a compiler error when the event keyword is present:
            // clock.ClockTicked();
        }
    }
    
    // One last thing to mention is another built-in delegate type: EventHandler
    // It returns nothing and has two arguments: an object (sender) and a type argument (event args).
    // This type provides a quick way to provide an event with the containing class and context as arguments.

    class Obj
    {
        public int I
        {
            get => _i;
            set
            {
                if (value == _i) return;
                _i = value;
                
                // Pass in the sender (this instance) and the context (which property was changed)
                PropertyChanged(this, nameof(I)); 
            }
        }
        private int _i;
        
        // Among other properties
        
        public event EventHandler<string> PropertyChanged = (sender, args) => { };
    }
    
    #endregion
}