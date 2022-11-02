import UserModel from '../models/User.js'
import bcrypt from 'bcrypt'
import jwt from 'jsonwebtoken'
import { JWT_SECRET_KEY } from '../utils.js'


class UserController {
  static userRegistration = async (req, res) => {
  
    const { name, email, password, skin } = req.body
    
    const User = await UserModel.findOne({ email: email })
    if (User) {
      res.json({ status: "failed", error: "Email already exists" })
    } else {
      if (name && email && password ) {
        try {
          const salt = await bcrypt.genSalt(10)
          const hashPassword = await bcrypt.hash(password, salt)
          const user = new UserModel({
            name: name,
            email: email,
            password: hashPassword,
            skin: skin,
            level: 1,
            coins: 50,
            strength: 1,
            dodge: 1            
          })
          await user.save()
          return res.status(200).json({ 
            status: "success", 
            message: "Registration Success", 
          })
        } catch (error) {
          console.log(error)
          res.json({ status: "failed", message: "Unable to Register" })
        }
      } else {
        res.json({ status: "failed", message: "All fields are required" })
      }
    }
  }

  static userLogin = async (req, res) => {
    try {
      const { name, password } = req.body
      if (name && password) {
        const user = await UserModel.findOne({ name: name })

        if (user != null) {
          const isMatch = await bcrypt.compare(password, user.password)
          if ((user.name === name) && isMatch) {
            // Generate JWT Token
            jwt.sign(
              { userID: user._id }, 
              JWT_SECRET_KEY, 
              { expiresIn: '5d' },
              ( err, token ) => {
                res.json({ 
                  status: "success", 
                  message: "Login Success", 
                  token: token,
                  user: user,
                })
              }
            )
          } else {
            res.json({ status: "failed", message: "Credential does not match" })
          }
        } else {
          res.json({ status: "failed", message: "You are not a Registered User" })
        }
      } else {
        res.json({ status: "failed", message: "All Fields are Required" })
      }
    } catch (error) {
      console.log(error)
      res.json({ status: "failed", message: "Unable to Login" })
    }
  }

  static getUserInfo = (req, res) => {
    return res.json({ 
      status: "success", 
      message: "User data showing succesfully",
      user: req.user 
    });
  }

  static buyItem = async (req, res) => {
    let user = req.user;
    try{
      if(user.items.find(item => item.name === req.body.name)) {
        return res.json({ 
          status: "failed", 
          message: "You have this item already."
        });
      } else {
        user.items.push(req.body);
        await user.save();
        return res.json({ 
          status: "success", 
          message: "Buy item successfully."
        });
      }
    } catch(error) {
      console.log(error);
      return res.json({ status: "failed", message: "Unable to buy item" })
    }
  }

  static completeMission = async (req, res) => {
    let user = req.user;
    try{
      if(user.level == 2) {
        return res.json({ 
          status: "failed", 
          message: "You already finished this mission."
        });
      } else {
        user.level = 2
        await user.save();
        return res.json({ 
          status: "success", 
          message: "Complete mission successfully."
        });
      }
    } catch(error) {
      console.log(error);
      return res.json({ status: "failed", message: "Unable to complete mission" })
    }
  }
}

export default UserController