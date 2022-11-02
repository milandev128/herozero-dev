import mongoose from 'mongoose';

const connectDB = async () => {
  
  try {
    let url = "";
    
    if (process.env.NODE_ENV === 'production') {
        url = process.env.MONGO_URI;
    } else {
        url = "mongodb://localhost:27017/HeroZeroDatabase";
    }

    await mongoose.connect(url ,{useNewUrlParser: true,useUnifiedTopology : true});
    console.log('DataBase Connected Successfully...')
  } catch (error) {
    console.log(error)
  }
}

export default connectDB