import { Photo } from './photo';

export interface User {
    id: number;
    userName: string;
    gender: string;
    age: number;
    zodiacSign: string;
    created: Date;
    lastActive: Date;
    city: string;
    country: string;
     // info
    growth: string;
    eyeColor: string;
    hairColor: string;
    martialStatus: string;
    education: string;
    profession: string;
    children: string;
    languages: string;
    // o mnie
    motto: string;
    description: string;
    personality: string;
    lookingFor: string;
    // pasje
    intrests?: any;
    freeTime: string;
    sport: string;
    movies: string;
    music: string;
    // preferencje
    iLike: string;
    iDontLike?: any;
    makesMeLaugh: string;
    itFeelsBestIn: string;
    friendsWouldDescribeMe?: any;
    // zdjęcia
    photos: Photo[];
    photoUrl: string;
}