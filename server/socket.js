import { Server } from "socket.io";
import { v4 } from "uuid";
var uuid = v4;

class SocketServer {
  static instance;

  constructor(server) {
    this.io = null;
    this.playerlist = [];
    this.createSocketServer(server);
  }

  static getInstance(server = null) {
    if (SocketServer.instance) {
      return SocketServer.instance;
    }
    SocketServer.instance = new SocketServer(server);
    return SocketServer.instance;
  }

  createSocketServer(server) {
    console.log('Socket server running on port 3000');
    this.io = new Server(server, {
      cors: {
        origin: "http://localhost:3000",
      },
    });
    this.playerlist = [];
    this.runSocketServer();
  }

  getSocketServer() {
    return this.io;
  }

  runSocketServer() {
    this.io.on("connection", (socket) => {
      const my_id = uuid();
      this.playerlist.push({
        id: my_id,
        socket: socket
      });
      console.log('A user connected:' + my_id);
      socket.on('disconnect', () => { 
        console.log('A user disconnected:' + my_id);
        this.playerlist = this.playerlist.filter(item => item.id !== my_id);
      });
    });
  } 
}

export default SocketServer;
