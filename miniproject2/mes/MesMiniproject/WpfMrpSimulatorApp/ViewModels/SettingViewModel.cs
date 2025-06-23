using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WpfMrpSimulatorApp.Helpers;
using WpfMrpSimulatorApp.Models;

namespace WpfMrpSimulatorApp.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        // readonly 생성자에서 할당하고나면 그 이후에 값 변경 불가
        private readonly IDialogCoordinator dialogCoordinator;

        #region View와 연동할 멤버변수들

        private string _basicCode;
        private string _codeName;
        private string? _coeDesc;
        private DateTime? _regDt;
        private DateTime? _modDt;

        private ObservableCollection<Setting> _settings;
        private Setting _selectedSetting;

        #endregion

        #region View와 연동할 속성

        // View와 연동될 데이터/컬렉션
        public ObservableCollection<Setting> Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        public Setting SelectedSetting
        {
            get => _selectedSetting;
            set => SetProperty(ref _selectedSetting, value);
        }

        /// <summary>
        /// 기본코드
        /// </summary>
        public string BasicCode { 
            get => _basicCode;
            set => SetProperty(ref _basicCode, value);
        }

        /// <summary>
        /// 코드명
        /// </summary>
        public string CodeName { 
            get => _codeName;
            set => SetProperty(ref _codeName, value);
        }

        /// <summary>
        /// 코드설명
        /// </summary>
        public string? CodeDesc {
            get => _coeDesc;
            set => SetProperty(ref _coeDesc, value);
        }

        public DateTime? RegDt {
            get => _regDt;
            set => SetProperty(ref _regDt, value);
        }

        public DateTime? ModDt { 
            get => _modDt;
            set => SetProperty(ref _modDt, value);
        }

        #endregion

        public SettingViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;  // 파라미터값으로 초기화
            LoadGridFromDb();   // DB에서 데이터 로드해서 그리드에 출력
        }

        private void LoadGridFromDb()
        {
            try
            {
                string query = @"SELECT basicCode
                                      , codeName
                                      , codeDesc
                                      , regDt
                                      , modDt
                                  FROM settings";
                ObservableCollection<Setting> settings = new ObservableCollection<Setting>();

                // DB 연동 방식 1
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var basicCode = reader.GetString("basicCode");
                        var codeName = reader.GetString("codeName");
                        var codeDesc = reader.GetString("codeDesc");
                        var regDt = reader.GetDateTime("regDt");
                        // modDt는 최초에 입력 후 항상 null. NULL 타입 체크 필수
                        var modDt = reader.IsDBNull(reader.GetOrdinal("modDt")) ? (DateTime?)null : reader.GetDateTime("modDt");
                        // ... 세개더

                        settings.Add(new Setting
                        {
                            BasicCode = basicCode,
                            CodeName = codeName,
                            CodeDesc = codeDesc,
                            RegDt = regDt,
                            ModDt = modDt,
                        });
                    }
                }

                Settings = settings;
            }
            catch (Exception ex)
            {

            }
        }

        #region View 버튼클릭 메서드

        [RelayCommand]
        public void NewData()
        {
            MessageBox.Show("신규");
        }

        [RelayCommand]
        public void SaveData()
        {
            MessageBox.Show("저장");
        }

        [RelayCommand]
        public async Task RemoveData()
        {
            var result = await this.dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
        }

        #endregion
    }
}