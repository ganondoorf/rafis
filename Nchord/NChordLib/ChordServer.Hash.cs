/*
 * ChordServer.Hash.cs:
 * 
 *  Implements the universally-used hash algorithm for hashing keys.  In this implementation, the
 *  MD5 hash algorithm is used, where the digest is truncated to 64-bits (in order to fit the
 *  UInt64 ID size).
 * 
 *  In previous implementations, SHA-1 hashing (again, truncated) was used with success as well,
 *  despite being considered a somewhat inferior hash algorithm for use in a DHT.
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
using System.Security.Cryptography;
using System.Text;
using System.Numerics;

namespace NChordLib
{
    public static partial class ChordServer
    {
        /// <summary>
        /// Gets the 64-bit truncated MD5 hash value of a given string key.
        /// </summary>
        /// <param name="key">The key to hash.</param>
        /// <returns>A ulong-truncated MD5 hash digest of the string key.</returns>
        public static UInt64 GetHash(string key)
        {
            //Alerado, MD5 por SHA256
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            SHA256Managed sha256 = new SHA256Managed();
            byte[] bytesSha = Encoding.ASCII.GetBytes(key);
            bytesSha=sha256.ComputeHash(bytesSha);
            byte[] byteMd5 = Encoding.ASCII.GetBytes(key);
            byteMd5 = md5.ComputeHash(byteMd5);
            byte[] teste = Encoding.ASCII.GetBytes("teste");
            byte[] testehash = sha256.ComputeHash(teste);
            //byte[] bytes = Encoding.ASCII.GetBytes(key);
            //bytes = md5.ComputeHash(bytes);
            //return BitConverter.ToUInt64(bytes,0);
            //var mahByteArray = new ArrayList<byte>();
            //mahByteArray.Add(0x00);
            //var bigInteger = new BigInteger(byteArray.Concat(new byte[] { 0 }).ToArray());
            //BigInteger bigteste;
            //bigteste = new BigInteger(testehash);
            return BitConverter.ToUInt64(bytesSha, 0);
        }
    }
}
