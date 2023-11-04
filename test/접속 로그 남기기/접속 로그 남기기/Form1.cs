using System;
using System.Windows.Forms;
using System.IO;

namespace 접속_로그_남기기
{
    public partial class Form1 : Form
    {
        private static string logFilePath = @"C:\a.txt";

        public Form1()
        {
            InitializeComponent();
            string userIdentifier = Environment.UserName; // 현재 로그인한 사용자의 이름을 가져옴
            LogAccess(userIdentifier);
        }

        public static void LogAccess(string userIdentifier)
        {
            string logEntry = $"{DateTime.Now}: {userIdentifier} 접속함";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}
