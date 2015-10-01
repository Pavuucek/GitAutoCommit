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
using System.Collections.Generic;
using System.Windows.Forms;
using GitAutoCommit.Core;

namespace GitAutoCommit.Controls
{
    /// <summary>
    ///     Auto commit list
    /// </summary>
    public class AutoCommitList : EditableList<AutoCommitTask>
    {
        /// <summary>
        ///     Binds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public void Bind(IList<AutoCommitTask> items)
        {
            Bind(items, CreateItem);
        }


        /// <summary>
        ///     Creates the item.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        private ListViewItem CreateItem(AutoCommitTask task)
        {
            return new ListViewItem
            {
                Text = string.Format("{0} ({1})", task.Name, task.Folder),
                ImageKey = task.IsValid() ? "ok" : "error"
            };
        }
    }
}