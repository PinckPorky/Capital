using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Capital.Enams;

namespace Capital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();  // что это?

            Init();
        }
        #region Filds====================================
        List<StrategyType> _strategies = new List<StrategyType> 
        {



                StrategyType.FIX,
                StrategyType.CAPITALIZATION,
                StrategyType.PROGRESS,
                StrategyType.DOWNGRADE



        };
       Random _random = new Random();

        #endregion
        #region Methods==================================
        private void Init()
        {
            
            _comboBox.SelectionChanged += _comboBox_SelectionChange;
            _comboBox.SelectedIndex = 0; 

            _depo.Text = "100000";
            _startLot.Text = "10";
            _take.Text = "300";
            _stop.Text = "100";
            _commis.Text = "5";
            _countTrades.Text = "1000";
            _persentProfit.Text = "30";
            _go.Text = "5000";
            _minStartPercent.Text = "20";



        }
        private void _comboBox_SelectionChange(object sender, SelectionChangedEventArgs e) 
            //поясните по поводу этих переменых
        {
            ComboBox comboBox = (ComboBox)sender;

            int index = comboBox.SelectedIndex;
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Data>datas=Calculate();
            Draw(datas);
        }
        private void Calculate()
        {
            decimal depoStart = GetDecimalFromString(_depo.Text);
            decimal startLot = GetDecimalFromString(_startLot.Text);
            decimal take = GetDecimalFromString(_take.Text);
            decimal stop = GetDecimalFromString(_stop.Text);
            decimal comiss = GetDecimalFromString(_commis.Text);
            decimal coutTrades = GetDecimalFromString(_countTrades.Text);
            decimal percProfit = GetDecimalFromString(_persentProfit.Text);
            decimal minStartPrecent = GetDecimalFromString(_minStartPercent.Text);
            decimal go = GetDecimalFromString(_go.Text);

            List<Data> datas = new List<Data>();



            foreach (StrategyType type in _strategies)
            {
                datas.Add(new Data(depoStart, type));
            }

            int lotDown = startLot;
            decimal precent = startLot * go * 100 / depoStart;

            decimal multiply = startLot / stop;
            int lotProgress = CalculateLot(depoStart, minStartPrecent, go);

            int lotDown = startLot;

            for (int i = 0; i < coutTrades; i++)
            {
                int rnd = _random.Next(1, 100);

                if (rnd <= percProfit)
                {
                    // Сделка в прибыль
                    // stratedy 1=========================================

                    datas[0].ResultDepo += (take - comiss) * startLot; // это список?

                    // stratedy 2=========================================

                    datas[1].ResultDepo += (take - comiss) * startLot;
                    int newLot = CalculateLot(datas[1].ResultDepo, percent, go);
                    if (lotPercent < newLot) lotPercent = newLot;

                    // stratedy 3=========================================

                    datas[3].ResultDepo += (take - comiss) * lotProgress;
                    lotDown = startLot;
                }
                else
                {
                    // сделка в убыток
                    // stratedy 1=========================================
                    datas[0].ResultDepo -= (stop + comiss) * startLot;
                    // stratedy 2=========================================
                    datas[1].ResultDepo -= (stop + comiss) * startLot;
                    // stratedy 3=========================================
                    datas[2].ResultDepo -= (stop + comiss) * startLot;
                    lotProgress = CalculateLot(depoStart, minStartPrecent, go);
                    // stratedy 3=========================================
                    datas[3].ResultDepo -= (stop + comiss) * startLot;
                    lotProgress /= 2;
                    if (lotDown == 0) lotDown = 1;


                }
            }
            _dataGrid.ItemsSource = datas;
            return datas;
        }
        private void Draw(List<Data>datas)
        {
            int index = _comboBox.SelectedIndex;

            List<decimal> listEquity = datas[index].GetLisstEquity();
            int cout = listEquity.Count;
            decimal maxEquity = listEquity.Max();
            decimal minEquity = listEquity.Min();

            double stepX = _canvas.ActualWidth / cout;
            double kof = (double)(maxEquity - minEquity) / _canvas.ActualHeight;

            double x = 0;
            double y = 0;

            for (int i = 0; i < cout; i++)
            {
                
                
                x+= stepX;
            }
        }

        private int CalculateLot(decimal curretDepo, decimal precent, decimal go)
        {
            if (precent > 100) { precent = 100; }
            decimal lot = curretDepo / go / 100 * precent;
            return (int)lot;  // поясните синтаксис
        }

        private decimal GetDecimalFromString(string str)
        {
            if (decimal.TryParse(str, out decimal result)) return result;

            return 0;
        }

        private int GetDecimalFromString(string str)
        {
            if (int.TryParse(str, out int result)) return result;

            return 0;
        }

        #endregion



    }
}