using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace messages_test
{

    public partial class Form1 : Form
    {
        LogList<logEntry> log = new LogList<logEntry>();
        BindingSource source = new BindingSource();

        public Color[] severityColors = new Color[] { Color.Red, Color.Red, Color.Orange, Color.Yellow, Color.Black, Color.Black, Color.Blue,Color.Blue };
        public string[] severityNames = new string[] { "EMERGENCY", "ALERT", "CRITICAL", "ERROR", "WARNING", "NOTICE", "INFO", "DEBUG" };
        public Random rand = new Random();

        public Form1()
        {
            InitializeComponent();


            dataGridView1.Columns[Delete.Index].CellTemplate.Value = messages_test.Properties.Resources.OK;
            dataGridView1.Columns[Info.Index].CellTemplate.Value = messages_test.Properties.Resources.Info;

            dataGridView1.Columns[Time.Index].DefaultCellStyle.Format = "HH:mm:ss";
            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            source.DataSource = log;
            dataGridView1.DataSource = source;

            var rand = new Random();


            Console.WriteLine(Severity.Index);

            for (int i = 0; i < 10; i++)
            {
                string message = "message" + (i % 20).ToString();
                addNewEntry(DateTime.Now.AddSeconds(rand.Next(240)), rand.Next(7), message);
            }
        }
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
          //dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = severityColors[Array.IndexOf(severityNames,dataGridView1.Rows[e.RowIndex].Cells[4].Value)];
        }


        private int getLogIndex(int severity, string message, byte sysID)
        {

            //Ok this is ugly, but works. 

            for (int i=0;i<log.Count;i++)
            {
                if ((log.OriginalList[i].SeverityText == severityNames[severity])
                    && (log.OriginalList[i].MessageText == message)
                    && (log.OriginalList[i].sysid == sysID))
                {
                    return i;
                }
            }

            return -1;

        }

        private void addNewEntry(DateTime time, int severity, string message, byte sysID = 1)
        {

            var filt = source.Filter;
            source.RemoveFilter();

            var index = getLogIndex(severity, message, sysID);

            if (index < 0)
            {
                log.Add(new logEntry(time, message, severity, sysID));
                log.ApplySort("Time", ListSortDirection.Descending);
            }
            else
            {
                log.OriginalList[index].Time = time;
                log.OriginalList[index].Repeats++;
                log.ApplySort("Time", ListSortDirection.Descending);
                source.ResetBindings(false);
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;


            }

            source.Filter = filt;

        }

        private void removeEntry(int severity, string message, byte sysID = 1)
        {
            var filt = source.Filter;
            source.RemoveFilter();

            var index = getLogIndex(severity, message, sysID);
            if (index < 0)
            {
            }
            else
            {
                log.OriginalList.RemoveAt(index);
                log.ApplySort("Time", ListSortDirection.Descending);
                source.ResetBindings(false);
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
            }
            source.Filter = "";
            source.Filter = filt;

        }



        private void button1_Click(object sender, EventArgs e)
        {
            source.RemoveFilter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addNewEntry(DateTime.Now.AddMinutes(4), rand.Next(7), "message XXX", 1);
        }


        private void button11_Click(object sender, EventArgs e)
        {
            addNewEntry(DateTime.Now.AddMinutes(4), rand.Next(7), "message XXX", 2);

        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity @ 7";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 2";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 3";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 4";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 5";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 6";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            source.Filter = "Severity = 7";
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                removeEntry(Array.IndexOf(severityNames, senderGrid.Rows[e.RowIndex].Cells[4].Value), senderGrid.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns["sysid"].Visible = false;
            dataGridView1.Columns["componentid"].Visible = false;
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[Info.Index].Value = messages_test.Properties.Resources.Info;
            e.Row.Cells[Delete.Index].Value = messages_test.Properties.Resources.OK;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //I supposed the image column is at index 1
            if (e.ColumnIndex == Info.Index)
                e.Value = messages_test.Properties.Resources.info1;
            else if (e.ColumnIndex == Delete.Index)
                e.Value = messages_test.Properties.Resources.trash;


        }
    }
}
