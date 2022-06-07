class FirstTrane
{
    static public List<object> num = new List<object> {1,2,3,4,5,6,7,8,9};
    static public List<int> numNoReturn = new List<int>{1,2,3,4,5,6,7,8,9};
    static public int countMatch = 0;
    static public Random randomNum = new Random();
    static public bool exit = false;
    static public int winCount = 0;
    static public int loseCount = 0;
    static public int draw = 0;
    
    static void Main()
    {
        if(!exit) Console.WriteLine("\nHi :)\n");
        else Console.WriteLine($"bye:(\nКол-во побед: {winCount}\nпоражений: {loseCount}\nничьей: {draw}\n");
        for(int i = 0; i < 6; i++) 
        {
            ReturnResultGame();
        } 
    }

    static public void ReturnResultGame()
    { 
        countMatch++;
        if(exit) return;

        /*
           1 │ 2 │ 3 
           — ┼ – ┼ ―
           4 │ 5 │ 6
           — ┼ – ┼ ―
           7 │ 8 │ 9
        */

        
        for(int i = 0; i < num.Count; i++)
           SetColorText(i, num[i].ToString());  

        void SetColorText(int index, string? word)
        {
                if (index == 5 || index == 2) {SetColorWord(index,word); Console.Write("\n— ┼ – ┼ ―\n");}
                else if (index == num.Count - 1) {SetColorWord(index,word); Console.WriteLine();}
                else {SetColorWord(index,word); Console.Write(" │ ");}
        }

        void SetColorWord(int index, string? word)
        {
            if(num[index].ToString() == "X") 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(word);
                Console.ResetColor();
            }
            else if(num[index].ToString() == "O")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(word);
                Console.ResetColor();
            }
            else Console.Write(word);
        }
        string WOL = WinOrLose();
        if(WOL != "Ничья") ClearAndRepaet();
        else if(countMatch == 6 && WOL == "Ничья") {draw++; ClearAndRepaet();}
        if(exit) return;
        
        //if (exit) return;
        Console.WriteLine("\nВыберите число, чтобы сделать ход (от 1 до 9 включительно)");
        string? strOfGamer = Console.ReadLine();

        
        bool findNum = false;
        int indexForGame = 0;
        int indexForGame2 = 0;

        for(int i = 0; i < numNoReturn.Count; i++)
        {
            if(numNoReturn[i].ToString() == strOfGamer) 
                {findNum = true; numNoReturn.Remove(numNoReturn[i]); indexForGame = int.Parse(strOfGamer) - 1;} 
        }
        
        if(!findNum)
        {
            indexForGame = IndexReturn();
            Console.WriteLine("\nВы ошиблись при написании числа,\n" +
                             $"поэтому вам присвоилось случайное число: {indexForGame + 1}");    
        }
        
        num[indexForGame] = "X";
        
        if (countMatch == 1) 
        {
            indexForGame2 = IndexReturn();
            numNoReturn.Remove((int)num[indexForGame2]);
            num[indexForGame2] = "O";
        }
        else if (countMatch > 1 && countMatch < 5)  
        {   
            numNoReturn.Remove((int)num[GameAI(num)]);
            num[GameAI(num)] = "O";
        } 
    }

    static public int GameAI(List<object> numArray)
    {
        //Все возможные последовательности двух "X", для предотвращения три в ряд "X"
        string[] numVersion = {"13462", "034527", "145086", "014765", "01235678", "214783", "347028", "634581", "745026"};
        string[] numVersion2 = {"36 48 12", "47 02", "01 64 58", "45 06", "08 26 17 35", "34 28", "03 24 78", "14 68", "04 25 67"};
        
        for(int i = 0; i < numArray.Count; i++)
        {
            if(numArray[i].ToString() == "X")
            {
                foreach(char charNumVersion in numVersion[i])
                {
                    if(numArray[int.Parse(charNumVersion.ToString())].ToString() == "X")
                    {
                        for(int j = 0; j < numVersion2.Length; j++)
                        {
                            string[] versionO = numVersion2[j].Split(" ");
                            foreach(string strNumVersionO in versionO)
                            {
                                string? findX = numArray[int.Parse(strNumVersionO[0].ToString())].ToString();
                                string? findX2 = numArray[int.Parse(strNumVersionO[1].ToString())].ToString();
                                if(findX == "O" && findX2 == "O")
                                {
                                    if(!NoReturn(num[j].ToString())) return j;
                                    else IndexReturn(); 
                                }
                                else if(findX == "X" && findX2 == "X") 
                                {
                                    if(!NoReturn(num[j].ToString())) return j;
                                    else IndexReturn();
                                }
                            }
                        } 
                    }
                }
            }
        }
        return IndexReturn();   
    }

    static public bool NoReturn(string? IFG)
    {
        if (IFG == "X" || IFG == "O") return true;
        return false;
    }

    static public int IndexReturn()
    {
        int numRan = randomNum.Next(0,9);
        while(NoReturn(num[numRan].ToString()))
            numRan = randomNum.Next(0,9);
        return numRan;
    }

    static public string WinOrLose()
    {
        string[] winOrLoseComb = {"012", "345", "678", "036", "147", "258", "048", "246"};
        foreach(string numWord in winOrLoseComb)
        {  
            int countForWin = 0;
            int countForLose = 0;
            foreach(char numChar in numWord)
            { 
                if(num[int.Parse(numChar.ToString())].ToString() == "X") countForWin++;
                else if(num[int.Parse(numChar.ToString())].ToString() == "O") countForLose++;
                if(countForWin == 3) {winCount++; return "Победа";}
                else if(countForLose == 3) {loseCount++; return "Проигрыш";}
            }
        }

         return "Ничья";
    }

    public static void ClearAndRepaet()
    {
        Console.WriteLine(WinOrLose() + "\nНажмите любую кнопку, чтобы начать заново(exit, чтобы выйти)");
        num.Clear(); numNoReturn.Clear(); countMatch = 0;
        for(int i = 1; i < 10; i++) {num.Add(i); numNoReturn.Add(i);}
        string? str = Console.ReadLine();
        if (str != null) 
        {
            if(str.ToLower() == "exit") {exit = true;}
        }
        Console.Clear(); Main();
    }   
}

