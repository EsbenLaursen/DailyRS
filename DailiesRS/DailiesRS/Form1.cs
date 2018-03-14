using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailiesRS
{
    public partial class Form1 : Form
    {
        private Database db;
        public Form1()
        {
            InitializeComponent();
            db = new Database();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Populate();
        }

        private void Populate()
        {
            var tasks = db.ReadAllTasks();
            checkedListBox1.Items.Clear();
            foreach (var task in tasks)
            {
                checkedListBox1.Items.Add(task);
            }
        }

        private void addDailyBtn_Click(object sender, EventArgs e)
        {
            string s = ShowDialog();
            if (s != "")
            {
                checkedListBox1.Items.AddRange(new object[] {
                    s});
            }
            db.CreateTask(s);


        }
        public new static string ShowDialog()
        {
            Form dlg1 = new Form()
            {
                Size = new Size(350, 100)
            };

            var textInput = new TextBox()
            {
                Left = 20,
                Top = 20,
                Size = new Size(100, 23),

            };
            var addBtn = new Button()
            {
                Left = 150,
                Top = 20,
                Text = "Add",
                Size = new System.Drawing.Size(75, 23),
                DialogResult = DialogResult.OK

            };
            var cancelBtn = new Button()
            {
                Left = 250,
                Top = 20,
                Text = "Cancel",
                Size = new System.Drawing.Size(75, 23),
                DialogResult = DialogResult.Cancel

            };

            addBtn.Click += (sender, e) => { dlg1.Close(); };
            dlg1.Controls.Add(textInput);
            dlg1.Controls.Add(addBtn); dlg1.Controls.Add(cancelBtn);


            var ok = dlg1.ShowDialog();

            return ok == DialogResult.OK ? textInput.Text : "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var selected = checkedListBox1.SelectedItem;
            if (selected != null)
            {
                string item = checkedListBox1.SelectedItem.ToString();
                db.RemoveTask(item);
                Populate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            db.DeleteAll();
            List<string> defaultTasks = new List<string>(new string[]
                {
                    "Vis vax", "Reaper task", "Ports", "Motherload"
                    , "City quests", "Scarabes", "Wobbegong"
                });
            foreach (var defaultTask in defaultTasks)
            {
                db.CreateTask(defaultTask);
            }
            Populate();
        }
    }
}
