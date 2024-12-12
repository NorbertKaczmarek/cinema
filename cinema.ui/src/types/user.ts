export type User = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  isAdmin: boolean;
  oldPassword?: string;
  password?: string;
  confirmPassword?: string;
};
