using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


class GameServer1 {
	static int gsize;
    static string [] ShipArray;
	static int hit;
   static int fire;
   static int accurateshot;
	static int GameSize(){
		
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
		
	}
	
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
		
		ShipArray = new string[gsize];
		for(int i=0; i<gsize;i++){
			ShipArray[i]="[    ]";
			
		}
		for(int j=0;j<gsize*0.3;j++){
			ShipArray[Location()-1]="[Ship]";
		}
		printships();
		
	}
	
	static void Check()
	{
		if(ShipArray[fire-1]=="[Ship]")
		{
			accurateshot = 1;
			hit++;
			ShipArray[fire-1]="[DESTROYED]";
			Console.WriteLine("Enemy destroyed your ship");
			printships();
		}
		else
		{
			accurateshot = 0;
			Console.WriteLine("Enemy missed.");
			printships();
		}
	}
	
	
	
	public static void Main()
	{
		
		//open socket for client and wait for the client to connect.
		int recv;
      byte[] data = new byte[1024];
      IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

      Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);

      socket.Bind(ipep);
      Console.WriteLine("Waiting for a client");

      IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
      EndPoint tmpRemote = (EndPoint)(sender);

      recv = socket.ReceiveFrom(data, ref tmpRemote);

      Console.WriteLine(tmpRemote.ToString()+" connected");
      Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
	  
	  string welcome = "Welcome to my BattleShip Game";
      data = Encoding.ASCII.GetBytes(welcome);
      socket.SendTo(data, data.Length,SocketFlags.None,tmpRemote);
	  
	  Console.WriteLine("Send a message to start");
	  string areyouready = Console.ReadLine();
      data = Encoding.ASCII.GetBytes(areyouready);
      socket.SendTo(data, data.Length,SocketFlags.None,tmpRemote);
	  
	  //receive accepting msg from client
	  recv = socket.Receive(data);
      string stringData = Encoding.ASCII.GetString(data, 0, recv);
      Console.WriteLine(stringData);
	  
	  //sending gamesize
	  gsize = GameSize();
	  string gsz = gsize.ToString();
      data = Encoding.ASCII.GetBytes(gsz);
      socket.SendTo(data, data.Length, SocketFlags.None, tmpRemote);
	  
	  SetGame();
	  while(true)
      {
		  
		  //attack from enemy.
          data = new byte[1024];
          recv = socket.ReceiveFrom(data, ref tmpRemote);
		  string rcData = Encoding.ASCII.GetString(data, 0, recv);
		  fire = Int32.Parse(rcData); // int e ceviriyorum
		 if(fire==193)
		  {
		  Console.WriteLine("You Win");
		  string a = Console.ReadLine();
		  Environment.Exit(-1);
		  }
			  
			 
		  Console.WriteLine("Enemy has attackted to number "+rcData+".");
		 
		    
		  Check();
		  if(accurateshot==1)
		  {
			  string enemyhit = "100";
              socket.SendTo(Encoding.ASCII.GetBytes(enemyhit),tmpRemote);
		  }
		  else if(accurateshot==0)
		  {
			  string enemymiss = "101";
              socket.SendTo(Encoding.ASCII.GetBytes(enemymiss),tmpRemote);
		  }
		  
		  //attack to enemy
		  if(hit<gsize*0.3)
		  {
			  Console.WriteLine("Enter a number between 1-"+gsize+" for attack to enemy.");
		      string input = Console.ReadLine();
              socket.SendTo(Encoding.ASCII.GetBytes(input), tmpRemote);
		      Console.WriteLine("You have attacked to "+input+"!!");
		  }
		
		   else if(hit>=gsize*0.3)
		  {
			  string tebrik = "193";
              socket.SendTo(Encoding.ASCII.GetBytes(tebrik),tmpRemote);
			  Console.WriteLine("You Lose!!!");
			  string a = Console.ReadLine();
			  Environment.Exit(-1);
	          socket.Close();
			  
		  }
		   // notify if you hit or miss.
		  data = new byte[1024];
          recv = socket.ReceiveFrom(data, ref tmpRemote);
          rcData = Encoding.ASCII.GetString(data, 0, recv);
		  int acc = Int32.Parse(rcData);
		  if(acc==100)
		  {
		  Console.WriteLine("You have destroyed an enemy ship!!!");
		  
		  }
		  else if(acc==101)
		  {
			  Console.WriteLine("You missed!!!");
			 
		  }
		  
      }
      
	}
	
}