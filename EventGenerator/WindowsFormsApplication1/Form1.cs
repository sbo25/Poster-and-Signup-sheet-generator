using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string path;
        private string size = "Half";
        private string type = "Poster";
        private int attnum;
        private int waitnum;

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Half");
            comboBox1.Items.Add("Full");
            comboBox2.Items.Add("Poster");
            comboBox2.Items.Add("Sign Up Sheet");
            waitlist.Hide();
            waitlistcount.Hide();
            attendee.Hide();
            attendeecount.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  create document
            try
            {

                //change date format
                string date2 = date.Text + "";
                date.Format = DateTimePickerFormat.Custom;
                date.CustomFormat = "yyyy.MM.dd";

                //initialize new doc's with naming convention
                string name = date.Text + " " + what.Text + " with " + who.Text;
                string filepath = "C:\\Users\\Samuel\\Desktop\\" + name + " " + type + ".docx";
                Console.WriteLine(type);
                //  copy poster format to  new doc
                if (type == "Poster") { 
                    if (size == "Half")
                    {
                        File.Copy("C:\\Users\\Samuel\\Desktop\\HalfTemplate.docx", filepath, true);
                    }
                    else {
                        File.Copy("C:\\Users\\Samuel\\Desktop\\FullTemplate.docx", filepath, true);
                    }
                }
                else
                {
                    File.Copy("C:\\Users\\Samuel\\Desktop\\SignUpTemplate.docx", filepath, true);
                }
                Console.WriteLine("4");
                //  create missing object
                object missing = Missing.Value;
                //  create Word application object
                Word.Application wordApp = new Word.Application();
                //  create Word document object
                Word.Document aDoc = null;
                //  create & define filename object with temp.doc
               
                object filename = filepath;
                //  if temp.doc available
                Console.WriteLine("1");
                if (File.Exists((string)filename))
                {
                    object readOnly = false;
                    object isVisible = false;
                    //  make visible Word application
                    wordApp.Visible = false;
                    //  open Word document named temp.doc
                    aDoc = wordApp.Documents.Open(ref filename, ref missing,
                                                   ref readOnly, ref missing, ref missing, ref missing,
                                                   ref missing, ref missing, ref missing, ref missing,
                                                   ref missing, ref isVisible, ref missing, ref missing,
                                                   ref missing, ref missing);
                    aDoc.Activate();
                    //  Call FindAndReplace()function for each change
                   
                    this.FindAndReplace(wordApp, "<Event_Name>", what.Text);
                    this.FindAndReplace(wordApp, "<Host>", who.Text);
                    this.FindAndReplace(wordApp, "<Date>", date2);
                    this.FindAndReplace(wordApp, "<Time>", time.Text);
                    this.FindAndReplace(wordApp, "<Place>", where.Text);
                    
                    //set pic dimensions
                    Word.Range range = aDoc.Bookmarks["picLocation"].Range;
                    float picHeight;
                    float picWidth;
                    float pic1Top;
                    float pic1Left;
                    float pic2Top;
                    float pic2Left;


                    if (type == "Poster")
                    {
                        if (size == "Half")
                        {
                            picHeight = (float)220.07;
                            picWidth = (float)200.94;
                            pic1Top = (float)Word.WdShapePosition.wdShapeTop;
                            pic1Left = (float)Word.WdShapePosition.wdShapeRight;
                            pic2Top = (float)288;
                            pic2Left = (float)Word.WdShapePosition.wdShapeRight;
                        }
                        else
                        {
                            picHeight = (float)247.07;
                            picWidth = (float)230.94;
                            pic1Top = (float)147;
                            pic1Left = (float)Word.WdShapePosition.wdShapeRight;
                            pic2Top = (float)288;
                            pic2Left = (float)Word.WdShapePosition.wdShapeRight;
                        }
                    }
                    else
                    {
                        picHeight = (float)100.07;
                        picWidth = (float)120.94;
                        pic1Top = (float)Word.WdShapePosition.wdShapeTop;
                        pic1Left = (float)Word.WdShapePosition.wdShapeLeft;
                        pic2Top = (float)288;
                        pic2Left = (float)Word.WdShapePosition.wdShapeRight;
                    }
                    Console.WriteLine("1");
                    
                    //insert top picture
                    this.insertPicture(aDoc, path, picWidth, picHeight, pic1Top, pic1Left, range);
                    Console.WriteLine("2");
                    //insert bottop picture
                    if ( type != "Sign Up Sheet" && size != "Full")
                    {
                        this.insertPicture(aDoc, path, picWidth, picHeight, pic2Top, pic2Left, range);
                    }

                    //insert table if sign up sheet
                    if (type == "Sign Up Sheet")
                    {
                        object start = 0;
                        object end = 0;
                        Word.Range tableLocation = aDoc.Bookmarks["tableLocation"].Range;
                        aDoc.Tables.Add(tableLocation, 3, 4);
                    }

                        //save new doc after modified
                        aDoc.Save();

                    //close doc
                    wordApp.Quit(Type.Missing, Type.Missing, Type.Missing);
                }
                else
                    MessageBox.Show("File does not exist.",
            "No File", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error in process.", "Internal Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
                

            }
        }

        private void FindAndReplace(Word.Application wordApp,
            object findText, object replaceText)
        {
            
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            wordApp.Selection.Find.Execute(ref findText, ref matchCase,
                ref matchWholeWord, ref matchWildCards, ref matchSoundsLike,
                ref matchAllWordForms, ref forward, ref wrap, ref format,
                ref replaceText, ref replace, ref matchKashida,
                        ref matchDiacritics,
                ref matchAlefHamza, ref matchControl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //store the path
                path = openFileDialog1.FileName;
                textBox1.Text = path;
            }
        }

        private void insertPicture(Word.Document doc, string path, float width, float height, float top, float left, Word.Range range)
        {
            //create picture
            Word.InlineShape picture = doc.InlineShapes.AddPicture(path, Type.Missing, Type.Missing, range);

            //set height and width 
            picture.Height = height;
            picture.Width = width;

            //convert to shape for further customization
            Word.Shape shape = picture.ConvertToShape();
            
            
            //add a border
            shape.Line.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;

            //send picture behind text then position it
            shape.WrapFormat.Type = Word.WdWrapType.wdWrapBehind;
            shape.Left = left;
            shape.Top = top;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            size = comboBox1.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = comboBox2.Text;
            if (comboBox2.Text == "Sign Up Sheet")
            {
                attendee.Show();
                attendeecount.Show();
                waitlist.Show();
                waitlistcount.Show();
                label6.Hide();
                comboBox1.Hide();
            }
            else {
                attendee.Hide();
                attendeecount.Hide();
                waitlist.Hide();
                waitlistcount.Hide();
                label6.Show();
                comboBox1.Show();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            
        }

        private void attendeecount_ValueChanged(object sender, EventArgs e)
        {
            attnum = (int)attendeecount.Value;
        }

        private void waitlistcount_ValueChanged(object sender, EventArgs e)
        {
            waitnum = (int)waitlistcount.Value;
        }
    }

        
    }

