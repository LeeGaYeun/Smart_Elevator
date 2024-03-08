using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Input;

namespace FaceDetectionApp
{
    public partial class Webcam : Form
    {
        private VideoCapture capture;
        private CascadeClassifier faceCascade;
        private SQLiteConnection conn;
        private Mat grayFrame;
        private bool isProcessing;

        public Webcam()
        {
            InitializeComponent();
            InitializeCamera();
            InitializeCascadeClassifier();
            InitializeDatabase();
        }

        private void InitializeCamera()
        {
            capture = new VideoCapture(0);
            Application.Idle += ProcessFrame;
        }

        private void InitializeCascadeClassifier()
        {
            faceCascade = new CascadeClassifier("haarcascade_frontalface_alt2.xml");
        }

        private void InitializeDatabase()
        {
            conn = new SQLiteConnection("Data Source=C:\\Users\\EMBEDDED\\source\\repos\\Intel\\OpenCV\\elevator_Prj\\faces.db;Version=3;");
            conn.Open();

            using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS faces (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, room TEXT, landmarks TEXT)", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            using (Mat frame = new Mat())
            {
                capture.Read(frame);

                if (!frame.Empty())
                {
                    if (!isProcessing)
                    {
                        grayFrame = new Mat();
                        Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

                        var faces = DetectFaces(grayFrame);

                        if (faces.Length == 1)
                        {
                            foreach (var face in faces)
                            {
                                Cv2.Rectangle(frame, face, Scalar.Red, 2);
                                if (Keyboard.IsKeyDown(Key.Q))
                                {
                                    isProcessing = true;
                                    RegisterFace(face, grayFrame);
                                }
                            }
                        }

                        Web.Image = BitmapConverter.ToBitmap(frame);
                    }
                }
            }
        }

        private Rect[] DetectFaces(Mat grayFrame)
        {
            return faceCascade.DetectMultiScale(grayFrame);
        }

        private void RegisterFace(Rect face, Mat grayFrame)
        {
            string name = PromptInput("이름을 입력하세요 :", "Name");
            string room = PromptInput("층 수를 입력하세요 :", "Room");

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(room))
            {
                try
                {
                    using (var hog = new HOGDescriptor())
                    {
                        hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

                        var roiGray = grayFrame[face];
                        var roiGrayResized = roiGray.Resize(new OpenCvSharp.Size(64, 128));
                        var landmarksVector = hog.Compute(roiGrayResized);
                        var landmarks = landmarksVector.ToArray();

                        string landmarksStr = string.Join(",", landmarks);

                        using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO faces (name, room, landmarks) VALUES (@name, @room, @landmarks)", conn))
                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@room", room);
                            cmd.Parameters.AddWithValue("@landmarks", landmarksStr);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("얼굴 등록 완료.");
                }
                finally
                {
                    isProcessing = false;
                }

            }
            else
            {
                MessageBox.Show("얼굴 등록을 취소하였습니다.");
                isProcessing = false;
            }
        }

        private string PromptInput(string message, string caption)
        {
            using (var inputForm = new Form())
            {
                var lblMessage = new Label();
                lblMessage.Text = message;
                lblMessage.Location = new System.Drawing.Point(10, 20);
                lblMessage.AutoSize = true;

                var txtInput = new TextBox();
                txtInput.Location = new System.Drawing.Point(10, 50);
                txtInput.Size = new System.Drawing.Size(250, 20);

                var btnOK = new Button();
                btnOK.Text = "OK";
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Location = new System.Drawing.Point(90, 80);
                btnOK.Size = new System.Drawing.Size(75, 23);

                inputForm.Text = caption;
                inputForm.ClientSize = new System.Drawing.Size(280, 120);
                inputForm.Controls.AddRange(new Control[] { lblMessage, txtInput, btnOK });
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.StartPosition = FormStartPosition.CenterScreen;
                inputForm.AcceptButton = btnOK;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return txtInput.Text;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
