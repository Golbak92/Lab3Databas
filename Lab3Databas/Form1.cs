using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Lab3Databas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<Böcker> books = new List<Böcker>();
        private List<Lagersaldo> stocks = new List<Lagersaldo>();

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var db = new Lab2BokhandelContext())
            {
                if (db.Database.CanConnect())
                {
                    var store = db.Butikers
                        .Include(b => b.Lagersaldos)
                        .ToList();

                    books = db.Böckers
                        .Include(l => l.Lagersaldos)
                        .Include(f => f.Författare)
                        .ToList();

                    stocks = db.Lagersaldos
                        .ToList();

                    foreach (var stores in store)
                    {
                        TreeNode storeNode = new TreeNode()
                        {
                            Text = $"{stores.Butiksnamn} ({stores.Stad})",
                            Tag = stores
                        };



                        TreeNode stockNode = new TreeNode()
                        {
                            Text = "Lagersaldo",
                            Tag = stores.Lagersaldos
                        };

                        foreach (var stock in stores.Lagersaldos)
                        {
                            foreach (var book in books)
                            {
                                if (book.Isbn == stock.Isbn)
                                {
                                    TreeNode bookNode = new TreeNode($"{book.Titel} x{stock.Antal}");
                                    stockNode.Nodes.Add(bookNode);

                                }
                            }
                        }
                        storeNode.Nodes.Add(stockNode);

                        treeView1.Nodes.Add(storeNode);
                    }
                }
                else Debug.WriteLine("Connection failed");
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is ICollection<Lagersaldo> stocks)
            {
                dataGridView1.Rows.Clear();

                foreach (var stock in stocks)
                {
                    var rowIndex = dataGridView1.Rows.Add();

                    var comboBoxCell = dataGridView1.Rows[rowIndex].Cells[0] as DataGridViewComboBoxCell;
                    comboBoxCell.ValueType = typeof(Böcker);
                    comboBoxCell.DisplayMember = "Isbn";
                    comboBoxCell.ValueMember = "This";


                    dataGridView1.Rows[rowIndex].Cells[3].Value = stock.Antal;

                    foreach (var book in books)
                    {
                        comboBoxCell.Items.Add(book);
                        if (stock.Isbn == book.Isbn)
                        {
                            dataGridView1.Rows[rowIndex].Cells[1].Value = book.Titel;
                            dataGridView1.Rows[rowIndex].Cells[2].Value = $"{book.Författare.Förnamn} {book.Författare.Efternamn}";
                        }
                    }
                    dataGridView1.Rows[rowIndex].Cells[0].Value = stock.IsbnNavigation;
                }
            }
        }

        private void AddBook_Click(object sender, EventArgs e)
        {
                var rowIndex = dataGridView1.Rows.Add();

                var comboBoxCell = dataGridView1.Rows[rowIndex].Cells[0] as DataGridViewComboBoxCell;
                comboBoxCell.ValueType = typeof(Böcker);
                comboBoxCell.DisplayMember = "Isbn";
                comboBoxCell.ValueMember = "This";

                foreach (var book in books)
                {
                    comboBoxCell.Items.Add(book);
                }

            //    dataGridView1.Rows[rowIndex].Cells[3].Value = stock.Antal;

            ////    foreach (var book in books)
            ////    {
            ////        comboBoxCell.Items.Add(book);
            ////        if (stock.Isbn == book.Isbn)
            ////        {
            ////            dataGridView1.Rows[rowIndex].Cells[1].Value = book.Titel;
            ////            dataGridView1.Rows[rowIndex].Cells[2].Value = $"{book.Författare.Förnamn} {book.Författare.Efternamn}";
            ////        }
            ////    }
            ////    dataGridView1.Rows[rowIndex].Cells[0].Value = stock.IsbnNavigation;           
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == books.)
            {
                if ()
                {

                }
            }
        }

        private void RemoveBook_Click(object sender, EventArgs e)
        {
            if ()
            {

            }
        }
    }
}

