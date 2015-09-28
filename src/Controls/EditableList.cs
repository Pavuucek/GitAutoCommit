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
using GitAutoCommit.Properties;

namespace GitAutoCommit.Controls
{
    public partial class EditableList<T> : UserControl where T : class
    {
        private Func<T, ListViewItem> _createItemCallback;
        private IList<T> _items;
        private bool _sortable = true;

        public EditableList()
        {
            InitializeComponent();

            list.Resize += delegate { OnListResize(); };

            OnListResize();

            imageList.Images.Add("ok", Resources.ok);
            imageList.Images.Add("error", Resources.error);
        }

        /// <summary>
        ///     Image list
        /// </summary>
        public ImageList ImageList
        {
            get { return imageList; }
        }

        public bool Sortable
        {
            get { return _sortable; }
            set
            {
                _sortable = value;

                separator.Visible = _sortable;
                moveDownButton.Visible = _sortable;
                moveUpButton.Visible = _sortable;
            }
        }

        public event EventHandler<ValueEventArgs<T>> ItemAdd;
        public event EventHandler<ValueEventArgs<T>> ItemEdit;
        public event EventHandler ListChanged;

        public void Bind(IList<T> items, Func<T, ListViewItem> createItemCallback)
        {
            list.SmallImageList = imageList.Images.Count == 0 ? null : imageList;

            _items = items;
            _createItemCallback = createItemCallback;

            list.SelectedIndices.Clear();
            list.Items.Clear();

            foreach (var item in _items)
            {
                list.Items.Add(_createItemCallback(item));
            }

            list_SelectedIndexChanged(list, EventArgs.Empty);
        }

        private void OnListResize()
        {
            column.Width = list.Width - SystemInformation.VerticalScrollBarWidth - 4;
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isSelected = list.SelectedItems.Count != 0;

            editButton.Enabled = isSelected;
            deleteButton.Enabled = isSelected;

            moveUpButton.Enabled = isSelected && list.SelectedIndices[0] > 0;
            moveDownButton.Enabled = isSelected && list.SelectedIndices[0] < list.Items.Count - 1;
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveItem(1);

            OnListChanged();
        }

        private void OnListChanged()
        {
            if (ListChanged != null)
                ListChanged(this, EventArgs.Empty);
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveItem(-1);

            OnListChanged();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(ParentForm, Resources.EditableList_deleteButton_Click_Are_you_sure, Resources.Program_Main_git_auto_commit, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var index = list.SelectedIndices[0];

                list.Items.RemoveAt(index);
                _items.RemoveAt(index);

                SelectItem(index);

                OnListChanged();
            }
        }

        private void SelectItem(int index)
        {
            if (list.Items.Count == 0)
                return;

            if (index > list.Items.Count - 1)
                index = list.Items.Count - 1;

            list.Items[index].Selected = true;
        }

        private void MoveItem(int indexChange)
        {
            var index = list.SelectedIndices[0];

            list.Items.Move(index, indexChange);
            _items.Move(index, indexChange);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var args = new ValueEventArgs<T>(default(T));

            if (ItemAdd != null)
                ItemAdd(this, args);

            if (args.Value == null || args.Equals(default(T)))
                return;

            _items.Add(args.Value);
            list.Items.Add(_createItemCallback(args.Value));

            SelectItem(list.Items.Count - 1);

            OnListChanged();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            var index = list.SelectedIndices[0];
            var item = _items[index];
            var args = new ValueEventArgs<T>(item);

            if (ItemEdit != null)
                ItemEdit(this, args);

            if (args.Value == null || args.Equals(default(T)))
                return;

            _items[index] = args.Value;
            list.Items[index] = _createItemCallback(args.Value);

            SelectItem(index);

            OnListChanged();
        }

        private void list_DoubleClick(object sender, EventArgs e)
        {
            editButton_Click(sender, e);
        }
    }
}