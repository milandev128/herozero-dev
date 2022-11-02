import mongoose from "mongoose";

// Defining Schema
const userSchema = new mongoose.Schema({
  name: { type: String, required: true, trim: true },
  email: { type: String, required: true, trim: true },
  password: { type: String, required: true, trim: true },
  skin: { type: Number, required: true, trim: true },
  level: { type: Number, required: true, trim: true },
  coins: { type: Number, required: true, trim: true },
  strength: { type: Number, required: true, trim: true },
  dodge: { type: Number, required: true, trim: true },
  items: [
    {
      name: {
        type: String,
        default: ""
      },
      strength: {
        type: Number,
        default: 0,
      },
      dodge: {
        type: Number,
        default: 0,
      },
    },
  ],
})

// Model
const UserModel = mongoose.model("user", userSchema)

export default UserModel