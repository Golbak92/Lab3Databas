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
        private Lab2BokhandelContext db = new Lab2BokhandelContext();
        private int currentRowIndex;
        private int currentStoreID;
        private bool isNewStock = false;

        private void Form1_Load(object sender, EventArgs e)
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Butiker store)
            {
                currentStoreID = store.ButiksId;
            }

            if (e.Node.Tag is ICollection<Lagersaldo> stocks)
            {
                dataGridView1.Rows.Clear();

                foreach (var stock in stocks)
                {
                    var rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Tag = stock;

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

            isNewStock = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (e.ColumnIndex == 0)
            {
                var bok = dataGridView1.Rows[e.RowIndex].Cells[0].Value as Böcker;

                dataGridView1.Rows[e.RowIndex].Cells[1].Value = bok.Titel;
                dataGridView1.Rows[e.RowIndex].Cells[2].Value = $"{bok.Författare.Förnamn} {bok.Författare.Efternamn}";
            }
            else if (e.ColumnIndex == 3)
            {
                int result;

                Int32.TryParse(cell.Value.ToString(), out result);

                var stockDetail = dataGridView1.Rows[e.RowIndex].Tag as Lagersaldo;
                if (stockDetail != null)
                {
                    stockDetail.Antal = (int?)result;
                }

                if (isNewStock)
                {
                    var book = dataGridView1.Rows[e.RowIndex].Cells[0].Value as Böcker;

                    Lagersaldo newStock = new Lagersaldo()
                    {
                        ButiksId = currentStoreID,
                        Antal = result,
                        Isbn = book.Isbn
                    };

                    db.Lagersaldos.Add(newStock);
                }
            }
        }

        private void RemoveBook_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows[currentRowIndex].Tag is Lagersaldo stock)
            {
                var result = MessageBox.Show(
                    $"Are you sure?", "Remove book",
                    MessageBoxButtons.YesNo
                   );

                if (result == DialogResult.Yes)
                {
                    db.Remove(stock);
                    var nodeToDelete = treeView1.SelectedNode;
                    nodeToDelete.Parent.Nodes.Remove(nodeToDelete);
                    dataGridView1.Rows.Remove(dataGridView1.Rows[currentRowIndex]);

                    db.SaveChanges();
                }
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.SaveChanges();
            db.Dispose();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            currentRowIndex = e.RowIndex;
        }
    }
}