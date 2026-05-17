using System;
using System.Collections.Generic;

namespace fans
{
    // Базовый класс для состояния
    public class State
    {
        public string Name = "";
        public Dictionary<char, State> Transitions = new Dictionary<char, State>();
        public bool IsAcceptState;
    }

    // Базовый класс для автомата
    public abstract class FA
    {
        protected State InitialState = null!;
        
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
        private static State S0 = new State() { Name = "S0", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State S1 = new State() { Name = "S1", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State S2 = new State() { Name = "S2", IsAcceptState = true, Transitions = new Dictionary<char, State>() };
        private static State S3 = new State() { Name = "S3", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        
        public FA1()
        {
            // S0: начальное, еще не видели 0
            S0.Transitions['0'] = S1;  // встретили первый 0
            S0.Transitions['1'] = S0;  // продолжаем ждать 0
            
            // S1: видели один 0, еще не видели 1
            S1.Transitions['0'] = S3;  // второй 0 - ошибка
            S1.Transitions['1'] = S2;  // встретили 1 - успех
            
            // S2: видели один 0 и хотя бы одну 1 (принимающее)
            S2.Transitions['0'] = S3;  // второй 0 - ошибка
            S2.Transitions['1'] = S2;  // можно сколько угодно 1
            
            // S3: ошибка (больше одного 0)
            S3.Transitions['0'] = S3;
            S3.Transitions['1'] = S3;
            
            InitialState = S0;
        }
    }

    // ДКА для строки, содержащей нечетное количество '0' И нечетное количество '1'
    public class FA2 : FA
    {
        // Состояния: (четность_0, четность_1)
        private static State S00 = new State() { Name = "S00", IsAcceptState = false, Transitions = new Dictionary<char, State>() }; // чет, чет
        private static State S01 = new State() { Name = "S01", IsAcceptState = false, Transitions = new Dictionary<char, State>() }; // чет, нечет
        private static State S10 = new State() { Name = "S10", IsAcceptState = false, Transitions = new Dictionary<char, State>() }; // нечет, чет
        private static State S11 = new State() { Name = "S11", IsAcceptState = true, Transitions = new Dictionary<char, State>() };  // нечет, нечет
        
        public FA2()
        {
            // Из каждого состояния по символу 0 меняется четность нулей
            S00.Transitions['0'] = S10;
            S00.Transitions['1'] = S01;
            
            S01.Transitions['0'] = S11;
            S01.Transitions['1'] = S00;
            
            S10.Transitions['0'] = S00;
            S10.Transitions['1'] = S11;
            
            S11.Transitions['0'] = S01;
            S11.Transitions['1'] = S10;
            
            InitialState = S00;
        }
    }

    // ДКА для строки, содержащей подстроку '11'
    public class FA3 : FA
    {
        private static State Q0 = new State() { Name = "Q0", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State Q1 = new State() { Name = "Q1", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State Q2 = new State() { Name = "Q2", IsAcceptState = true, Transitions = new Dictionary<char, State>() };
        
        public FA3()
        {
            Q0.Transitions['0'] = Q0;
            Q0.Transitions['1'] = Q1;
            
            Q1.Transitions['0'] = Q0;
            Q1.Transitions['1'] = Q2;
            
            Q2.Transitions['0'] = Q2;
            Q2.Transitions['1'] = Q2;
            
            InitialState = Q0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FA1: ровно один '0' и хотя бы одна '1' ===");
            TestFA1();
            
            Console.WriteLine("\n=== FA2: нечетное кол-во '0' и нечетное кол-во '1' ===");
            TestFA2();
            
            Console.WriteLine("\n=== FA3: содержит '11' ===");
            TestFA3();
        }
        
        static void TestFA1()
        {
            var fa1 = new FA1();
            string[] tests = { "01", "10", "011", "101", "010", "001", "110", "1", "0", "1111101" };
            foreach (var test in tests)
            {
                bool? result = fa1.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
        
        static void TestFA2()
        {
            var fa2 = new FA2();
            string[] tests = { "01", "10", "0011", "1100", "0001", "1110", "0", "1", "00", "11", "0101", "1010" };
            foreach (var test in tests)
            {
                bool? result = fa2.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
        
        static void TestFA3()
        {
            var fa3 = new FA3();
            string[] tests = { "11", "011", "110", "1011", "101", "0110", "1", "0", "111" };
            foreach (var test in tests)
            {
                bool? result = fa3.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
    }
}
