#region License

/*
Copyright (c) 2011 Gareth Lennox (garethl@dwakn.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.
    * Neither the name of Gareth Lennox nor the names of its
    contributors may be used to endorse or promote products derived from this
    software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace GitAutoCommit
{
    /// <summary>
    ///     Extensions class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Move an item in a list
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="index">Index of the item</param>
        /// <param name="difference">Direction to move (-ve for up, +ve for down. The actual value determines how many places)</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is not a valid index in the
        ///     <see cref="T:System.Collections.IList" />.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="T:System.Collections.IList" /> is read-only.-or- The
        ///     <see cref="T:System.Collections.IList" /> has a fixed size.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///     <paramref name="list" /> is null reference in the
        ///     <see cref="T:System.Collections.IList" />.
        /// </exception>
        public static void Move(this IList list, int index, int difference)
        {
            var newIndex2 = index + difference;

            var temp1 = list[index];

            list.RemoveAt(index);
            list.Insert(newIndex2, temp1);
        }

        /// <summary>
        ///     Move an item in a list
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="list">List</param>
        /// <param name="index">Index of the item</param>
        /// <param name="difference">Direction to move (-ve for up, +ve for down. The actual value determines how many places)</param>
        public static void Move<T>(this IList<T> list, int index, int difference)
        {
            var newIndex2 = index + difference;

            var temp1 = list[index];

            list.RemoveAt(index);
            list.Insert(newIndex2, temp1);
        }
    }
}