using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabLinqJoin22
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataAccess dataAcc = new DataAccess();


        private void Form1_Load(object sender, EventArgs e)
        {

           
            dataGridView1Example.DataSource = dataAcc.Query1Example();
            label1ex.Text = dataGridView1Example.RowCount.ToString();

            dataGridView1.DataSource = dataAcc.Query1();
            dataGridView1.Refresh();
            if (dataGridView1.RowCount > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabQuery1"];
            label1.Text = dataGridView1.RowCount.ToString();

            //dataGridView2.DataSource = dataAcc.Query2();
            //if (dataGridView2.RowCount > 0)
            //    tabControl.SelectedTab = tabControl.TabPages["tabQuery2"];
            //label2.Text = dataGridView2.RowCount.ToString();

            dataGridView3.DataSource = dataAcc.Query3();
            if (dataGridView3.RowCount > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabQuery3"];
            label3.Text = dataGridView3.RowCount.ToString();


            dataGridView4.DataSource = dataAcc.Query4();
            if (dataGridView4.RowCount > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabQuery4"];
            label4.Text = dataGridView4.RowCount.ToString();

            IOrderedEnumerable<IGrouping<string, Models.Tutor>> groupsEx = dataAcc.Query7Example();
            if (groupsEx != null)
            {
                foreach (var gr in groupsEx)
                {
                    textBoxGroupExample.Text += gr.Key + "\r\n";
                    foreach (Models.Tutor t in gr)
                    {
                        textBoxGroupExample.Text += "    " + t.NameFio + "\r\n";
                    }
                }
            }

            IOrderedEnumerable<IGrouping<string, Models.Student>> groupsSt = dataAcc.Query7();
            if (groupsSt != null)
            {
                foreach (var gr in groupsSt)
                {
                    textBoxGroup.Text += gr.Key + "\r\n";
                    foreach (Models.Student st in gr)
                    {
                        textBoxGroup.Text += "    " + st.Surname + " " + (st.Name ?? "") + " " + (st.Patronymic ?? "");
                        if (st.Group != null)
                            textBoxGroup.Text += ", " + (st.Group.GroupNumber ?? "");
                        textBoxGroup.Text += "\r\n";
                    }
                }
            }

            if (textBoxGroup.Text.Length > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabTask7"];

            object Task5DataEx = dataAcc.Query5Example();
            dataGridViewAggrExample.DataSource = Task5DataEx;
            dataGridViewAggrExample.Refresh();
            label5ex.Text = dataGridViewAggrExample.RowCount.ToString();

            object Task5Data = dataAcc.Query5();
            dataGridViewAggr.DataSource = Task5Data;
            if (dataGridViewAggr.RowCount > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabTask5"];
            label5.Text = dataGridViewAggr.RowCount.ToString();

            dataGridView6Example.DataSource = dataAcc.Query6Example(); 
            dataGridView6Example.Refresh();
            label6ex.Text = dataGridView6Example.RowCount.ToString();

            dataGridView6.DataSource = dataAcc.Query6(); 
            if (dataGridView6.RowCount > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabTask6"];
            label6.Text = dataGridView6.RowCount.ToString();

            dataGridView8Example.DataSource = dataAcc.Query8Example();
            dataGridView8Example.Refresh();
            label8ex.Text = dataGridView8Example.RowCount.ToString();

            dataGridView8.DataSource = dataAcc.Query8();
            if (dataGridView8.RowCount > 0)
                tabControl.SelectedTab = tabControl.TabPages["tabTask8"];
            label8.Text = dataGridView8.RowCount.ToString();
        }
    }
}
