import {Strategy as JwtStrategy} from 'passport-jwt';
import {ExtractJwt} from 'passport-jwt';
import UserModel from '../models/User.js';
import { JWT_SECRET_KEY } from '../utils.js'

const opts = {};
opts.jwtFromRequest = ExtractJwt.fromAuthHeaderAsBearerToken();
opts.secretOrKey = JWT_SECRET_KEY;

export default passport => {
  passport.use(
    new JwtStrategy(opts, (jwt_payload, done) => {
      UserModel.findById(jwt_payload.userID)
        .then(user => {
          if (user) {
            return done(null, user);
          }
          return done(null, false);
        })
        .catch(err => console.log(err));
    })
  );
};