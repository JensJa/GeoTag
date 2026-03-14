using ExifEdit;

namespace ExifEditDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            // Dateien aus den gezogenen Daten auslesen
            string[] filesNames = (string[])e.Data.GetData(DataFormats.FileDrop,
            false);
            foreach (string fileName in filesNames)
            {
                // FileInfo-Objekt erzeugen
                FileInfo fi = new FileInfo(fileName);
                if (fi.Exists)
                    // Wenn es sich nicht um einen Ordner handelt: FileInfo-Objekt der
                    // Liste anfügen
                    tb_filename.Text = fi.FullName.ToString();

            }
        }
        private void FormMain_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void but_work_Click(object sender, EventArgs e)
        {


            ReadWriteFile exifReader = new ReadWriteFile();
            exifReader.readFile(@"D:\TestBilder\Watvögel\IR5_2022-10-28-3046A.jpg");
            lb_exifdata.Items.Add("---EXIF---");
            foreach (var item in exifReader.ExifEntryList)
            {

                lb_exifdata.Items.Add($"{item.Value.ifdTyp}:{item.Key} - {item.Value.tagtyp.ToString()} = {item.Value.value} [{item.Value.typ} ]");

            }
            lb_exifdata.Items.Add("---IPTC---");
            foreach (var item in exifReader.IptcList)
            {
                foreach (var field in item.Value)
                {
                    lb_exifdata.Items.Add($" {item.Key} - {field.size} - {field.Tag} = {field.DataString} ");
                }

            }
        }
    }
}

