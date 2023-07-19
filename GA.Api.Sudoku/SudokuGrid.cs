using System.Text;

namespace GA.Api.Sudoku
{
    public partial class SudokuGrid : UserControl
    {
        SudokuCell[,] cells = new SudokuCell[9, 9];
        Color InitialCellColor = Color.Black;
        Color NewCellColor = Color.Red;

        public SudokuGrid()
        {
            InitializeComponent();            
        }

        public void Set(SudokuData data)
        {
            Controls.Clear();

            for(int row = 0; row < 9; row++)
            {
                for(int col = 0; col < 9; col++)
                {
                    var value = data.Values[row, col].Value;
                    var is_empty = data.Values[row, col].IsEmpty;
                    cells[row, col] = new SudokuCell(row, col, value, is_empty);
                    Controls.Add(cells[row, col]);
                }
            }
        }

        public void SetValue(int x, int y, int value)
        {
            cells[x, y].Value = value;
            cells[x, y].Text = value.ToString();
        }

        //public void Populate(string values)
        //{
        //    var chars = values.ToCharArray();

        //    if (chars.Length != 81)
        //        throw new Exception("Puzzle is not correct");

        //    Controls.Clear();

        //    for(int i = 0; i < 9; i++)
        //    {
        //        for(int j = 0; j < 9; j++)
        //        {
        //            var val = Int32.Parse(chars[i + (j * 9)].ToString());
        //            cells[i, j] = new SudokuCell(i, j, val, val == 0);
        //            cells[i, j].IsNew = val == 0;
        //            Controls.Add(cells[i, j]);
        //        }
        //    }
        //}



        //public void SetValue(int indice, int value)
        //{
        //    var x = indice % 9;
        //    var y = indice / 9;
        //    cells[x, y].Value = value;
        //    cells[x, y].Text = value.ToString();
        //}

        //public int GetValue(int x, int y)
        //{
        //    return cells[x, y].Value;
        //}

        //public string GetValues()
        //{
        //    var sbuilder = new StringBuilder();

        //    for(var i = 0; i < 9; i++)
        //    {
        //        for(var j = 0; j < 9; j++)
        //        {
        //            sbuilder.Append(cells[i, j].Value);
        //        }
        //    }

        //    return sbuilder.ToString();
        //}

        //public List<int> GetNewValues()
        //{
        //    var r = new List<int>();

        //    for(int i = 0; i < 9; i++)
        //    {
        //        for(int j = 0; j < 9; j++)
        //        {
        //            if(cells[i, j].IsNew)
        //            {
        //                r.Add(cells[i, j].Value);
        //            }
        //        }
        //    }

        //    return r;
        //}       

        //public SudokuCell GetCell(int x, int y)
        //{
        //    return cells[x, y];
        //}

        //public int ColumnTotal(int column)
        //{
        //    var r = 0;

        //    for (int i = 0; i < 9; i++)
        //    {
        //        r += GetValue(column, i);
        //    }

        //    return r;
        //}

        //public int RowTotal(int row)
        //{
        //    var r = 0;

        //    for (int i = 0; i < 9; i++)
        //    {
        //        r += GetValue(i, row);
        //    }

        //    return r;
        //}        
    }
}
