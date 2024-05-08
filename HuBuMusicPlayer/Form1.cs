using NAudio;
using NAudio.Wave;
using NAudio.Vorbis;
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
using System.Reflection.Emit;

namespace WindowsMusic2
{
    public partial class Form1 : Form
    {
        private WaveOutEvent waveOut;
        private VorbisWaveReader reader;
        string[] files;
        List<string> localmusiclist = new List<string> { };
        public Form1()
        {
            InitializeComponent();
            waveOut = new WaveOutEvent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "选择音频|*.mp3;*.flac;*.wav";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                localmusiclist.Clear();

                if (files != null)
                {
                    Array.Clear(files, 0, files.Length);
                }
                files = openFileDialog1.FileNames;
                string[] array = files;
                try
                {
                    foreach (string filePath in array)
                    {
                        // 检查文件是否存在  
                        if (File.Exists(filePath))
                        {
                            listBox1.Items.Add(filePath);
                            localmusiclist.Add(filePath);
                        }
                        else
                        {
                            // 文件不存在，可以记录日志或通知用户  
                            MessageBox.Show($"文件 {filePath} 不存在。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 捕获并处理异常  
                    MessageBox.Show($"发生了一个错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StopAndDisposeWaveOut()
        {
            // 停止和释放WaveOutEvent
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            // 停止和释放VorbisWaveReader
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        private void PlayOgg(string filePath)
        {
            try
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                StopAndDisposeWaveOut(); // 停止和释放之前的播放器和读取器资源
                // 初始化音频输出设备
                waveOut = new WaveOutEvent();

                // 加载OGG文件
                reader = new VorbisWaveReader(filePath);

                // 设置音频数据源并开始播放
                waveOut.Init(reader);
                waveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"播放失败: {ex.Message}");
            }
        }
        private void musicPlay(string filename)
        {
            string extension = Path.GetExtension(filename);
            if (extension == ".ogg")
            {
                PlayOgg(filename);
            }
            else
            {
                StopAndDisposeWaveOut();
                // 对于非OGG文件，使用axWindowsMediaPlayer控件播放
                axWindowsMediaPlayer1.URL = filename;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (localmusiclist.Count > 0)
            {
                string selectedFile = localmusiclist[listBox1.SelectedIndex];
                string extension = Path.GetExtension(selectedFile).ToLowerInvariant();

                // 更新UI显示当前选中的曲目名
                labelplaying.Text = Path.GetFileNameWithoutExtension(selectedFile);

                // 根据音频文件类型选择播放方式
                if (extension == ".ogg")
                {
                    // 对于OGG格式的音频文件，使用NAudio进行播放
                    PlayOgg(selectedFile);
                }
                else
                {
                    // 对于其他格式的音频文件，使用Windows Media Player控件进行播放
                    axWindowsMediaPlayer1.URL = selectedFile;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "打开音频|*.ogg"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string oggFilePath = openFileDialog.FileName;

                // 添加文件到列表并更新界面
                listBox1.Items.Add(oggFilePath);
                localmusiclist.Add(oggFilePath);

                // 选中新添加的歌曲
                listBox1.SelectedIndex = listBox1.Items.Count - 1;

                // 播放选中的OGG文件
                PlayOgg(oggFilePath);

                // 更新正在播放的曲目信息
                labelplaying.Text = Path.GetFileNameWithoutExtension(oggFilePath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (localmusiclist.Count > 0)
            {
                int index = listBox1.SelectedIndex + 1;
                if (index >= localmusiclist.Count()) { index = 0; }

                axWindowsMediaPlayer1.URL = localmusiclist[index];

                musicPlay(axWindowsMediaPlayer1.URL);
                labelplaying.Text = Path.GetFileNameWithoutExtension(localmusiclist[index]);

                listBox1.SelectedIndex = index;
            }
        }
    }
}
