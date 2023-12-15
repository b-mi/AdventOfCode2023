using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day9
    {
        public Day9()
        {
            int day = 9;
            var input = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";

            //input = @"10 13 16 21 30 45";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();
            part1(lines);
            //part2(lines);

        }

        private void part1(string[] lines)
        {
            ulong total = 0;

            foreach (var line in lines)
            {
                var nums = line.Split(' ').Select(i => int.Parse(i));
                Node prev = null, first = null;
                foreach (var num in nums)
                {
                    new Node(num, ref prev, ref first, null);
                }

                while (true)
                {
                    first = createChilds(first, out var allZero);
                    if (allZero)
                    {
                        break;
                    }
                }

                // extrapolate
                ulong sum = 0;
                var startPoint = first;
                while (startPoint.Right != null)
                    startPoint = startPoint.Right;

                while (startPoint.ParentL != null)
                {
                    sum += (ulong)startPoint.ParentL.Right.Value;
                    startPoint = startPoint.ParentL.Right;
                }
                total += sum;

            }

            Console.WriteLine($"Vysledok je: {total}");
        }

        private static Node createChilds(Node firstParent, out bool allZero)
        {
            Node node = firstParent, firstChild, prevChild;
            firstChild = prevChild = null;
            allZero = true;
            while (node.Right != null)
            {
                var newValue = node.Right.Value - node.Value;
                if (newValue != 0)
                    allZero = false;
                new Node(newValue, ref prevChild, ref firstChild, node);
                node = node.Right;
            }
            return firstChild;
        }
    }

    [DebuggerDisplay("Value: {Value}")]
    internal class Node
    {
        public Node(int num, ref Node prev, ref Node first, Node parentLnode)
        {
            Value = num;
            if (prev != null)
                prev.Right = this;
            prev = this;
            if (first == null)
                first = this;

            if (parentLnode != null)
                this.ParentL = parentLnode;
        }

        public int Value { get; private set; }
        public Node Right { get; set; }

        public Node ParentL { get; set; }

        public Node Child { get; set; }


    }

    [DebuggerDisplay("{str}")]
    internal class NodeViewer
    {
        Node node;
        string str;
        public NodeViewer(Node n)
        {
            this.node = n;
            setStr();
        }




        public void setStr()
        {
            str = "";
            Node n = node;
            while (n != null)
            {
                str += $"{n.Value} ";
                n = n.Right;
            }
        }
    }

}