using System;
using System.Collections.Generic;
using System.Linq;

namespace fans
{
    // Базовый класс для состояния
    public class State
    {
        public string Name;
        public Dictionary<char, State> Transitions;
        public bool IsAcceptState;
    }

    // Базовый класс для автомата
    public abstract class FA
    {
        protected State InitialState;
        
        public bool? Run(IEnumerable<char> s)
        {
            State current = InitialState;
            foreach (var c in s)
            {
                if (!current.Transitions.ContainsKey(c))
                    return null;
                current = current.Transitions[c];
            }
            return current.IsAcceptState;
        }
    }

    // ДКА для строки, содержащей ровно один '0' и хотя бы одну '1'
    public class FA1 : FA
    {
        // Состояния:
        // q0 - начальное: 0 еще не встречали, 1 еще не встречали
        // q1 - встретили 0, 1 еще не встречали
        // q2 - встретили 0 и хотя бы одну 1 (принимающее)
        // q3 - встретили больше одного 0 (отвергающее)
        
        private static State q0 = new State()
        {
            Name = "q0",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State q1 = new State()
        {
            Name = "q1",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State q2 = new State()
        {
            Name = "q2",
            IsAcceptState = true,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State q3 = new State()
        {
            Name = "q3",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        public FA1()
        {
            // q0: 0 -> q1, 1 -> q0
            q0.Transitions['0'] = q1;
            q0.Transitions['1'] = q0;
            
            // q1: 0 -> q3, 1 -> q2
            q1.Transitions['0'] = q3;
            q1.Transitions['1'] = q2;
            
            // q2: 0 -> q3, 1 -> q2
            q2.Transitions['0'] = q3;
            q2.Transitions['1'] = q2;
            
            // q3: 0 -> q3, 1 -> q3 (ловушка)
            q3.Transitions['0'] = q3;
            q3.Transitions['1'] = q3;
            
            InitialState = q0;
        }
    }

    // ДКА для строки, содержащей нечетное количество '0' и нечетное количество '1'
    public class FA2 : FA
    {
        // Состояния: (четность нулей, четность единиц)
        // (0,0) - q00: четное 0, четное 1
        // (0,1) - q01: четное 0, нечетное 1
        // (1,0) - q10: нечетное 0, четное 1
        // (1,1) - q11: нечетное 0, нечетное 1 (принимающее)
        
        private static State q00 = new State()
        {
            Name = "q00",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State q01 = new State()
        {
            Name = "q01",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State q10 = new State()
        {
            Name = "q10",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State q11 = new State()
        {
            Name = "q11",
            IsAcceptState = true,
            Transitions = new Dictionary<char, State>()
        };
        
        public FA2()
        {
            // q00: 0 -> q10, 1 -> q01
            q00.Transitions['0'] = q10;
            q00.Transitions['1'] = q01;
            
            // q01: 0 -> q11, 1 -> q00
            q01.Transitions['0'] = q11;
            q01.Transitions['1'] = q00;
            
            // q10: 0 -> q00, 1 -> q11
            q10.Transitions['0'] = q00;
            q10.Transitions['1'] = q11;
            
            // q11: 0 -> q01, 1 -> q10
            q11.Transitions['0'] = q01;
            q11.Transitions['1'] = q10;
            
            InitialState = q00;
        }
    }

    // ДКА для строки, содержащей подстроку '11'
    public class FA3 : FA
    {
        // Состояния:
        // s0 - начальное: не встретили '1' или последний символ не '1'
        // s1 - последний символ '1', но еще нет '11'
        // s2 - найдено '11' (принимающее)
        
        private static State s0 = new State()
        {
            Name = "s0",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State s1 = new State()
        {
            Name = "s1",
            IsAcceptState = false,
            Transitions = new Dictionary<char, State>()
        };
        
        private static State s2 = new State()
        {
            Name = "s2",
            IsAcceptState = true,
            Transitions = new Dictionary<char, State>()
        };
        
        public FA3()
        {
            // s0: 0 -> s0, 1 -> s1
            s0.Transitions['0'] = s0;
            s0.Transitions['1'] = s1;
            
            // s1: 0 -> s0, 1 -> s2
            s1.Transitions['0'] = s0;
            s1.Transitions['1'] = s2;
            
            // s2: 0 -> s2, 1 -> s2
            s2.Transitions['0'] = s2;
            s2.Transitions['1'] = s2;
            
            InitialState = s0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Тестирование FA1
            Console.WriteLine("=== FA1: ровно один '0' и хотя бы одна '1' ===");
            TestFA1();
            
            // Тестирование FA2
            Console.WriteLine("\n=== FA2: нечетное кол-во '0' и нечетное кол-во '1' ===");
            TestFA2();
            
            // Тестирование FA3
            Console.WriteLine("\n=== FA3: содержит '11' ===");
            TestFA3();
        }
        
        static void TestFA1()
        {
            var fa1 = new FA1();
            string[] tests = { "01", "10", "011", "101", "010", "001", "110", "1", "0", "" };
            foreach (var test in tests)
            {
                bool? result = fa1.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
        
        static void TestFA2()
        {
            var fa2 = new FA2();
            string[] tests = { "01", "10", "0011", "1100", "0001", "1110", "0", "1", "00", "11", "" };
            foreach (var test in tests)
            {
                bool? result = fa2.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
        
        static void TestFA3()
        {
            var fa3 = new FA3();
            string[] tests = { "11", "011", "110", "1011", "101", "0110", "1", "0", "", "111" };
            foreach (var test in tests)
            {
                bool? result = fa3.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
    }
}
