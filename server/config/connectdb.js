import mongoose from 'mongoose';
//const moongose = require('mongoose')

const connectDB = async () => {
  
  try {
   
    const url = "mongodb://localhost:27017/HeroZeroDatabase"
    await mongoose.connect(url ,{useNewUrlParser: true,useUnifiedTopology : true});
    console.log('DataBase Connected Successfully...')
  } catch (error) {
    console.log(error)
  }
}

export default connectDB