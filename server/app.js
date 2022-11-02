//import dotenv from 'dotenv'
//dotenv.config()
import express from 'express'
import cors from 'cors';
import connectDB from './config/connectdb.js'
import userRoutes from './routes/userRoutes.js'
import http from 'http'
import path from 'path';
import UserModel from './models/User.js'
import bodyParser from 'body-parser';
import bcrypt from 'bcrypt'
import jwt from 'jsonwebtoken'
import passport from 'passport';
import passportConfig from './config/passport.js';
import SocketServer from './socket.js';
import dotenv from 'dotenv';

const app = express()
const port = 3000
const server = http.createServer(app);

dotenv.config();
SocketServer.getInstance(server);

// CORS Policy
app.use(cors())

// Passport
app.use(passport.initialize());
passportConfig(passport);

// Database Connection
connectDB()

// JSON
app.use('/',express.static(path.join('static')))
app.use(bodyParser.json())

// Load Routes
app.use('/api', userRoutes);

server.listen(port, (req , res) => {
  console.log(`Server listening at http://localhost:${port}`)
})