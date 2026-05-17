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
        // Состояния:
        // A: начальное, ещё не видели 0 и не видели 1
        // B: видели хотя бы одну 1, но ещё не видели 0
        // C: видели ровно один 0 (и при этом уже была хотя бы одна 1) - ДОПУСКАЮЩЕЕ
        // D: видели больше одного 0 (отвергающее)
        // E: видели один 0, но ещё не видели 1
        
        private static State A = new State() { Name = "A", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State B = new State() { Name = "B", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State C = new State() { Name = "C", IsAcceptState = true, Transitions = new Dictionary<char, State>() };
        private static State D = new State() { Name = "D", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State E = new State() { Name = "E", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        
        public FA1()
        {
            // Из состояния A (начальное)
            A.Transitions['0'] = E;  // встретили первый 0, но ещё нет 1 -> идём в E
            A.Transitions['1'] = B;  // встретили 1, ждём 0 -> идём в B
            
            // Из состояния B (видели хотя бы одну 1, но ещё нет 0)
            B.Transitions['0'] = C;  // встретили первый 0 -> успех (есть и 0, и 1)
            B.Transitions['1'] = B;  // можно добавлять сколько угодно 1
            
            // Из состояния C (успех: один 0 и хотя бы одна 1) - ДОПУСКАЮЩЕЕ
            C.Transitions['0'] = D;  // второй 0 -> ошибка
            C.Transitions['1'] = C;  // можно добавлять сколько угодно 1
            
            // Из состояния D (ошибка: два или более нулей)
            D.Transitions['0'] = D;
            D.Transitions['1'] = D;
            
            // Из состояния E (видели один 0, но ещё нет 1)
            E.Transitions['0'] = D;  // второй 0 -> ошибка
            E.Transitions['1'] = C;  // встретили 1 -> успех (есть и 0, и 1)
            
            InitialState = A;
        }
    }

    // ДКА для строки, содержащей нечетное количество '0' и нечетное количество '1'
    public class FA2 : FA
    {
        // Состояния: (четность_0, четность_1)
        private static State S00 = new State() { Name = "S00", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State S01 = new State() { Name = "S01", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State S10 = new State() { Name = "S10", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private static State S11 = new State() { Name = "S11", IsAcceptState = true, Transitions = new Dictionary<char, State>() };
        
        public FA2()
        {
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
            string[] tests = { "01", "10", "011", "101", "1101", "1111101", "010", "001", "110", "1", "0", "" };
            foreach (var test in tests)
            {
                bool? result = fa1.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
        
        static void TestFA2()
        {
            var fa2 = new FA2();
            string[] tests = { "01", "10", "000111", "111000", "0011", "1100", "011", "001", "110", "010", "0", "1", "" };
            foreach (var test in tests)
            {
                bool? result = fa2.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
        
        static void TestFA3()
        {
            var fa3 = new FA3();
            string[] tests = { "11", "011", "110", "1011", "111", "0110", "10110", "1", "0", "10", "01", "101", "010", "10101", "" };
            foreach (var test in tests)
            {
                bool? result = fa3.Run(test);
                Console.WriteLine($"\"{test}\" -> {result}");
            }
        }
    }
}
