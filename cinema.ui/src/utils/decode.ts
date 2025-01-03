import { jwtDecode } from 'jwt-decode';

type Token = {
  FullName: string;
  Id: string;
  aud: string;
  exp: number;
  iss: string;
  Role: 'Admin' | 'User';
};

export const decodeToken = (token: string): Token => jwtDecode(token);
