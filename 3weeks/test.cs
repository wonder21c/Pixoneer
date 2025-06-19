using Microsoft.Win32;
using Pixoneer.NXDL.NXVideo;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MT.ExtractorToCSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public ObservableCollection<TargetInfo> SourceList { get; set; } = new ObservableCollection<TargetInfo>();
        //public ObservableCollection<TargetInfo> OutputList { get; set; } = new ObservableCollection<TargetInfo>();
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Multiselect = true;
            dialog.Filter = "TS 파일 (*.ts)|*.ts|모든 파일 (*.*)|*.*";
            dialog.Title = "비디오 파일 선택";

            if (dialog.ShowDialog() == true)
            {
                foreach (var f in dialog.FileNames)
                {
                    this.SourceList.Add(new TargetInfo()
                    {
                        Level = 1,
                        TargetPath = f,
                        FileSize = new FileInfo(f).Length
                    });
                }
                MessageBox.Show("Complete");
            }
        }

        private async void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            var checkedItems = SourceList
                .Where(x => x.IsChecked)
                .OrderBy(x => new FileInfo(x.TargetPath).Length) 
                .ToList();

            if (checkedItems.Count == 0)
            {
                MessageBox.Show("선택된 파일이 없습니다.");
                return;
            }
           int maxConcurrentTasks = 5;
           var semaphore = new SemaphoreSlim(maxConcurrentTasks);

            List<Task> tasks = new List<Task>();
            
            foreach (var item in checkedItems)
            {
                await semaphore.WaitAsync();

                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        var folderPath = Path.GetDirectoryName(item.TargetPath);
                        var fileName = Path.GetFileName(item.TargetPath);
                        LoadVideoData(folderPath, fileName, item);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);

            MessageBox.Show("모든 파일 변환이 완료되었습니다.");
        }

        private void chkAllSelect_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in SourceList)
                SetIsCheckedRecursive(item, true);
        }

        private void chkAllSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in SourceList)
                SetIsCheckedRecursive(item, false);
        }

        // 트리 구조 전체에 대해 체크/해제 적용
        private void SetIsCheckedRecursive(TargetInfo item, bool isChecked)
        {
            item.IsChecked = isChecked;
            foreach (var child in item.Items.OfType<TargetInfo>())
                SetIsCheckedRecursive(child, isChecked);
        }

        void MakeTreeView_Source(TargetInfo _target)
        {
            foreach (var folder in Directory.GetDirectories(_target.TargetPath))
            {
                _target.Items.Add(new TargetInfo() { Level = _target.Level + 1, TargetPath = folder });
                this.MakeTreeView_Source((TargetInfo)_target.Items.Last());
            }

            foreach (var file in Directory.GetFiles(_target.TargetPath).Where(o => o.EndsWith(".ts")))
            {
                _target.Items.Add(new TargetInfo() { Level = _target.Level + 1, TargetPath = file });
            }
        }


        bool isFinishedProc = false;
        List<MT_MV> listMetad = null;
      
        void LoadVideoData(string _folderPath, string _filePath, TargetInfo target)
        {
            
            //Debug.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Start: {target.TargetPath} {DateTime.Now:HH:mm:ss.fff}");

            XVideoIO videoIO = new XVideoIO();
            XVideo video = null;

            if (!string.IsNullOrEmpty(_filePath) && _filePath.StartsWith(@"\"))
                _filePath = _filePath.Substring(1);

            string filePath = Path.Combine(_folderPath, _filePath);
            if (!File.Exists(filePath))
                return;

            var metadList = new List<MT_MV>();
            DateTime lastReadMetad = DateTime.Now;

            Dispatcher.Invoke(() =>
            {
                target.Progress = 0;
                target.TotalFrames = 0;
            });

            int processedFrames = 0;

            video = videoIO.OpenFile(
                filePath,
                @"XFFMPDriver",
                true,
                false,
                null,
                (videoIO, streamID, data) =>
                {
                    MT_MV metad = new MT_MV();
                    metad.SetData(data.PTS, data.GetData());
                    metadList.Add(metad);
                    lastReadMetad = DateTime.Now;
                    processedFrames++;
                
                        Dispatcher.Invoke(() =>
                        {
                            target.TotalFrames = processedFrames;
                            target.Progress = Math.Min(1.0, (double)processedFrames / target.TotalFrames);
                            Debug.WriteLine("@@@@@" + processedFrames.ToString() + " / " + target.TotalFrames.ToString());
                        });
                    
                },
                null,
                out string err
            );

            if (!string.IsNullOrEmpty(err))
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"파일: {filePath}\n에러: {err}");
                });
                return;
            }

            while ((DateTime.Now - lastReadMetad).TotalSeconds < 1)
            {
                Thread.Sleep(10);
            }

            // CSV 저장
            string csvPath = Path.Combine(_folderPath, "output", Path.GetFileNameWithoutExtension(_filePath) + ".csv");
            Directory.CreateDirectory(Path.GetDirectoryName(csvPath));
            GenerateMetadDataToCSV(csvPath, metadList);
        }


        void GenerateMetadDataToCSV(string _csvPath, List<MT_MV> metadList)
        {
            StringBuilder sb = new StringBuilder();

            var properties = typeof(MT_MV).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          .Where(p => p.Name.EndsWith("_DP") && p.PropertyType == typeof(string))
                          .ToArray();

            var headers = properties.Select(o => o.Name.Replace("_DP", "")).ToArray();
            sb.AppendLine(string.Join(",", headers));

            // 데이터 작성
            foreach (var item in metadList)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item) as string;
                    return Escape(value);
                });

                sb.AppendLine(string.Join(",", values));
            }

            File.WriteAllText(_csvPath, sb.ToString(), Encoding.UTF8);
        }

        string Escape(string input)
        {
            if (input == null) return "";
            if (input.Contains(",") || input.Contains("\"") || input.Contains("\n"))
            {
                input = input.Replace("\"", "\"\"");
                return $"\"{input}\"";
            }
            return input;
        }

        DateTime dtLastReadMetad = DateTime.MinValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void PreviewFrameMetad(XVideoIO videoIO, int streamID, XFrameMetad data)
        {
            MT_MV metad = new MT_MV();
            metad.SetData(data.PTS, data.GetData());
            this.listMetad.Add(metad);

            this.dtLastReadMetad = DateTime.Now;
        }

        /// <summary>
        /// 지정한 폴더에서 지정한 확장자를 가진 모든 파일 경로를 반환합니다 (하위 폴더 포함).
        /// </summary>
        /// <param name="folderPath">검색할 루트 폴더 경로</param>
        /// <param name="extension">확장자 (예: ".txt")</param>
        /// <returns>파일 경로 리스트</returns>
        public List<string> GetFilesByExtension(string folderPath, string extension)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"폴더를 찾을 수 없습니다: {folderPath}");

            if (!extension.StartsWith("."))
                extension = "." + extension;

            var files = Directory.GetFiles(folderPath, "*" + extension, SearchOption.AllDirectories);
            return new List<string>(files);
        }
    }

    public class TargetInfo : TreeData
    {
        public string Type
        {
            get
            {
                if (File.Exists(this.targetPath))
                    return "File";
                else if (Directory.Exists(this.targetPath))
                    return "Folder";
                else
                    return "N/A";
            }
        }

        public string FileSizeMb
        {
            get
            {
                if (FileSize == 0)
                {
                    return "0 MB";
                }
                double mb = FileSize / 1024d / 1024d;
                return $"{mb:N2} MB";
            }
        }

        string targetPath = "";
        public string TargetPath
        {
            get => this.targetPath;
            set
            {
                this.targetPath = value;
                this.Name = Path.GetFileName(TargetPath);
                if (!this.isManualNotifyPropertyChanged)
                    this.OnPropertyChanged();
                UpdateFileSize();
            }
        }

        long fileSize = 0;
        public long FileSize
        {
            get => this.fileSize;
            set
            {
                this.fileSize = value;
                if (!this.isManualNotifyPropertyChanged)
                    this.OnPropertyChanged();
            }
        }

        void UpdateFileSize()
        {
            if (File.Exists(this.targetPath))
                this.FileSize = new FileInfo(this.targetPath).Length;
            else
                this.FileSize = 0;
        }

        double progress = 0;
        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                if (!this.isManualNotifyPropertyChanged)
                    this.OnPropertyChanged();
            }
        }
        public int TotalFrames { get; set; }

    }

    public class TreeData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool isManualNotifyPropertyChanged = false;
        public void SetManualNotifyChangedMode(bool _isManualNotifyPropertyChanged) => this.isManualNotifyPropertyChanged = _isManualNotifyPropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual void RefreshAll() => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        protected bool isChecked = false;
        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.isChecked = value;
                if (!this.isManualNotifyPropertyChanged)
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsChecked)));
            }
        }

        protected int level = -1;
        public int Level
        {
            get => this.level;
            set
            {
                this.level = value;
                if (!this.isManualNotifyPropertyChanged)
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Level)));
            }
        }

        protected string name = "-";
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                if (!this.isManualNotifyPropertyChanged)
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Name)));
            }
        }

        protected TreeData parent;
        protected List<TreeData> items = new List<TreeData>();
        public List<TreeData> Items
        {
            get => this.items;
            set
            {
                this.items = value;
                if (!this.isManualNotifyPropertyChanged)
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Items)));
            }
        }

        public virtual bool Add(TreeData _item)
        {
            bool res = true;

            if (this.items.Contains(_item))
                res = false;
            else
            {
                this.items.Add(_item);
                if (!this.isManualNotifyPropertyChanged)
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Items)));
            }

            return res;
        }

        public virtual bool Remove(TreeData _item)
        {
            bool res = true;

            if (_item == null)
                res = false;

            res = this.items.Remove(_item);
            if (res && !this.isManualNotifyPropertyChanged)
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Items)));

            return res;
        }
    }
}
