// ExactCover.cs
// ------------------------------------------------------------------
//
// A clean-room implementation of Donald Knuth's Algorithm-X in C#,
// employing the Dancing Links optimization, to solve the Exact Cover
// problem, which is:
//
//   Given an array of M*N bits, either 0 or 1, find a set of one or
//   more rows in the array such that every column has exactly one
//   "1".
//
// see:
// http://www-cs-faculty.stanford.edu/~uno/papers/dancing-color.ps.gz
// http://en.wikipedia.org/wiki/Dancing_Links#Main_ideas
//
// The way this program works: it generates a sparse matrix using random
// numbers, then tries to solve the exact Cover problem on that matrix.
// If not possible, it generates another matrix and tries again.
// Keeps trying until it gets a solvable matrix, at which point it
// prints out the matrix and the solution.
//
// Then exits.
//
// ==================================================================
//
// compile: csc.exe /t:exe /debug- /optimize+ /out:ExactCover.exe ExactCover.cs
//
// ==================================================================
//
// Author     : Dino
// Created    : Wed May 25 13:24:47 2011
// Last Saved : <2011-May-26 23:49:00>
//
// ------------------------------------------------------------------
//
// Copyright (c) 2011 by Dino Chiesa
// All rights reserved!
//
// This code is licensed under the Ms-PL,
//   see http://opensource.org/licenses/ms-pl.html
//
// ------------------------------------------------------------------


using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;



namespace Ionic.ToolsAndTests
{
    class ExactCover
    {
        class LinkedNode
        {
            public LinkedNode Above { get; set; }
            public LinkedNode Below { get; set; }
            public LinkedNode Left { get; set; }
            public LinkedNode Right { get; set; }

            public LinkedNode Head { get; set; }
            public String Id { get; set; }
            public int ColumnCount; // used only for head
            public int r { get; set; } // for tracking the solution

            // safety check
            public bool Removed;

            /// <summary>
            ///   For diagnostic purposes only
            /// </summary>
            public override String ToString()
            {
                return Id;
            }
        }

        LinkedNode root;
        LinkedNode[] rightMost; // for each row
        int M; // row count
        int N; // col count
        Stack<int> mstate; // state of matrix - which rows have been eliminated
        System.Random rnd;
        int _seed;

        public ExactCover()
        {
            rnd = new System.Random();
            _seed = rnd.Next();
            rnd = new System.Random(_seed);
        }

        public ExactCover(bool[,] matrix)
            : this()
        {
            Data = matrix;
            M = Data.GetLength(0);
            N = Data.GetLength(1);
        }

        /// <summary>
        ///   Set this to the matrix to be solved for Exact Cover
        /// </summary>
        public bool[,] Data
        {
            get;
            set;
        }


        /// <summary>
        ///   Build the doubly-linked toroidal lists.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Given the matrix, this builds the linked list.  It
        ///     constructs all the column headers, then a node for each
        ///     element in the column that is filled. It links each
        ///     element to its North, South, East, and West neighbors.
        ///   </para>
        /// </remarks>
        private LinkedNode BuildLinks()
        {
            root = new LinkedNode();
            root.Id = "root";
            LinkedNode leftHead = root;
            LinkedNode northernNeighbor, head = null, c;
            rightMost = new LinkedNode[M]; // in each row
            LinkedNode[] leftMost = new LinkedNode[M];  // in each row
            //links = new LinkedNode[M,N];

            for (int j = 0; j < N; j++) // columns
            {
                head = new LinkedNode();
                head.Id = ":" + j + ":"; // diagnostic purposes
                head.Below = head;
                head.Above = head;
                head.Left = leftHead;

                leftHead.Right = head;
                northernNeighbor = head;

                for (int i = 0; i < M; i++)  // rows
                {
                    if (Data[i, j])
                    {
                        c = new LinkedNode();
                        //links[i,j] = c;
                        c.Id = String.Format("[{0},{1}]", (char)(i + 'A'), j);
                        c.Head = head;
                        c.r = i;
                        c.Above = northernNeighbor;
                        northernNeighbor.Below = c;
                        head.ColumnCount++;
                        if (leftMost[i] == null)
                        {
                            leftMost[i] = c;
                        }
                        if (rightMost[i] != null)
                        {
                            rightMost[i].Right = c;
                            c.Left = rightMost[i];
                        }

                        // for the next cycle
                        rightMost[i] = c;
                        northernNeighbor = c;
                    }
                }
                head.Above = northernNeighbor;
                northernNeighbor.Below = head;
                leftHead = head;
            }

            // close the loop on each row
            for (int i = 0; i < M; i++)
            {
                if (leftMost[i] != null)
                    leftMost[i].Left = rightMost[i];

                if (rightMost[i] != null)
                    rightMost[i].Right = leftMost[i];
            }

            // close the loop on the column heads
            head.Right = root;
            root.Left = head;

            //PruneMatrix();

            return root;
        }



