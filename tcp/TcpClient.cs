using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


class GameClients1 {
	
    static string [] ShipArray;
	static int boardsize;
	static int hit;
	static int accurateshot;
   static int fire;
	/*static int GameSize(){
		
		Console.WriteLine("Enter the size of the game:");
		int size;
		
		string line = Console.ReadLine();
		if(int.TryParse(line, out size)){
			Console.WriteLine("Game area is set "+ size + ".");
		}
		else
		{
			Console.WriteLine("Please enter an integer");
		}
		
		return size;
		
	}*/
	
	static int Location(){
		int location;
		Console.WriteLine("Locate your ships.");
		while(true)
		{
		string line = Console.ReadLine();
		if(int.TryParse(line, out location))
		{
			Console.WriteLine("Game area is set on "+ location + ". location.");
		}
		else
		{
			Console.WriteLine("Please enter an integer");
		}
		return location;
		}
	}
	static void printships(){
		foreach(string str in ShipArray)
		{
			Console.Write(str+ " ");
		}
		Console.WriteLine();
		
	}
	static void SetGame(){
		
		ShipArray = new string[boardsize];
		for(int i=0; i<boardsize;i++){
			ShipArray[i]="[    ]";
			
		}
		for(int j=0;j<boardsize*0.3;j++){
			ShipArray[Location()-1]="[Ship]";
		}
		printships();
		
	}
	static void Check()
	{
		if(ShipArray[fire-1]=="[Ship]")
		{
			accurateshot=1;
			hit++;
			ShipArray[fire-1]="[DESTROYED]";
			Console.WriteLine("Enemy destroyed your ship");
			printships();
		}
		else
		{
			accurateshot=0;
			Console.WriteLine("Enemy missed.");
			printships();
		}
	}
	public static void Main(){
		//connecting to server
		byte[] data = new byte[1024];
        string input, stringData;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
                      

        Socket server = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
        //if there is no point to connect get error.             
        try
      {
         server.Connect(ipep);
      } catch (SocketException e)
      {
         Console.WriteLine("Unable to connect to server.");
         Console.WriteLine(e.ToString());
         return;
      }
      //get msg from server
      int recv = server.Receive(data);
      stringData = Encoding.ASCII.GetString(data, 0, recv);
      Console.WriteLine(stringData);
	  //get msg from server
	  recv = server.Receive(data);
      stringData = Encoding.ASCII.GetString(data, 0, recv);
      Console.WriteLine(stringData);
	  //send ready msg to server
	  string imready = Console.ReadLine();
      data = Encoding.ASCII.GetBytes(imready);
      server.Send(data, data.Length,SocketFlags.None);

      
		
		
		  // get gamesize from server
	        data = new byte[1024];
            recv = server.Receive(data);
            string gelenData = Encoding.ASCII.GetString(data, 0, recv);
		    int sz = Int32.Parse(gelenData);
			boardsize=sz;
			SetGame();
	  
	  
	  while(true)
	  {
		  //attack to enemy
		  if(hit<boardsize*0.3)
		  {
			  Console.WriteLine("Enter a number between 1-"+boardsize+" for attack to enemy.");
		      input = Console.ReadLine();
              server.Send(Encoding.ASCII.GetBytes(input));
		      Console.WriteLine("Attacked to "+input+" !!");
			  
		  }
	      else if(hit>=boardsize*0.3)
		  {
			  string tebrik = "193";
              server.Send(Encoding.ASCII.GetBytes(tebrik));
			  Console.WriteLine("You Lose");
			  string a = Console.ReadLine();
			  Environment.Exit(-1);
              server.Close();
			  
		  }
		   // notify if you hit or miss.
		  data = new byte[1024];
          recv = server.Receive(data);
          string rcData = Encoding.ASCII.GetString(data, 0, recv);
		  int acc = Int32.Parse(rcData);
		  if(acc==100)
		  {
		  Console.WriteLine("You have destroyed an enemy ship!!!");
		  
		  }
		  else if(acc==101)
		  {
			  Console.WriteLine("You missed!!!");
			 
		  }
		  
		  
		  //attack from enemy
		  data = new byte[1024];
          recv = server.Receive(data);
          rcData = Encoding.ASCII.GetString(data, 0, recv);
		  fire = Int32.Parse(rcData);
		  
		  if(fire == 193)
		  {
			  Console.WriteLine("You Win");
		   string a = Console.ReadLine();
		   Environment.Exit(-1);
		   
		  }
		  
		 Console.WriteLine("Enemy has attacked to number "+rcData+" !!");
			  
		  Check();
		    if(accurateshot==1)
		  {
			  string enemyhit = "100";
              server.Send(Encoding.ASCII.GetBytes(enemyhit));
		  }
		  else if(accurateshot==0)
		  {
			  string enemymiss = "101";
              server.Send(Encoding.ASCII.GetBytes(enemymiss));
		  }
		  
	
	
	
	}
	
	
	
	
		 
        
}      
	
}