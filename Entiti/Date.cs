using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capital.Enams;

namespace Capital.Entiti
{
    public class Data
    {

        public Data(decimal depoStart, StrategyType strategyType)
        {
            StrategyType = strategyType;

            Depo = depoStart;
        }
        #region Propertis ==================================

        public StrategyType StrategyType { get; set; } 

        public decimal Depo
        {
            get => _depo;

            set
            {
                _depo = value;
                ResultDepo = value;
            }
        }

        decimal _depo;
        /// <summary>
        /// результат эквити(депо)
        /// </summary>
        public decimal ResultDepo
        {
            get => _resultdepo;

            set
            {
                _resultdepo = value;
                Profit = value - Depo;
                PercentProfit = Profit*100/Depo;

                ListEquity.Add(ResultDepo);
                CalcDrawDown();


            }
        }

        decimal _resultdepo;

        public decimal Profit { get; set; }
        /// <summary>
        /// относительный профит в процентах
        /// </summary>
        public decimal PercentProfit { get; set; }
        /// <summary>
        /// максимальная просатка в деньгах(абсолютная)
        /// </summary>
        public decimal MaxDrawDown
        {
            get => _maxDrawDown;

            set
            {
                _maxDrawDown = value;
                CaicPrecentDrawDown();
            }
        }
        decimal _maxDrawDown;
        /// <summary>
        /// максимальная относительная просадка в процентах
        /// </summary>
        public decimal PrecentDrawDown { get; set; }
        

        #endregion

        #region Filds =======================================
        List<decimal> ListEquity = new List<decimal>();

        private decimal _max = 0;

        public decimal _min = 0;









        #endregion

        #region Methods =====================================

        public List<decimal>GetListEquity()
        {
            return ListEquity;
        }

        private void CalcDrawDown()
        {
            if (_max < ResultDepo)
            {
                _max = ResultDepo;
                _min = ResultDepo;

            }
            if (_min < ResultDepo)
            {
                _min = ResultDepo;

                if (MaxDrawDown < _max - _min)
                {
                    MaxDrawDown = _max - _min;
                }
            }
        }

        private void CaicPrecentDrawDown()
        {
            decimal precent  = MaxDrawDown*100/ResultDepo;
            if (precent > PrecentDrawDown) PrecentDrawDown = Math.Round(precent, 2);
            
                
           
        }





        #endregion
    }
}