        private void RemoveColumn(LinkedNode colNode)
        {
            // Guard against removing the same column twice.
            //
            // If we try to remove the same column twice, the pointers
            // get corrupted. Must not do this!
            //
            // Normally, there's no need to check to see if a column has
            // already been removed. If the algorithm is correctly
            // implemented, it will never try to remove a column twice. BUT !
            // Supposing an incorrect Sudoku puzzle, which is not solvable
            // (for example, there are two 9's in a single row), then
            // it can result in the same column being removed twice, for
            // the "givens".
            //
            // So this safety flag protects against bad input.
            //
            if (colNode.Removed)
                return;

            // remove the column head
            colNode.Right.Left = colNode.Left;
            colNode.Left.Right = colNode.Right;

            // fixup all nodes in the matrix from this column
            for (LinkedNode r1 = colNode.Below; r1 != colNode; r1 = r1.Below)
            {
                for (LinkedNode n = r1.Right; n != r1; n = n.Right)
                {
                    n.Above.Below = n.Below;
                    n.Below.Above = n.Above;
                }
            }
            colNode.Removed = true;
        }


        private void ReinsertColumn(LinkedNode colNode)
        {
            // reinsert in reverse order from removal
            for (LinkedNode r1 = colNode.Above; r1 != colNode; r1 = r1.Above)
            {
                for (LinkedNode n = r1.Left; n != r1; n = n.Left)
                {
                    n.Above.Below = n;
                    n.Below.Above = n;
                }
            }

            colNode.Right.Left = colNode;
            colNode.Left.Right = colNode;
            colNode.Removed = false;
        }



        /// <summary>
        ///   Choose one column to eliminate from the matrix
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The heuristic of using a column with the fewest 1's
        ///     tends to make the search run more quickly.
        ///   </para>
        /// </remarks>
        private LinkedNode ChooseColumn()
        {
            int s = M * N;  // a large number, more than the number of
            // ones that could fit in a column
            LinkedNode selected = root.Right;
            for (LinkedNode n = root.Right; n != root; n = n.Right)
            {
                if (n.ColumnCount < s)
                {
                    s = n.ColumnCount;
                    selected = n;
                }
            }
            return selected;
        }


        List<LinkedNode> ActveRowsInColumn(LinkedNode colHead)
        {
            var rows = new List<LinkedNode>();
            for (LinkedNode n1 = colHead.Below; n1 != colHead; n1 = n1.Below)
            {
                rows.Add(n1);
            }
            return rows;
        }


        private bool Search(int depth)
        {
            if (root.Left == root && root.Right == root)
            {
                // the matrix is empty - problem solved
                return true;
            }

            // select column deterministically
            LinkedNode candidate = ChooseColumn();
            RemoveColumn(candidate);

            // choose rows non-deterministically
            foreach (LinkedNode n in ActveRowsInColumn(candidate).OrderRandomly())
            {
                // found a row.
                mstate.Push(n.r);
                for (LinkedNode r1 = n.Right; r1 != n; r1 = r1.Right)
                    RemoveColumn(r1.Head);

                if (Search(depth + 1))
                    return true;

                // put the pointers back, in reverse order
                for (LinkedNode r1 = n.Left; r1 != n; r1 = r1.Left)
                    ReinsertColumn(r1.Head);

                mstate.Pop();
            }

            ReinsertColumn(candidate);
            return false;
        }


        public bool Solve()
        {
            return Solve(null);
        }


        /// <summary>
        ///   Solve the matrix.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     You must have first set the Data property.
        ///   </para>
        /// </remarks>
        public bool Solve(Stack<int> startingState)
        {
            if (Data == null)
                throw new ArgumentException("Data");

            BuildLinks();

            if (startingState != null)
            {
                mstate = startingState;
                // remove (cover) the columns which have 1's in the row
                // corresponding to the starting state of the matrix.
                foreach (int v in startingState)
                {
                    var n = rightMost[v];
                    for (LinkedNode r1 = n; r1 != n; r1 = r1.Right)
                    {
                        RemoveColumn(r1.Head);
                    }
                }
            }
            else mstate = new Stack<int>();

            return Search(0);
        }


        public Stack<int> MatrixState
        {
            get
            {
                return mstate;
            }
        }

    }

    static class Extensions
    {
        public static IEnumerable<T> OrderRandomly<T>(this IList<T> list)
        {
            Random random = new Random();
            while (list.Count > 0)
            {
                int index = random.Next(list.Count);
                yield return list[index];
                list.RemoveAt(index);
            }
        }
    }


}
