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
	
	
	
	public static void Main()
	{
		
		//create a tcp connection as a server, wait for the client to connect.
	  int recv;  
      byte[] data = new byte[1024];
      IPEndPoint ipep = new IPEndPoint(IPAddress.Any,9050);
                             

      Socket newsock = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
                      
      newsock.Bind(ipep);
      newsock.Listen(10);
      Console.WriteLine("Waiting for a client...");
      Socket client = newsock.Accept(); 

      
      IPEndPoint clientep =(IPEndPoint)client.RemoteEndPoint;
                   
      Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);
                     

      //send welcome msg to client
      string welcome = "Welcome to my BattleShip Game";
      data = Encoding.ASCII.GetBytes(welcome);
      client.Send(data, data.Length,SocketFlags.None);
	 
	 //send a msg to check if client is ready
	  Console.WriteLine("Send a message to start");
	  string areyouready = Console.ReadLine();
      data = Encoding.ASCII.GetBytes(areyouready);
      client.Send(data, data.Length,SocketFlags.None);
	  
	  //get msg from client to start game.
	  recv = client.Receive(data);
      string stringData = Encoding.ASCII.GetString(data, 0, recv);
      Console.WriteLine(stringData);
	  
	  //send gamesize to client
	  gsize = GameSize();
	  string gsz = gsize.ToString();
      data = Encoding.ASCII.GetBytes(gsz);
      client.Send(data, data.Length, SocketFlags.None);
      
	  SetGame();
	  
      while(true)
      {
		  //attack from enemy
         data = new byte[1024];
         recv = client.Receive(data);
		 string rcData = Encoding.ASCII.GetString(data, 0, recv);
		  fire = Int32.Parse(rcData);
         
		 
		 
		 if(fire==193)
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
              client.Send(Encoding.ASCII.GetBytes(enemyhit));
		  }
		  else if(accurateshot==0)
		  {
			  string enemymiss = "101";
              client.Send(Encoding.ASCII.GetBytes(enemymiss));
		  }
		  //attack to enemy
		  if(hit<gsize*0.3)
		  {
			  Console.WriteLine("Enter a number between 1-"+gsize+" for attack to enemy.");
		      string input = Console.ReadLine();
              client.Send(Encoding.ASCII.GetBytes(input));
		      Console.WriteLine("Attacked to "+input+" !!");
			  
		  }
		   else if(hit>=gsize*0.3)
		  {
			  string tebrik = "193";
              client.Send(Encoding.ASCII.GetBytes(tebrik));
			  Console.WriteLine("You Lose!!!");
			  string a = Console.ReadLine();
			  Environment.Exit(-1);
	          client.Close();
              newsock.Close();
			  
		  }
		   // notify if you hit or miss.
		  data = new byte[1024];
          recv = client.Receive(data);
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