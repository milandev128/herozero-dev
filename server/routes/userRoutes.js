import express from 'express';
import passport from 'passport';
import UserController from '../controllers/userController.js';

const router = express.Router();

// Authentication Routes
router.post('/register', UserController.userRegistration);
router.post('/login', UserController.userLogin);

// Game Routes
router.get('/getuserdata', passport.authenticate('jwt', { session: false }), UserController.getUserInfo);
router.post('/buyitem', passport.authenticate('jwt', { session: false }), UserController.buyItem);
router.get('/startmission', passport.authenticate('jwt', { session: false }), UserController.startMission);
router.get('/completemission', passport.authenticate('jwt', { session: false }), UserController.completeMission);

export default router