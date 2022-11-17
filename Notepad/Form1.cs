using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private RichTextBox GetRichTextBox() 
        {
            RichTextBox rtb = null;
            TabPage tp = tbCtrl.SelectedTab;

            if (tp != null)
                rtb = tp.Controls[0] as RichTextBox;

            return rtb;
        }

        private void createNewTabPage(string caption) {
            TabPage tp = new TabPage(caption);
            tp.BorderStyle = BorderStyle.None;
            RichTextBox rtb = new RichTextBox();
            rtb.BorderStyle = BorderStyle.None;
            rtb.Dock = DockStyle.Fill;

            tp.Controls.Add(rtb);
            tbCtrl.TabPages.Add(tp);
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createNewTabPage("Новий документ");
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Paste();
        }

        private string getJustFileName(string fullFilePath) {
            string[] filenamePaths = fullFilePath.Split('\\');
            return filenamePaths[filenamePaths.Length - 1];
        }

        private void readFile(string fileName) {
            if (File.Exists(fileName))
            {
                string fileText = File.ReadAllText(fileName);

                createNewTabPage(getJustFileName(fileName));
                tbCtrl.SelectedTab = tbCtrl.TabPages[tbCtrl.TabPages.Count - 1];

                RichTextBox rtb = GetRichTextBox();

                if (getFileExt(fileName) == "rtf")
                    rtb.Rtf = fileText;
                else
                    rtb.Text = fileText;
            }
        }
        private void openFile() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "RichTextFormat (*.rtf)|*.rtf|Text Files |*.txt";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                readFile(openFileDialog.FileName);

            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private string getFileExt(string fileName) {
            string[] fileParts = fileName.Split('.');
            return fileParts[fileParts.Length - 1];
        }
        private void saveFile() {
            if (tbCtrl.TabCount != 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = tbCtrl.SelectedTab.Text;
                saveFileDialog.Filter = "RichTextFormat (*.rtf)|*.rtf|Text Files |*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileMode mode;
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        mode = FileMode.Truncate;
                    }
                    else
                    {
                        mode = FileMode.CreateNew;
                    }

                    string fileName = saveFileDialog.FileName;

                    using (Stream s = File.Open(fileName, mode))
                    using (StreamWriter sw = new StreamWriter(s))
                    {

                        
                        if (getFileExt(fileName) == "rtf") {
                            sw.Write(GetRichTextBox().Rtf);
                        }
                        else
                        {
                            sw.Write(GetRichTextBox().Text);
                        }
                        
                    }
                    tbCtrl.SelectedTab.Text = getJustFileName(saveFileDialog.FileName);

                }
            }
        }
        
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void closeFile() {

            if (tbCtrl.TabCount != 0)
            {
                if (MessageBox.Show("Чи бажаєте ви зберігти файл " + tbCtrl.SelectedTab.Text,
                    "Інформаційне повідомлення", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    saveFile();
                tbCtrl.TabPages.Remove(tbCtrl.SelectedTab);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeFile();
        }

        private void setFont() {
            RichTextBox rtb = GetRichTextBox();
            if (rtb == null)
                return;
            if (rtb.SelectedText == "")
                return;
            FontDialog fd = new FontDialog();
            fd.Font = rtb.SelectionFont;
            fd.ShowColor = false;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                rtb.SelectionFont = fd.Font;
            }
        }

        private void editFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setFont();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            createNewTabPage("Новий документ");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            closeFile();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Cut();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Copy();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Paste();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            setFont();
        }

        private Color getColor(Color prevColor) {
            ColorDialog cd = new ColorDialog();
            cd.Color = prevColor;
            Color color = prevColor;
            if (cd.ShowDialog() == DialogResult.OK)
                color = cd.Color;
            return color;
        }

        private void setFontColor() {
            RichTextBox rtb = GetRichTextBox();
            if (rtb != null && rtb.SelectedText != "")
            {
                rtb.SelectionColor = getColor(rtb.SelectionColor);
            }
        }

        private void setBackColor() {
            RichTextBox rtb = GetRichTextBox();
            if (rtb != null && rtb.SelectedText != "")
            {
                rtb.SelectionBackColor = getColor(rtb.SelectionBackColor);
            }
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setFontColor();
        }

        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBackColor();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            setFontColor();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            setBackColor();
        }

        private void openFolder() {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                foreach (string file in files) {
                    string fileExt = getFileExt(file);
                    if(fileExt == "rtf" || fileExt == "txt")
                        readFile(file);
                }
            }


        }
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFolder();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            openFolder();
        }
    }
}
