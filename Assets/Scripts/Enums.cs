// maebleme2

namespace StickBlast
{
    public enum ItemTypes
    {
        None = -1,
        I = 10,
        FlatI = 20,
        I2 = 30,
        FlatI2 = 40,

        RightBottomL = 50,
        LeftBottomL = 60,
        LeftTopL = 70,
        RightTopL = 80,

        BottomLeftL2 = 90,
        BottomRightL2 = 100,
        LeftBottomL2 = 110,
        LeftTopL2 = 120,
        RightBottomL2 = 130,
        RightTopL2 = 140,
        TopLeftL2 = 150,
        TopRightL2 = 160,

        LeftU = 170,
        TopU = 180,
        RightU = 190,
        BottomU = 200,


        // flatI ve 2 aşağıya II
        // flatI ve 2 yukarıya II
        //.....
    }

    public enum ColorTypes
    {
        ItemStill,
        Passive,
        Hover,
        Active,
    }

    public enum LineDirection
    {
        Horizontal,
        Vertical
    }
}