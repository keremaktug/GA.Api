namespace GA.Api.Sudoku
{
    public partial class SudokuCell : Label
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Value { get; set; }

        public bool IsNew { get; set; }

        public SudokuCell(int x, int y, int value, bool is_new)
        {
            X = x;
            Y = y;
            Value = value;
            IsNew = is_new;

            Font = new Font(SystemFonts.DefaultFont.FontFamily, 10);
            Size = new Size(40, 40);
            ForeColor = IsNew ? Color.Red : Color.Black;
            Location = new Point(y * 40, x * 40);
            BackColor = ((x / 3) + (y / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
            FlatStyle = FlatStyle.Flat;
            Text = value.ToString();
            BorderStyle = BorderStyle.FixedSingle;
            TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}
