using System;
using System.Collections.Generic;

namespace fans
{
    public class State
    {
        public string Name = "";
        public Dictionary<char, State> Transitions = new Dictionary<char, State>();
        public bool IsAcceptState;
    }

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

    // ДКА: ровно один '0' и хотя бы одна '1'
    // A: старт (нет ни 0, ни 1)
    // B: видели только единицы, нет нуля
    // C: видели один 0 и хотя бы одну 1 — ФИНАЛЬНОЕ
    // D: видели один 0, но ещё нет 1
    // E: мёртвое (два+ нуля)
    public class FA1 : FA
    {
        private State A = new State() { Name = "A", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State B = new State() { Name = "B", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State C = new State() { Name = "C", IsAcceptState = true,  Transitions = new Dictionary<char, State>() };
        private State D = new State() { Name = "D", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State E = new State() { Name = "E", IsAcceptState = false, Transitions = new Dictionary<char, State>() };

        public FA1()
        {
            A.Transitions['0'] = D;
            A.Transitions['1'] = B;

            B.Transitions['0'] = C;
            B.Transitions['1'] = B;

            C.Transitions['0'] = E;
            C.Transitions['1'] = C;

            D.Transitions['0'] = E;
            D.Transitions['1'] = C;

            E.Transitions['0'] = E;
            E.Transitions['1'] = E;

            InitialState = A;
        }
    }

    // ДКА: нечётное кол-во '0' И нечётное кол-во '1'
    // Состояния кодируют (чётность_0, чётность_1)
    // S00: чёт/чёт (старт)
    // S01: чёт/нечёт
    // S10: нечёт/чёт
    // S11: нечёт/нечёт — ФИНАЛЬНОЕ
    public class FA2 : FA
    {
        private State S00 = new State() { Name = "S00", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State S01 = new State() { Name = "S01", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State S10 = new State() { Name = "S10", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State S11 = new State() { Name = "S11", IsAcceptState = true,  Transitions = new Dictionary<char, State>() };

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

    // ДКА: содержит подстроку '11'
    // Q0: старт
    // Q1: последний символ '1'
    // Q2: встретили '11' — ФИНАЛЬНОЕ
    public class FA3 : FA
    {
        private State Q0 = new State() { Name = "Q0", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State Q1 = new State() { Name = "Q1", IsAcceptState = false, Transitions = new Dictionary<char, State>() };
        private State Q2 = new State() { Name = "Q2", IsAcceptState = true,  Transitions = new Dictionary<char, State>() };

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
            var fa1 = new FA1();
            string[] tests1 = { "01", "10", "011", "101", "1101", "1111101", "010", "001", "110", "1", "0", "" };
            foreach (var t in tests1)
                Console.WriteLine($"\"{t}\" -> {fa1.Run(t)}");

            Console.WriteLine("\n=== FA2: нечётное кол-во '0' и нечётное кол-во '1' ===");
            var fa2 = new FA2();
            string[] tests2 = { "01", "10", "000111", "111000", "0011", "1100", "011", "001", "110", "010", "0", "1", "" };
            foreach (var t in tests2)
                Console.WriteLine($"\"{t}\" -> {fa2.Run(t)}");

            Console.WriteLine("\n=== FA3: содержит '11' ===");
            var fa3 = new FA3();
            string[] tests3 = { "11", "011", "110", "1011", "111", "0110", "10110", "1", "0", "10", "01", "101", "010", "10101", "" };
            foreach (var t in tests3)
                Console.WriteLine($"\"{t}\" -> {fa3.Run(t)}");
        }
    }
}
