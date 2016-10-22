/*
 * Program.cs
 * 
 * A very basic example program that serves as an NChord server running on the command line.
 *
 * ****************************************************************************
 *  Copyright (c) 2008 Andrew Cencini
 *
 *  Permission is hereby granted, free of charge, to any person obtaining
 *  a copy of this software and associated documentation files (the
 *  "Software"), to deal in the Software without restriction, including
 *  without limitation the rights to use, copy, modify, merge, publish,
 *  distribute, sublicense, and/or sell copies of the Software, and to
 *  permit persons to whom the Software is furnished to do so, subject to
 *  the following conditions:
 *
 *  The above copyright notice and this permission notice shall be
 *  included in all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 *  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 *  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 *  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 *  LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 *  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 *  WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * ****************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Text;
using NChordLib;

namespace NChordServer
{
    class Program
    {
        

        static void Main(string[] args)
        {
            try
            {
                List<string> estados = new List<string> { "AC", "AL", "AM", "AP", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RO", "RS", "RR", "SC", "SE", "SP", "TO" };
                string fqdn = System.Net.Dns.GetHostEntry("LocalHost").HostName;

                if (args.Length == 1)
                {
                    // start new ring
                    int portNum = Convert.ToInt32(args[0]);
                    //ChordServer.LocalNode = new ChordNode(System.Net.Dns.GetHostName(), portNum); 
                    ChordServer.LocalNode = new ChordNode(fqdn, portNum);
                    if (ChordServer.RegisterService(portNum))
                    {
                        ChordInstance instance = ChordServer.GetInstance(ChordServer.LocalNode);
                        instance.Join(null, ChordServer.LocalNode.Host, ChordServer.LocalNode.PortNumber);
                        
                        //int i=0;
                        //ulong i = 0;
                        //UInt64 j = 0;
                        while (true)
                        {
                            switch (Char.ToUpperInvariant(Console.ReadKey(true).KeyChar))
                            {
                                case 'I':
                                    {
                                        PrintNodeInfo(instance, false);
                                        break;
                                    }
                                case 'A':
                                    {
                                        //instance.AddKey(ChordServer.LocalNode.ID + i);
                                        //ulong temp = ChordServer.LocalNode.ID + i - 10;
                                        //instance.AddKey(temp,"RG"+i.ToString());
                                        //Console.Write("Insira o identificador:\n");
                                        //string arg1 = Console.ReadLine();
                                        //Console.Write("Insira o Estado:\n");
                                        //string arg2 = Console.ReadLine();
                                        //instance.AddKey(arg1, arg2);
                                        //i++;
                                        //break;
                                        //foreach (string item in GetNumber(12))
                                        //{
                                        //    var rnd = new Random(DateTime.Now.Millisecond);
                                        //    int index = rnd.Next(0, 26);
                                        //    //instance.AddLocalKey(item,estados[index]);
                                        //    instance.AddKey(item, estados[index]);
                                        //    Console.WriteLine("Item {0} inserido na base.", item);
                                        //}
                                        for (int i = 0; i < 4000; i++)
                                        {
                                          
                                                int length = Convert.ToInt32(11);
                                                var item = rng.NextDouble().ToString("0.000000000000").Substring(2, length);
                                                var rnd = new Random(DateTime.Now.Millisecond);
                                                int uf = rnd.Next(0, 26);
                                                //instance.AddLocalKey(item,estados[index]);
                                                instance.AddKey(item, estados[uf]+".rafis.net");
                                                Console.WriteLine("Item {0} inserido na base.", item);
                                         
                                        }

                                        break;
                                        //for (j = 20000000000000000; j < 60000000000000000; j = j + 11248993875331)
                                        //{
                                        //    instance.AddKey(j.ToString(), "RO");
                                        //    Console.WriteLine("{0}%\n",j/1);
                                        //}
                                        //break;
                                    }
                                case 'F':
                                    {
                                        //Console.Write("Insira o identificador:\n");
                                        //string id = Console.ReadLine();
                                        //Console.WriteLine("{0}\n", instance.FindKey(ChordServer.GetHash(id)));
                                        //Console.WriteLine("{0}\n", instance.FindSuccessor(ChordServer.GetHash(id)));
                                        //break;
                                        Console.Write("Insira o identificador:\n");
                                        ulong id = ChordServer.GetHash(Console.ReadLine());
                                        if (instance.FindKey(id)!="")
                                        {
                                            Console.Write("CPF encontrado:\n");
                                            Console.WriteLine("{0}\n", instance.FindKey(id));
                                        }
                                        else
                                        {
                                            Console.Write("CPF não localizado!\n");
                                        }
                                        //Console.WriteLine("{0}\n", instance.FindKey(id));
                                        //Console.WriteLine("{0}\n", ChordServer.CallFindKey(instance.FindSuccessor(id), id));
                                        //Console.WriteLine("{0}\n", instance.FindSuccessor(id));
                                        break;
                                    }
                                case 'L':
                                    {
                                        int count=0;
                                        foreach (var item in instance.PrintKeys())
                                        {
                                            //Console.Write("{0}\t > hash:{1}({2}) - Estado: {3} \n", item.Value, item.Key, string.Format("0x{0:X}", item.Key), instance.PrintRegion(item.Key));
                                            Console.Write("CPF:{0}\t hash:{1} -> Estado:{2} \n", item.Value, string.Format("0x{0:X}", item.Key), instance.PrintRegion(item.Key));
                                            count++;
                                        }//0x{0:X}
                                        Console.Write("\n------------\n Total de registros neste nó: {0} identificadores.\n------------\n", count);
                                        break;
                                    }
                                case 'X':
                                    {
                                        PrintNodeInfo(instance, true);
                                        break;
                                    }
                                case '?':
                                    {
                                        Console.WriteLine("Get Server [I]nfo, E[x]tended Info, [Q]uit, or Get Help[?]");
                                        break;
                                    }
                                case 'Q':
                                    {
                                        instance.Depart();
                                        return;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Get Server [I]nfo, E[x]tended Info, [Q]uit, or Get Help[?]");
                                        break;
                                    }
                            }
                        }
                    }
                }
                else if (args.Length == 3)
                {
                    // join to existing node
                    int portNum = Convert.ToInt32(args[0]);
                    int seedPort = Convert.ToInt32(args[2]);
                    //ChordServer.LocalNode = new ChordNode(System.Net.Dns.GetHostName(), portNum); 
                    ChordServer.LocalNode = new ChordNode(fqdn, portNum); 

                    if (ChordServer.RegisterService(portNum))
                    {
                        ChordInstance instance = ChordServer.GetInstance(ChordServer.LocalNode);
                        instance.Join(new ChordNode(args[1], seedPort), ChordServer.LocalNode.Host, ChordServer.LocalNode.PortNumber);
                        //int i = 1;
                        //ulong i = 0;
                        while (true)
                        {
                            switch (Char.ToUpperInvariant(Console.ReadKey(true).KeyChar))
                            {
                                case 'I':
                                    {
                                        PrintNodeInfo(instance, false);
                                        break;
                                    }
                                case 'A':
                                    {
                                        //instance.AddKey(ChordServer.LocalNode.ID + i);
                                        //ulong temp = ChordServer.LocalNode.ID + i - 10;
                                        //instance.AddKey(temp,"RG"+i.ToString());
                                        //Console.Write("Insira o identificador:\n");
                                        //string arg1 = Console.ReadLine();
                                        //Console.Write("Insira o Estado:\n");
                                        //string arg2 = Console.ReadLine();
                                        //instance.AddKey(arg1, arg2);
                                        //i++;
                                        foreach (string item in GetNumber(11))
                                        {
                                            var rnd = new Random(DateTime.Now.Millisecond);
                                            int index = rnd.Next(0, 26);
                                            //instance.AddKey(item, estados[index]);
                                            //instance.AddKey(item, fqdn);
                                            instance.AddKey(item, estados[index] + ".rafis.net");
                                            Console.WriteLine("Item {0} inserido na base.", item);
                                        }
                                        break;
                                    }
                                case 'F':
                                    {
                                        //Console.Write("Insira o identificador:\n");
                                        //string id = Console.ReadLine();
                                        //Console.WriteLine("{0}\n", instance.FindKey(ChordServer.GetHash(id)));
                                        //Console.WriteLine("{0}\n", instance.FindSuccessor(ChordServer.GetHash(id)));
                                        //break;
                                        Console.Write("Insira o identificador:\n");
                                        ulong id = ChordServer.GetHash(Console.ReadLine());
                                        if (instance.FindKey(id) != "")
                                        {
                                            Console.Write("CPF encontrado:\n");
                                            Console.WriteLine("{0}\n", instance.FindKey(id));
                                        }
                                        else
                                        {
                                            Console.Write("CPF não localizado!\n");
                                        }
                                        //Console.WriteLine("{0}\n", instance.FindKey(id));
                                        //Console.WriteLine("{0}\n", ChordServer.CallFindKey(instance.FindSuccessor(id), id));
                                        //Console.WriteLine("{0}\n", instance.FindSuccessor(id));
                                        break;
                                    }
                                case 'L':
                                    {
                                        int count = 0;
                                        foreach (var item in instance.PrintKeys())
                                        {
                                            Console.Write("CPF:{0}\t hash:{1} -> Estado:{2} \n", item.Value, string.Format("0x{0:X}", item.Key), instance.PrintRegion(item.Key));
                                            count++;
                                        }//0x{0:X}
                                        Console.Write("\n------------\n Total de registros neste nó: {0} identificadores.\n------------\n", count);
                                        break;
                                    }
                                case 'X':
                                    {
                                        PrintNodeInfo(instance, true);
                                        break;
                                    }
                                case '?':
                                    {
                                        Console.WriteLine("Get Server [I]nfo, E[x]tended Info, [Q]uit, or Get Help[?]");
                                        break;
                                    }
                                case 'Q':
                                    {
                                        instance.Depart();
                                        return;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Get Server [I]nfo, E[x]tended Info, [Q]uit, or Get Help[?]");
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    Usage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception: {0}", ex);
                Usage();
            }
        }

        /// <summary>
        /// Print usage information.
        /// </summary>
        static void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\tNChord.exe <portToRunOn> [ <seedHost> <seedPort> ]");
        }

        /// <summary>
        /// Print information about a given Chord node.
        /// </summary>
        /// <param name="instance">The Chord instance to get information from.</param>
        /// <param name="extended">Whether or not to print extended information.</param>
        static void PrintNodeInfo(ChordInstance instance, bool extended)
        {
            
            ChordNode successor = instance.Successor;
            ChordNode predecessor = instance.Predecessor;
            ChordFingerTable fingerTable = instance.FingerTable;
            ChordNode[] successorCache = instance.SuccessorCache;

            string successorString, predecessorString, successorCacheString, fingerTableString;
            if (successor != null)
            {
                successorString = successor.ToString();
            }
            else
            {
                successorString = "NULL";
            }

            if (predecessor != null)
            {
                predecessorString = predecessor.ToString();
            }
            else
            {
                predecessorString = "NULL";
            }

            successorCacheString = "SUCCESSOR CACHE:";
            for (int i = 0; i < successorCache.Length; i++)
            {
                successorCacheString += string.Format("\n\r{0}: ", i);
                if (successorCache[i] != null)
                {
                    successorCacheString += successorCache[i].ToString();
                }
                else
                {
                    successorCacheString += "NULL";
                }
            }

            fingerTableString = "FINGER TABLE:";
            for (int i = 0; i < fingerTable.Length; i++)
            {
                fingerTableString += string.Format("\n\r{1:x8}: -{0}- ", i, fingerTable.StartValues[i]);
                if (fingerTable.Successors[i] != null)
                {
                    fingerTableString += fingerTable.Successors[i].ToString();
                }
                else
                {
                    fingerTableString += "NULL";
                }
            }

            Console.WriteLine("\n\rNODE INFORMATION:\n\rSuccessor: {1}\r\nLocal Node: {0}\r\nPredecessor: {2}\r\n", ChordServer.LocalNode, successorString, predecessorString);

            if (extended)
            {
                Console.WriteLine("\n\r" + successorCacheString);

                Console.WriteLine("\n\r" + fingerTableString);
            }
        }

        private static Random rng = new Random(Environment.TickCount);

        private static List<string> GetNumber(object objlength)
        {
            List<string> codigos = new List<string>();
            for (int index = 0; index < 200; index++)
            {
                int length = Convert.ToInt32(objlength);
                var number = rng.NextDouble().ToString("0.000000000000").Substring(2, length);
                codigos.Add(number);
            }
            return codigos;
        }
       
    }
}
